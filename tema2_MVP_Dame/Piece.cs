
namespace tema2_MVP_Dame
{
    public class Piece
    {
        public string ImagePath { get; set; }
        public PieceColor Color { get; set; } // Culoarea piesei (alb sau roșu)

        public Piece(PieceColor color)
        {
            Color = color;
            if (color == PieceColor.White)
            {
                ImagePath = "D:\\facultate\\II\\SEM II\\MVP\\tema2_MVP_Dame\\Resurse\\white_piece.png";
            }
            else if (color == PieceColor.Black)
            {
                ImagePath = "D:\\facultate\\II\\SEM II\\MVP\\tema2_MVP_Dame\\Resurse\\black_piece.png";
            }
            else
            {
                ImagePath = "D:\\facultate\\II\\SEM II\\MVP\\tema2_MVP_Dame\\Resurse\\empty.png";
            }
        }
    }
}