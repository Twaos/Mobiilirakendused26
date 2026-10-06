using System;

namespace Mobiilirakendused
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // See käivitub, kui vajutad "Click me" nuppu
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            // 1. Uuendame nupu teksti
            if (count == 1)
                CounterBtn.Text = $"Vajutatud {count} kord";
            else
                CounterBtn.Text = $"Vajutatud {count} korda";

            // 2. Pöörame pilti 15 kraadi iga vajutusega
            BotImage.Rotation += 15;

            // 3. Kui loendur on 10 või rohkem, peidame pildi
            if (count >= 10)
            {
                BotImage.IsVisible = false;
                CounterLabel.Text = "Pilt kadus ära! Vajuta Reset.";
            }
            else
            {
                CounterLabel.Text = $"Nuppu on vajutatud kokku: {count}";
            }

            // 4. Kui loendur on 5 või rohkem, muudame nupu punaseks
            if (count >= 5)
            {
                CounterBtn.BackgroundColor = Colors.Red;
                CounterBtn.TextColor = Colors.White;
            }

            // 5. Juhuslik värv Reset nupule
            var random = new Random();
            var randomColor = Color.FromRgb(
                random.Next(0, 256),
                random.Next(0, 256),
                random.Next(0, 256)
            );
            ResetBtn.BackgroundColor = randomColor;

            // 6. Iseseisev ülesanne: Muudame pildi läbipaistvust (Opacity)
            if (BotImage.Opacity > 0.1)
            {
                BotImage.Opacity -= 0.1;
            }

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        // See käivitub, kui vajutad "Tagasi nulli" nuppu
        private void OnResetClicked(object sender, EventArgs e)
        {
            // Nullime loenduri
            count = 0;
            CounterBtn.Text = "Click me";
            CounterLabel.Text = "Alustame uuesti!";

            // Keerame pildi tagasi algasendisse
            BotImage.Rotation = 0;

            // Toome pildi tagasi nähtavale
            BotImage.IsVisible = true;

            // Muudame nupu värvi tagasi siniseks
            CounterBtn.BackgroundColor = Colors.Blue;
            CounterBtn.TextColor = Colors.White;

            // 5. Ülesanne: Liigutame pilti vasakule/paremale
            if (BotImage.HorizontalOptions == LayoutOptions.Start)
            {
                BotImage.HorizontalOptions = LayoutOptions.End;
            }
            else
            {
                BotImage.HorizontalOptions = LayoutOptions.Start;
            }

            // Taastame läbipaistvuse
            BotImage.Opacity = 1.0;

            // Taastame Reset nupu värvi
            ResetBtn.BackgroundColor = Colors.Red;
        }
    }
}