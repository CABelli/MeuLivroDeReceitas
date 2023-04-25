using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MeuLivroDeReceitas.CrossCutting.Extensions
{
    public static class StringExtensions
    {
        public static bool ExtensionToBool(this string extension)
        {
            string[] extensoesValidas = new string[] { ".jpg", ".png", ".gif" };
            return extensoesValidas.Contains(extension);
        }

        public static string TitleNameFileExtension(this string title, string extension)
        {
            return title + "_" + DateTime.Now.ToString("HH:mm:ss") + extension;
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static bool EmptyOrFilledText(this string? text)
        {
            return string.IsNullOrEmpty(text) ? false : true;
        }

        public static double ToDouble(this string stringInp)
        {
            if (Double.TryParse(stringInp, NumberStyles.Any, CultureInfo.InvariantCulture, out double numValue))            
                return numValue;            
            else            
                return 0;
        }

        public static bool ValidatorPhone(this string phoneNumber)
        {
            var match = Regex.Match(phoneNumber, "([0-9]{2,3})?([0-9]{2})([0-9]{4,5})([0-9]{4})");
            return  match.Value.Equals(phoneNumber) ? true : false;
        }
    }
}
