using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using Microsoft.AspNetCore.Http;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeResponseDTO>> GetRecipies();

        Task<RecipeResponseDTO> GetRecipeById(Guid id);

        Task<IEnumerable<RecipeResponseDTO>> GetRecipiesTitle(string title);

        Task<IEnumerable<CrossCutting.Dto.Response.RecipeImageDraftDTO>> GetRecipiesDownLoad(string title);

        Task AddRecipe(CrossCutting.Dto.Request.RecipeDTO recipeDTO);

        Task UpdateRecipeDraftString(RecipeDTO recipeDTO);

        Task UpdateRecipeDraftImage(ICollection<IFormFile> files, string title, string fileExtension);
    }
}
