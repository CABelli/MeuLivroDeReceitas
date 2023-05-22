using MeuLivroDeReceitas.CrossCutting.EnumClass;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MeuLivroDeReceitas.CrossCutting.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnumExtension
    {
        public static string GetLocalizedDescription(this Enum enumeration)
        {
            if (enumeration == null) return null;

            string description = enumeration.ToString();

            FieldInfo fieldInfo = enumeration.GetType().GetField(description);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any()) return attributes[0].Description;

            return description;
        }

        public static List<string> GetAllEnumDescription()
        {
            List<string> resultado = new List<string>();

            foreach (ECategory value in Enum.GetValues(typeof(ECategory)))
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                LocalizedEnumAttribute[] attributes =
                    (LocalizedEnumAttribute[])fi.GetCustomAttributes(typeof(LocalizedEnumAttribute), false);

                if (attributes != null && attributes.Length > 0)
                    resultado.Add(value.GetLocalizedDescription());
                else
                    resultado.Add(value.ToString());
            }

            return resultado;
        }
    }
}
