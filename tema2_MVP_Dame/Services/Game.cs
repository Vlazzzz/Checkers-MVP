using System.IO;
using System.Windows;
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
            ResetHighlightedCells();
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
                     GameBoard.GetPiece(sourcePieceRow - 1, sourcePieceColumn - 1).Color == EPiece.BlackKing))
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

            if (IsGameOver() == EPiece.Black)
            {
                MessageBox.Show("Black wins", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateWinHistoryInFile("D:\\facultate\\II\\SEM II\\MVP\\tema1_dictionar\\Checkers-MVP\\tema2_MVP_Dame\\Resources\\win_history.txt");
                //GameBoard.ResetBoard();
                //IsBlackTurn = true;
            }
            else if (IsGameOver() == EPiece.White)
            {
                MessageBox.Show("White wins", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateWinHistoryInFile("D:\\facultate\\II\\SEM II\\MVP\\tema1_dictionar\\Checkers-MVP\\tema2_MVP_Dame\\Resources\\win_history.txt");
                //GameBoard.ResetBoard();
                //IsBlackTurn = true;
            }

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

        public void UpdateWinHistoryInFile(string filename)
        {
            // Read the win history from the file
            string winHistoryTxt = File.ReadAllText(filename);

            int numberOfWhiteWins, numberOfBlackWins;
            //the first element from the file is the number of white wins, and the second one is the number of black wins
            string[] winHistoryArray = winHistoryTxt.Split(' ');
            numberOfWhiteWins = int.Parse(winHistoryArray[0]);
            numberOfBlackWins = int.Parse(winHistoryArray[1]);

            // Call IsGameOver to determine the winning player
            EPiece winningPlayer = IsGameOver();

            if (winningPlayer == EPiece.Black)
                numberOfBlackWins++;
            else if (winningPlayer == EPiece.White)
                numberOfWhiteWins++;

            // Write the updated win history back to the file, which are the two int values, one for white wins, one for black wins
            string updatedWinHistory = numberOfWhiteWins + " " + numberOfBlackWins;
            File.WriteAllText(filename, updatedWinHistory);
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

        public EPiece IsGameOver()
        {
            //implement the function that checks if the game is over
            //the game is over when one of the players has no more pieces on the board
            int whitePiecesCounter = 0, blackPiecesCounter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (GameBoard.GetPiece(i, j).Color == EPiece.Black ||
                        GameBoard.GetPiece(i, j).Color == EPiece.BlackKing)
                    {
                        blackPiecesCounter++;
                    }
                    else if (GameBoard.GetPiece(i, j).Color == EPiece.White ||
                             GameBoard.GetPiece(i, j).Color == EPiece.WhiteKing)
                    {
                        whitePiecesCounter++;
                    }
                }
            }

            if (blackPiecesCounter == 0)
            {
                return EPiece.White;
            }

            if (whitePiecesCounter == 0)
            {
                return EPiece.Black;
            }

            //if there are still pieces on the table, we can check if any of the players has all of his pieces blocked by the other player so the game is over
            int whiteHighlightedPiecesCounter = 0, blackHighlightedPiecesCounter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (GameBoard.GetPiece(i, j).Color == EPiece.Black ||
                        GameBoard.GetPiece(i, j).Color == EPiece.BlackKing)
                    {
                        ShowPotentialMoves(i, j);
                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 0; l < 8; l++)
                            {
                                if (GameBoard.GetPiece(k, l).Color == EPiece.IsHighlighted)
                                {
                                    blackHighlightedPiecesCounter++;
                                }
                            }
                        }
                    }
                    else if (GameBoard.GetPiece(i, j).Color == EPiece.White ||
                             GameBoard.GetPiece(i, j).Color == EPiece.WhiteKing)
                    {
                        ShowPotentialMoves(i, j);
                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 0; l < 8; l++)
                            {
                                if (GameBoard.GetPiece(k, l).Color == EPiece.IsHighlighted)
                                {
                                    whiteHighlightedPiecesCounter++;
                                }
                            }
                        }
                    }
                }
            }

            ResetHighlightedCells();

            //if the black player forced the white one the first to have all his pieces blocked, the black player wins
            if(IsBlackTurn)
            {
                if (whiteHighlightedPiecesCounter == 0)
                {
                    return EPiece.Black;
                }
            }
            else
            {
                if (blackHighlightedPiecesCounter == 0)
                {
                    return EPiece.White;
                }
            }
            
            return EPiece.Empty;
        }
    }
}

//bug: daca apas pe orice linie incepand cu a doua si ultima coloana si am piesa alba rege, da crash
//bug: nu pot captura piese cu piesele rege pe ultima coloana
//nu pot captura in sus stanga piesa neagra rege cu piesa alba rege; dupa ce nu pot captura piesa, in urmatoarea runda imi apare ca highlight pozitia de mai devreme unde ar fi trebuit sa mut
//la fel si pentru piesa neagra rege;
//BUG PENTRU POZITIA URMATOARE: daca piesa alba se afla stanga sus si piesa neagra se afla in dreapta jos nu pot captura piesa alba cu piesa neagra
//                              daca mut in 2 mutari dreapta sus si piesa neagra stanga jos, o sa pot captura piesa alba cu piesa neagra DAR
//                              DAR, voi putea face si mutarea pe care nu o puteam face mai devreme, lucru care nu ar trebui sa se intample



//!!! MAI AM DEIMPLEMENTAT URMATOARELE: LA INCEPUT VOI PUTEA ALEGE DACA VREAU SA JOC CU SARITURI MULTIPLE SAU NU(DACA NU, VOI DEZACTIVA BUTONUL DE END TURN DE sus stanga), lucru pe care il voi
//                                                                          tine minte si intr-un fisier pentru a pastra aceasta setare pentru urmatoarea rulare a jocului
//                                      TREBUIE SA AFISEZ CATE PIESE MAI ARE FIECARE JUCATOR
//                                      DE ELIMINAT DIN CONSTRUCTOR jucatorii(PLAYER1 si PLAYER2) si de sters clasa PLAYER