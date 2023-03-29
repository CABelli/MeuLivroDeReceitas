using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Enum;
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

        public const string SEM_ARQUIVO = "Haaaa    hummm     sem arquivo";

        //private readonly IMapper _mapper;

        public RecipeService(IServiceProvider serviceProvider, IUnitOfWork unitOfWork
        //  IMapper mapper, 
            )
        {
            _unitOfWork = unitOfWork;
            _logger = serviceProvider.GetRequiredService<ILogger<RecipeService>>();
            _recipeRepository = serviceProvider.GetRequiredService<IRecipeRepository>();
            // _mapper = mapper;
        }

        public async Task<IEnumerable<RecipeResponseDTO>> GetRecipies()
        {
            var recipies = await _recipeRepository.GetAll();
            if (recipies == null)
            {
                _logger.LogInformation(Resource.GetRecipies_Info_RecipeNotFound, nameof(GetRecipies));
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipies_Info_RecipeNotFound, nameof(GetRecipies)) });
            }
            var listRecipeResponseDTO = ListRecipeResponseDTO(recipies);
            return listRecipeResponseDTO;
        }

        public async Task<RecipeResponseDTO> GetRecipeById(Guid id)
        {
            var recipe = await _recipeRepository.GetById(id);
            if (recipe == null)
            {
                _logger.LogInformation(Resource.GetRecipeById_Info_RecipeNotFound, nameof(GetRecipeById), id);
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipeById_Info_RecipeNotFound, nameof(GetRecipeById), id) });
            }

            return RecipeResult(recipe);
        }

        public async Task<IEnumerable<RecipeResponseDTO>> GetRecipiesTitle(string title)
        {
            var recipies = await _recipeRepository.WhereAsync(x => x.Title == title);

            if (recipies.Count() == 0)
            {
                _logger.LogInformation(Resource.GetRecipiesTitle_Info_RecipeNotFound, nameof(GetRecipiesTitle), title);
                throw new ErrorsNotFoundException(new List<string>() { string.Format(Resource.GetRecipiesTitle_Info_RecipeNotFound, nameof(GetRecipiesTitle), title) });                
            }

            var listRecipeResponseDTO = ListRecipeResponseDTO(recipies);

            return listRecipeResponseDTO;
        }

        public async Task<IEnumerable<CrossCutting.Dto.Response.RecipeImageDraftDTO>> GetRecipiesDownLoad(string title)
        {
            if (title == null) return Enumerable.Empty<CrossCutting.Dto.Response.RecipeImageDraftDTO>();

            var recipies = await _recipeRepository.WhereAsync(x => x.Title == title);

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

        public async Task AddRecipe(CrossCutting.Dto.Request.RecipeDTO recipeDTO)
        {
            _logger.LogInformation(Resource.AddRecipe_Info_Starting, nameof(AddRecipe), recipeDTO.Title);

            //   Rotina para forçar um erro de programa/sistema
            //   int a = 0, b = 0;
            //   a = a / b;

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

        private  async Task  ValidarRecipeDTO(RecipeDTO recipeDTO)
        {
            var recipie = await _recipeRepository.WhereAsync(x => x.Title == recipeDTO.Title);
            if (recipie.Count() > 0)
            {
                throw new ErrosDeValidacaoException(new List<string>() { Resource.ValidarRecipeDTO_Info_RecipeAlreadyExists });
            }

            var validator = new RecipeValidator(1);
            var resultado = validator.Validate(recipeDTO);

            var recipies = await _recipeRepository.WhereAsync(x => x.Title == recipeDTO.Title);
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

        public async Task Update(RecipeDTO recipeDTO)
        {
            var recipe =  await ValidateRecipeModification(recipeDTO);

            recipe.PreparationTime = recipeDTO.PreparationTime == 0 ? recipe.PreparationTime : recipeDTO.PreparationTime;
            recipe.PreparationMode = recipeDTO.PreparationMode == null ? recipe.PreparationMode : recipeDTO.PreparationMode;
            recipe.Category = recipeDTO.Category == 0 ? recipe.Category : recipeDTO.Category;
            recipe.FileExtension = recipeDTO.FileExtension;
            recipe.DataDraft = Encoding.ASCII.GetBytes(recipeDTO.DataDraft);           

            _recipeRepository.Update(recipe);
            await _unitOfWork.CommitAsync();
        }

        private async Task<Recipe> ValidateRecipeModification(RecipeDTO recipeDTO)
        {
            var validator = new RecipeValidator(2);
            var resultado = validator.Validate(recipeDTO);

            //if (recipeDTO.FileExtension == null && recipeDTO.DataDraft != null) await Task.FromException<Exception>(new Exception("FileExtension is null"));
            //if (recipeDTO.FileExtension != null && recipeDTO.DataDraft == null) await Task.FromException<Exception>(new Exception("DataDraft is null"));

            var recipie = await _recipeRepository.WhereAsync(x => x.Title == recipeDTO.Title);
            if (recipie.Count() == 0)
            {
               // throw new ErrosDeValidacaoException(new List<string>() { Resource.ValidateRecipeModification_Info_RecipeNotFound });
            }

            if (!resultado.IsValid)
            {
                var mensagesDeErro = resultado.Errors.Select(c => c.ErrorMessage).ToList();
                throw new ErrosDeValidacaoException(mensagesDeErro);
            }

            return recipie.FirstOrDefault();
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

            var recipe = await _recipeRepository.GetById(recipies.First().Id);

            recipe.FileExtension = dataDraft.FileExtension;
            recipe.DataDraft = fileDrfat; 

            _recipeRepository.Update(recipe);
            await _unitOfWork.CommitAsync();
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
