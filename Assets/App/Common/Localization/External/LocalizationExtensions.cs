namespace App.Common.Localization.External
{
    public static class LocalizationExtensions
    {
        public static string Localize(this string str)
        {
            return LocalizationManager.Translate(str);
        }
    }
}

