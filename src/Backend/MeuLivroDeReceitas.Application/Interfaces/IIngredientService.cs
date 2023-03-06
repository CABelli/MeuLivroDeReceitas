using MeuLivroDeReceitas.Comunicacao.Dto.Request.Ingredient;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDTO>> GetRecipies();
    }
}
