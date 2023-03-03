using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.InterfacesRepository;
using MeuLivroDeReceitas.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.Repositories
{
    public class IngredientRepository : IIngredientRepository  // : GenericRepository<Ingredient>, IIngredientRepository
    {
        //public IngredientRepository(DbContext dbContext) : base(dbContext)
        //{
        //}

        ApplicationDbContext _ingredientContext;

        public IngredientRepository(ApplicationDbContext context) //: base(context)
        {
            _ingredientContext = context;
        }
    }
}
