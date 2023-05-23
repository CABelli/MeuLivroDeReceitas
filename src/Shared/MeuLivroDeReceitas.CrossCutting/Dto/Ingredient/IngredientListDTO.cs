namespace MeuLivroDeReceitas.CrossCutting.Dto.Ingredient
{
    public class IngredientListDTO
    {
        public string TitleRecipe { get; set; }

        public List<IngredientListDetailsDTO> RecipeItems { get; set; } = new List<IngredientListDetailsDTO>();
    }
}
