using MediatR;
using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Enum;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;
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

        public async Task<IEnumerable<CrossCutting.Dto.Response.RecipeImageDraftDTO>> GetRecipiesDownLoad(string title)
        {
            if (title == null) return Enumerable.Empty<CrossCutting.Dto.Response.RecipeImageDraftDTO>();

            var recipies = await _recipeRepository.GetRecTitle(title);

            if (recipies == null || recipies.Count() == 0)
                return Enumerable.Empty<CrossCutting.Dto.Response.RecipeImageDraftDTO>();
                //return  Ok("  Vaaazio ");
                //ReturnTypeEncoder(ReturnTypeEncoder);
            

            List<CrossCutting.Dto.Response.RecipeImageDraftDTO> listRecipeImageDraftDTO = new List<CrossCutting.Dto.Response.RecipeImageDraftDTO>();

            CrossCutting.Dto.Response.RecipeImageDraftDTO recipeImageDraftDTO = new CrossCutting.Dto.Response.RecipeImageDraftDTO();

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

        public async Task Add(CrossCutting.Dto.Request.RecipeDTO recipeDTO)
        {
            //int a = 0; int b = 0;
            //a = a / b;

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

            await _recipeRepository.Create(recipe);
        }

        private  async Task  ValidarRecipeDTO(RecipeDTO recipeDTO)
        {
            var recipie = await _recipeRepository.GetRecTitle(recipeDTO.Title);
            if (recipie.Count() > 0)
            {
                throw new ErrosDeValidacaoException(new List<string>() { Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists });
            }

            var validator = new RecipeValidator();
            var resultado = validator.Validate(recipeDTO);

            var recipies = await _recipeRepository.GetRecTitle(recipeDTO.Title);
            if (recipies.Count() > 0)
            {
                resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("Receita", Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists));
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
