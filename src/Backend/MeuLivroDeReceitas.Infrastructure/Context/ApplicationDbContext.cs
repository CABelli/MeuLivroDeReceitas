using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.EntityGeneric;
using MeuLivroDeReceitas.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Recipe> Recipies { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfiguration(new RecipeConfigurMap());
            //builder.ApplyConfiguration(new IngredientConfigurMap());

            //foreach (var pb in builder.Model.GetEntityTypes()
            //    .SelectMany(t => t.GetProperties())
            //    .Where(p => p.ClrType == typeof(string))
            //    .Select(p => builder.Entity(p.DeclaringEntityType.ClrType).Property(p.Name)))
            //{
            //    pb.HasColumnType("varchar");
            //    pb.HasMaxLength(100);
            //}

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            //  configurationBuilder.Properties<decimal>().HavePrecision(18, 4);

            // configurationBuilder.Properties<string>().HaveMaxLength(100);  esse cara altera os campos  da entity
        }

        private void SetAuditForEntityGeneric()
        {
            var CreationDate = nameof(GenericEntity.CreationDate);
            var CreationUsuarioId = nameof(GenericEntity.CreationUsuarioId);
            var AlterationDate = nameof(GenericEntity.AlterationDate);
            var AlterationUsuarioId = nameof(GenericEntity.AlterationUsuarioId);

            foreach (var entry in base.ChangeTracker.Entries().Where(entry => entry.Entity is GenericEntity))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(CreationDate).CurrentValue = DateTime.Now;
                    if (entry.Property(CreationUsuarioId).CurrentValue is null)
                    {
                        //entry.Property(CreationUsuarioId).CurrentValue = _documentSession.GetUserName() ?? string.Empty;
                    }

                    entry.Property(AlterationDate).IsModified = false;
                    entry.Property(AlterationUsuarioId).IsModified = false;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(CreationDate).IsModified = false;
                    entry.Property(CreationUsuarioId).IsModified = false;

                    entry.Property(AlterationDate).CurrentValue = DateTime.Now;
                    if (entry.Property(AlterationUsuarioId).CurrentValue is null)
                    {
                        // entry.Property(usuarioAlteracao).CurrentValue = _documentSession.GetUserName() ?? string.Empty;
                    }
                }
            }
        }
    }
}
