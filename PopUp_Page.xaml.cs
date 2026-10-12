using System;

namespace Mobiilirakendused;

public partial class PopUp_Page : ContentPage
{
    public PopUp_Page()
    {
        // 1. Loon esimese nupu (Lihtne teade)
        Button alertButton = new Button
        {
            Text = "TEADE",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        // Seon nupu klikkimise sündmuse funktsiooniga
        alertButton.Clicked += AlertButton_Clicked;

        // 2. Loon teise nupu (Kinnitus)
        Button alertYesNoButton = new Button
        {
            Text = "JAH VÕI EI",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertYesNoButton.Clicked += AlertYesNoButton_Clicked;

        // 3. Loon kolmanda nupu (Valikumenüü)
        Button alertListButton = new Button
        {
            Text = "VALIK",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertListButton.Clicked += AlertListButton_Clicked;

        // 4. Loon neljanda nupu (Küsimus)
        Button alertQuestButton = new Button
        {
            Text = "KÜSIMUS",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertQuestButton.Clicked += AlertQuestButton_Clicked;

        // Paigutan kõik nupud ekraanile üksteise alla
        Content = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = new Thickness(0, 50, 0, 0),
            Children = { alertButton, alertYesNoButton, alertListButton, alertQuestButton }
        };
    }

    // 1. Nupp: Lihtne teade
    private async void AlertButton_Clicked(object sender, EventArgs e)
    {
        // Kuvab lihtsalt teate ja ootab, kuni kasutaja vajutab "OK"
        await DisplayAlertAsync("Teade", "Teil on uus teade", "OK");
    }

    // 2. Nupp: Jah või ei valik
    private async void AlertYesNoButton_Clicked(object sender, EventArgs e)
    {
        // Küsime kasutajalt kinnitust (tagastab true või false)
        bool result = await DisplayAlertAsync("Kinnitus", "Kas oled kindel?", "Olen kindel", "Ei ole kindel");

        // Kuvame uue teate vastavalt sellele, mida kasutaja valis
        await DisplayAlertAsync("Teade", "Teie valik on: " + (result ? "Jah" : "Ei"), "OK");
    }

    // 3. Nupp: Valikute nimekiri
    private async void AlertListButton_Clicked(object sender, EventArgs e)
    {
        // Kuvab menüü ja salvestab kasutaja valitud teksti muutujasse 'action'
        string action = await DisplayActionSheet("Mida teha?", "Loobu", null, "Tantsida", "Laulda", "Joonestada");

        // Kontrollime, et kasutaja ei vajutanud lihtsalt kõrvale ega valinud "Loobu"
        if (action != null && action != "Loobu")
        {
            await DisplayAlertAsync("Valik", "Sa valisid tegevuse: " + action, "OK");
        }
    }

    // 4. Nupp: Küsimus (teksti sisestamine)
    private async void AlertQuestButton_Clicked(object sender, EventArgs e)
    {
        // Küsime kasutajalt teksti
        string vastus = await DisplayPromptAsync("Küsimus", "Kuidas läheb?", "OK", "Loobu", "Tore!");

        if (vastus != null)
        {
            await DisplayAlertAsync("Vastus", "Sa vastasid: " + vastus, "OK");
        }
    }
}