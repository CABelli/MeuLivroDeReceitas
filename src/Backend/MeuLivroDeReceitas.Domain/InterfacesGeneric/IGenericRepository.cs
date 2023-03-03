using MeuLivroDeReceitas.Domain.EntityGeneric;
using System.Linq.Expressions;

namespace MeuLivroDeReceitas.Domain.InterfacesGeneric
{
    public interface IGenericRepository<T> where T : GenericEntity
    {
        void Create(T entity);

        void Create(List<T> entities);

        void Update(T entity);

        void Update(List<T> entities);

        void UpdateRange(List<T> entities);

        void Delete(T entity);

        Task<List<T>> GetAll();

        Task<T> GetById(Guid id);

        Task<T> GetByTitle(string title);

        Task<List<T>> GetAllWhere(Expression<Func<T, bool>> expression);

        Task<T> GetByWhere(Expression<Func<T, bool>> expression);
    }
}