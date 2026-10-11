using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Mobiilirakendused;

public partial class TreePage : ContentPage
{
    // Puu osad
    private BoxView puuTüvi;
    private Ellipse lehestik;
    private Ellipse õis1, õis2, õis3;
    private AbsoluteLayout al;

    // Juhtimise elemendid
    private Picker tegevusPicker;
    private Button käivitaBtn;
    private Label tegevusLabel;
    private Slider läbipaistvusSlider;
    private Stepper kiirusStepper;
    private DatePicker kuupäevPicker;
    private TimePicker kellaaegPicker;

    private Random rnd = new Random();

    public TreePage()
    {
        InitializeComponent();

        // --- PUU OSAD ---
        // Puutüvi
        puuTüvi = new BoxView
        {
            Color = Colors.SaddleBrown,
            CornerRadius = 5
        };

        // Lehestik
        lehestik = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.ForestGreen),
            Stroke = Colors.DarkGreen,
            StrokeThickness = 2
        };

        // Õied (alguses peidetud)
        õis1 = new Ellipse { Fill = new SolidColorBrush(Colors.Pink), WidthRequest = 25, HeightRequest = 25, IsVisible = false };
        õis2 = new Ellipse { Fill = new SolidColorBrush(Colors.Pink), WidthRequest = 25, HeightRequest = 25, IsVisible = false };
        õis3 = new Ellipse { Fill = new SolidColorBrush(Colors.Pink), WidthRequest = 25, HeightRequest = 25, IsVisible = false };

        // --- JUHTIMISE ELEMENDID ---
        tegevusPicker = new Picker
        {
            Title = "Vali tegevus",
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 250
        };
        tegevusPicker.Items.Add("Kasva");
        tegevusPicker.Items.Add("Õitse");
        tegevusPicker.Items.Add("Värise");
        tegevusPicker.Items.Add("Langeta");
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
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center
        };

        läbipaistvusSlider = new Slider
        {
            Minimum = 0.0,
            Maximum = 1.0,
            Value = 1.0,
            WidthRequest = 250,
            MinimumTrackColor = Colors.Green,
            MaximumTrackColor = Colors.LightGray
        };
        läbipaistvusSlider.ValueChanged += LäbipaistvusSlider_ValueChanged;

        kiirusStepper = new Stepper
        {
            Minimum = 500,
            Maximum = 2000,
            Increment = 100,
            Value = 1000,
            HorizontalOptions = LayoutOptions.Center
        };

        kuupäevPicker = new DatePicker
        {
            Date = DateTime.Now,
            Format = "D",
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 250
        };

        kellaaegPicker = new TimePicker
        {
            Time = DateTime.Now.TimeOfDay,
            Format = "T",
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 250
        };

        // --- ABSOLUTELAYOUT ---
        al = new AbsoluteLayout();

        // ZIndex väärtused
        puuTüvi.ZIndex = 0;
        lehestik.ZIndex = 1;
        õis1.ZIndex = 2;
        õis2.ZIndex = 2;
        õis3.ZIndex = 2;

        // Lisan elemendid
        al.Children.Add(puuTüvi);
        al.Children.Add(lehestik);
        al.Children.Add(õis1);
        al.Children.Add(õis2);
        al.Children.Add(õis3);

        al.Children.Add(tegevusPicker);
        al.Children.Add(käivitaBtn);
        al.Children.Add(tegevusLabel);
        al.Children.Add(läbipaistvusSlider);
        al.Children.Add(kiirusStepper);
        al.Children.Add(kuupäevPicker);
        al.Children.Add(kellaaegPicker);

        // --- PAIGUTUS ---
        // Puu tüvi (keskel, all)
        AbsoluteLayout.SetLayoutBounds(puuTüvi, new Rect(0.5, 0.55, 40, 150));
        AbsoluteLayout.SetLayoutFlags(puuTüvi, AbsoluteLayoutFlags.PositionProportional);

        // Lehestik (tüve otsas)
        AbsoluteLayout.SetLayoutBounds(lehestik, new Rect(0.5, 0.40, 200, 200));
        AbsoluteLayout.SetLayoutFlags(lehestik, AbsoluteLayoutFlags.PositionProportional);

        // Õied (lehestiku sees)
        AbsoluteLayout.SetLayoutBounds(õis1, new Rect(0.40, 0.35, 25, 25));
        AbsoluteLayout.SetLayoutFlags(õis1, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(õis2, new Rect(0.55, 0.42, 25, 25));
        AbsoluteLayout.SetLayoutFlags(õis2, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(õis3, new Rect(0.50, 0.30, 25, 25));
        AbsoluteLayout.SetLayoutFlags(õis3, AbsoluteLayoutFlags.PositionProportional);

        // Juhtimise elemendid (all)
        AbsoluteLayout.SetLayoutBounds(tegevusPicker, new Rect(0.5, 0.72, 250, 45));
        AbsoluteLayout.SetLayoutFlags(tegevusPicker, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(käivitaBtn, new Rect(0.5, 0.78, 150, 45));
        AbsoluteLayout.SetLayoutFlags(käivitaBtn, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(tegevusLabel, new Rect(0.5, 0.83, 350, 30));
        AbsoluteLayout.SetLayoutFlags(tegevusLabel, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(läbipaistvusSlider, new Rect(0.5, 0.87, 250, 30));
        AbsoluteLayout.SetLayoutFlags(läbipaistvusSlider, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(kiirusStepper, new Rect(0.5, 0.91, 200, 40));
        AbsoluteLayout.SetLayoutFlags(kiirusStepper, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(kuupäevPicker, new Rect(0.5, 0.95, 250, 40));
        AbsoluteLayout.SetLayoutFlags(kuupäevPicker, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(kellaaegPicker, new Rect(0.5, 0.99, 250, 40));
        AbsoluteLayout.SetLayoutFlags(kellaaegPicker, AbsoluteLayoutFlags.PositionProportional);

        Content = al;
    }

    // --- SLIDER: Läbipaistvus (sügis) ---
    private void LäbipaistvusSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        lehestik.Opacity = e.NewValue;
        õis1.Opacity = e.NewValue;
        õis2.Opacity = e.NewValue;
        õis3.Opacity = e.NewValue;

        tegevusLabel.Text = $"Lehestiku läbipaistvus: {e.NewValue:F2}";
    }

    // --- KÄIVITA NUPU SÜNDMUS ---
    private async void KäivitaBtn_Clicked(object sender, EventArgs e)
    {
        string valik = tegevusPicker.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(valik)) return;

        switch (valik)
        {
            case "Kasva":
                await Kasva();
                break;
            case "Õitse":
                await Õitse();
                break;
            case "Värise":
                await Värise();
                break;
            case "Langeta":
                await Langeta();
                break;
        }
    }

    // --- ABIMEETOD: Algseisu taastamine ---
    private void TaastaAlgseis()
    {
        // Pööre tagasi nulli
        puuTüvi.Rotation = 0;
        lehestik.Rotation = 0;
        õis1.Rotation = 0;
        õis2.Rotation = 0;
        õis3.Rotation = 0;

        // Asukoht tagasi algseks
        puuTüvi.TranslationX = 0;
        lehestik.TranslationX = 0;
        õis1.TranslationX = 0;
        õis2.TranslationX = 0;
        õis3.TranslationX = 0;

        puuTüvi.TranslationY = 0;
        lehestik.TranslationY = 0;
        õis1.TranslationY = 0;
        õis2.TranslationY = 0;
        õis3.TranslationY = 0;

        // Skaala tagasi algseks
        puuTüvi.Scale = 1.0;
        lehestik.Scale = 1.0;
        õis1.Scale = 1.0;
        õis2.Scale = 1.0;
        õis3.Scale = 1.0;
    }

    // --- TEGEVUSED ---

    // Kasva: puu muutub suuremaks
    private async Task Kasva()
    {
        uint kiirus = (uint)kiirusStepper.Value;

        // Taastame algseisu
        TaastaAlgseis();

        // Kasvatame puud juhusliku suurusega
        double scale = 1.0 + (rnd.NextDouble() * 0.8); // 1.0 - 1.8
        tegevusLabel.Text = $"Puu kasvab! (skaala: {scale:F2})";
        tegevusLabel.TextColor = Colors.Black;

        await Task.WhenAll(
            puuTüvi.ScaleTo(scale, kiirus),
            lehestik.ScaleTo(scale, kiirus),
            õis1.ScaleTo(scale, kiirus),
            õis2.ScaleTo(scale, kiirus),
            õis3.ScaleTo(scale, kiirus)
        );

        tegevusLabel.Text = "Puu on kasvanud!";
    }

    // Õitse: õied muutuvad nähtavaks
    private async Task Õitse()
    {
        uint kiirus = (uint)kiirusStepper.Value;

        // Taastame algseisu (pööre ja asukoht)
        TaastaAlgseis();

        // Näitame õisi
        õis1.IsVisible = true;
        õis2.IsVisible = true;
        õis3.IsVisible = true;

        õis1.Opacity = 0;
        õis2.Opacity = 0;
        õis3.Opacity = 0;

        await Task.WhenAll(
            õis1.FadeTo(1, kiirus),
            õis2.FadeTo(1, kiirus),
            õis3.FadeTo(1, kiirus)
        );

        tegevusLabel.Text = "Puu õitseb! 🌸";
        tegevusLabel.TextColor = Colors.Black;
    }

    // Värise: puu liigub edasi-tagasi
    private async Task Värise()
    {
        uint kiirus = (uint)kiirusStepper.Value;

        // Taastame algseisu (pööre ja skaala)
        TaastaAlgseis();

        for (int i = 0; i < 3; i++)
        {
            await Task.WhenAll(
                puuTüvi.TranslateTo(-15, 0, kiirus / 2),
                lehestik.TranslateTo(-15, 0, kiirus / 2),
                õis1.TranslateTo(-15, 0, kiirus / 2),
                õis2.TranslateTo(-15, 0, kiirus / 2),
                õis3.TranslateTo(-15, 0, kiirus / 2)
            );

            await Task.WhenAll(
                puuTüvi.TranslateTo(15, 0, kiirus / 2),
                lehestik.TranslateTo(15, 0, kiirus / 2),
                õis1.TranslateTo(15, 0, kiirus / 2),
                õis2.TranslateTo(15, 0, kiirus / 2),
                õis3.TranslateTo(15, 0, kiirus / 2)
            );
        }

        // Tagasi algasendisse
        await Task.WhenAll(
            puuTüvi.TranslateTo(0, 0, kiirus / 2),
            lehestik.TranslateTo(0, 0, kiirus / 2),
            õis1.TranslateTo(0, 0, kiirus / 2),
            õis2.TranslateTo(0, 0, kiirus / 2),
            õis3.TranslateTo(0, 0, kiirus / 2)
        );

        tegevusLabel.Text = "Puu värises tuules! 🍃";
        tegevusLabel.TextColor = Colors.Black;
    }

    // Langeta: puu kukub külili (ainult talvel ja valgel ajal)
    private async Task Langeta()
    {
        // Taastame algseisu enne kontrolli
        TaastaAlgseis();

        // 1. Kontrollime aastaaega (kuupäev)
        int kuu = kuupäevPicker.Date.Value.Month;
        if (kuu != 12 && kuu != 1 && kuu != 2)
        {
            tegevusLabel.Text = "Puid tohib langetada ainult talvel! (detsember, jaanuar, veebruar)";
            tegevusLabel.TextColor = Colors.Red;
            return;
        }

        // 2. Kontrollime kellaaega (päevavalgus)
        int tund = kellaaegPicker.Time.Value.Hours;
        if (tund < 8 || tund > 17)
        {
            tegevusLabel.Text = "Puid tohib langetada ainult valgel ajal (08:00 - 17:00)!";
            tegevusLabel.TextColor = Colors.Red;
            return;
        }

        // 3. Kõik tingimused on täidetud – langetame puu
        uint kiirus = (uint)kiirusStepper.Value;

        tegevusLabel.Text = "Puu langetatakse! 🪓";
        tegevusLabel.TextColor = Colors.Black;

        await Task.WhenAll(
            puuTüvi.RotateTo(90, kiirus),
            lehestik.RotateTo(90, kiirus),
            õis1.RotateTo(90, kiirus),
            õis2.RotateTo(90, kiirus),
            õis3.RotateTo(90, kiirus)
        );

        tegevusLabel.Text = "Puu on langetatud!";
    }
}