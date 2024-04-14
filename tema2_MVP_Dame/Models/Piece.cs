namespace tema2_MVP_Dame.Models
{
    public class Piece
    {
        public EPiece Color { get; set; }

        public Piece(EPiece color)
        {
            Color = color;
        }
    }
}