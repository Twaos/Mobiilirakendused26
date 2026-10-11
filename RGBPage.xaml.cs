using System;
using Microsoft.Maui.Layouts;

namespace Mobiilirakendused;

public partial class RGBPage : ContentPage
{
    Slider redSlider, greenSlider, blueSlider;
    Label redLabel, greenLabel, blueLabel;
    BoxView colorBox;
    AbsoluteLayout al;
    Random rnd = new Random();

    public RGBPage()
    {
        InitializeComponent();

        // Pealkiri
        var pealkiri = new Label
        {
            Text = "RGB mudel",
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        // Värvi kast
        colorBox = new BoxView
        {
            Color = Colors.Black,
            CornerRadius = 20
        };

        // Red liugur ja label
        redSlider = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 0,
            MinimumTrackColor = Colors.Red,
            MaximumTrackColor = Colors.LightGray,
            ThumbColor = Colors.Red
        };
        redSlider.ValueChanged += OnSliderValueChanged;
        redLabel = new Label { Text = "Red = 00", TextColor = Colors.Red, HorizontalOptions = LayoutOptions.Center };

        // Green liugur ja label
        greenSlider = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 0,
            MinimumTrackColor = Colors.Green,
            MaximumTrackColor = Colors.LightGray,
            ThumbColor = Colors.Green
        };
        greenSlider.ValueChanged += OnSliderValueChanged;
        greenLabel = new Label { Text = "Green = 00", TextColor = Colors.Green, HorizontalOptions = LayoutOptions.Center };

        // Blue liugur ja label
        blueSlider = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 0,
            MinimumTrackColor = Colors.Blue,
            MaximumTrackColor = Colors.LightGray,
            ThumbColor = Colors.Blue
        };
        blueSlider.ValueChanged += OnSliderValueChanged;
        blueLabel = new Label { Text = "Blue = 00", TextColor = Colors.Blue, HorizontalOptions = LayoutOptions.Center };

        // Juhuslik värv
        var randomBtn = new Button
        {
            Text = "Juhuslik värv",
            BackgroundColor = Colors.DarkGray,
            TextColor = Colors.White,
            CornerRadius = 10
        };
        randomBtn.Clicked += RandomBtn_Clicked;

        // Paigutus AbsoluteLayout abil
        al = new AbsoluteLayout
        {
            Children = { pealkiri, colorBox, redSlider, redLabel, greenSlider, greenLabel, blueSlider, blueLabel, randomBtn }
        };

        AbsoluteLayout.SetLayoutBounds(pealkiri, new Rect(0.5, 0.05, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(pealkiri, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(colorBox, new Rect(0.5, 0.35, 300, 200));
        AbsoluteLayout.SetLayoutFlags(colorBox, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(redSlider, new Rect(0.5, 0.55, 300, 40));
        AbsoluteLayout.SetLayoutFlags(redSlider, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(redLabel, new Rect(0.5, 0.60, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(redLabel, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(greenSlider, new Rect(0.5, 0.66, 300, 40));
        AbsoluteLayout.SetLayoutFlags(greenSlider, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(greenLabel, new Rect(0.5, 0.71, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(greenLabel, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(blueSlider, new Rect(0.5, 0.77, 300, 40));
        AbsoluteLayout.SetLayoutFlags(blueSlider, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(blueLabel, new Rect(0.5, 0.82, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(blueLabel, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(randomBtn, new Rect(0.5, 0.92, 200, 50));
        AbsoluteLayout.SetLayoutFlags(randomBtn, AbsoluteLayoutFlags.PositionProportional);

        Content = al;
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (sender == redSlider)
        {
            redLabel.Text = $"Red = {(int)args.NewValue:X2}";
        }
        else if (sender == greenSlider)
        {
            greenLabel.Text = $"Green = {(int)args.NewValue:X2}";
        }
        else if (sender == blueSlider)
        {
            blueLabel.Text = $"Blue = {(int)args.NewValue:X2}";
        }

        colorBox.Color = Color.FromRgb(
            (int)redSlider.Value,
            (int)greenSlider.Value,
            (int)blueSlider.Value
        );
    }

    private async void RandomBtn_Clicked(object sender, EventArgs e)
    {
        // Juhuslikud väärtused
        int r = rnd.Next(0, 256);
        int g = rnd.Next(0, 256);
        int b = rnd.Next(0, 256);

        // Anname liuguritele uued väärtused
        redSlider.Value = r;
        greenSlider.Value = g;
        blueSlider.Value = b;

        // Väike animatsioon
        await colorBox.ScaleTo(1.1, 100);
        await colorBox.ScaleTo(1.0, 100);
    }
}