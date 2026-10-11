using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Mobiilirakendused;

public partial class LumememmPage : ContentPage
{
    // Lumememme osad (nüüd Ellipse, mitte Frame)
    private Ellipse pea;
    private Ellipse keha;
    private BoxView ämber;
    private Ellipse silm1, silm2;
    private Ellipse nööp1, nööp2, nööp3;
    private AbsoluteLayout al;

    // Juhtimise elemendid
    private Picker tegevusPicker;
    private Button käivitaBtn;
    private Label tegevusLabel;
    private Slider läbipaistvusSlider;
    private Stepper kiirusStepper;

    private Random rnd = new Random();
    private bool onNähtav = true;

    public LumememmPage()
    {
        InitializeComponent();

        // --- LUMEMEMME OSAD ---
        // Ämber (müts)
        ämber = new BoxView
        {
            Color = Colors.DarkGray,
            CornerRadius = 5
        };

        // Pea (Ellipse)
        pea = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.White),
            Stroke = Colors.LightGray,
            StrokeThickness = 2,
            WidthRequest = 150,
            HeightRequest = 150
        };

        // Keha (Ellipse)
        keha = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.White),
            Stroke = Colors.LightGray,
            StrokeThickness = 2,
            WidthRequest = 200,
            HeightRequest = 200
        };

        // Silmad
        silm1 = new Ellipse { Fill = new SolidColorBrush(Colors.Black), WidthRequest = 15, HeightRequest = 15 };
        silm2 = new Ellipse { Fill = new SolidColorBrush(Colors.Black), WidthRequest = 15, HeightRequest = 15 };

        // Nööbid
        nööp1 = new Ellipse { Fill = new SolidColorBrush(Colors.Black), WidthRequest = 20, HeightRequest = 20 };
        nööp2 = new Ellipse { Fill = new SolidColorBrush(Colors.Black), WidthRequest = 20, HeightRequest = 20 };
        nööp3 = new Ellipse { Fill = new SolidColorBrush(Colors.Black), WidthRequest = 20, HeightRequest = 20 };

        // --- JUHTIMISE ELEMENDID ---
        tegevusPicker = new Picker
        {
            Title = "Vali tegevus",
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 250
        };
        tegevusPicker.Items.Add("Peida lumememm");
        tegevusPicker.Items.Add("Näita lumememm");
        tegevusPicker.Items.Add("Muuda värvi");
        tegevusPicker.Items.Add("Sulata");
        tegevusPicker.Items.Add("Tantsi");
        tegevusPicker.SelectedIndex = 0;

        käivitaBtn = new Button
        {
            Text = "Käivita",
            BackgroundColor = Colors.LightBlue,
            TextColor = Colors.Black,
            CornerRadius = 10,
            WidthRequest = 150
        };
        käivitaBtn.Clicked += KäivitaBtn_Clicked;

        tegevusLabel = new Label
        {
            Text = "Vali tegevus ja vajuta Käivita",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };

        läbipaistvusSlider = new Slider
        {
            Minimum = 0.0,
            Maximum = 1.0,
            Value = 1.0,
            WidthRequest = 250,
            MinimumTrackColor = Colors.LightBlue,
            MaximumTrackColor = Colors.LightGray
        };
        läbipaistvusSlider.ValueChanged += LäbipaistvusSlider_ValueChanged;

        kiirusStepper = new Stepper
        {
            Minimum = 100,
            Maximum = 2000,
            Increment = 100,
            Value = 500,
            HorizontalOptions = LayoutOptions.Center
        };

        // --- ABSOLUTELAYOUT PAIGUTUS ---
        al = new AbsoluteLayout();

        // --- ZINDEX VÄÄRTUSED ---
        keha.ZIndex = 0;
        pea.ZIndex = 1;
        ämber.ZIndex = 2;
        silm1.ZIndex = 3;
        silm2.ZIndex = 3;
        nööp1.ZIndex = 3;
        nööp2.ZIndex = 3;
        nööp3.ZIndex = 3;

        // --- LISAME ELEMENDID (suured -> väikesed) ---
        al.Children.Add(keha);
        al.Children.Add(pea);
        al.Children.Add(ämber);
        al.Children.Add(silm1);
        al.Children.Add(silm2);
        al.Children.Add(nööp1);
        al.Children.Add(nööp2);
        al.Children.Add(nööp3);

        al.Children.Add(tegevusPicker);
        al.Children.Add(käivitaBtn);
        al.Children.Add(tegevusLabel);
        al.Children.Add(läbipaistvusSlider);
        al.Children.Add(kiirusStepper);

        // --- PAIGUTUS ---
        AbsoluteLayout.SetLayoutBounds(keha, new Rect(0.5, 0.55, 200, 200));
        AbsoluteLayout.SetLayoutFlags(keha, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(pea, new Rect(0.5, 0.35, 150, 150));
        AbsoluteLayout.SetLayoutFlags(pea, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(ämber, new Rect(0.5, 0.22, 80, 40));
        AbsoluteLayout.SetLayoutFlags(ämber, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(silm1, new Rect(0.45, 0.33, 15, 15));
        AbsoluteLayout.SetLayoutFlags(silm1, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(silm2, new Rect(0.55, 0.33, 15, 15));
        AbsoluteLayout.SetLayoutFlags(silm2, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(nööp1, new Rect(0.5, 0.5, 20, 20));
        AbsoluteLayout.SetLayoutFlags(nööp1, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(nööp2, new Rect(0.5, 0.58, 20, 20));
        AbsoluteLayout.SetLayoutFlags(nööp2, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(nööp3, new Rect(0.5, 0.66, 20, 20));
        AbsoluteLayout.SetLayoutFlags(nööp3, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(tegevusPicker, new Rect(0.5, 0.80, 250, 50));
        AbsoluteLayout.SetLayoutFlags(tegevusPicker, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(käivitaBtn, new Rect(0.5, 0.87, 150, 50));
        AbsoluteLayout.SetLayoutFlags(käivitaBtn, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(tegevusLabel, new Rect(0.5, 0.93, 300, 30));
        AbsoluteLayout.SetLayoutFlags(tegevusLabel, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(läbipaistvusSlider, new Rect(0.5, 0.97, 250, 30));
        AbsoluteLayout.SetLayoutFlags(läbipaistvusSlider, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(kiirusStepper, new Rect(0.5, 0.70, 200, 40));
        AbsoluteLayout.SetLayoutFlags(kiirusStepper, AbsoluteLayoutFlags.PositionProportional);

        Content = al;
    }

    private void LäbipaistvusSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double opacity = e.NewValue;
        pea.Opacity = opacity;
        keha.Opacity = opacity;
        ämber.Opacity = opacity;
        silm1.Opacity = opacity;
        silm2.Opacity = opacity;
        nööp1.Opacity = opacity;
        nööp2.Opacity = opacity;
        nööp3.Opacity = opacity;

        tegevusLabel.Text = $"Läbipaistvus: {opacity:F2}";
    }

    private async void KäivitaBtn_Clicked(object sender, EventArgs e)
    {
        string valik = tegevusPicker.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(valik)) return;

        switch (valik)
        {
            case "Peida lumememm":
                await PeidaLumememm();
                break;
            case "Näita lumememm":
                await NäitaLumememm();
                break;
            case "Muuda värvi":
                await MuudaVarv();
                break;
            case "Sulata":
                await Sulata();
                break;
            case "Tantsi":
                await Tantsi();
                break;
        }
    }

    private async Task PeidaLumememm()
    {
        if (!onNähtav) return;

        await pea.FadeTo(0, 500);
        await keha.FadeTo(0, 500);
        await ämber.FadeTo(0, 500);
        await silm1.FadeTo(0, 500);
        await silm2.FadeTo(0, 500);
        await nööp1.FadeTo(0, 500);
        await nööp2.FadeTo(0, 500);
        await nööp3.FadeTo(0, 500);

        pea.IsVisible = false;
        keha.IsVisible = false;
        ämber.IsVisible = false;
        silm1.IsVisible = false;
        silm2.IsVisible = false;
        nööp1.IsVisible = false;
        nööp2.IsVisible = false;
        nööp3.IsVisible = false;

        onNähtav = false;
        tegevusLabel.Text = "Lumememm on peidetud!";
    }

    private async Task NäitaLumememm()
    {
        if (onNähtav) return;

        // Taastame skaala ja asukoha
        pea.Scale = 1.0;
        keha.Scale = 1.0;
        ämber.Scale = 1.0;
        silm1.Scale = 1.0;
        silm2.Scale = 1.0;
        nööp1.Scale = 1.0;
        nööp2.Scale = 1.0;
        nööp3.Scale = 1.0;

        pea.TranslationX = 0;
        keha.TranslationX = 0;
        ämber.TranslationX = 0;
        silm1.TranslationX = 0;
        silm2.TranslationX = 0;
        nööp1.TranslationX = 0;
        nööp2.TranslationX = 0;
        nööp3.TranslationX = 0;

        pea.IsVisible = true;
        keha.IsVisible = true;
        ämber.IsVisible = true;
        silm1.IsVisible = true;
        silm2.IsVisible = true;
        nööp1.IsVisible = true;
        nööp2.IsVisible = true;
        nööp3.IsVisible = true;

        await pea.FadeTo(1, 500);
        await keha.FadeTo(1, 500);
        await ämber.FadeTo(1, 500);
        await silm1.FadeTo(1, 500);
        await silm2.FadeTo(1, 500);
        await nööp1.FadeTo(1, 500);
        await nööp2.FadeTo(1, 500);
        await nööp3.FadeTo(1, 500);

        onNähtav = true;
        tegevusLabel.Text = "Lumememm on tagasi!";
    }

    private async Task MuudaVarv()
    {
        bool kinnitus = await DisplayAlert("Kinnitus", "Kas muudame lumememme värvi?", "Jah", "Ei");
        if (!kinnitus) return;

        Color uusVarv = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

        pea.Fill = new SolidColorBrush(uusVarv);
        keha.Fill = new SolidColorBrush(uusVarv);

        tegevusLabel.Text = "Värv muudetud!";
    }

    private async Task Sulata()
    {
        uint kiirus = (uint)kiirusStepper.Value;

        await pea.FadeTo(0, kiirus);
        await pea.ScaleTo(0.5, kiirus);

        await keha.FadeTo(0, kiirus);
        await keha.ScaleTo(0.5, kiirus);

        await ämber.FadeTo(0, kiirus);
        await ämber.ScaleTo(0.5, kiirus);

        await silm1.FadeTo(0, kiirus);
        await silm2.FadeTo(0, kiirus);
        await nööp1.FadeTo(0, kiirus);
        await nööp2.FadeTo(0, kiirus);
        await nööp3.FadeTo(0, kiirus);

        pea.IsVisible = false;
        keha.IsVisible = false;
        ämber.IsVisible = false;
        silm1.IsVisible = false;
        silm2.IsVisible = false;
        nööp1.IsVisible = false;
        nööp2.IsVisible = false;
        nööp3.IsVisible = false;

        onNähtav = false;
        tegevusLabel.Text = "Lumememm sulas! 🌞";
    }

    private async Task Tantsi()
    {
        uint kiirus = (uint)kiirusStepper.Value;

        for (int i = 0; i < 3; i++)
        {
            await pea.TranslateTo(-30, 0, kiirus / 2);
            await keha.TranslateTo(-30, 0, kiirus / 2);
            await ämber.TranslateTo(-30, 0, kiirus / 2);
            await silm1.TranslateTo(-30, 0, kiirus / 2);
            await silm2.TranslateTo(-30, 0, kiirus / 2);
            await nööp1.TranslateTo(-30, 0, kiirus / 2);
            await nööp2.TranslateTo(-30, 0, kiirus / 2);
            await nööp3.TranslateTo(-30, 0, kiirus / 2);

            await pea.TranslateTo(30, 0, kiirus / 2);
            await keha.TranslateTo(30, 0, kiirus / 2);
            await ämber.TranslateTo(30, 0, kiirus / 2);
            await silm1.TranslateTo(30, 0, kiirus / 2);
            await silm2.TranslateTo(30, 0, kiirus / 2);
            await nööp1.TranslateTo(30, 0, kiirus / 2);
            await nööp2.TranslateTo(30, 0, kiirus / 2);
            await nööp3.TranslateTo(30, 0, kiirus / 2);
        }

        await pea.TranslateTo(0, 0, kiirus / 2);
        await keha.TranslateTo(0, 0, kiirus / 2);
        await ämber.TranslateTo(0, 0, kiirus / 2);
        await silm1.TranslateTo(0, 0, kiirus / 2);
        await silm2.TranslateTo(0, 0, kiirus / 2);
        await nööp1.TranslateTo(0, 0, kiirus / 2);
        await nööp2.TranslateTo(0, 0, kiirus / 2);
        await nööp3.TranslateTo(0, 0, kiirus / 2);

        tegevusLabel.Text = "Lumememm tantsis! 💃";
    }
}