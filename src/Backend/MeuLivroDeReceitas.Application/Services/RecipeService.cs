using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Comunicacao.Dto.Request;
using MeuLivroDeReceitas.Comunicacao.Dto.Response;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Interfaces;
using System.Text;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeService : IRecipeService
    {

        private IRecipeRepository _recipeRepository;

        public const string JA_EXISTE = "Haaaa    hummm     já existe";

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
            var recipiesEntity = await _recipeRepository.GetRecipies();

            List<RecipeResponseDTO> listRecipeResponseDTO = new List<RecipeResponseDTO>();

            foreach (var rec in recipiesEntity)
            {
                var recipeResponseDTO = new RecipeResponseDTO()
                {
                    Id = rec.Id,
                    Title = rec.Title,
                    Category = rec.Category,
                    PreparationMode = rec.PreparationMode,
                    PreparationTime = rec.PreparationTime,
                    DataDraft = rec.DataDraft != null ? true : false
                };
                listRecipeResponseDTO.Add(recipeResponseDTO);
            }

            return listRecipeResponseDTO;
        }

        public async Task<RecipeResponseDTO> GetById(Guid id)
        {
            var rec = await _recipeRepository.GetId(id);

            if (rec == null) return new RecipeResponseDTO();

            return new RecipeResponseDTO()
            {
                Title = rec.Title,
                Category = rec.Category,
                PreparationMode = rec.PreparationMode,
                PreparationTime = rec.PreparationTime,
                DataDraft = rec.DataDraft != null ? true : false
            };
        }

        public async Task<IEnumerable<RecipeResponseDTO>> GetRecipiesTitle(string title)
        {
            var recipiesEntity = await _recipeRepository.GetRecTitle(title);

            List<RecipeResponseDTO> listRecipeResponseDTO = new List<RecipeResponseDTO>();

            foreach (var rec in recipiesEntity)
            {
                var recipeDTO = new RecipeResponseDTO()
                {
                    Id = rec.Id,
                    Title = rec.Title,
                    Category = rec.Category,
                    PreparationMode = rec.PreparationMode,
                    PreparationTime = rec.PreparationTime,
                    DataDraft = rec.DataDraft != null ? true : false
                };
                listRecipeResponseDTO.Add(recipeDTO);
            }
            return listRecipeResponseDTO;
        }

        public async Task<IEnumerable<Comunicacao.Dto.Response.RecipeImageDraftDTO>> GetRecipiesDownLoad(string title)
        {
            if (title == null) return Enumerable.Empty<Comunicacao.Dto.Response.RecipeImageDraftDTO>();

            var recipies = await _recipeRepository.GetRecTitle(title);

            if(recipies == null || recipies.Count() == 0) return Enumerable.Empty<Comunicacao.Dto.Response.RecipeImageDraftDTO>();

            List<Comunicacao.Dto.Response.RecipeImageDraftDTO> listRecipeImageDraftDTO = new List<Comunicacao.Dto.Response.RecipeImageDraftDTO>();

            Comunicacao.Dto.Response.RecipeImageDraftDTO recipeImageDraftDTO = new Comunicacao.Dto.Response.RecipeImageDraftDTO();

            foreach (var rec in recipies)
            {                
                recipeImageDraftDTO.Title = rec.Title;
                recipeImageDraftDTO.NamyFile = DateTime.Now.ToString("HH:mm:ss") + "imagem.png";
                recipeImageDraftDTO.ListDataDraft.Add(rec.DataDraft);
            }
            listRecipeImageDraftDTO.Add(recipeImageDraftDTO);
            return listRecipeImageDraftDTO;
        }


        public async Task Add(Comunicacao.Dto.Request.RecipeDTO recipeDTO)
        {
            if(recipeDTO == null) await Task.FromException<Exception>(new Exception("Recipe is null"));
            if (recipeDTO.Title == null) await Task.FromException<Exception>(new Exception("Recipe is null"));

            var recipies = await _recipeRepository.GetRecTitle(recipeDTO.Title);
            if (recipies == null || recipies.Count() == 0)
            {
                var recipe = new Recipe()
                {
                    Title = recipeDTO.Title,
                    PreparationMode = recipeDTO.PreparationMode,
                    PreparationTime = recipeDTO.PreparationTime,
                    Category = recipeDTO.Category
                };
                await _recipeRepository.Create(recipe);
            }
            else
            {
                await Task.FromException<Exception>(new Exception(JA_EXISTE));
            }
        }

        public async Task Update(RecipeStringDraftDTO dataDraft)
        {
            var recipe = await _recipeRepository.GetId(dataDraft.Id);

            recipe.DataDraft = Encoding.ASCII.GetBytes(dataDraft.DataDraft);

            await _recipeRepository.UpdateAsync(recipe);
        }

        public async Task Update(RecipeImageDraftRequestDTO dataDraft)
        {
            var recipies = await GetRecipiesTitle(dataDraft.Title);

            if (recipies == null) await Task.FromException<Exception>(new Exception("Recipe is null"));

            var recipe = await _recipeRepository.GetId(recipies.First().Id);

            recipe.DataDraft = dataDraft.DataDraft;

            await _recipeRepository.UpdateAsync(recipe);
        }
    }
}
