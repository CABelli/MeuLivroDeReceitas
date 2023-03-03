using MeuLivroDeReceitas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuLivroDeReceitas.Infrastructure.EntitiesConfigurationMap
{
    public class RecipeConfigurMap : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(p => p.Title).HasMaxLength(30).IsRequired();
            builder.Property(p => p.Category).IsRequired();
            builder.Property(p => p.PreparationMode).HasMaxLength(500).IsRequired();
            builder.Property(p => p.PreparationTime).IsRequired();

            builder.Property(p => p.DataDraft).IsRequired(false);
        }
    }
}
