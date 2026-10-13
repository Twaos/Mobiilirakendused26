using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobiilirakendused;

public partial class TicTacToePage : ContentPage
{
    // Mängu seis
    private Grid mänguväli;
    private Button[] ruudud = new Button[9];
    private string[] laud = new string[9]; // "", "X", "O"
    private string praeguneMängija = "X";
    private bool mängLäbi = false;
    private Random rnd = new Random();

    // UI elemendid
    private Label staatusLabel;
    private Button uusMängBtn;
    private Button kesAlustabBtn;
    private Button reeglidBtn;

    public TicTacToePage()
    {
        InitializeComponent();

        
        var pealkiri = new Label
        {
            Text = "Trips Traps Trull",
            FontSize = 32,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkBlue
        };

        // STAATUS
        staatusLabel = new Label
        {
            Text = "X alustab!",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkGreen
        };

        // MÄNGUVÄLI (3x3 Grid)
        mänguväli = new Grid
        {
            RowSpacing = 5,
            ColumnSpacing = 5,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 300,
            HeightRequest = 300
        };

        for (int i = 0; i < 3; i++)
        {
            mänguväli.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            mänguväli.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        // Loome 9 nuppu
        for (int i = 0; i < 9; i++)
        {
            Button ruut = new Button
            {
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10
            };

            int indeks = i; // Salvestame indeksi
            ruut.Clicked += (sender, e) => Ruut_Clicked(indeks);

            ruudud[i] = ruut;
            laud[i] = "";

            // Paigutame gridi (rida = i / 3, veerg = i % 3)
            mänguväli.Add(ruut, i % 3, i / 3);
        }

        // NUPUD
        uusMängBtn = new Button
        {
            Text = "Uus mäng",
            BackgroundColor = Colors.LightBlue,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50
        };
        uusMängBtn.Clicked += UusMängBtn_Clicked;

        kesAlustabBtn = new Button
        {
            Text = "Kes alustab?",
            BackgroundColor = Colors.LightYellow,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50
        };
        kesAlustabBtn.Clicked += KesAlustabBtn_Clicked;

        reeglidBtn = new Button
        {
            Text = "Reeglid",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HeightRequest = 50
        };
        reeglidBtn.Clicked += ReeglidBtn_Clicked;

        // Nuppude paigutus (2 veergu)
        var nupudGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 10,
            RowSpacing = 10
        };

        nupudGrid.Add(uusMängBtn, 0, 0);
        nupudGrid.Add(kesAlustabBtn, 1, 0);
        nupudGrid.Add(reeglidBtn, 0, 1);
        nupudGrid.SetColumnSpan(reeglidBtn, 2);

        // KOGU PAIGUTUS
        Content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 20,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                pealkiri,
                staatusLabel,
                mänguväli,
                nupudGrid
            }
        };
    }

    // RUUDU KLÕPS
    private async void Ruut_Clicked(int indeks)
    {
        if (mängLäbi) return; // Mäng on läbi
        if (laud[indeks] != "") return; // Ruut on juba täidetud

        // Teeme käigu
        laud[indeks] = praeguneMängija;
        ruudud[indeks].Text = praeguneMängija;
        ruudud[indeks].TextColor = praeguneMängija == "X" ? Colors.Red : Colors.Blue;
        ruudud[indeks].BackgroundColor = Colors.White;

        // Kontrollime võitu
        string võitja = KontrolliVõitu();
        if (võitja != null)
        {
            mängLäbi = true;
            staatusLabel.Text = $"{võitja} võitis! 🎉";

            bool uus = await DisplayAlertAsync("Mäng läbi!", $"{võitja} võitis! Kas soovid uut mängu?", "Jah", "Ei");
            if (uus) AlustaUutMängu();
            return;
        }

        // Kontrollime viiki
        if (OnViik())
        {
            mängLäbi = true;
            staatusLabel.Text = "Viik! 🤝";

            bool uus = await DisplayAlertAsync("Mäng läbi!", "Viik! Kas soovid uut mängu?", "Jah", "Ei");
            if (uus) AlustaUutMängu();
            return;
        }

        // Vahetame mängijat
        praeguneMängija = praeguneMängija == "X" ? "O" : "X";
        staatusLabel.Text = $"{praeguneMängija} kord!";
    }

    // VÕIDU KONTROLL
    private string KontrolliVõitu()
    {
        // Võidukombinatsioonid
        int[][] kombinatsioonid = new int[][]
        {
            new int[] { 0, 1, 2 }, // 1. rida
            new int[] { 3, 4, 5 }, // 2. rida
            new int[] { 6, 7, 8 }, // 3. rida
            new int[] { 0, 3, 6 }, // 1. veerg
            new int[] { 1, 4, 7 }, // 2. veerg
            new int[] { 2, 5, 8 }, // 3. veerg
            new int[] { 0, 4, 8 }, // diagonaal
            new int[] { 2, 4, 6 }  // diagonaal
        };

        foreach (var kombo in kombinatsioonid)
        {
            if (laud[kombo[0]] != "" &&
                laud[kombo[0]] == laud[kombo[1]] &&
                laud[kombo[1]] == laud[kombo[2]])
            {
                return laud[kombo[0]];
            }
        }
        return null;
    }

    // VIIGI KONTROLL
    private bool OnViik()
    {
        foreach (string ruut in laud)
        {
            if (ruut == "") return false;
        }
        return true;
    }

    // UUS MÄNG
    private void UusMängBtn_Clicked(object sender, EventArgs e)
    {
        AlustaUutMängu();
    }

    private void AlustaUutMängu()
    {
        for (int i = 0; i < 9; i++)
        {
            laud[i] = "";
            ruudud[i].Text = "";
            ruudud[i].BackgroundColor = Colors.LightGray;
        }
        praeguneMängija = "X";
        mängLäbi = false;
        staatusLabel.Text = "X alustab!";
    }

    // KES ALUSTAB?
    private async void KesAlustabBtn_Clicked(object sender, EventArgs e)
    {
        string valik = await DisplayActionSheet("Kes alustab?", "Loobu", null, "X alustab", "O alustab", "Juhuslik");

        if (valik == "X alustab")
        {
            praeguneMängija = "X";
            staatusLabel.Text = "X alustab!";
        }
        else if (valik == "O alustab")
        {
            praeguneMängija = "O";
            staatusLabel.Text = "O alustab!";
        }
        else if (valik == "Juhuslik")
        {
            praeguneMängija = rnd.Next(2) == 0 ? "X" : "O";
            staatusLabel.Text = $"{praeguneMängija} alustab! (juhuslik)";
        }

        AlustaUutMängu();
        staatusLabel.Text = $"{praeguneMängija} alustab!";
    }

    // REEGLID
    private async void ReeglidBtn_Clicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Reeglid",
            "Trips Traps Trull reeglid:\n\n" +
            "1. Mängija X alustab.\n" +
            "2. Mängijad võtavad kordamööda käike.\n" +
            "3. Võidab see, kes saab kolm oma sümbolit järjest (rida, veerg või diagonaal).\n" +
            "4. Kui kõik ruudud on täidetud ja võitjat pole, on viik.",
            "Selge");
    }
}