using Mobiilirakendused.Resources.Localization;
using System.Globalization;
using Microsoft.Maui.Storage;

namespace Mobiilirakendused.Services
{
    public static class LanguageService
    {
        public static event Action? LanguageChanged;

        public static void ChangeLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            AppResources.Culture = culture;

            // Salvestab valitud keele
            Preferences.Set("AppLanguage", languageCode);

            LanguageChanged?.Invoke();
        }
    }
}