using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeuLivroDeReceitas.Comunicacao.Dto.Request.Ingredient
{
    public class IngredientDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Sku is Required")]
        [MinLength(3)]
        [MaxLength(100)]
        [DisplayName("Sku")]
        public string? Sku { get; set; }
    }
}
