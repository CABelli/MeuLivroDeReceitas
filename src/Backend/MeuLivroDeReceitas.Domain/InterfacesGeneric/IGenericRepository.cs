using MeuLivroDeReceitas.Domain.EntityGeneric;
using System.Linq.Expressions;

namespace MeuLivroDeReceitas.Domain.InterfacesGeneric
{
    public interface IGenericRepository<T> where T : GenericEntity
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<List<T>> GetAll();

        Task<List<T>> WhereAsync(Expression<Func<T, bool>> expression);

        Task<T> WhereFirstAsync(Expression<Func<T, bool>> expression);

        Task<T> GetById(Guid id);

        //    void Create(List<T> entities);

        //    void Update(List<T> entities);

        //    void UpdateRange(List<T> entities);

        //    void Delete(T entity);

        //    Task<T> GetByTitle(string title);

        //    Task<List<T>> GetAllWhere(Expression<Func<T, bool>> expression);

        //     Task<T> GetByWhere(Expression<Func<T, bool>> expression);
    }
}