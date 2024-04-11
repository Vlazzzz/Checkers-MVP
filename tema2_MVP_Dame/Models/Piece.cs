namespace tema2_MVP_Dame.Models
{
    public class Piece
    {
        public EPiece Color { get; set; } // Culoarea piesei (alb sau roșu)

        public Piece(EPiece color)
        {
            Color = color;
        }
    }
}