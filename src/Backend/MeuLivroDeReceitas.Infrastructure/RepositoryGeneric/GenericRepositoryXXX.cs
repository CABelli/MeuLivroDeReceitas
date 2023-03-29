//using MeuLivroDeReceitas.Domain.EntityGeneric;
//using MeuLivroDeReceitas.Domain.InterfacesGeneric;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;

//namespace MeuLivroDeReceitas.Infrastructure.Repositories
//{
//    public class GenericRepository<T> : IGenericRepository<T> where T : GenericEntity
//    {
//        protected readonly DbContext _dbContext;
//        protected readonly DbSet<T> _dbSet;

//        public GenericRepository(DbContext dbContext)
//        {
//            _dbContext = dbContext;
//            _dbSet = dbContext.Set<T>();
//        }

//        public void Create(T entity)
//        {
//            _dbSet.Add(entity);
//        }

//        public void Create(List<T> entities)
//        {
//            _dbSet.AddRange(entities);
//        }

//        public void Update(T entity)
//        {
//            Console.WriteLine(entity.Id);
//            entity.SetAlterationDate(DateTime.Now);
//           // entity.SetAlterationUsuarioId(_documentSession.GetUserName() ?? "NaoInformado");
//            _dbSet.Update(entity);
//        }

//        public void Update(List<T> entities)
//        {
//            foreach (var entity in entities)
//            {
//                Console.WriteLine(entity.Id);
//                entity.SetAlterationDate(DateTime.Now);
//               // entity.SetAlterationUsuarioId(_documentSession.GetUserName() ?? "NaoInformado");
//            }

//            _dbSet.UpdateRange(entities);
//        }

//        public void UpdateRange(List<T> entities)
//        {
//            foreach (T entity in entities)
//            {
//                entity.SetAlterationDate(DateTime.Now);
//            }
//            _dbSet.UpdateRange(entities);
//        }

//        public void Delete(T entity)
//        {
//            _dbSet.Remove(entity);
//        }

//        public async Task<T> GetById(Guid id)
//        {
//            return await _dbSet.FirstAsync(d => d.Id == id);
//        }

//        public Task<List<T>> GetAll()
//        {
//            return _dbSet.AsQueryable().ToListAsync<T>();
//        }

//        public Task<List<T>> GetAllWhere(Expression<Func<T, bool>> expression)
//        {
//            return _dbSet.AsQueryable().ToListAsync<T>();
//        }

//        public async Task<T> GetByWhere(Expression<Func<T, bool>> expression)
//        {
//            return await _dbSet.FirstAsync(expression);
//        }
//    }
//}
