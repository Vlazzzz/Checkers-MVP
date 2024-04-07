namespace tema2_MVP_Dame
{
    public class Game
    {
        public Board GameBoard { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public Game()
        {
            // Inițializează tabla de joc
            GameBoard = new Board();
        }
    }
}