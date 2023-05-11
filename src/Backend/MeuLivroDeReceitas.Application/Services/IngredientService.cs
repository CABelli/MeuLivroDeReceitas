using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Domain.InterfacesGeneric;
using MeuLivroDeReceitas.Domain.InterfacesRepository;
using MeuLivroDeReceitas.Exceptions.ExceptionBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MeuLivroDeReceitas.Application.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ILogger<IngredientService> _logger;
        private IUnitOfWork _unitOfWork;
        private IIngredientRepository _ingredientRepository;
        private IRecipeService _recipeService;

        private IAuthenticate _authenticateService;

        public IngredientService(IServiceProvider serviceProvider,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = serviceProvider.GetRequiredService<ILogger<IngredientService>>();
            _ingredientRepository = serviceProvider.GetRequiredService<IIngredientRepository>();
            _recipeService = serviceProvider.GetRequiredService<IRecipeService>();
            _authenticateService = serviceProvider.GetRequiredService<IAuthenticate>();
        }

        public async Task<IEnumerable<IngredientDTO>> GetRecipies()
        {
            return new List<IngredientDTO>();
        }

        public async Task AddIngredient(IngredientAddDto ingredientAddDTO)
        {
            var recipe = await _recipeService.GetRecipiesTitle(ingredientAddDTO.Title);

            ////  AaAaAa
            
            ////int recipeId = recipe.Id.GetHashCode();
            if (recipe != null)
            {
                var ingredient = new Ingredient()
                {
                    Sku = ingredientAddDTO.Sku,
                    Quantity = ingredientAddDTO.Quantity,
                    RecipeId = recipe.Id
                };
                _ingredientRepository.Create(ingredient);

                await _unitOfWork.CommitAsync();
            }

        }

        public async Task<IngredientListDTO> GetIngredients(string title)
        {
            var recipe = await _recipeService.GetRecipiesTitle(title);

            var ingredients = await _ingredientRepository.WhereAsync(x => x.RecipeId == recipe.Id);

            var ingredientList = new IngredientListDTO()
            {
                Title = title
            };

            foreach ( var ingr in ingredients )
            {
                var ingredientListDetails = new IngredientListDetailsDTO()
                {
                    Sku = ingr.Sku,
                    Quantity = ingr.Quantity
                };

                ingredientList.Items.Add(ingredientListDetails);
            }

            return ingredientList;
        }
    }
}
