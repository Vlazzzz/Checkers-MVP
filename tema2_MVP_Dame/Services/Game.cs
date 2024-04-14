using System.IO;
using System.Xml;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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

        public void ResetHighlightedCells()
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

        private void ShowKingPotentialMoves(int sourcePieceRow, int sourcePieceColumn)
        {
            var piece = GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn);

            ResetHighlightedCells();

            // Check if the source piece is on the left edge of the board
            if (sourcePieceColumn == 0)
            {
                if (sourcePieceRow + 1 < 8 &&
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                }
            }
            // Check if the source piece is on the right edge of the board
            else if (sourcePieceColumn == 7)
            {
                if (sourcePieceRow + 1 < 8 &&
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                }
            }
            // Otherwise, check both directions
            else
            {
                if (sourcePieceRow + 1 < 8 && sourcePieceColumn + 1 < 8 &&
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                }

                if (sourcePieceRow + 1 < 8 && sourcePieceColumn - 1 >= 0 &&
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                }
            }
            //FUNCTIE SEPARATA DE VERIFICARE A CAPTURILOR

            // Check if the source piece is on the left edge of the board
            if (sourcePieceColumn == 0)
            {
                if (sourcePieceRow - 1 >= 0 &&
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                    return;
                }
            }

            // Check if the source piece is on the right edge of the board
            if (sourcePieceColumn == 7)
            {
                if (sourcePieceRow - 1 >= 0 &&
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                    return;
                }
            }

            // Otherwise, check both directions
            if (sourcePieceRow - 1 >= 0 && sourcePieceColumn + 1 < 8 &&
                GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.Empty)
            {
                GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
            }

            if (sourcePieceRow - 1 >= 0 && sourcePieceColumn - 1 >= 0 &&
                GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.Empty)
            {
                GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
            }

            CapturePieceCheck(sourcePieceRow, sourcePieceColumn);
        }

        public bool CapturePieceCheck(int sourcePieceRow, int sourcePieceColumn)
        {
            // Check for capturing opponent's pieces
            if (GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn).Color == EPiece.WhiteKing)
            {
                if (sourcePieceRow + 1 < 8 && sourcePieceColumn + 1 < 8 &&
                    (GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Black ||
                     GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.BlackKing))
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn + 2 < 8 &&
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow + 1 < 8 && sourcePieceColumn - 1 >= 0 &&
                    (GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Black ||
                     GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.BlackKing))
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn - 2 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn + 1 < 8 &&
                    (GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.Black ||
                     GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.BlackKing))
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn + 2 < 8 &&
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn - 1 >= 0 &&
                    (GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.Black ||
                     GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.BlackKing))
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn - 2 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }
            }
            else if (GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn).Color == EPiece.BlackKing)
            {
                if (sourcePieceRow + 1 < 8 && sourcePieceColumn + 1 < 8 &&
                    (GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.White ||
                     GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.WhiteKing))
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn + 2 < 8 &&
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow + 1 < 8 && sourcePieceColumn - 1 >= 0 &&
                    (GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.White ||
                     GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.WhiteKing))
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn - 2 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn + 1 < 8 &&
                    (GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.White ||
                     GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.WhiteKing))
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn + 2 < 8 &&
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn - 1 >= 0 &&
                    (GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.White ||
                     GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.WhiteKing))
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn - 2 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }
            }
            else if (GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn).Color == EPiece.White)
            {
                if (sourcePieceRow + 1 < 8 && sourcePieceColumn + 1 < 8 &&
                    (GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Black ||
                    GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.BlackKing))
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn + 2 < 8 &&
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow + 1 < 8 && sourcePieceColumn - 1 >= 0 &&
                    (GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Black ||
                     GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.BlackKing))
                {
                    if (sourcePieceRow + 2 < 8 && sourcePieceColumn - 2 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }
            }
            else if (GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn).Color == EPiece.Black)
            {
                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn + 1 < 8 &&
                    (GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.White ||
                     GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.WhiteKing))
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn + 2 < 8 &&
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn + 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn - 1 >= 0 &&
                    (GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.White ||
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.WhiteKing))
                {
                    if (sourcePieceRow - 2 >= 0 && sourcePieceColumn - 2 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 2, sourcePieceColumn - 2).Color = EPiece.IsHighlighted;
                        return true;
                    }
                }
            }
            return false;
        }

        void ShowNormalPiecePotentialMoves(int sourcePieceRow, int sourcePieceColumn)
        {
            Piece piece = GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn);
            if (piece.Color == EPiece.White)
            {
                // Check if the source piece is on the left edge of the board
                if (sourcePieceColumn == 0)
                {
                    if (sourcePieceRow + 1 < 8 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color ==
                        EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                    }
                }
                // Check if the source piece is on the right edge of the board
                else if (sourcePieceColumn == 7)
                {
                    if (sourcePieceRow + 1 < 8 && GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color ==
                        EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                    }
                }
                // Otherwise, check both directions
                else
                {
                    if (sourcePieceRow + 1 < 8 && sourcePieceColumn + 1 < 8 &&
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                    }

                    if (sourcePieceRow + 1 < 8 && sourcePieceColumn - 1 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow + 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                    }
                }
            }
            else if (piece.Color == EPiece.Black)
            {
                // Check if the source piece is on the left edge of the board
                if (sourcePieceColumn == 0)
                {
                    if (sourcePieceRow - 1 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                        return;
                    }
                }

                // Check if the source piece is on the right edge of the board
                if (sourcePieceColumn == 7)
                {
                    if (sourcePieceRow - 1 >= 0 &&
                        GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                    {
                        GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                        return;
                    }
                }

                // Otherwise, check both directions
                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn + 1 < 8 &&
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn + 1).Color = EPiece.IsHighlighted;
                }

                if (sourcePieceRow - 1 >= 0 && sourcePieceColumn - 1 >= 0 &&
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.Empty)
                {
                    GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color = EPiece.IsHighlighted;
                }
            }
        }

        public void ShowPotentialMoves(int sourcePieceRow, int sourcePieceColumn)
        {
            var piece = GameBoard.GetPiece(sourcePieceRow, sourcePieceColumn);

            ResetHighlightedCells();
            if (piece.Color == EPiece.WhiteKing || piece.Color == EPiece.BlackKing)
                ShowKingPotentialMoves(sourcePieceRow, sourcePieceColumn);
            else if (piece.Color == EPiece.White || piece.Color == EPiece.Black)
                ShowNormalPiecePotentialMoves(sourcePieceRow, sourcePieceColumn);

            CapturePieceCheck(sourcePieceRow, sourcePieceColumn);
        }

        private void CheckIfKing()
        {
            for (int i = 0; i < 8; i++)
            {
                if (GameBoard.GetPiece(0, i).Color == EPiece.Black)
                    GameBoard.GetPiece(0, i).Color = EPiece.BlackKing;
                if (GameBoard.GetPiece(7, i).Color == EPiece.White)
                    GameBoard.GetPiece(7, i).Color = EPiece.WhiteKing;
            }
        }

        public bool MovePiece(int sourcePieceRow, int sourcePieceColumn, int targetPieceRow, int targetPieceColumn)
        {
            ResetHighlightedCells();
            if (!IsValidMove(sourcePieceRow, sourcePieceColumn, targetPieceRow, targetPieceColumn))
                return false;

            GameBoard.MovePiece(sourcePieceRow, sourcePieceColumn, targetPieceRow, targetPieceColumn);

            //daca piesa captureaza o piesa de culoarea opusa, ea dispare de pe Board
            if (targetPieceRow == sourcePieceRow + 2 || targetPieceRow == sourcePieceRow - 2)
            {
                GameBoard.RemovePiece((sourcePieceRow + targetPieceRow) / 2,
                    (sourcePieceColumn + targetPieceColumn) / 2);
            }

            CheckIfKing();

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
                                                                  targetPieceColumn == sourcePieceColumn - 2)))
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
            else if (sourcePiece.Color == EPiece.BlackKing || sourcePiece.Color == EPiece.WhiteKing)
            {
                if (targetPiece.Color == EPiece.Empty)
                {
                    if ((targetPieceRow == sourcePieceRow + 1 && (targetPieceColumn == sourcePieceColumn + 1 ||
                                                                  targetPieceColumn == sourcePieceColumn - 1))
                        ||
                        (targetPieceRow == sourcePieceRow + 2 && (targetPieceColumn == sourcePieceColumn + 2 ||
                                                                  targetPieceColumn == sourcePieceColumn - 2)))
                    {
                        return true;
                    }

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

        public void ResetGame()
        {
            //reset the gameboard
            GameBoard.ResetBoard();
            IsBlackTurn = true;
        }

        public void SaveGame(string filePath)
        {
            // Create a new instance of Game to store the game information
            Game savedGame = new Game();

            // Add information about each piece on the board to the saved game instance
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Piece piece = GameBoard.GetPiece(row, column);
                    savedGame.GameBoard.SetPiece(row, column, piece.Color);
                }
            }

            savedGame.IsBlackTurn = IsBlackTurn;

            // Serialize the current game instance and save the data to the specified file in JSON format
            try
            {
                string json = JsonConvert.SerializeObject(savedGame, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json);

                Console.WriteLine("Game saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving the game: {ex.Message}");
            }
        }


        public void LoadGame(string filePath)
        {
            try
            {
                // Read the JSON data from the file
                string jsonData = File.ReadAllText(filePath);

                // Parse the JSON data manually
                JObject jsonObject = JObject.Parse(jsonData);

                // Populate the GameBoard manually
                JArray boardArray = (JArray)jsonObject["GameBoard"];
                for (int i = 0; i < boardArray.Count; i++)
                {
                    int color = (int)boardArray[i]["Color"];
                    int row = i / 8;
                    int column = i % 8;
                    GameBoard.SetPiece(row, column, (EPiece)color);
                }

                // Set the IsBlackTurn property
                bool isBlackTurn = (bool)jsonObject["IsBlackTurn"];
                IsBlackTurn = isBlackTurn;

                Console.WriteLine("Game loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading the game: {ex.Message}");
            }
        }
    }
}