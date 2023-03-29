using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.InterfacesGeneric;

namespace MeuLivroDeReceitas.Domain.Interfaces
{
    public interface IRecipeRepository : IGenericRepository<Recipe>
    {
    }
}
