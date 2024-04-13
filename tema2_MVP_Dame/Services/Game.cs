namespace tema2_MVP_Dame.Models
{
    public class Game
    {
        public Board GameBoard { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public bool IsBlackTurn { get; internal set; }

        public Game()
        {
            // Inițializează tabla de joc
            GameBoard = new Board();
            IsBlackTurn = true; //black always starts
        }
        public void SwitchTurn()
        {
            IsBlackTurn = !IsBlackTurn;
        }

        private void ResetHighlightedCells()
        {
            // Reset previously highlighted cells back to their original color
            foreach (var piece in GameBoard)
            {
                if (piece.Color == EPiece.IsHighlighted)
                {
                    piece.Color = EPiece.Empty;
                }
            }
        }

        public void ShowPotentialMoves(int sourcePieceRow, int sourcePieceColumn)
        {
            var piece = GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn);

            ResetHighlightedCells();

            if (piece.Color == EPiece.White)
            {
                // Check if the source piece is on the left edge of the board
                if (sourcePieceColumn == 0)
                {
                    if (sourcePieceRow + 1 < 8 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                    }
                }
                // Check if the source piece is on the right edge of the board
                else if (sourcePieceColumn == 7)
                {
                    if (sourcePieceRow + 1 < 8 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                    }
                }
                // Otherwise, check both directions
                else
                {
                    if (sourcePieceRow + 1 < 8 && sourcePieceColumn + 1 < 8 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                    }

                    if (sourcePieceRow + 1 < 8 && sourcePieceColumn - 1 >= 0 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                    }
                }

                // Check for capturing opponent's pieces
                if (sourcePieceRow + 1 < 8 && sourcePieceColumn + 1 < 8 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Black)
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn + 2 < 8 && GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                    }
                }

                if (sourcePieceRow + 1 < 8 && sourcePieceColumn - 1 >= 0 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Black)
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn - 2 >= 0 && GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                    }
                }
            }
            else if (piece.Color == EPiece.Black)
            {
                // Check if the source piece is on the left edge of the board
                if (sourcePieceColumn == 0)
                {
                    if (sourcePieceRow - 1 >= 0 && GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                        return;
                    }
                }

                // Check if the source piece is on the right edge of the board
                if (sourcePieceColumn == 7)
                {
                    if (sourcePieceRow - 1 >= 0 && GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                        return;
                    }
                }

                // Otherwise, check both directions
                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn + 1 < 8 && GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn - 1 >= 0 && GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn + 1 < 8 && GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.White)
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn + 2 < 8 && GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                    }
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn - 1 >= 0 && GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.White)
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn - 2 >= 0 && GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                    }
                }
            }
        }


        public bool MovePiece(int sourcePieceRow, int sourcePieceColumn, int targetPieceRow, int targetPieceColumn)
        {
            ResetHighlightedCells();
            // Verifică dacă mutarea este validă
            if (!IsValidMove(sourcePieceRow, sourcePieceColumn, targetPieceRow, targetPieceColumn))
            {
                // Mutarea nu este validă, returnează false
                return false;
            }

            GameBoard.MovePiece(sourcePieceRow, sourcePieceColumn, targetPieceRow, targetPieceColumn);
            //daca piesa captureaza o piesa de culoarea opusa, ea dispare de pe Board
            if (targetPieceRow == sourcePieceRow + 2 || targetPieceRow == sourcePieceRow - 2)
            {
                GameBoard.GetPiece((sourcePieceRow + targetPieceRow) / 2, (sourcePieceColumn + targetPieceColumn) / 2).Color = EPiece.Empty;
            }

            // Actualizează starea jocului (ex: verifică capturi, schimbă jucătorul curent etc.)

            return true; // Mutarea a fost efectuată cu succes
        }

        private bool IsValidMove(int sourcePieceRow, int sourcePieceColumn, int targetPieceRow, int targetPieceColumn)
        {
            // Implementează logica de validare a mutării aici
            // Verifică dacă mutarea este în limitele tablei, dacă este o mutare validă pentru piesa respectivă etc.

            Piece sourcePiece = GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn);
            Piece targetPiece = GameBoard.GetPiece(targetPieceRow, targetPieceColumn);

            if (sourcePiece.Color == EPiece.White)
            {
                if (targetPiece.Color == EPiece.Empty)
                {
                    if ((targetPieceRow == sourcePieceRow + 1 && (targetPieceColumn == sourcePieceColumn + 1 ||
                                                                 targetPieceColumn == sourcePieceColumn - 1)) 
                        || 
                        (targetPieceRow == sourcePieceRow + 2 && (targetPieceColumn == sourcePieceColumn + 2 ||
                                                                  targetPieceColumn == sourcePieceColumn -2)))
                    {
                        return true;
                    }
                }
            }
            else if (sourcePiece.Color == EPiece.Black)
            {
                if (targetPiece.Color == EPiece.Empty)
                {
                    if ((targetPieceRow == sourcePieceRow - 1 && (targetPieceColumn == sourcePieceColumn + 1 ||
                                                                 targetPieceColumn == sourcePieceColumn - 1)) 
                        ||
                        (targetPieceRow == sourcePieceRow - 2 && (targetPieceColumn == sourcePieceColumn + 2 ||
                                                                  targetPieceColumn == sourcePieceColumn - 2)))
                    {
                        return true;
                    }
                }
            }

            //verifica daca source cell si target cel sunt in dimensiunile tablei
            if (sourcePieceRow < 0 || sourcePieceRow > 7 || sourcePieceColumn < 0 || sourcePieceColumn > 7)
            {
                return false;
            }

            if (targetPieceRow < 0 || targetPieceRow > 7 || targetPieceColumn < 0 || targetPieceColumn > 7)
            {
                return false;
            }

            return false; // Returnează true dacă mutarea este validă, altfel false
        }
    }
}