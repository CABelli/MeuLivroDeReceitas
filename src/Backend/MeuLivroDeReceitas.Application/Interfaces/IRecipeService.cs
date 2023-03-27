using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using Microsoft.AspNetCore.Http;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeResponseDTO>> GetRecipies();

        Task<RecipeResponseDTO> GetById(Guid id);

        Task<IEnumerable<RecipeResponseDTO>> GetRecipiesTitle(string title);

        Task<IEnumerable<CrossCutting.Dto.Response.RecipeImageDraftDTO>> GetRecipiesDownLoad(string title);

        Task Add(CrossCutting.Dto.Request.RecipeDTO recipeDTO);

        Task Update(RecipeStringDraftDTO dataDraft);

        Task Update(ICollection<IFormFile> files, RecipeImageDraftRequestDTO dataDraft);
    }
}
