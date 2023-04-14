using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.Domain.EntityGeneric;

namespace MeuLivroDeReceitas.Domain.Entities
{
    public sealed class Recipe : GenericEntity
    {
        public string? Title { get; set; }

        public Category Category { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTime { get; set; }

        public string? FileExtension { get; set; }

        public byte[]? DataDraft { get; set; }

        public ICollection<Ingredient>? Ingredients { get; set; }
    }
}