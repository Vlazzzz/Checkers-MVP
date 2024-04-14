﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
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
            SaveGameCommand = new RelayCommand(SaveGame);
            LoadGameCommand = new RelayCommand(LoadGame);
            ResetGameCommand = new RelayCommand(ResetGame);
            //game.MovePiece(2, 1, 3, 2);
            //game.ShowPotentialMoves(2, 3);
            UpdateCells();
        }

        private void ResetGame(object obj)
        {
            game.ResetGame();
            UpdateCells();
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
                ResetClicks();
                UpdateCells();
                game.ResetHighlightedCells();
            }

            if (firstClick && !secondClick)
            {
                if (game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black || game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.BlackKing))
                    game.ShowPotentialMoves(firstRow, firstColumn);
                else if (!game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White || game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.WhiteKing))
                    game.ShowPotentialMoves(firstRow, firstColumn);
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

            //daca e tura lui negru si apas pe negru si dupa pe alb, desi nu am mutat nimic, se schimba tura. cum rezolv problema asta?

            if (firstClick && secondClick)
            {
                if(game.GameBoard.GetPiece(secondRow, secondColumn).Color == EPiece.IsHighlighted)
                {
                    if (game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.Black || 
                                             game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.BlackKing))
                    {
                        game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                        //verificare daca a fost o capturare pentru a vedea daca mai am mutari posibile
                        if (firstRow == secondRow + 2 || firstRow == secondRow - 2)
                        {
                            //daca pot face capturari multiple, nu schimb tura si resetez al doilea click astfel incat sa pot continua capturarea
                            if (game.CapturePieceCheck(secondRow, secondColumn) != false)
                            {
                                okMoved = false;
                                //firstClick = false;
                                secondClick = false;
                                firstRow = secondRow;
                                firstColumn = secondColumn;
                                secondRow = -1;
                                secondColumn = -1;
                            }
                            else
                            {
                                ResetClicks();
                            }
                        }
                        //daca nu pot face capturari multiple, schimb tura si resetez clickurile
                        else
                        {
                            okMoved = true;
                            ResetClicks();
                        }
                    }
                    //verificare daca a fost o capturare pentru a vedea daca mai am mutari posibile
                    if (!game.IsBlackTurn && (game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.White || 
                                              game.GameBoard.GetPiece(firstRow, firstColumn).Color == EPiece.WhiteKing))
                    {
                        game.MovePiece(firstRow, firstColumn, secondRow, secondColumn);
                        if (firstRow == secondRow + 2 || firstRow == secondRow - 2)
                        {
                            //daca pot face capturari multiple, nu schimb tura si resetez al doilea click astfel incat sa pot continua capturarea
                            if (game.CapturePieceCheck(secondRow, secondColumn) != false)
                            {
                                okMoved = false;
                                //firstClick = false;
                                secondClick = false;
                                firstRow = secondRow;
                                firstColumn = secondColumn;
                                secondRow = -1;
                                secondColumn = -1;
                            }
                            else
                            {
                                ResetClicks();
                            }
                        }
                        //daca nu pot face capturari multiple, schimb tura si resetez clickurile
                        else
                        {
                            okMoved = true;
                            ResetClicks();
                        }
                    }
                }
            }

            if (okMoved)
            {
                game.SwitchTurn();
                okMoved = false;
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
                        Cells.Add(new Cell(i, j, "/Resources/highlight_2.jpg"));
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

// de implementat remiza