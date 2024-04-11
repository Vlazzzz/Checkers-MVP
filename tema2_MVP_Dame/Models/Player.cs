using System;

namespace tema2_MVP_Dame.Models
{
    public class Player
    {
        public string Name { get; set; } // Numele jucătorului
        public EPiece Color { get; set; } // Culoarea pieselor (alb sau roșu)

        public Player(string name, EPiece color)
        {
            Name = name;
            Color = color;
        }
    }
}