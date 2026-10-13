using System;
using System.Collections.Generic;

namespace Mobiilirakendused;

public partial class PickerImageGridPage : ContentPage
{
    // Muutujad
    Grid gr4x1, gr3x3;
    Picker picker;          // Piltide valik
    Image img;              // Piltide näitamiseks
    Switch s_pilt, s_grid;  // Piltide ja gridi kuvamiseks/peitmiseks
    Random rnd = new Random(); // Piltide juhuslikuks valimiseks

    public PickerImageGridPage()
    {
        InitializeComponent();

        // 4x1 GRID (peamine paigutus)
        gr4x1 = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
        };

        // PICKER (piltide valik)
        picker = new Picker
        {
            Title = "Vali pilt",
            ItemsSource = new List<string> { "Pilt 1", "Pilt 2", "Pilt 3" },
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        picker.SelectedIndexChanged += Piltide_valik;

        // IMAGE (pildi kuvamine)
        img = new Image
        {
            Source = "dotnet_bot.png",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HeightRequest = 200
        };

        // SWITCH 1 (pildi näitamine/peitmine)
        s_pilt = new Switch
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            IsToggled = true,
            IsEnabled = true
        };
        s_pilt.Toggled += (sender, e) =>
        {
            if (e.Value)
            {
                img.IsVisible = true;
            }
            else
            {
                img.IsVisible = false;
            }
        };

        // SWITCH 2 (gridi näitamine/peitmine)
        s_grid = new Switch
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            IsToggled = false,
            IsEnabled = true
        };
        s_grid.Toggled += (sender, e) =>
        {
            if (e.Value)
            {
                gr3x3 = Täida_gr3x3();
                gr4x1.Add(gr3x3, 0, 2);
                gr4x1.SetColumnSpan(gr3x3, 2);
            }
            else
            {
                gr4x1.RemoveAt(4);
            }
        };

        // PAIGUTUS gr4x1 GRID-I
        // Picker (ülemine rida, vasak veerg)
        gr4x1.Add(picker, 0, 0);
        gr4x1.SetColumnSpan(picker, 2);

        // Image (teine rida, mõlemad veerud)
        gr4x1.Add(img, 0, 1);
        gr4x1.SetColumnSpan(img, 2);

        // Switch 1 (alumine rida, vasak veerg)
        gr4x1.Add(s_pilt, 0, 3);

        // Switch 2 (alumine rida, parem veerg)
        gr4x1.Add(s_grid, 1, 3);

        Content = gr4x1;
    }

    // --- PILDIVALIK (Picker) ---
    private void Piltide_valik(object sender, EventArgs e)
    {
        if (picker.SelectedIndex == -1) return; // Kui midagi pole valitud, siis ei tee midagi

        if (picker.SelectedIndex == 0) img.Source = "dotnet_bot1.png";
        else if (picker.SelectedIndex == 1) img.Source = "dotnet_bot2.png";
        else if (picker.SelectedIndex == 2) img.Source = "dotnet_bot3.png";
    }

    // 3x3 GRID (värvilised kastid)
    private Grid Täida_gr3x3()
    {
        gr3x3 = new Grid();

        // Lisame read ja veerud
        for (int i = 0; i < 3; i++)
        {
            gr3x3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gr3x3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        // Lisame kastid
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                BoxView kast = new BoxView
                {
                    BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)),
                };
                gr3x3.Add(kast, c, r);

                // Salvestame rea ja veeru numbrid
                int rida = r;
                int veerg = c;

                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += async (s, args) =>
                {
                    kast.BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    await DisplayAlertAsync("Koordinaadid", $"Vajutasid lahtrisse:\nRida: {rida}\nVeerg: {veerg}", "Selge");
                };
                kast.GestureRecognizers.Add(tap);
            }
        }

        return gr3x3;
    }
}