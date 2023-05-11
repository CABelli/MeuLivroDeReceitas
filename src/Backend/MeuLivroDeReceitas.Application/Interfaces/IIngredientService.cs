using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Ingredient;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDTO>> GetRecipies();

        Task AddIngredient(IngredientAddDto ingredientAddDto);

        Task<IngredientListDTO> GetIngredients(string title);
    }
}
