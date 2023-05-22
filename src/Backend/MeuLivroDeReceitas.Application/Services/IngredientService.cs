using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Dto.Recipess;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.InterfacesGeneric;
using MeuLivroDeReceitas.Domain.InterfacesRepository;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
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

        public async Task AddIngredient(IngredientAddDto ingredientAddDTO)
        {
            await _recipeService.GenerateLogAudit(nameof(GetIngredients) + " , key: " + ingredientAddDTO.Title);

            var recipe = await ValidateIngredient(ingredientAddDTO);

            var ingredient = new Ingredient()
            {
                Sku = ingredientAddDTO.Sku,
                Quantity = ingredientAddDTO.Quantity,
                RecipeId = recipe.Id
            };

            _ingredientRepository.Create(ingredient);

            await _unitOfWork.CommitAsync();           
        }

        public async Task<IngredientListDTO> GetIngredients(string title)
        {
            await _recipeService.GenerateLogAudit(nameof(GetIngredients) + " , key: " + title);

            var recipe = await _recipeService.GetRecipiesTitle(title);

            var ingredients = await _ingredientRepository.WhereAsync(x => x.RecipeId == recipe.Id);

            var ingredientList = new IngredientListDTO()
            {
                Title = title
            };

            ingredients.ForEach(ingredList => ingredientList.RecipeItems.Add(
                new IngredientListDetailsDTO()
                {
                    Sku = ingredList.Sku,
                    Quantity = ingredList.Quantity
                }));

            //foreach ( var ingr in ingredients )
            //{
            //    var ingredientListDetails = new IngredientListDetailsDTO()
            //    {
            //        Sku = ingr.Sku,
            //        Quantity = ingr.Quantity
            //    };
            //
            //    ingredientList.RecipeItems.Add(ingredientListDetails);
            //}

            return ingredientList;
        }

        private async Task<RecipeResponseDTO> ValidateIngredient(IngredientAddDto ingredientAddDTO)
        {
            var validator = new IngredientValidator(EMethodIngredientValidator.AddIngredient);
            var resultado = validator.Validate(ingredientAddDTO);
            if (!resultado.IsValid)
                throw new ErrosDeValidacaoException(resultado.Errors.Select(c => c.ErrorMessage).ToList());
            
            return await _recipeService.GetRecipiesTitle(ingredientAddDTO.Title);
        }
    }
}
