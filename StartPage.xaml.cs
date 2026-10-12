using System.Collections.Generic;
using Microsoft.Maui.Storage;

namespace Mobiilirakendused;

public partial class StartPage : ContentPage
{
    VerticalStackLayout vst;
    ScrollView sv;
    public List<ContentPage> Lehed = new List<ContentPage>() { new TextPage(), new FigurePage(), new ValgusfoorPage(), new DateTimePage(), new StepperSliderPage(), new RGBPage(), new LumememmPage(), new TreePage(), new PopUp_Page() };
    public List<string> LeheNimed = new List<string>() { "Tekst", "Kujund", "Valgusfoor", "Kuupäev ja aeg", "Stepper/Slider", "RGB mudel", "Lumememm", "Puu hooajad", "Pop-up aknad" };

    public StartPage()
    {
        InitializeComponent();

        // Title = "Avaleht";
        vst = new VerticalStackLayout { Padding = 20, Spacing = 15 };

        for (int i = 0; i < Lehed.Count; i++)
        {
            Button nupp = new Button
            {
                Text = LeheNimed[i],
                FontSize = 36,
                FontFamily = "DancingScript",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10,
                HeightRequest = 60,
                ZIndex = i
            };
            vst.Add(nupp);
            nupp.Clicked += (sender, e) =>
            {
                var valik = Lehed[nupp.ZIndex];
                Navigation.PushAsync(valik);
            };
        }

        // Testnupp: Nulli seaded
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