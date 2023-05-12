using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IIngredientService
    {
        Task AddIngredient(IngredientAddDto ingredientAddDto);

        Task<IngredientListDTO> GetIngredients(string title);
    }
}
