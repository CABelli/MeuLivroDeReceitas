using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.EnumClass
{
    public enum EMethodRecipeValidator
    {
        [Description("AddRecipe")]
        AddRecipe = 1,
        [Description("ModifyRecipe")]
        ModifyRecipe = 2
    }
}
