using MeuLivroDeReceitas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuLivroDeReceitas.Infrastructure.EntitiesConfigurationMap
{
    public class IngredientConfigurMap : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(p => p.Sku).HasMaxLength(30).IsRequired();
            builder.Property(p => p.Quantity).HasMaxLength(30).IsRequired();

            builder.Property(p => p.RecipeId).IsRequired();

            builder.HasOne(e => e.Recipe).WithMany(e => e.Ingredients).HasForeignKey(e => e.RecipeId);

        }
    }
}
