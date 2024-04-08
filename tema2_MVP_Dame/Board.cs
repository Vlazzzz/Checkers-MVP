using System;
using System.Collections;
using System.Collections.Generic;

namespace tema2_MVP_Dame
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] squares;

        public Board()
        {
            squares = new Cell[8, 8];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Inițializarea celulelor tablei de joc
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    squares[row, col] = new Cell(row, col);
                }
            }

            // Inițializarea pieselor albe pe prima și a opta linie
            for (int col = 1; col < 8; col += 2)
            {
                squares[0, col].Piece = new Piece(PieceColor.White);
                squares[2, col].Piece = new Piece(PieceColor.White);
                squares[6, col].Piece = new Piece(PieceColor.Black);
            }

            // Inițializarea pieselor albe pe a doua linie
            for (int col = 0; col < 8; col += 2)
            {
                squares[1, col].Piece = new Piece(PieceColor.White);
                squares[5, col].Piece = new Piece(PieceColor.Black);
                squares[7, col].Piece = new Piece(PieceColor.Black);
            }
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            foreach (Cell cell in squares)
            {
                yield return cell;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void MovePiece(Cell sourceCell, Cell targetCell)
        {
            //daca mutarea este vida, muta piesa
            targetCell.Piece.Color = sourceCell.Piece.Color;
            sourceCell.Piece.Color = PieceColor.Empty;
        }

        public Piece GetPiece(int row, int column)
        {
            return squares[row, column].Piece;
        }

        //implement get cell method
        public Cell GetCell(int row, int column)
        {
            return squares[row, column];
        }
    }
}