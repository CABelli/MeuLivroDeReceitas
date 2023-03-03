using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MeuLivroDeReceitas.Domain.Enum;

namespace MeuLivroDeReceitas.Application.DTOs
{
    public class RecipeDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is Required")]
        [MinLength(3)]
        [MaxLength(100)]
        [DisplayName("Title")]
        public string? Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTime { get; set; }

        public string? DataDraft { get; set; }

        public Category Category { get; set; }
    }
}
