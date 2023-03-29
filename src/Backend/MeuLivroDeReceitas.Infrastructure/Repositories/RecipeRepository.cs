using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Infrastructure.RepositoryGeneric;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.Repositories
{
    public class RecipeRepository //: IRecipeRepository 
        : GenericRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
