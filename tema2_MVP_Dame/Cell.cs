namespace tema2_MVP_Dame
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Piece Piece { get; set; } // Represents the piece on the cell (if any)

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            Piece = new Piece(PieceColor.Empty); // Initially no piece on the cell
        }
    }
}