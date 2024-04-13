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
        private void SwitchTurn(object parameter)
        {
            game.SwitchTurn();
        }

        //imi iau 2 variabile pt click... cand prima e null, o modific si afisez pozitiile highlighted si dupa la al doilea click fac miscarea
        bool firstClick = false, secondClick = false;
        int firstRow, firstColumn, secondRow, secondColumn;
        private void CellClick(object parameter)
        {
            // Logic for handling click on the cell
            Cell clickedCell = (Cell)parameter;

            if (!firstClick)
            {
                firstClick = true;
                firstRow = clickedCell.Row;
                firstColumn = clickedCell.Column;
            }
            else if(firstClick && !secondClick)
            {
                secondClick = true;
                secondRow = clickedCell.Row;
                secondColumn = clickedCell.Column;
            }

            //if(firstClick && !secondClick)
            //{ if(game.getPiece(firstRow, firstColumn).Color == EPiece.empty)
            //  { firstClick = false; secondClick = false; firstRow=-1, firstColumn=-1, secondRow=-1, secondColumn=-1 return; }


            //daca primul click e null modific primele coordonate, altfel, daca al doilea click e null, modific al doilea click si afisez mutarile posibile


            if (firstClick && !secondClick)
            {
                if (game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black)
                {
                    game.ShowPotentialMoves(firstRow, firstColumn);
                }
                else if (!game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White)
                {
                    game.ShowPotentialMoves(firstRow, firstColumn);
                }
                else
                {
                    firstClick = false;
                    secondClick = false;
                    return;
                }
                UpdateCells();
            }

            if (firstClick && secondClick)
            {
                if (game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black)
                {
                    game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                    UpdateCells();
                    firstClick = false;
                    secondClick = false;
                    firstRow = -1;
                    firstColumn = -1;
                    secondRow = -1;
                    secondColumn = -1;
                    //verificare pentru mutare multipla
                    //if(firstClick != secondClick)
                    //SwitchTurn(null);
                }
                else if (!game.IsBlackTurn && game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White)
                {
                    game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                    UpdateCells();
                    firstClick = false;
                    secondClick = false;
                    firstRow = -1;
                    firstColumn = -1;
                    secondRow = -1;
                    secondColumn = -1;
                    //verificare pentru mutare multipla
                    //if(firstClick != secondClick)
                    //SwitchTurn(null);
                }
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