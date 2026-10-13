using System;
using System.Collections.Generic;

namespace Mobiilirakendused;

public partial class MysteryPage : ContentPage
{
    // Mõistatuste andmebaas (teema -> küsimus -> vastus)
    private Dictionary<string, List<(string küsimus, string vastus)>> mõistatused;

    // Praegune teema ja mõistatus
    private string praeguneTeema = "Loodus";
    private int praeguneIndeks = 0;
    private int punktid = 0;

    // UI elemendid
    private Label pealkiriLabel;
    private Label punktidLabel;
    private Label teemaLabel;
    private Button näitaMõistatustBtn;
    private Button vastaBtn;
    private Button vahetaTeematBtn;
    private Button lõpetaBtn;

    public MysteryPage()
    {
        InitializeComponent();

        // ANDMEBAAS 
        mõistatused = new Dictionary<string, List<(string, string)>>()
        {
            {
                "Loodus", new List<(string, string)>()
                {
                    ("Mis on kõige suurem loom maailmas?", "Sinivaal"),
                    ("Mis puu on Eesti rahvuspuit?", "Tamm"),
                    ("Mitu jalga on ämblikul?", "8"),
                    ("Mis on kõige kõrgem mägi maailmas?", "Everest"),
                }
            },
            {
                "Ajalugu", new List<(string, string)>()
                {
                    ("Mis aastal algas Teine maailmasõda?", "1939"),
                    ("Kes oli esimene Eesti president?", "Konstantin Päts"),
                    ("Mis aastal toimus Eesti laulva revolutsiooni algus?", "1987"),
                }
            },
            {
                "Geograafia", new List<(string, string)>()
                {
                    ("Mis on Eesti pealinn?", "Tallinn"),
                    ("Mis on kõige pikem jõgi maailmas?", "Niilus"),
                    ("Mitu riiki on Aafrikas?", "54"),
                    ("Mis on kõige suurem ookean?", "Vaikne ookean"),
                }
            }
        };

        // UI ELEMENDID 
        pealkiriLabel = new Label
        {
            Text = "🧩 Mõistatused ja rebused",
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkBlue
        };

        punktidLabel = new Label
        {
            Text = "Punktid: 0",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Green
        };

        teemaLabel = new Label
        {
            Text = $"Teema: {praeguneTeema}",
            FontSize = 18,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkGray
        };

        näitaMõistatustBtn = new Button
        {
            Text = "Näita mõistatust",
            BackgroundColor = Colors.LightBlue,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50,
            WidthRequest = 250
        };
        näitaMõistatustBtn.Clicked += NäitaMõistatustBtn_Clicked;

        vastaBtn = new Button
        {
            Text = "Vasta küsimusele",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50,
            WidthRequest = 250
        };
        vastaBtn.Clicked += VastaBtn_Clicked;

        vahetaTeematBtn = new Button
        {
            Text = "Vaheta teemat",
            BackgroundColor = Colors.LightYellow,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50,
            WidthRequest = 250
        };
        vahetaTeematBtn.Clicked += VahetaTeematBtn_Clicked;

        lõpetaBtn = new Button
        {
            Text = "Lõpeta mäng",
            BackgroundColor = Colors.LightCoral,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50,
            WidthRequest = 250
        };
        lõpetaBtn.Clicked += LõpetaBtn_Clicked;

        // PAIGUTUS
        Content = new VerticalStackLayout
        {
            Padding = 30,
            Spacing = 20,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                pealkiriLabel,
                punktidLabel,
                teemaLabel,
                näitaMõistatustBtn,
                vastaBtn,
                vahetaTeematBtn,
                lõpetaBtn
            }
        };

        // Küsime kasutajalt nime kohe, kui leht avaneb
        KüsiNime();
    }

    // --- KÜSI NIME (DisplayPromptAsync) ---
    private async void KüsiNime()
    {
        string nimi = await DisplayPromptAsync("Tere tulemast!", "Mis on sinu nimi?", "Alusta", "Jäta vahele", "Sisesta nimi...");

        if (!string.IsNullOrEmpty(nimi))
        {
            await DisplayAlertAsync("Tere, " + nimi + "!", "Alustame mõistatuste lahendamist!", "OK");
        }
        else
        {
            await DisplayAlertAsync("Tere!", "Alustame mõistatuste lahendamist!", "OK");
        }
    }

    // NÄITA MÕISTATUST (DisplayAlert)
    private async void NäitaMõistatustBtn_Clicked(object sender, EventArgs e)
    {
        if (!mõistatused.ContainsKey(praeguneTeema))
        {
            await DisplayAlertAsync("Viga", "Teemat ei leitud!", "OK");
            return;
        }

        var teemaMõistatused = mõistatused[praeguneTeema];

        if (praeguneIndeks >= teemaMõistatused.Count)
        {
            await DisplayAlertAsync("Lõpp!", $"Oled kõik {praeguneTeema} mõistatused läbi käinud!", "OK");
            return;
        }

        var mõistatus = teemaMõistatused[praeguneIndeks];

        // Kuvame mõistatuse DisplayAlert abil
        await DisplayAlertAsync($"Mõistatus #{praeguneIndeks + 1}", mõistatus.küsimus, "Vasta");
    }

    // VASTA KÜSIMUSELE (DisplayPromptAsync)
    private async void VastaBtn_Clicked(object sender, EventArgs e)
    {
        if (!mõistatused.ContainsKey(praeguneTeema)) return;

        var teemaMõistatused = mõistatused[praeguneTeema];

        if (praeguneIndeks >= teemaMõistatused.Count)
        {
            await DisplayAlertAsync("Lõpp!", "Pole rohkem küsimusi.", "OK");
            return;
        }

        var mõistatus = teemaMõistatused[praeguneIndeks];

        // Küsime vastust DisplayPromptAsync abil
        string vastus = await DisplayPromptAsync("Sinu vastus", mõistatus.küsimus, "Kinnita", "Loobu", "Kirjuta vastus...");

        if (vastus == null)
        {
            // Kasutaja vajutas "Loobu"
            return;
        }

        // Kontrollime vastust (ei ole tõstutundlik)
        if (vastus.Trim().ToLower() == mõistatus.vastus.ToLower())
        {
            punktid++;
            punktidLabel.Text = $"Punktid: {punktid}";
            await DisplayAlertAsync("Õige! 🎉", $"Tubli! Õige vastus on: {mõistatus.vastus}", "Järgmine");
        }
        else
        {
            await DisplayAlertAsync("Vale! ❌", $"Vale vastus. Õige on: {mõistatus.vastus}", "Järgmine");
        }

        // Liigume järgmise mõistatuse juurde
        praeguneIndeks++;
    }

    // --- VAHETA TEEMAT (DisplayActionSheet) ---
    private async void VahetaTeematBtn_Clicked(object sender, EventArgs e)
    {
        string valik = await DisplayActionSheet("Vali teema", "Loobu", null, "Loodus", "Ajalugu", "Geograafia");

        if (valik != null && valik != "Loobu")
        {
            praeguneTeema = valik;
            praeguneIndeks = 0;
            teemaLabel.Text = $"Teema: {praeguneTeema}";
            await DisplayAlertAsync("Teema vahetatud", $"Nüüd lahendad teemat: {praeguneTeema}", "Alustame!");
        }
    }

    // --- LÕPETA MÄNG (DisplayAlert Yes/No) ---
    private async void LõpetaBtn_Clicked(object sender, EventArgs e)
    {
        bool kinnitus = await DisplayAlertAsync("Lõpeta mäng", $"Kas soovid mängu lõpetada? Sinu punktid: {punktid}", "Jah", "Ei");

        if (kinnitus)
        {
            await DisplayAlertAsync("Aitäh mängimast!", $"Sinu lõplikud punktid: {punktid}", "OK");

            // Nullime mängu
            punktid = 0;
            praeguneIndeks = 0;
            punktidLabel.Text = "Punktid: 0";
        }
    }
}