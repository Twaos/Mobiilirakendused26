namespace Mobiilirakendused.GameLogic
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }

        public Player(string name)
        {
            Name = name;
            Score = 0;
            Moves = 0;
        }

        public void AddScore(int points)
        {
            Score += points;
        }

        public void AddMove()
        {
            Moves++;
        }
    }
}