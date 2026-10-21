namespace Mobiilirakendused.GameLogic
{
    public class Card
    {
        public string Symbol { get; set; } 
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }

        public Card(string symbol)
        {
            Symbol = symbol;
            IsFlipped = false;
            IsMatched = false;
        }
    }
}