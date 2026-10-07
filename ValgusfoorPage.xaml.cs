using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;

namespace Mobiilirakendused;

public partial class ValgusfoorPage : ContentPage
{
    private Ellipse punaneTuli;
    private Ellipse kollaneTuli;
    private Ellipse rohelineTuli;
    private Label pealkiriLabel;
    private bool foorSees = false;
    private bool autoSees = false;
    private CancellationTokenSource autoCts;

    public ValgusfoorPage()
    {
        InitializeComponent();

        pealkiriLabel = new Label
        {
            Text = "Vali valgus",
            FontSize = 30,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Black,
            FontAttributes = FontAttributes.Bold
        };

        // Loome tuleelemendid eraldi
        punaneTuli = new Ellipse
        {
            WidthRequest = 200,
            HeightRequest = 200,
            Fill = Colors.Gray,
            Stroke = Colors.DarkGray,
            StrokeThickness = 2
        };
        var punaneGrid = LooTuliGrid("punane", Colors.Red, punaneTuli);

        kollaneTuli = new Ellipse
        {
            WidthRequest = 200,
            HeightRequest = 200,
            Fill = Colors.Gray,
            Stroke = Colors.DarkGray,
            StrokeThickness = 2
        };
        var kollaneGrid = LooTuliGrid("kollane", Colors.Yellow, kollaneTuli);

        rohelineTuli = new Ellipse
        {
            WidthRequest = 200,
            HeightRequest = 200,
            Fill = Colors.Gray,
            Stroke = Colors.DarkGray,
            StrokeThickness = 2
        };
        var rohelineGrid = LooTuliGrid("roheline", Colors.Green, rohelineTuli);

        var sisseBtn = new Button
        {
            Text = "SISSE",
            FontSize = 14,                    
            Padding = new Thickness(10, 8),   
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HorizontalOptions = LayoutOptions.Fill
        };
        sisseBtn.Clicked += SisseBtn_Clicked;

        var valjaBtn = new Button
        {
            Text = "VÄLJA",
            FontSize = 14,                    
            Padding = new Thickness(10, 8),   
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HorizontalOptions = LayoutOptions.Fill
        };
        valjaBtn.Clicked += ValjaBtn_Clicked;

        var autoBtn = new Button
        {
            Text = "AUTO",
            FontSize = 14,                    
            Padding = new Thickness(10, 8),   
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            CornerRadius = 10,
            HorizontalOptions = LayoutOptions.Fill
        };
        autoBtn.Clicked += AutoBtn_Clicked;

        var nupudLayout1 = new HorizontalStackLayout
        {
            Spacing = 20,
            HorizontalOptions = LayoutOptions.Center,
            Children = { sisseBtn, valjaBtn }
        };

        var nupudLayout2 = new HorizontalStackLayout
        {
            Spacing = 20,
            HorizontalOptions = LayoutOptions.Center,
            Children = { autoBtn }
        };

        var nupudLayout = new HorizontalStackLayout
        {
            Spacing = 10,                     
            HorizontalOptions = LayoutOptions.Center,
            Children = { sisseBtn, valjaBtn, autoBtn }
        };

        var põhiLayout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 20,
            VerticalOptions = LayoutOptions.Center,
            Children =
    {
        pealkiriLabel,
        punaneGrid,
        kollaneGrid,
        rohelineGrid,
        nupudLayout
    }
        };

        Content = põhiLayout;
        UuendaFooriVarvid();
    }


    private Grid LooTuliGrid(string nimi, Color aktiivneVarv, Ellipse tuli)
    {
        var grid = new Grid
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 200,
            HeightRequest = 200
        };

        var tekst = new Label
        {
            Text = nimi,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black,
            FontSize = 18
        };

        grid.Children.Add(tuli);
        grid.Children.Add(tekst);

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (sender, e) =>
        {
            if (!foorSees) return;

            // Taastame kõik värvid algseks
            punaneTuli.Fill = Colors.Gray;
            kollaneTuli.Fill = Colors.Gray;
            rohelineTuli.Fill = Colors.Gray;

            // Muudame selle tule värvi
            tuli.Fill = aktiivneVarv;

            // Muudame pealkirja
            if (nimi == "punane")
            {
                pealkiriLabel.Text = "Seisa";
                pealkiriLabel.TextColor = Colors.Red;
            }
            else if (nimi == "kollane")
            {
                pealkiriLabel.Text = "Valmista";
                pealkiriLabel.TextColor = Colors.Orange;
            }
            else if (nimi == "roheline")
            {
                pealkiriLabel.Text = "Sõida";
                pealkiriLabel.TextColor = Colors.Green;
            }

            // Väike animatsioon: suurenda ja tuhmista, siis taasta
            await Task.WhenAll(
                tuli.ScaleTo(1.2, 150),
                tuli.FadeTo(0.5, 150)
            );
            await Task.WhenAll(
                tuli.ScaleTo(1.0, 150),
                tuli.FadeTo(1.0, 150)
            );
        };

        grid.GestureRecognizers.Add(tap);
        return grid;
    }

    private void SisseBtn_Clicked(object sender, EventArgs e)
    {
        autoSees = false;
        autoCts?.Cancel();
        foorSees = true;
        pealkiriLabel.Text = "Vali valgus";
        pealkiriLabel.TextColor = Colors.Black;
        UuendaFooriVarvid();
    }

    private void ValjaBtn_Clicked(object sender, EventArgs e)
    {
        autoSees = false;
        autoCts?.Cancel();
        foorSees = false;
        pealkiriLabel.Text = "Lülita esmalt foor sisse";
        pealkiriLabel.TextColor = Colors.Black;
        UuendaFooriVarvid();
    }

    private async void AutoBtn_Clicked(object sender, EventArgs e)
    {
        if (autoSees)
        {
            // Peatame automaatrežiimi
            autoSees = false;
            autoCts?.Cancel();
            pealkiriLabel.Text = "Automaat väljas";
            pealkiriLabel.TextColor = Colors.Black;
            UuendaFooriVarvid();
        }
        else
        {
            // Käivitame automaatrežiimi
            foorSees = true;
            autoSees = true;
            autoCts = new CancellationTokenSource();
            pealkiriLabel.Text = "Automaatrežiim...";
            pealkiriLabel.TextColor = Colors.Black;

            try
            {
                while (autoSees && !autoCts.Token.IsCancellationRequested)
                {
                    // Punane
                    UuendaFooriVarvid();
                    punaneTuli.Fill = Colors.Red;
                    pealkiriLabel.Text = "Seisa";
                    pealkiriLabel.TextColor = Colors.Red;
                    await Task.Delay(2000, autoCts.Token);

                    // Kollane
                    UuendaFooriVarvid();
                    kollaneTuli.Fill = Colors.Yellow;
                    pealkiriLabel.Text = "Valmista";
                    pealkiriLabel.TextColor = Colors.Orange;
                    await Task.Delay(1000, autoCts.Token);

                    // Roheline
                    UuendaFooriVarvid();
                    rohelineTuli.Fill = Colors.Green;
                    pealkiriLabel.Text = "Sõida";
                    pealkiriLabel.TextColor = Colors.Green;
                    await Task.Delay(2000, autoCts.Token);
                }
            }
            catch (TaskCanceledException)
            {
                // Tsükkel peatati, see on OK
            }
        }
    }

    private void UuendaFooriVarvid()
    {
        punaneTuli.Fill = Colors.Gray;
        kollaneTuli.Fill = Colors.Gray;
        rohelineTuli.Fill = Colors.Gray;
    }
}