using MeuLivroDeReceitas.Application.DTOs;
using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Interfaces;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
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

        public async Task<IEnumerable<RecipeDTO>> GetRecipies()
        {
            var recipiesEntity = await _recipeRepository.GetRecipies();

            List<RecipeDTO> recipiesDTO = new List<RecipeDTO>();

            foreach (var rec in recipiesEntity)
            {
                if (rec.DataDraft != null)
                {
                    var t1 = Encoding.ASCII.GetString(rec.DataDraft);
                    //var t2 = System.Text.Json.JsonSerializer.Deserialize<string>(t1);
                }

                var recipeDTO = new RecipeDTO()
                {
                    Title = rec.Title,
                    Category = rec.Category,
                    PreparationMode = rec.PreparationMode,
                    PreparationTime = rec.PreparationTime,
                    DataDraft = rec.DataDraft != null ? Encoding.ASCII.GetString(rec.DataDraft) : null
                };

                recipiesDTO.Add(recipeDTO);
            }

            return recipiesDTO;
        }

        public async Task<RecipeDTO> GetById(Guid id)
        {
            var rec = await _recipeRepository.GetId(id);
            return new RecipeDTO()
            {
                Title = rec.Title,
                Category = rec.Category,
                PreparationMode = rec.PreparationMode,
                PreparationTime = rec.PreparationTime,
                DataDraft = rec.DataDraft != null ? Encoding.ASCII.GetString(rec.DataDraft) : null
            };
        }

        public async Task<IEnumerable<RecipeDTO>> GetRecipiesTitle(string title)
        {
            var recipiesEntity = await _recipeRepository.GetRecTitle(title);

            List<RecipeDTO> recipiesDTO = new List<RecipeDTO>();

            foreach (var rec in recipiesEntity)
            {
                var recipeDTO = new RecipeDTO()
                {
                    Title = rec.Title,
                    Category = rec.Category,
                    PreparationMode = rec.PreparationMode,
                    PreparationTime = rec.PreparationTime,
                    DataDraft = rec.DataDraft != null ? Encoding.ASCII.GetString(rec.DataDraft) : null
                };
                recipiesDTO.Add(recipeDTO);
            }
            return recipiesDTO;
        }

        public async Task Add(RecipeDTO recipeDTO)
        {
            var recipiesTitle = await _recipeRepository.GetRecTitle(recipeDTO.Title);
            if (recipiesTitle == null)
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

        public async Task Update(RecipeDraftDTO dataDraft)
        {
            //var recipe = new Recipe()
            //{
            //    Title = recipeDTO.Title,
            //    DataDraft = recipeDTO.DataDraft
            //};


            //var recipe = await _recipeRepository.GetTitle(dataDraft.Title);
            // var recipe = await _recipeRepository.GetByWhere(dataDraft.Title);

            var recipe = await _recipeRepository.GetId(dataDraft.Id);

            recipe.DataDraft = Encoding.ASCII.GetBytes(dataDraft.DataDraft);

            await _recipeRepository.UpdateAsync(recipe);

            //foreach (IFormFile obj in recipeDraftDto.DataDraft)
            //{

            /// //Imagem img = new Imagem();
            /////img.ImagemTitulo = file.FileName;

            /////recipe.DataDraft = recipeDraftDto.DataDraft;

            //MemoryStream ms = new MemoryStream();

            //obj.CopyTo(ms);

            //recipe.DataDraft = ms.ToArray();

            //ms.Close();
            //ms.Dispose();

            /////_recipeContext.Recipies.Add(recipe);
            /////_recipeContext.SaveChanges();

            //await _recipeRepository.UpdateAsync(recipe);

            //await _recipeRepository.SaveChangesAsync();
            //}



            //await _recipeRepository.UpdateAsync(recipe);
        }

        public async Task Update(RecipeImageDraftDTO dataDraft)
        {
            var recipe = await _recipeRepository.GetId(dataDraft.Id);

            recipe.DataDraft = dataDraft.DataDraft;

            await _recipeRepository.UpdateAsync(recipe);
        }
    }
}
