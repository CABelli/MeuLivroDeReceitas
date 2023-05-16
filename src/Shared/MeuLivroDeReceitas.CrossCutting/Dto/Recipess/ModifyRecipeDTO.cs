using MeuLivroDeReceitas.CrossCutting.EnumClass;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Recipess
{
    public class ModifyRecipeDTO
    {
        public string Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTime { get; set; }

        public Category Category { get; set; }

        public string? FileExtension { get; set; }
    }
}
