using MeuLivroDeReceitas.Domain.InterfacesGeneric;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeuLivroDeReceitas.Domain.EntityGeneric
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly ILogger _logger;

        public UnitOfWork(DbContext dbContext, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> CommitAsync()
        {
            var rows = await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"{nameof(CommitAsync)} - {rows} rows changes");
            return rows;
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }
    }
}
