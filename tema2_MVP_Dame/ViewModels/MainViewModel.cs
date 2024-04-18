using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using ICommandDemoAgain.Commands;
using Microsoft.Win32;
using Newtonsoft.Json;
using tema2_MVP_Dame.Models;

namespace tema2_MVP_Dame.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Game game;
        public ICommand CellClickCommand { get; private set; }
        public ICommand SwitchTurnCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }
        public ICommand LoadGameCommand { get; private set; }
        public ICommand ResetGameCommand { get; private set; }

        public ObservableCollection<Cell> Cells { get; set; }

        private string _winHistoryBlackText;
        public string WinHistoryBlackText
        {
            get { return _winHistoryBlackText; }
            set
            {
                _winHistoryBlackText = value;
                OnPropertyChanged(nameof(WinHistoryBlackText));
            }
        }

        private string _winHistoryWhiteText;
        public string WinHistoryWhiteText
        {
            get { return _winHistoryWhiteText; }
            set
            {
                _winHistoryWhiteText = value;
                OnPropertyChanged(nameof(WinHistoryWhiteText));
            }
        }

        private bool _allowMultipleJumpsCheck;
        public bool AllowMultipleJumpsCheck
        {
            get { return _allowMultipleJumpsCheck; }
            set
            {
                if (_allowMultipleJumpsCheck != value)
                {
                    _allowMultipleJumpsCheck = value;
                    OnPropertyChanged(nameof(AllowMultipleJumpsCheck));
                }
            }
        }
        
        private bool _allowMultipleJumpsRadioButtonState;
        public bool AllowMultipleJumpsRadioButtonState
        {
            get { return _allowMultipleJumpsRadioButtonState; }
            set
            {
                if (_allowMultipleJumpsRadioButtonState != value)
                {
                    _allowMultipleJumpsRadioButtonState = value;
                    OnPropertyChanged(nameof(AllowMultipleJumpsRadioButtonState));
                }
            }
        }

        private int _blackPiecesRemaining;
        public int BlackPiecesRemaining
        {
            get { return _blackPiecesRemaining; }
            set
            {
                if (_blackPiecesRemaining != value)
                {
                    _blackPiecesRemaining = value;
                    OnPropertyChanged(nameof(BlackPiecesRemaining));
                }
            }
        }

        private int _whitePiecesRemaining;
        public int WhitePiecesRemaining
        {
            get { return _whitePiecesRemaining; }
            set
            {
                if (_whitePiecesRemaining != value)
                {
                    _whitePiecesRemaining = value;
                    OnPropertyChanged(nameof(WhitePiecesRemaining));
                }
            }
        }

        private string _currentTurnImage;
        public string CurrentTurnImage
        {
            get { return _currentTurnImage; }
            set
            {
                if (_currentTurnImage != value)
                {
                    _currentTurnImage = value;
                    OnPropertyChanged(nameof(CurrentTurnImage));
                }
            }
        }

        //ALLOW MULTIPLE JUMPS poate fi activata doar daca inca nu am facut nicio mutare
        //daca am facut o mutare si apoi dau click pe AllowMultipleJumps, nu se va activa
        public MainViewModel()
        {
            game = new Game();

            Cells = new ObservableCollection<Cell>();
            CellClickCommand = new RelayCommand(CellClick);
            SwitchTurnCommand = new RelayCommand(SwitchTurn);
            SaveGameCommand = new RelayCommand(SaveGame);
            LoadGameCommand = new RelayCommand(LoadGame);
            ResetGameCommand = new RelayCommand(ResetGame);

            AllowMultipleJumpsCheck = false;
            AllowMultipleJumpsRadioButtonState = true;
            CurrentTurnImage = game.IsBlackTurn ? "/Resources/black_piece.png" : "/Resources/white_piece.png";

            WhitePiecesRemaining = game.white_pieces_remaining;
            BlackPiecesRemaining = game.black_pieces_remaining;

            LoadWinHistoryFromFile("../../../Resources/win_history.txt");

            UpdateCells();
        }

        private void LoadWinHistoryFromFile(string filename)
        {
            // Read the win history from the file
            string winHistoryTxt = File.ReadAllText(filename);

            int numberOfWhiteWins, numberOfBlackWins;
            //the first element from the file is the number of white wins, and the second one is the number of black wins
            string[] winHistoryArray = winHistoryTxt.Split(' ');
            numberOfWhiteWins = int.Parse(winHistoryArray[0]);
            numberOfBlackWins = int.Parse(winHistoryArray[1]);

            WinHistoryWhiteText = numberOfWhiteWins.ToString();
            WinHistoryBlackText = numberOfBlackWins.ToString();
        }

        private void ResetGame(object obj)
        {
            LoadWinHistoryFromFile("../../../Resources/win_history.txt");
            game.ResetGame();
            UpdateCells();
            CurrentTurnImage = game.IsBlackTurn ? "/Resources/black_piece.png" : "/Resources/white_piece.png";
            AllowMultipleJumpsCheck = false;
            AllowMultipleJumpsRadioButtonState = true;
            WhitePiecesRemaining = game.white_pieces_remaining;
            BlackPiecesRemaining = game.black_pieces_remaining;
        }

        private void SaveGame(object obj)
        {
            // Create a new instance of SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the file filters and dialog title
            saveFileDialog.Filter = "Fișiere JSON (*.json)|*.json|Toate fișierele (*.*)|*.*";
            saveFileDialog.Title = "Salvare Joc";

            // Display the dialog and check if the user clicked OK
            if (saveFileDialog.ShowDialog() == true)
            {
                // Get the selected file path chosen by the user
                string filePath = saveFileDialog.FileName;

                // Save the game state using the chosen file path
                game.SaveGame(filePath);
            }
        }


        private void LoadGame(object obj)
        {
            // Create an instance of OpenFileDialog to allow the user to select the JSON file
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the file filters and dialog title
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.Title = "Load Game";

            // Display the dialog and check if the user clicked OK
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                game.LoadGame(filePath);
            }
            UpdateCells();
            CurrentTurnImage = game.IsBlackTurn ? "/Resources/black_piece.png" : "/Resources/white_piece.png";
            BlackPiecesRemaining = game.black_pieces_remaining;
            WhitePiecesRemaining = game.white_pieces_remaining;
            AllowMultipleJumpsCheck = false;
            AllowMultipleJumpsRadioButtonState = true;
        }

        //imi iau 2 variabile pt click... cand prima e null, o modific si afisez pozitiile highlighted si dupa la al doilea click fac miscarea
        bool firstClick = false, secondClick = false;
        int firstRow, firstColumn, secondRow, secondColumn; 
        
        private void SwitchTurn(object parameter)
        {
            if(pieceCaptured)
            {
                game.SwitchTurn();
                CurrentTurnImage = game.IsBlackTurn ? "/Resources/black_piece.png" : "/Resources/white_piece.png";
                firstClick = false;
                secondClick = false;
                firstRow = -1;
                firstColumn = -1;
                secondRow = -1;
                secondColumn = -1;
                UpdateCells();
                pieceCaptured = false;
            }
        }

        private bool canSwitchTurn = false;
        private bool pieceCaptured = false;
        private void CellClick(object parameter)
        {
            // Logic for handling click on the cell
            Cell clickedCell = (Cell)parameter;
            if (!firstClick)
            {
                firstRow = clickedCell.Row;
                firstColumn = clickedCell.Column;
                firstClick = true;
                pieceCaptured = false;
            }
            else if (firstClick && !secondClick)
            {
                secondRow = clickedCell.Row;
                secondColumn = clickedCell.Column;
                secondClick = true;
            }

            //la mutarea multipla
            if (firstClick && secondClick && pieceCaptured)
            {
                if (game.GameBoard.GetPiece(secondRow, secondColumn).Color != EPiece.IsHighlighted)
                {
                    secondClick = false;
                    secondRow = -1;
                    secondColumn = -1;
                    return;
                }
            }

            if (firstRow == secondRow && firstColumn == secondColumn)
            { 
                //in cazul erorii cu miscarea multipla si switch turn
                ResetClicks();
                UpdateCells();
                game.ResetHighlightedCells();
            }

            if (firstClick && !secondClick)
            {
                if (game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black || game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.BlackKing))
                { 
                    game.ShowPotentialMoves(firstRow, firstColumn);
                }
                else if (!game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White || game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.WhiteKing))
                {
                    game.ShowPotentialMoves(firstRow, firstColumn);
                }
                else
                {
                    ResetClicks();
                }
            }
            //daca al doilea click e diferit de culoarea primei piese sau nu e highlighted, resetam al doilea click
            if (firstClick && secondClick)
            {
                if (game.GameBoard.GetPiece(secondRow, secondColumn).Color != EPiece.IsHighlighted)
                {
                    ResetClicks();
                    game.ResetHighlightedCells();
                }
            }

            if (firstClick && secondClick)
            {
                if(game.GameBoard.GetPiece(secondRow, secondColumn).Color == EPiece.IsHighlighted)
                {
                    if (game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black || 
                                             game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.BlackKing))
                    {
                        game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                        AllowMultipleJumpsRadioButtonState = false;
                        WhitePiecesRemaining = game.white_pieces_remaining;
                        //verificare daca a fost o capturare pentru a vedea daca mai am mutari posibile
                        if(AllowMultipleJumpsCheck)
                        {
                            if (firstRow == secondRow + 2 || firstRow == secondRow - 2)
                            {
                                //WhitePiecesRemaining--;
                                //daca pot face capturari multiple, nu schimb tura si resetez al doilea click astfel incat sa pot continua capturarea
                                if (game.CapturePieceCheck(secondRow, secondColumn) != false)
                                {
                                    pieceCaptured = true;
                                    canSwitchTurn = false;
                                    //firstClick = false;
                                    secondClick = false;
                                    firstRow = secondRow;
                                    firstColumn = secondColumn;
                                    secondRow = -1;
                                    secondColumn = -1;
                                }
                                else
                                {
                                    canSwitchTurn = true;
                                    ResetClicks();
                                }
                            }
                            //daca nu pot face capturari multiple, schimb tura si resetez clickurile
                            else
                            {
                                canSwitchTurn = true;
                                ResetClicks();
                            }
                        }
                        else
                        {
                            if (firstRow == secondRow + 2 || firstRow == secondRow - 2)
                            {
                                WhitePiecesRemaining = game.white_pieces_remaining;
                            }
                            canSwitchTurn = true;
                            ResetClicks();
                        }
                    }
                    //verificare daca a fost o capturare pentru a vedea daca mai am mutari posibile
                    if (!game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White || 
                                              game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.WhiteKing))
                    {
                        game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                        AllowMultipleJumpsRadioButtonState = false;
                        BlackPiecesRemaining = game.black_pieces_remaining;
                        if (AllowMultipleJumpsCheck)
                        {
                            if (firstRow == secondRow + 2 || firstRow == secondRow - 2)
                            {
                                //BlackPiecesRemaining--;
                                //daca pot face capturari multiple, nu schimb tura si resetez al doilea click astfel incat sa pot continua capturarea
                                if (game.CapturePieceCheck(secondRow, secondColumn) != false)
                                {
                                    pieceCaptured = true;
                                    canSwitchTurn = false;
                                    //firstClick = false;
                                    secondClick = false;
                                    firstRow = secondRow;
                                    firstColumn = secondColumn;
                                    secondRow = -1;
                                    secondColumn = -1;
                                }
                                else
                                {
                                    canSwitchTurn = true;
                                    ResetClicks();
                                }
                            }
                            //daca nu pot face capturari multiple, schimb tura si resetez clickurile
                            else
                            {
                                canSwitchTurn = true;
                                ResetClicks();
                            }
                        }
                        else
                        {
                            if (firstRow == secondRow + 2 || firstRow == secondRow - 2)
                            {
                                BlackPiecesRemaining = game.black_pieces_remaining;
                            }
                            canSwitchTurn = true;
                            ResetClicks();
                        }
                    }
                }
            }

            if (canSwitchTurn)
            {
                game.SwitchTurn();
                CurrentTurnImage = game.IsBlackTurn ? "/Resources/black_piece.png" : "/Resources/white_piece.png";
                canSwitchTurn = false;
            }

            UpdateCells();
        }

        void ResetClicks()
        {
            firstClick = false;
            secondClick = false;
            firstRow = -1;
            firstColumn = -1;
            secondRow = -1;
            secondColumn = -1;
        }

        void UpdateCells()
        {
            // Update the observable collection of cells with the cells from the game board
            Cells.Clear();
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var piece = game.GameBoard.GetPiece(i, j);
                    if (piece.Color == EPiece.Empty)
                    {
                        Cells.Add(new Cell(i, j, "/Resources/empty.png"));
                    }
                    else if (piece.Color == EPiece.White)
                    {
                        Cells.Add(new Cell(i, j, "/Resources/white_piece.png"));
                    }
                    else if (piece.Color == EPiece.Black)
                    {
                        Cells.Add(new Cell(i, j, "/Resources/black_piece.png"));
                    }
                    else if (piece.Color == EPiece.IsHighlighted)
                    {
                        Cells.Add(new Cell(i, j, "/Resources/highlight.jpg"));
                    }
                    else if (piece.Color == EPiece.WhiteKing)
                    {
                        Cells.Add(new Cell(i, j, "/Resources/white_king.png"));
                    }
                    else if (piece.Color == EPiece.BlackKing)
                    {
                        Cells.Add(new Cell(i, j, "/Resources/black_king.png"));
                    }
                }
            }
        }
    }

}