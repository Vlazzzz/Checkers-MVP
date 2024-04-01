using System;

namespace tema2_MVP_Dame
{
    internal class Player
    {
        public string Name { get; set; } // Numele jucătorului
        public PieceColor Color { get; set; } // Culoarea pieselor (alb sau roșu)
        public int RemainingPieces { get; set; } // Numărul de piese rămase pe tablă pentru acest jucător

        public Player(string name, PieceColor color)
        {
            Name = name;
            Color = color;
            RemainingPieces = 12; // Presupunând că fiecare jucător începe cu 12 piese
        }
    }
}