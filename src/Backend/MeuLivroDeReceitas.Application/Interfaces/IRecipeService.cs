using MeuLivroDeReceitas.Comunicacao.Dto.Request;
using MeuLivroDeReceitas.Comunicacao.Dto.Response;
using Microsoft.AspNetCore.Http;

namespace MeuLivroDeReceitas.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeResponseDTO>> GetRecipies();

        Task<RecipeResponseDTO> GetById(Guid id);

        Task<IEnumerable<RecipeResponseDTO>> GetRecipiesTitle(string title);

        Task<IEnumerable<Comunicacao.Dto.Response.RecipeImageDraftDTO>> GetRecipiesDownLoad(string title);

        Task Add(Comunicacao.Dto.Request.RecipeDTO recipeDTO);

        Task Update(RecipeStringDraftDTO dataDraft);

        Task Update(ICollection<IFormFile> files, RecipeImageDraftRequestDTO dataDraft);
    }
}
