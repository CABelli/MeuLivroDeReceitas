using MeuLivroDeReceitas.CrossCutting.Resources.CrossCutting;

namespace MeuLivroDeReceitas.CrossCutting.EnumClass
{
    public enum ECategory
    {
        [LocalizedEnum("Category_Enum_Breakfast", ResourceType = typeof(Resource))]
        Breakfast = 0,
        [LocalizedEnum("Category_Enum_Lunch", ResourceType = typeof(Resource))]
        Lunch = 1,
        [LocalizedEnum("Category_Enum_Dessert", ResourceType = typeof(Resource))]
        Dessert = 2,
        [LocalizedEnum("Category_Enum_Dinner", ResourceType = typeof(Resource))]
        Dinner = 3            
    }
}

