using System.Collections.Generic;
using Microsoft.Maui.Storage;
using Mobiilirakendused.Services;
using Mobiilirakendused.Resources.Localization; 

namespace Mobiilirakendused;

public partial class StartPage : ContentPage
{
    VerticalStackLayout vst;
    ScrollView sv;
    Label keeleNaideLabel; 

    public List<ContentPage> Lehed = new List<ContentPage>() { new TextPage(), new FigurePage(), new ValgusfoorPage(), new DateTimePage(), new StepperSliderPage(), new RGBPage(), new LumememmPage(), new TreePage(), new PopUp_Page(),
    new MysteryPage(), new PickerImageGridPage(), new TicTacToePage(), new Table_Page(), new RiikPage(), new KarussellPage(), new CarouselGalleryPage(),new MemoPage() };
    public List<string> LeheNimed = new List<string>() { "Tekst", "Kujund", "Valgusfoor", "Kuupäev ja aeg", "Stepper/Slider", "RGB mudel", "Lumememm", "Puu hooajad", "Pop-up aknad", "Mõistatused", "Pilt ja Grid", "Trips Traps Trull", "Tabel", "Euroopa Riigid", "Karussell", "Programmeerimiskeelte portfoolio", "Memo mäng" };

    public StartPage()
    {
        InitializeComponent();

        vst = new VerticalStackLayout { Padding = 20, Spacing = 15 };

        
        var keeleLabel = new Label
        {
            Text = "Vali keel / Choose language / Выберите язык",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkBlue
        };

        var enBtn = new Button
        {
            Text = "🇬🇧 English",
            BackgroundColor = Colors.LightBlue,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50,
            WidthRequest = 100
        };
        enBtn.Clicked += (s, e) => MuudaKeelt("en");

        var ruBtn = new Button
        {
            Text = "🇷🇺 Русский",
            BackgroundColor = Colors.LightCoral,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50,
            WidthRequest = 100
        };
        ruBtn.Clicked += (s, e) => MuudaKeelt("ru");

        var etBtn = new Button
        {
            Text = "🇪🇪 Eesti",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50,
            WidthRequest = 100
        };
        etBtn.Clicked += (s, e) => MuudaKeelt("et");

        var keeleNupud = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center,
            Children = { enBtn, ruBtn, etBtn }
        };

        
        keeleNaideLabel = new Label
        {
            Text = AppResources.GreetingText, 
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkGreen,
            Margin = new Thickness(0, 10)
        };

        vst.Add(keeleLabel);
        vst.Add(keeleNupud);
        vst.Add(keeleNaideLabel); 

        
        vst.Add(new BoxView { HeightRequest = 2, BackgroundColor = Colors.Gray, Margin = new Thickness(0, 10) });

        
        for (int i = 0; i < Lehed.Count; i++)
        {
            Button nupp = new Button
            {
                Text = LeheNimed[i],
                FontSize = 28,
                FontFamily = "DancingScript",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10,
                HeightRequest = 80,
                Padding = new Thickness(10),
                ZIndex = i
            };
            vst.Add(nupp);
            nupp.Clicked += (sender, e) =>
            {
                var valik = Lehed[nupp.ZIndex];
                Navigation.PushAsync(valik);
            };
        }

        
        Button nulliNupp = new Button
        {
            Text = "Nulli seaded (Testimiseks)",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            CornerRadius = 10,
            HeightRequest = 50,
            Margin = new Thickness(0, 30, 0, 0)
        };

        nulliNupp.Clicked += async (sender, e) =>
        {
            Preferences.Default.Remove("EsimeneKäivitamine");
            await DisplayAlertAsync("Edukalt nullitud", "Mälu on tühjendatud. Kui sa lehe uuesti avad, käitub äpp nagu täiesti uus!", "OK");
        };
        vst.Add(nulliNupp);

        sv = new ScrollView { Content = vst };
        Content = sv;


        Dispatcher.Dispatch(() =>
        {
            sv.ScrollToAsync(0, 0, false);
        });
    }

    
    private void MuudaKeelt(string keel)
    {
        LanguageService.ChangeLanguage(keel);
        
        keeleNaideLabel.Text = AppResources.GreetingText;
    }

    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool onEsimeneStart = Preferences.Default.Get("EsimeneKäivitamine", true);

        if (onEsimeneStart)
        {
            bool vastus = await DisplayAlertAsync("Tere tulemast!",
                                "Tundub, et avasid selle rakenduse esimest korda. Kas soovid näha lühikest juhendit?",
                                "Jah, palun",
                                "Ei, saan ise hakkama");

            if (vastus)
            {
                await DisplayAlertAsync("Juhend",
                    "Siin on sinu lühike juhend: vali menüüst sobiv teema ja uuri, kuidas elemendid töötavad!",
                    "Selge");
            }

            Preferences.Default.Set("EsimeneKäivitamine", false);
        }
    }
}