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

        public static bool EmptyOrFilledText(this string? text)
        {
            return string.IsNullOrEmpty(text) ? false : true;
        }
    }
}
