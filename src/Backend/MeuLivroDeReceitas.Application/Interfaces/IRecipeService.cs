using MeuLivroDeReceitas.Application.DTOs;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Enum;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeDTO>> GetRecipies();

        Task<RecipeDTO> GetById(Guid id);

        Task<IEnumerable<RecipeDTO>> GetRecipiesTitle(string title);

        Task Add(RecipeDTO recipeDTO);

        Task Update(RecipeDraftDTO dataDraft);

        Task Update(RecipeImageDraftDTO dataDraft);
    }
}
