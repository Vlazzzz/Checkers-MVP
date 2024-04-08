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

        private void ResetHighlightedCells()
        {
            // Reset previously highlighted cells back to their original color
            foreach (Cell cell in GameBoard)
            {
                if (cell.Piece.Color == PieceColor.IsHighlighted)
                {
                    cell.Piece.Color = PieceColor.Empty;
                }
            }
        }
        public void ShowPotentialMoves(Cell sourceCell)
        {
            //make a function that shows the potential moves of a piece if it is white or black
            //if the piece is white, it can move only down and to the left or right
            //if the piece is black, it can move only up and to the left or right

            ResetHighlightedCells();

            if (sourceCell.Piece.Color == PieceColor.White)
            {
                if (sourceCell.Row + 1 < 8 && sourceCell.Column + 1 < 8)
                {
                    if (GameBoard.GetPiece(sourceCell.Row + 1, sourceCell.Column + 1).Color == PieceColor.Empty)
                    {
                        GameBoard.GetPiece(sourceCell.Row + 1, sourceCell.Column + 1).Color = PieceColor.IsHighlighted;
                    }
                }

                if (sourceCell.Row + 1 < 8 && sourceCell.Column - 1 >= 0)
                {
                    if (GameBoard.GetPiece(sourceCell.Row + 1, sourceCell.Column - 1).Color == PieceColor.Empty)
                    {
                        GameBoard.GetPiece(sourceCell.Row + 1, sourceCell.Column - 1).Color = PieceColor.IsHighlighted;
                    }
                }
            }
            else if (sourceCell.Piece.Color == PieceColor.Black)
            {
                if (sourceCell.Row - 1 >= 0 && sourceCell.Column + 1 < 8)
                {
                    if (GameBoard.GetPiece(sourceCell.Row - 1, sourceCell.Column + 1).Color == PieceColor.Empty)
                    {
                        GameBoard.GetPiece(sourceCell.Row - 1, sourceCell.Column + 1).Color = PieceColor.IsHighlighted;
                    }
                }

                if (sourceCell.Row - 1 >= 0 && sourceCell.Column - 1 >= 0)
                {
                    if (GameBoard.GetPiece(sourceCell.Row - 1, sourceCell.Column - 1).Color == PieceColor.Empty)
                    {
                        GameBoard.GetPiece(sourceCell.Row - 1, sourceCell.Column - 1).Color = PieceColor.IsHighlighted;
                    }
                }
            }

        }

        public bool MovePiece(Cell sourceCell, Cell targetCell)
        {
            ResetHighlightedCells();
            // Verifică dacă mutarea este validă
            if (!IsValidMove(sourceCell, targetCell))
            {
                // Mutarea nu este validă, returnează false
                return false;
            }

            // Efectuează mutarea piesei în cadrul tablei de joc
            //DE IMPLEMENTAT IN BOARD!!!!!!!!!
            GameBoard.MovePiece(sourceCell, targetCell);

            // Actualizează starea jocului (ex: verifică capturi, schimbă jucătorul curent etc.)

            return true; // Mutarea a fost efectuată cu succes
        }

        private bool IsValidMove(Cell sourceCell, Cell targetCell)
        {
            // Implementează logica de validare a mutării aici
            // Verifică dacă mutarea este în limitele tablei, dacă este o mutare validă pentru piesa respectivă etc.

            if (sourceCell.Piece.Color == PieceColor.White)
            {
                if (targetCell.Piece == null)
                {
                    if (targetCell.Row == sourceCell.Row + 1 && (targetCell.Column == sourceCell.Column + 1 ||
                                                                 targetCell.Column == sourceCell.Column - 1))
                    {
                        return true;
                    }
                }
            }
            else if (sourceCell.Piece.Color == PieceColor.Black)
            {
                if (targetCell.Piece == null)
                {
                    if (targetCell.Row == sourceCell.Row - 1 && (targetCell.Column == sourceCell.Column + 1 ||
                                                                 targetCell.Column == sourceCell.Column - 1))
                    {
                        return true;
                    }
                }
            }

            //verifica daca source cell si target cel sunt in dimensiunile tablei
            if (sourceCell.Row < 0 || sourceCell.Row > 7 || sourceCell.Column < 0 || sourceCell.Column > 7)
            {
                return false;
            }

            if (targetCell.Row < 0 || targetCell.Row > 7 || targetCell.Column < 0 || targetCell.Column > 7)
            {
                return false;
            }

            return false; // Returnează true dacă mutarea este validă, altfel false
        }
    }
}