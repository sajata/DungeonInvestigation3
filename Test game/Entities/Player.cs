using System;
using Microsoft.Xna.Framework;

namespace Test_game.Entities
{
    //Creates player with default glyph '@'
    public class Player : Actor
    {
        public Player(Color foreground, Color background) : base(foreground, background, '@')
        {
            //Setting the players base stats and name
            Attack = 10;
            AttackChance = 40;
            Defense = 5;
            DefenseChance = 20;
            Health = 100;
            Name = "Tony"; // All SadConsole entities have a Name variable by default
        }
    }
}
