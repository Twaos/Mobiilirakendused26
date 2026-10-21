using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Mobiilirakendused.GameLogic
{
    public class Theme
    {
        public string Name { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Color CardBackColor { get; set; }
        public Color CardFrontColor { get; set; }
        public string FontFamily { get; set; }

        public Theme(string name, Color bg, Color text, Color cardBack, Color cardFront, string font)
        {
            Name = name;
            BackgroundColor = bg;
            TextColor = text;
            CardBackColor = cardBack;
            CardFrontColor = cardFront;
            FontFamily = font;
        }

        // Rakendab teema lehele
        public void Apply(ContentPage page)
        {
            page.BackgroundColor = BackgroundColor;
        }
    }
}