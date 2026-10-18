using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using Mobiilirakendused.Services;

namespace Mobiilirakendused
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Laeme salvestatud keele (vaikimisi "en")
            var savedLang = Preferences.Get("AppLanguage", "en");
            LanguageService.ChangeLanguage(savedLang);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var startPage = new StartPage();

            var navPage = new NavigationPage(startPage)
            {
                BarBackgroundColor = Colors.Blue,
                BarTextColor = Colors.White
            };

            return new Window(navPage);
        }
    }
}