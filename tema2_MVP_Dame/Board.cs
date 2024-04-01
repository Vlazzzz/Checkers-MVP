using System;

namespace tema2_MVP_Dame
{
    public class Board
    {
        private readonly Piece[,] squares; // Matricea de celule pentru tabla de joc

        public Board()
        {
            squares = new Piece[8, 8]; // Inițializarea tablei de joc cu 8 linii și 8 coloane
            InitializeBoard(); // Inițializarea pieselor pe tabla de joc
        }

        private void InitializeBoard()
        {
            // Inițializarea pieselor albe pe prima și a opta linie
            for (int col = 0; col < 8; col += 2)
            {
                squares[0, col] = new Piece(PieceColor.White);
                squares[2, col] = new Piece(PieceColor.White);
                squares[6, col] = new Piece(PieceColor.Red);
            }

            // Inițializarea pieselor albe pe a doua linie
            for (int col = 1; col < 8; col += 2)
            {
                squares[1, col] = new Piece(PieceColor.White);
                squares[5, col] = new Piece(PieceColor.Red);
                squares[7, col] = new Piece(PieceColor.Red);
            }
        }

        // Metoda pentru a verifica dacă o anumită mutare este validă
        public bool IsValidMove(int startX, int startY, int endX, int endY)
        {
            // Implementarea verificării legalității mutării
            // (sărim peste piese proprii, sărim în afara tablei, etc.)
            // Returnează true dacă mutarea este validă, false în caz contrar.
            return false;
        }

        // Metoda pentru a efectua o mutare a piesei
        public void MakeMove(int startX, int startY, int endX, int endY)
        {
            // Implementarea efectuării mutării piesei pe tabla de joc
            // (actualizarea matricei de celule)
        }
    }
}