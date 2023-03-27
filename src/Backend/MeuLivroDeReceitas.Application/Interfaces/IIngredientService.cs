using MeuLivroDeReceitas.CrossCutting.Dto.Request.Ingredient;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDTO>> GetRecipies();
    }
}
