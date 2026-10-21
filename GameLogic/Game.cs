using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobiilirakendused.GameLogic
{
    public class Game
    {
        public List<Card> Cards { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public int PairsFound { get; private set; }
        public int TotalPairs { get; private set; }

        public Game(Player player)
        {
            CurrentPlayer = player;
            Cards = new List<Card>();
        }

        // Algatab mängu (loob kaardid)
        public void StartGame(int totalPairs = 8)
        {
            TotalPairs = totalPairs;
            PairsFound = 0;
            CurrentPlayer.Score = 0;
            CurrentPlayer.Moves = 0;
            Cards.Clear();

            // Emojid, mida kasutame
            string[] symbols = { "🍎", "🍌", "🍇", "🍓", "🍒", "🍑", "🍍", "🥝" };

            // Võtame vajalikul hulgal sümboleid
            var selected = symbols.Take(totalPairs).ToList();

            // Loome iga sümboli jaoks 2 kaarti
            foreach (var symbol in selected)
            {
                Cards.Add(new Card(symbol));
                Cards.Add(new Card(symbol));
            }

            // Segame kaardid
            Cards = Cards.OrderBy(c => Guid.NewGuid()).ToList();
        }

        // Kontrollib, kas kaks kaarti on paar
        public bool CheckMatch(Card first, Card second)
        {
            if (first.Symbol == second.Symbol && first != second)
            {
                first.IsMatched = true;
                second.IsMatched = true;
                PairsFound++;
                CurrentPlayer.AddScore(10);
                return true;
            }
            return false;
        }

        // Kontrollib, kas mäng on läbi
        public bool IsGameOver()
        {
            return PairsFound == TotalPairs;
        }
    }
}