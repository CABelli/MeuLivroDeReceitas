﻿using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.EnumClass
{
    public enum EMethodIngredientValidator
    {
        [Description("AddIngredient")]
        AddIngredient = 1,
        [Description("IngredientChange")]
        IngredientChange = 2,
        [Description("DeleteIngredient")]
        DeleteIngredient = 3
    }
}
