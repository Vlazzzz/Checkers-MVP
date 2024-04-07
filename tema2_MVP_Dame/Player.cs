using System;

namespace tema2_MVP_Dame
{
    public class Player
    {
        public string Name { get; set; } // Numele jucătorului
        public PieceColor Color { get; set; } // Culoarea pieselor (alb sau roșu)

        public Player(string name, PieceColor color)
        {
            Name = name;
            Color = color;
        }
    }
}