using MeuLivroDeReceitas.CrossCutting.Dto.Recipess;
using Microsoft.AspNetCore.Http;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeResponseDTO>> GetRecipies();

        Task<RecipeResponseDTO> GetRecipeById(Guid id);

        Task<RecipeResponseDTO> GetRecipiesTitle(string title);

        Task<RecipeResponseImageDraftDTO> GetRecipiesDownLoad(string title);

        Task AddRecipe(RecipeDTO recipeDTO);

        Task UpdateRecipeDraftString(RecipeDTO recipeDTO);

        Task<string> UpdateRecipeDraftImage(ICollection<IFormFile> files, string title);

        Task DeleteRecipeByTitle(string title);

        Task GenerateLogAudit(string title);
    }
}
