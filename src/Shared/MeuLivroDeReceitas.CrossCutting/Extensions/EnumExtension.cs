using MeuLivroDeReceitas.CrossCutting.Resources.CrossCutting;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace MeuLivroDeReceitas.CrossCutting.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnumExtension
    {
        public static string GetDescriptionResources<T>(this T enumValue) where T : Enum
        {
            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return Resource.ResourceManager.GetString(description);
        }
    }
}
