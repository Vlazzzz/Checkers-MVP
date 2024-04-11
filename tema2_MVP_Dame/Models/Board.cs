﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace tema2_MVP_Dame.Models
{
    public class Board : IEnumerable<Piece>
    {
        private readonly Piece[,] squares;

        public Board()
        {
            squares = new Piece[8, 8];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Inițializarea celulelor tablei de joc
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    squares[row, col] = new Piece(EPiece.Empty);
                }
            }

            // Inițializarea pieselor albe pe prima și a opta linie
            for (int col = 1; col < 8; col += 2)
            {
                squares[0, col] = new Piece(EPiece.White);
                squares[2, col]= new Piece(EPiece.White);
                squares[6, col]= new Piece(EPiece.Black);
            }

            // Inițializarea pieselor albe pe a doua linie
            for (int col = 0; col < 8; col += 2)
            {
                squares[1, col] = new Piece(EPiece.White);
                squares[5, col] = new Piece(EPiece.Black);
                squares[7, col] = new Piece(EPiece.Black);
            }
        }

        public IEnumerator<Piece> GetEnumerator()
        {
            foreach (Piece piece in squares)
            {
                yield return piece;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void MovePiece(int souceRow, int sourceColumn, int targetRow, int targetColumn)
        {
            //daca mutarea este vida, muta piesa
            //targetCell.Piece.Color = sourceCell.Piece.Color;
            //sourceCell.Piece.Color = EPiece.Empty;
        }

        public Piece GetPiece(int row, int column)
        {
            return squares[row, column];
        }
    }
}