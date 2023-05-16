using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.EnumClass
{
    public enum MethodRecipeValidator
    {
        [Description("AddRecipe")]
        AddRecipe = 1,
        [Description("ModifyRecipe")]
        ModifyRecipe = 2
    }
}
