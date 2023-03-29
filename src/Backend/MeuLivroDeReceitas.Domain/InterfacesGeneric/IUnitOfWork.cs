using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Domain.InterfacesGeneric
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
        Task Rollback();
    }
}