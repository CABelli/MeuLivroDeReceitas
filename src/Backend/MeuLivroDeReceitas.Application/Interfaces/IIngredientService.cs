using MeuLivroDeReceitas.Application.DTOs;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDTO>> GetRecipies();
    }
}
