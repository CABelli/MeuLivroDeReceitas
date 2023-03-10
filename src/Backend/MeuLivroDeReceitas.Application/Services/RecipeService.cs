using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Comunicacao.Dto.Request;
using MeuLivroDeReceitas.Comunicacao.Dto.Response;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Enum;
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

            if(recipies == null || recipies.Count() == 0) return Enumerable.Empty<Comunicacao.Dto.Response.RecipeImageDraftDTO>();

            List<Comunicacao.Dto.Response.RecipeImageDraftDTO> listRecipeImageDraftDTO = new List<Comunicacao.Dto.Response.RecipeImageDraftDTO>();

            Comunicacao.Dto.Response.RecipeImageDraftDTO recipeImageDraftDTO = new Comunicacao.Dto.Response.RecipeImageDraftDTO();

            foreach (var rec in recipies)
            {

                //byte[] img;
                //using (var str = new FileStream("", FileMode.Open, FileAccess.Read))
                //{
                //    using (var rd = new BinaryReader(str))
                //    {
                //        img = rd.ReadBytes((int)str.Length);
                //    }
                //}

                recipeImageDraftDTO.Title = rec.Title;
                recipeImageDraftDTO.NameFile = DateTime.Now.ToString("HH:mm:ss") + "_" + rec.Title + "." + rec.FileExtension;
                recipeImageDraftDTO.DataDraft = rec.DataDraft;
                //recipeImageDraftDTO.ListDataDraft.Add(rec.DataDraft);

            }
            listRecipeImageDraftDTO.Add(recipeImageDraftDTO);
            return listRecipeImageDraftDTO;
        }


        public async Task Add(Comunicacao.Dto.Request.RecipeDTO recipeDTO)
        {
            if (recipeDTO == null) await Task.FromException<Exception>(new Exception("Recipe is null"));
            if (recipeDTO.Title == null) await Task.FromException<Exception>(new Exception("Recipe is null"));

            var recipies = await _recipeRepository.GetRecTitle(recipeDTO.Title);
            if (recipies == null || recipies.Count() == 0)
            {
                var recipe = new Recipe()
                {
                    Title = recipeDTO.Title,
                    PreparationMode = recipeDTO.PreparationMode,
                    PreparationTime = recipeDTO.PreparationTime,
                    Category = recipeDTO.Category,
                    FileExtension = recipeDTO.FileExtension
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
            if (dataDraft.FileExtension == null && dataDraft.DataDraft != null) await Task.FromException<Exception>(new Exception("FileExtension is null"));
            if (dataDraft.FileExtension != null && dataDraft.DataDraft == null) await Task.FromException<Exception>(new Exception("DataDraft is null"));

            var recipe = await _recipeRepository.GetId(dataDraft.Id);

            recipe.FileExtension = dataDraft.FileExtension;
            recipe.DataDraft = Encoding.ASCII.GetBytes(dataDraft.DataDraft);

            await _recipeRepository.UpdateAsync(recipe);
        }

        public async Task Update(RecipeImageDraftRequestDTO dataDraft)
        {
            if (dataDraft.FileExtension == null && dataDraft.DataDraft != null) await Task.FromException<Exception>(new Exception("FileExtension is null"));
            if (dataDraft.FileExtension != null && dataDraft.DataDraft == null) await Task.FromException<Exception>(new Exception("DataDraft is null"));

            var recipies = await GetRecipiesTitle(dataDraft.Title);

            if (recipies == null) await Task.FromException<Exception>(new Exception("Recipe is null"));

            var recipe = await _recipeRepository.GetId(recipies.First().Id);

            recipe.FileExtension = dataDraft.FileExtension;
            recipe.DataDraft = dataDraft.DataDraft;

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
    }
}
