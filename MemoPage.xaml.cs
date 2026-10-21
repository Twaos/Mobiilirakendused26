using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Mobiilirakendused.GameLogic;

namespace Mobiilirakendused;

public partial class MemoPage : ContentPage
{
    private Game game;
    private Player player;
    private List<Button> cardButtons;
    private Button firstFlipped;
    private Button secondFlipped;
    private bool isChecking = false;

    // Teemad
    private Theme currentTheme;
    private Theme lightTheme;
    private Theme darkTheme;
    private Theme colorfulTheme;

    public MemoPage()
    {
        InitializeComponent();

        // Mängija
        player = new Player("Mängija");

        // Mäng
        game = new Game(player);

        // Kaardid
        cardButtons = new List<Button>();

        // Teemad
        lightTheme = new Theme("Hele", Colors.White, Colors.Black, Colors.LightGray, Colors.White, null);
        darkTheme = new Theme("Tume", Colors.Black, Colors.White, Colors.DarkGray, Colors.Gray, null);
        colorfulTheme = new Theme("Värviline", Colors.LightPink, Colors.DarkBlue, Colors.Orange, Colors.Yellow, null);

        currentTheme = lightTheme;

        // Alustame mängu
        StartNewGame();
    }

    // Alustab uue mängu
    private void StartNewGame()
    {
        game.StartGame(8);
        CreateBoard();
        UpdateStats();
    }

    // Loob mängulaua (4x4)
    private void CreateBoard()
    {
        GameBoard.Children.Clear();
        GameBoard.RowDefinitions.Clear();
        GameBoard.ColumnDefinitions.Clear();
        cardButtons.Clear();

        int size = 4; // 4x4

        for (int i = 0; i < size; i++)
        {
            GameBoard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            GameBoard.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        for (int i = 0; i < game.Cards.Count; i++)
        {
            var card = game.Cards[i];
            var button = new Button
            {
                Text = "?",
                FontSize = 30,
                BackgroundColor = currentTheme.CardBackColor,
                TextColor = currentTheme.TextColor,
                CornerRadius = 10,
                BorderWidth = 2,
                BorderColor = Colors.Gray,
                BindingContext = card // Seome kaardi andmed nupuga
            };

            button.Clicked += Card_Clicked;

            int row = i / size;
            int col = i % size;
            GameBoard.Add(button, col, row);

            cardButtons.Add(button);
        }
    }

    // Kaardi klõps
    private async void Card_Clicked(object sender, EventArgs e)
    {
        if (isChecking) return;

        var button = sender as Button;
        var card = button.BindingContext as Card;

        if (card == null || card.IsMatched || card.IsFlipped) return;

        // Pöörame kaardi
        card.IsFlipped = true;
        button.Text = card.Symbol;
        button.BackgroundColor = currentTheme.CardFrontColor;
        button.TextColor = Colors.Black;

        // Kui see on esimene kaart
        if (firstFlipped == null)
        {
            firstFlipped = button;
            return;
        }

        // Teine kaart
        secondFlipped = button;
        player.AddMove();
        UpdateStats();

        var firstCard = firstFlipped.BindingContext as Card;
        var secondCard = secondFlipped.BindingContext as Card;

        isChecking = true;

        // Kontrollime paari
        if (game.CheckMatch(firstCard, secondCard))
        {
            // Õige paar
            firstFlipped.BackgroundColor = Colors.LightGreen;
            secondFlipped.BackgroundColor = Colors.LightGreen;
            firstFlipped.IsEnabled = false;
            secondFlipped.IsEnabled = false;
            firstFlipped = null;
            secondFlipped = null;
            isChecking = false;

            UpdateStats();

            // Kontrollime, kas mäng on läbi
            if (game.IsGameOver())
            {
                await DisplayAlertAsync("Palju õnne!", $"Leidsid kõik paarid!\nPunktid: {player.Score}\nKäigud: {player.Moves}", "Mängi uuesti");
                StartNewGame();
            }
        }
        else
        {
            // Vale paar – ootame 1 sekund ja pöörame tagasi
            await Task.Delay(1000);

            firstFlipped.Text = "?";
            firstFlipped.BackgroundColor = currentTheme.CardBackColor;
            firstFlipped.TextColor = currentTheme.TextColor;
            secondFlipped.Text = "?";
            secondFlipped.BackgroundColor = currentTheme.CardBackColor;
            secondFlipped.TextColor = currentTheme.TextColor;

            firstCard.IsFlipped = false;
            secondCard.IsFlipped = false;

            firstFlipped = null;
            secondFlipped = null;
            isChecking = false;
        }
    }

    // Uuendab statistikat
    private void UpdateStats()
    {
        ScoreLabel.Text = $"Punktid: {player.Score}";
        MovesLabel.Text = $"Käigud: {player.Moves}";
    }

    // Teema valik
    private void ThemeLight_Clicked(object sender, EventArgs e)
    {
        ApplyTheme(lightTheme);
    }

    private void ThemeDark_Clicked(object sender, EventArgs e)
    {
        ApplyTheme(darkTheme);
    }

    private void ThemeColorful_Clicked(object sender, EventArgs e)
    {
        ApplyTheme(colorfulTheme);
    }

    
    private void ApplyTheme(Theme theme)
    {
        currentTheme = theme;
        theme.Apply(this);

        // Uuendame kõikide kaartide värve
        foreach (var button in cardButtons)
        {
            var card = button.BindingContext as Card;

            if (card != null && !card.IsMatched)
            {
                if (card.IsFlipped)
                {
                    button.BackgroundColor = theme.CardFrontColor;
                }
                else
                {
                    button.BackgroundColor = theme.CardBackColor;
                    button.TextColor = theme.TextColor;
                }
            }
        }

        // Uuendame tekstide värvi
        ScoreLabel.TextColor = theme.TextColor;
        MovesLabel.TextColor = theme.TextColor;
    }

    // Restardi nupp
    private void RestartBtn_Clicked(object sender, EventArgs e)
    {
        StartNewGame();
    }
}