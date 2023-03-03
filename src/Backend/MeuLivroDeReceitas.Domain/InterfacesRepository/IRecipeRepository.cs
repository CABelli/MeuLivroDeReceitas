using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Domain.Interfaces
{
    public interface IRecipeRepository // : IGenericRepository<Recipe>
    {
        Task<IEnumerable<Recipe>> GetRecipies();

        Task<Recipe> Create(Recipe recipe);

        Task<Recipe> UpdateAsync(Recipe recipe);

        Task<Recipe> GetId(Guid id);

        Task<IEnumerable<Recipe>> GetRecTitle(string title);
    }
}
