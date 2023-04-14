using MeuLivroDeReceitas.CrossCutting.Resources.CrossCutting;
using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.EnumClass
{
    public enum Category
    {
        [Description(nameof(Resource.Category_Enum_Breakfast))]
        Breakfast = 0,
        [Description(nameof(Resource.Category_Enum_Lunch))]
        Lunch = 1,
        [Description(nameof(Resource.Category_Enum_Dessert))]
        Dessert = 2,
        [Description(nameof(Resource.Category_Enum_Dinner))]
        Dinner = 3
    }
}
