using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ICommandDemoAgain.Commands;
using tema2_MVP_Dame.Models;

namespace tema2_MVP_Dame.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Game game;
        public ICommand CellClickCommand { get; private set; }
        public ICommand SwitchTurnCommand { get; private set; }
        public static bool CanExecute() => true;

        public ObservableCollection<Cell> Cells { get; set; }

        //fac o functie updateCells care sa imi faca update la observable collection de fiecare data cand se schimba ceva in game

        public MainViewModel()
        {
            // Create an instance of GameViewModel
            game = new Game();

            // Initialize the observable collection of cells with the cells from the game board
            Cells = new ObservableCollection<Cell>();
            CellClickCommand = new RelayCommand(CellClick);
            SwitchTurnCommand = new RelayCommand(SwitchTurn);
            //game.MovePiece(2, 1, 3, 2);
            //game.ShowPotentialMoves(2, 3);
            UpdateCells();
        }

        //imi iau 2 variabile pt click... cand prima e null, o modific si afisez pozitiile highlighted si dupa la al doilea click fac miscarea
        bool firstClick = false, secondClick = false;
        int firstRow, firstColumn, secondRow, secondColumn; 
        

        private void SwitchTurn(object parameter)
        {
            game.SwitchTurn();
            firstClick = false;
            secondClick = false;
            firstRow = -1;
            firstColumn = -1;
            secondRow = -1;
            secondColumn = -1;
        }

        private bool okMoved = false;
        private void CellClick(object parameter)
        {
            // Logic for handling click on the cell
            Cell clickedCell = (Cell)parameter;
            if (!firstClick)
            {
                firstRow = clickedCell.Row;
                firstColumn = clickedCell.Column;
                firstClick = true;
            }

            else if (firstClick && !secondClick)
            {
                secondRow = clickedCell.Row;
                secondColumn = clickedCell.Column;
                secondClick = true;
            }

            if (firstRow == secondRow && firstColumn == secondColumn)
            {
                firstClick = false;
                secondClick = false;
            }

            if (firstClick && !secondClick)
            {
                if (game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black)
                    game.ShowPotentialMoves(firstRow, firstColumn);
                else if (!game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White)
                    game.ShowPotentialMoves(firstRow, firstColumn);
                else
                {
                    firstClick = false;
                    secondClick = false;
                    firstRow = -1;
                    firstColumn = -1;
                    secondRow = -1;
                    secondColumn = -1;
                }
            }

            if (firstClick && secondClick)
            {
                if (game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black)
                {
                    game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                    okMoved = true;
                    firstClick = false;
                    secondClick = false;
                    firstRow = -1;
                    firstColumn = -1;
                    secondRow = -1;
                    secondColumn = -1;
                }

                if (!game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White)
                {
                    game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                    okMoved = true;
                    firstClick = false;
                    secondClick = false;
                    firstRow = -1;
                    firstColumn = -1;
                    secondRow = -1;
                }
            }

            if (okMoved)
            {
                game.SwitchTurn();
                okMoved = false;
            }
            UpdateCells();
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
                        Cells.Add(new Cell(i, j, "/Resources/x_shape.png"));
                    }
                }
            }
        }
    }

}

// de implementat remiza