using MeuLivroDeReceitas.Domain.EntityGeneric;

namespace MeuLivroDeReceitas.Domain.Entities
{
    public class Ingredient : GenericEntity
    {
        public string? Sku { get; set; }

        public string? Quantity { get; set; }

        //public string UnitMeasureSku { get; set; }

        //public int NewQuantity { get; set; }

        public Guid? RecipeId { get; set; }

        public Recipe? Recipe { get; set; }
    }
}