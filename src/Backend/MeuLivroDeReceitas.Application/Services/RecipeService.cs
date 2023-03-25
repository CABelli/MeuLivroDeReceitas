using MediatR;
using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Comunicacao.Dto.Request;
using MeuLivroDeReceitas.Comunicacao.Dto.Response;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Enum;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeService : IRecipeService
    {

        private IRecipeRepository _recipeRepository;

        public const string JA_EXISTE = "Haaaa    hummm     já existe";
        public const string SEM_ARQUIVO = "Haaaa    hummm     sem arquivo";

        //private readonly IMapper _mapper;
        //private readonly IUnitOfWork _unitOfWork;

        public RecipeService(IRecipeRepository recipeRepository
            //,
            //  IMapper mapper, 
            //  IUnitOfWork unitOfWork
            )
        {
            _recipeRepository = recipeRepository;
            // _mapper = mapper;
            // _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RecipeResponseDTO>> GetRecipies()
        {
            var recipies = await _recipeRepository.GetRecipies();
            var listRecipeResponseDTO = ListRecipeResponseDTO(recipies);
            return listRecipeResponseDTO;
        }

        public async Task<RecipeResponseDTO> GetById(Guid id)
        {
            var recipe = await _recipeRepository.GetId(id);
            if (recipe == null) return new RecipeResponseDTO();

            return RecipeResult(recipe);
        }

        public async Task<IEnumerable<RecipeResponseDTO>> GetRecipiesTitle(string title)
        {
            var recipies = await _recipeRepository.GetRecTitle(title);
            var listRecipeResponseDTO = ListRecipeResponseDTO(recipies);

            return listRecipeResponseDTO;
        }

        public async Task<IEnumerable<Comunicacao.Dto.Response.RecipeImageDraftDTO>> GetRecipiesDownLoad(string title)
        {
            if (title == null) return Enumerable.Empty<Comunicacao.Dto.Response.RecipeImageDraftDTO>();

            var recipies = await _recipeRepository.GetRecTitle(title);

            if (recipies == null || recipies.Count() == 0)
                return Enumerable.Empty<Comunicacao.Dto.Response.RecipeImageDraftDTO>();
                //return  Ok("  Vaaazio ");
                //ReturnTypeEncoder(ReturnTypeEncoder);
            

            List<Comunicacao.Dto.Response.RecipeImageDraftDTO> listRecipeImageDraftDTO = new List<Comunicacao.Dto.Response.RecipeImageDraftDTO>();

            Comunicacao.Dto.Response.RecipeImageDraftDTO recipeImageDraftDTO = new Comunicacao.Dto.Response.RecipeImageDraftDTO();

            foreach (var rec in recipies)
            {
                if (rec.FileExtension == null || String.IsNullOrEmpty(rec.FileExtension))
                {
                    //await Task.FromResult(HttpStatusCode.NoContent + SEM_ARQUIVO);
                    return listRecipeImageDraftDTO;
                }
                    //await Task.FromException<Exception>(new Exception(JA_EXISTE));
                //return Request.CreateResponse(HttpStatusCode.NoContent, rec);
                recipeImageDraftDTO.Title = rec.Title;
                recipeImageDraftDTO.NameFile = DateTime.Now.ToString("HH:mm:ss") + "_" + rec.Title + "." + rec.FileExtension;
                recipeImageDraftDTO.DataDraft = rec.DataDraft;
            }
            listRecipeImageDraftDTO.Add(recipeImageDraftDTO);
            return listRecipeImageDraftDTO;
        }

        public async Task Add(Comunicacao.Dto.Request.RecipeDTO recipeDTO)
        {

            await ValidarRecipeDTO(recipeDTO);

            var recipe = new Recipe()
            {
                Title = recipeDTO.Title,
                PreparationMode = recipeDTO.PreparationMode,
                PreparationTime = recipeDTO.PreparationTime,
                Category = recipeDTO.Category,
                FileExtension = recipeDTO.FileExtension
            };
            await _recipeRepository.Create(recipe);

            //if (recipeDTO == null) await Task.FromException<Exception>(new Exception("Recipe is null"));
            //if (recipeDTO.Title == null) await Task.FromException<Exception>(new Exception("Recipe Title is null"));

            //var recipies = await _recipeRepository.GetRecTitle(recipeDTO.Title);
            //if (recipies == null || recipies.Count() == 0)
            //{
            //    var recipe = new Recipe()
            //    {
            //        Title = recipeDTO.Title,
            //        PreparationMode = recipeDTO.PreparationMode,
            //        PreparationTime = recipeDTO.PreparationTime,
            //        Category = recipeDTO.Category,
            //        FileExtension = recipeDTO.FileExtension
            //    };
            //    await _recipeRepository.Create(recipe);
            //}
            //else
            //{
            //    await Task.FromException<Exception>(new Exception(JA_EXISTE));
            //}
        }

        private  async Task  ValidarRecipeDTO(RecipeDTO recipeDTO)
        {
            var validator = new RecipeValidator();
            var resultado = validator.Validate(recipeDTO);

            var recipies = await _recipeRepository.GetRecTitle(recipeDTO.Title);
            if (!recipies.Any() || recipies.Count() > 0)
            {
                //resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMensagensDeErro.EMAIL_JA_REGISTRADO));
                resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("Receita", JA_EXISTE));
            }

            if (!resultado.IsValid)
            {
                var mensagesDeErro = resultado.Errors.Select(c => c.ErrorMessage).ToList();
                throw new ErrosDeValidacaoException(mensagesDeErro);
            }

        }

        public async Task Update(RecipeStringDraftDTO dataDraft)
        {
            if (dataDraft.FileExtension == null && dataDraft.DataDraft != null) await Task.FromException<Exception>(new Exception("FileExtension is null"));
            if (dataDraft.FileExtension != null && dataDraft.DataDraft == null) await Task.FromException<Exception>(new Exception("DataDraft is null"));

            var recipe = await _recipeRepository.GetId(dataDraft.Id);

            recipe.FileExtension = dataDraft.FileExtension;
            recipe.DataDraft = Encoding.ASCII.GetBytes(dataDraft.DataDraft);

            await _recipeRepository.UpdateAsync(recipe);
        }

        public async Task Update(ICollection<IFormFile> files, RecipeImageDraftRequestDTO dataDraft)
        {
            var listByteFiles = ConverteFilesToBytes(files);

            //var recipeImageDraftDTO = new RecipeImageDraftRequestDTO
            //{
            //    Title = title,
            //    FileExtension = fileExtension,
            //    DataDraft = fileDrfat
            //};

            var fileDrfat = new byte[0];

            fileDrfat = listByteFiles.FirstOrDefault();


            if (dataDraft.FileExtension == null && fileDrfat != null) await Task.FromException<Exception>(new Exception("FileExtension is null"));
            if (dataDraft.FileExtension != null && fileDrfat == null) await Task.FromException<Exception>(new Exception("DataDraft is null"));

            var recipies = await GetRecipiesTitle(dataDraft.Title);

            if (recipies == null) await Task.FromException<Exception>(new Exception("Recipe is null"));

            var recipe = await _recipeRepository.GetId(recipies.First().Id);

            recipe.FileExtension = dataDraft.FileExtension;
            recipe.DataDraft = fileDrfat;   // dataDraft.DataDraft;

            await _recipeRepository.UpdateAsync(recipe);
        }

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
                NameCategoty = ((Category)recipe.Category).ToString(),
                PreparationMode = recipe.PreparationMode,
                PreparationTime = recipe.PreparationTime,
                DataDraft = recipe.DataDraft != null ? true : false,
                FileExtension = recipe.FileExtension
            };
            return recipeResponseDTO;
        }

        private List<byte[]> ConverteFilesToBytes(ICollection<IFormFile> files)
        {
            var fileDrfat = new byte[0];
            List<byte[]> lista = new();

            foreach (IFormFile fil in files)
            {
                if (fil.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fil.CopyTo(memoryStream);
                        fileDrfat = memoryStream.ToArray();
                        lista.Add(memoryStream.ToArray());
                    }
                }
                else
                    return lista;
            }
            return lista;
        }
    }
}
