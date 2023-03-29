using MeuLivroDeReceitas.Domain.EntityGeneric;
using MeuLivroDeReceitas.Domain.InterfacesGeneric;
using Microsoft.EntityFrameworkCore;

namespace Sparta.Dados.Repositories
{
    public class GenericRepositoryYYY //<T> : IGenericRepository<T> where T : GenericEntity
    {
        //protected readonly DbContext _dbContext;
        //protected readonly DbSet<T> _dbSet;

        //public GenericRepositoryYYY(DbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //    _dbSet = dbContext.Set<T>();
        //}

        //public void Create(T entity)
        //{
        //    _dbSet.Add(entity);
        //}

        //public void Create(List<T> entities)
        //{
        //    _dbSet.AddRange(entities);
        //}

        //public void Update(T entity)
        //{
        //    Console.WriteLine(entity.Id);
        //    entity.SetDataHoraAlteracao(DateTime.Now);
        //    entity.SetUsuarioAlteracao(_documentSession.GetUserName() ?? "NaoInformado");
        //    _dbSet.Update(entity);
        //}

        //public void Update(List<T> entities)
        //{
        //    foreach (var entity in entities)
        //    {
        //        Console.WriteLine(entity.Id);
        //        entity.SetDataHoraAlteracao(DateTime.Now);
        //        entity.SetUsuarioAlteracao(_documentSession.GetUserName() ?? "NaoInformado");
        //    }

        //    _dbSet.UpdateRange(entities);
        //}

        //public void UpdateRange(List<T> entities)
        //{
        //    foreach (T entity in entities)
        //    {
        //        entity.SetDataHoraAlteracao(DateTime.Now);
        //    }
        //    _dbSet.UpdateRange(entities);
        //}

        //public void Delete(T entity)
        //{
        //    _dbSet.Remove(entity);
        //}

        //public async Task<T> GetById(Guid id)
        //{
        //    return await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
        //}

        //public Task<List<T>> GetAll()
        //{
        //    return _dbSet.AsQueryable().ToListAsync<T>();
        //}

        //public Task<List<T>> WhereAsync(Expression<Func<T, bool>> expression)
        //{
        //    return _dbSet.Where(expression).ToListAsync();
        //}

        //public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        //{
        //    return await _dbSet.FirstOrDefaultAsync(expression);
        //}

        //public Task<T> GetByWhere(Expression<Func<T, bool>> expression)
        //{
        //}

    }
}
