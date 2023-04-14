using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using MeuLivroDeReceitas.CrossCutting.Extensions;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Domain.InterfacesGeneric;
using MeuLivroDeReceitas.Exceptions.ExceptionBase;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ILogger<RecipeService> _logger;
        private IUnitOfWork _unitOfWork;
        private IRecipeRepository _recipeRepository;

        public RecipeService(IServiceProvider serviceProvider, IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
            _logger = serviceProvider.GetRequiredService<ILogger<RecipeService>>();
            _recipeRepository = serviceProvider.GetRequiredService<IRecipeRepository>();
        }

        public async Task<IEnumerable<RecipeResponseDTO>> GetRecipies()
        {
            var recipies = await _recipeRepository.GetAll();
            if (recipies == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipies_Info_RecipeNotFound, nameof(GetRecipies)) });

            var listRecipeResponseDTO = ListRecipeResponseDTO(recipies);
            return listRecipeResponseDTO;
        }

        public async Task<RecipeResponseDTO> GetRecipeById(Guid id)
        {
            var recipe = await _recipeRepository.GetById(id);
            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipeById_Info_RecipeNotFound, nameof(GetRecipeById), id) });

            return RecipeResult(recipe);
        }

        public async Task<RecipeResponseDTO> GetRecipiesTitle(string title)
        {
            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == title);
            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipiesTitle_Info_RecipeNotFound.RemoveAccents(), nameof(GetRecipiesTitle), title.RemoveAccents()) });

            return RecipeResult(recipe);
        }

        public async Task<RecipeImageDraftDTO> GetRecipiesDownLoad(string title)
        {
            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == title);

            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipiesTitle_Info_RecipeNotFound, nameof(GetRecipiesTitle), title) });
            if (recipe.FileExtension == null || String.IsNullOrEmpty(recipe.FileExtension))
                throw new ErrosDeValidacaoException(new List<string>() { string.Format(Resource.GetRecipiesDownLoad_Info_NotContainImageFile, nameof(GetRecipiesTitle), title) });

            return new RecipeImageDraftDTO
            {
                Title = recipe.Title,
                NameFile = recipe.Title?.TitleNameFileExtension(recipe.FileExtension),
                DataDraft = recipe.DataDraft
            };
        }

        public async Task AddRecipe(RecipeDTO recipeDTO)
        {
            _logger.LogInformation(Resource.AddRecipe_Info_Starting, nameof(AddRecipe), recipeDTO.Title);

            await ValidarRecipeDTO(recipeDTO);

            var recipe = new Recipe()
            {
                Title = recipeDTO.Title,
                PreparationMode = recipeDTO.PreparationMode,
                PreparationTime = recipeDTO.PreparationTime,
                Category = recipeDTO.Category,
                FileExtension = recipeDTO.FileExtension
            };

            if (recipeDTO.FileExtension == "Cel") recipe.DataDraft = Encoding.ASCII.GetBytes(recipeDTO.DataDraft);

            _recipeRepository.Create(recipe);

            await _unitOfWork.CommitAsync();
        }

        private async Task ValidarRecipeDTO(RecipeDTO recipeDTO)
        {
            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == recipeDTO.Title);
            if (recipe != null)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists });

            var validator = new RecipeValidator(1, recipeDTO.Title.Length);
            var resultado = validator.Validate(recipeDTO);

            //var recipies = await _recipeRepository.WhereAsync(x => x.Title == recipeDTO.Title);
            //if (recipies.Count() > 0)            
            //    resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("Receita", Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists));            

            int a = 0, b = 0;
            a = a / b;

            if (!resultado.IsValid)
                throw new ErrosDeValidacaoException(resultado.Errors.Select(c => c.ErrorMessage).ToList());
        }

        public async Task UpdateRecipeDraftString(RecipeDTO recipeDTO)
        {
            var recipe = await ValidateRecipeModification(recipeDTO);

            recipe.PreparationTime = recipeDTO.PreparationTime == 0 ? recipe.PreparationTime : recipeDTO.PreparationTime;
            recipe.PreparationMode = recipeDTO.PreparationMode == null ? recipe.PreparationMode : recipeDTO.PreparationMode;
            recipe.Category = recipeDTO.Category;
            recipe.FileExtension = recipeDTO.FileExtension;
            if (recipeDTO.FileExtension != null) recipe.DataDraft = Encoding.ASCII.GetBytes(recipeDTO.DataDraft);

            _recipeRepository.Update(recipe);
            await _unitOfWork.CommitAsync();
        }

        private async Task<Recipe> ValidateRecipeModification(RecipeDTO recipeDTO)
        {
            var recipe = await _recipeRepository.WhereFirstAsync(x => x.Title == recipeDTO.Title);
            if (recipe == null)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.ValidateRecipeModification_Info_RecipeNotFound, nameof(GetRecipiesTitle), recipeDTO.Title) });

            var validator = new RecipeValidator(2, recipeDTO.Title.Length);
            var resultado = validator.Validate(recipeDTO);

            if (!resultado.IsValid)
                throw new ErrosDeValidacaoException(resultado.Errors.Select(c => c.ErrorMessage).ToList());

            return recipe;
        }
               
        public async Task<string> UpdateRecipeDraftImage(ICollection<IFormFile> files, string title)
        {
            if (files.Count() == 0) 
                throw new ErrosDeValidacaoException(new List<string>() { Resource.UpdateRecipeDraftImage_Error_DataDraftIsNull });

            string fileExtension = Path.GetExtension(files.FirstOrDefault().FileName);

            if (!fileExtension.ExtensionToBool())           
                throw new ErrosDeValidacaoException(new List<string>() 
                { string.Format(Resource.UpdateRecipeDraftImage_Info_FileExtensionNotAccepted, nameof(UpdateRecipeDraftImage), title, fileExtension) });            

            var recipeTitle = await GetRecipiesTitle(title);

            var fileDrfat = ConverteFilesToBytes(files);

            var recipe = await _recipeRepository.GetById(recipeTitle.Id);
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
            var recipies = await _recipeRepository.WhereAsync(x => x.Title == title);
            if (recipies.Count() == 0)
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.DeleteRecipeByTitle_Info_RecipeNotFound, nameof(DeleteRecipeByTitle), title) });

            _recipeRepository.Delete(await _recipeRepository.GetById(recipies.First().Id));
            await _unitOfWork.CommitAsync();
        }

        #region Private

        private List<RecipeResponseDTO> ListRecipeResponseDTO(IEnumerable<Recipe> recipies)
        {
            List<RecipeResponseDTO> listRecipeResponseDTO = new List<RecipeResponseDTO>();
            foreach (var recipe in recipies)
            {
                var recipeResponseDTO = RecipeResult(recipe);
                listRecipeResponseDTO.Add(recipeResponseDTO);
            }
            return listRecipeResponseDTO;
        }

        private RecipeResponseDTO RecipeResult(Recipe recipe)
        {
            var recipeResponseDTO = new RecipeResponseDTO()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Category = recipe.Category,
                NameCategoty = recipe.Category.GetDescriptionResources(),
                PreparationMode = recipe.PreparationMode,
                PreparationTime = recipe.PreparationTime,
                DataDraftBool = recipe.FileExtension.EmptyOrFilledText(),
                FileExtension = recipe.FileExtension,
                DataDraftCel = recipe.FileExtension == "Cel" ? Encoding.ASCII.GetString(recipe.DataDraft) : null                
            };
            return recipeResponseDTO;
        }

        private byte[] ConverteFilesToBytes(ICollection<IFormFile> files)
        {
            var lista = files.Select(formFile => BuildList(formFile));
            return lista.First().FirstOrDefault();
        }

        private List<byte[]> BuildList(IFormFile file)
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
