using System;

namespace Mobiilirakendused;

public partial class UusRetseptPage : ContentPage
{
    public UusRetseptPage()
    {
        InitializeComponent();
    }

    private async void Salvesta_Clicked(object sender, EventArgs e)
    {
        
        if (string.IsNullOrWhiteSpace(EntryNimi.Text) ||
            string.IsNullOrWhiteSpace(EntryKategooria.Text) ||
            string.IsNullOrWhiteSpace(EntryPilt.Text))
        {
            await DisplayAlertAsync("Viga", "Palun täida kõik väljad!", "OK");
            return;
        }

        // Loome uue retsepti
        var uus = new Retsept
        {
            Nimi = EntryNimi.Text.Trim(),
            Kategooria = EntryKategooria.Text.Trim(),
            PildiLink = EntryPilt.Text.Trim()
        };

        // Salvestame faili
        FailiHaldur.SalvestaRetsept(uus);

        // Tühjendame väljad
        EntryNimi.Text = "";
        EntryKategooria.Text = "";
        EntryPilt.Text = "";

        await DisplayAlertAsync("Salvestatud", $"Retsept '{uus.Nimi}' on lisatud!", "OK");
    }
}