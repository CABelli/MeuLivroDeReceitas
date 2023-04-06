using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.InterfacesRepository;
using MeuLivroDeReceitas.Infrastructure.RepositoryGeneric;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.Repositories
{
    public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository    
    {
        //public IngredientRepository(DbContext dbContext) : base(dbContext)
        //{
        //}

       // ApplicationDbContext _ingredientContext;

        public IngredientRepository(DbContext dbContext) : base(dbContext)
        //(ApplicationDbContext context) //: base(context)
        {
         //   _ingredientContext = context;
        }
    }
}
