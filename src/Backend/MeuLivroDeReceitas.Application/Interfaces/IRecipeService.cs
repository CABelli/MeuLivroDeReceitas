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

        Task AddRecipe(AddRecipeDTO addRecipeDTO);

        Task UpdateRecipe(ModifyRecipeDTO modifyRecipeDTO);

        Task<string> UpdateRecipeDraftImage(ICollection<IFormFile> files, string title);

        Task DeleteRecipeByTitle(string title);

        Task<string> GenerateLogAudit(string title);
    }
}
