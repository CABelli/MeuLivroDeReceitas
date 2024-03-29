﻿using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Recipess;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Extensions;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Domain.InterfacesGeneric;
using MeuLivroDeReceitas.Exceptions.ExceptionBase;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ILogger<RecipeService> _logger;
        private IUnitOfWork _unitOfWork;
        private IRecipeRepository _recipeRepository;
        
        private IAuthenticate _authenticateService;

        public RecipeService(IServiceProvider serviceProvider, 
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
            _logger = serviceProvider.GetRequiredService<ILogger<RecipeService>>();
            _recipeRepository = serviceProvider.GetRequiredService<IRecipeRepository>();
            _authenticateService = serviceProvider.GetRequiredService<IAuthenticate>();
        }

        public async Task<IEnumerable<RecipeResponseDTO>> GetRecipies()
        {
            await GenerateLogAudit(nameof(GetRecipies));
            var listRecipeResponseDTO = new List<RecipeResponseDTO>();
            var recipies = await _recipeRepository.GetAllAsync();
            if (recipies != null)
                recipies.OrderBy(x => x.Title).ToList().ForEach(userList => listRecipeResponseDTO.Add(RecipeResult(userList)));

            return listRecipeResponseDTO;
        }

        public async Task<RecipeResponseDTO> GetRecipeById(Guid id)
        {
            await GenerateLogAudit(nameof(GetRecipeById) + " , key: " + id);
            var recipe = await _recipeRepository.GetByIdAsync(id);
            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipeById_Info_RecipeNotFound, nameof(GetRecipeById), id) });

            return RecipeResult(recipe);
        }

        public async Task<RecipeResponseDTO> GetRecipiesTitle(string title)
        {
            await GenerateLogAudit(nameof(GetRecipiesTitle) + " , key: " + title);
            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == title);
            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipiesTitle_Info_RecipeNotFound.RemoveAccents(), nameof(GetRecipiesTitle), title.RemoveAccents()) });

            return RecipeResult(recipe);
        }

        public async Task<RecipeResponseImageDraftDTO> GetRecipiesDownLoad(string title)
        {
            await GenerateLogAudit(nameof(GetRecipiesDownLoad) + " , key: " + title);
            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == title);

            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipiesTitle_Info_RecipeNotFound, nameof(GetRecipiesTitle), title) });
            
            if (string.IsNullOrEmpty(recipe.FileExtension))
                    throw new ErrosDeValidacaoException(new List<string>() { string.Format(Resource.GetRecipiesDownLoad_Info_NotContainImageFile, nameof(GetRecipiesTitle), title) });

            return new RecipeResponseImageDraftDTO
            {
                Title = recipe.Title,
                NameFile = recipe.Title?.TitleNameFileExtension(recipe.FileExtension),
                DataDraft = recipe.DataDraft
            };
        }

        public async Task AddRecipe(AddRecipeDTO addRecipeDTO)
        {
            var usuarioId = await GenerateLogAudit(nameof(AddRecipe) + " , key: " + addRecipeDTO.Title);
            
            await ValidateAddRecipe(addRecipeDTO);

            var recipe = new Recipe()
            {
                Title = addRecipeDTO.Title,
                PreparationMode = addRecipeDTO.PreparationMode,
                PreparationTime = addRecipeDTO.PreparationTimeMinute,
                Category = addRecipeDTO.Category
            };
            recipe.SetCreationUsuarioId(usuarioId);

            _recipeRepository.Create(recipe);

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRecipe(ModifyRecipeDTO modifyRecipeDTO)
        {
            var usuarioId = await GenerateLogAudit(nameof(UpdateRecipe) + " , key: " + modifyRecipeDTO.Title);

            var recipe = await ValidateRecipeModification(modifyRecipeDTO);

            recipe.SetAlterationUsuarioId(usuarioId);
            recipe.PreparationTime = modifyRecipeDTO.PreparationTimeMinute;
            recipe.PreparationMode = modifyRecipeDTO.PreparationMode;         
            recipe.Category = modifyRecipeDTO.Category;
            if (modifyRecipeDTO.DeleteImageFile)
            {
                recipe.DataDraft = null;
                recipe.FileExtension = null;
            }

            _recipeRepository.Update(recipe);

            await _unitOfWork.CommitAsync();
        }
               
        public async Task<string> UpdateRecipeDraftImage(ICollection<IFormFile> files, string title)
        {
            var usuarioId = await GenerateLogAudit(nameof(UpdateRecipeDraftImage) + " , key: " + title);

            var fileExtension = await ValidateFile(files, title);

            var recipeTitle = await GetRecipiesTitle(title);

            var fileDrfat = ConverteFilesToBytes(files);

            var recipe = await _recipeRepository.GetByIdAsync(recipeTitle.Id);
            recipe.SetAlterationUsuarioId(usuarioId);
            recipe.FileExtension = fileExtension;
            recipe.DataDraft = fileDrfat; 

            _recipeRepository.Update(recipe);

            await _unitOfWork.CommitAsync();

            return title.TitleNameFileExtension(fileExtension);

            //  Exemplos de criticas com saida da API
            // await Task.FromException<Exception>(new Exception(string.Format(Resource.UpdateRecipeDraftImage_Error_DataDraftIsNull, nameof(UpdateRecipeDraftImage))));
            // throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipiesTitle_Info_RecipeNotFound, nameof(GetRecipiesTitle), title) });
            // throw new ErrosDeValidacaoException(new List<string>() { Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists });
        }

        public async Task DeleteRecipeByTitle(string title)
        {
            await GenerateLogAudit(nameof(UpdateRecipeDraftImage) + " , key: " + title);
            var recipies = await _recipeRepository.WhereAsync(x => x.Title == title);
            if (recipies.Count() == 0)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.DeleteRecipeByTitle_Info_RecipeNotFound, nameof(DeleteRecipeByTitle), title) });

            _recipeRepository.Delete(await _recipeRepository.GetByIdAsync(recipies.First().Id));
            
            await _unitOfWork.CommitAsync();
        }

        public async Task<string> GenerateLogAudit(string text) 
        {
            var appUserDto = await _authenticateService.RetrieveUserByIdentity();
            _logger.LogWarning(Resource.GenerateLogAudit_LogWarning, 
                nameof(GenerateLogAudit),
                text,
                appUserDto.UserName,
                appUserDto.PhoneNumber,
                appUserDto.Email);
            
            return appUserDto.UserName;
        }

        #region Private

        private async Task<string> ValidateFile(ICollection<IFormFile> files, string title)
        {
            if (files.Count() == 0)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.UpdateRecipeDraftImage_Error_DataDraftIsNull });

            string fileExtension = Path.GetExtension(files.FirstOrDefault().FileName);

            if (!fileExtension.ExtensionToBool())
                throw new ErrosDeValidacaoException(new List<string>()
                { string.Format(Resource.UpdateRecipeDraftImage_Info_FileExtensionNotAccepted, nameof(UpdateRecipeDraftImage), title, fileExtension) });

            return fileExtension;
        }

        private RecipeResponseDTO RecipeResult(Recipe recipe)
        {            
            var recipeResponseDTO = new RecipeResponseDTO()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Category = recipe.Category,
                NameCategory = recipe.Category.GetLocalizedDescription(),
                PreparationMode = recipe.PreparationMode,
                PreparationTimeMinute = recipe.PreparationTime,
                FileExtension = recipe.FileExtension                
            };
            return recipeResponseDTO;
        }

        private async Task ValidateAddRecipe(AddRecipeDTO addRecipeDTO)
        {
            var validator = new RecipeValidator(EMethodRecipeValidator.AddRecipe);

            var resultado = validator.Validate(new RecipeDTO() { 
                Title = addRecipeDTO.Title,
                Category =  addRecipeDTO.Category, 
                PreparationMode = addRecipeDTO.PreparationMode, 
                PreparationTimeMinute = addRecipeDTO.PreparationTimeMinute });

            if (!resultado.IsValid)
                throw new ErrosDeValidacaoException(resultado.Errors.Select(c => c.ErrorMessage).ToList());

            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == addRecipeDTO.Title);
            if (recipe != null)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists });

            ///var recipies = await _recipeRepository.WhereAsync(x => x.Title == recipeDTO.Title);
            ///if (recipies.Count() > 0)            
            ///    resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("Receita", Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists));            

            ///int a = 0, b = 0;
            ///a = a / b;
        }

        private async Task<Recipe> ValidateRecipeModification(ModifyRecipeDTO modifyRecipeDTO)
        {
            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == modifyRecipeDTO.Title);
            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() 
                { string.Format(Resource.ValidateRecipeModification_Info_RecipeNotFound, nameof(GetRecipiesTitle), modifyRecipeDTO.Title) });

            var validator = new RecipeValidator(EMethodRecipeValidator.ModifyRecipe);
            var resultado = validator.Validate(new RecipeDTO()  {
                Title = modifyRecipeDTO.Title,
                Category = modifyRecipeDTO.Category,
                PreparationMode = modifyRecipeDTO.PreparationMode,
                PreparationTimeMinute = modifyRecipeDTO.PreparationTimeMinute,
                OldFileExtension = recipe.FileExtension,
                DeleteImageFile = modifyRecipeDTO.DeleteImageFile});

            if (!resultado.IsValid)
                throw new ErrosDeValidacaoException(resultado.Errors.Select(c => c.ErrorMessage).ToList());

            return recipe;
        }

        private byte[] ConverteFilesToBytes(ICollection<IFormFile> files)
        {
            var lista = files.Select(formFile => BuildListByte(formFile));
            return lista.First().FirstOrDefault();
        }

        private List<byte[]> BuildListByte(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return new List<byte[]> { memoryStream.ToArray() };
            }
        }

        #endregion
    }
}
