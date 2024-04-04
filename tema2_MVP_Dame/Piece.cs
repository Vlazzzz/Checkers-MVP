namespace tema2_MVP_Dame
{
    public class Piece
    {
        public PieceColor Color { get; set; } // Culoarea piesei (alb sau roșu)

        public Piece(PieceColor color)
        {
            Color = color;
        }
    }
}