using System;
using Microsoft.Xna.Framework;

namespace Test_game.Entities
{
    public class Monster : Actor
    {
        //Creates a monster with glyph 'M'
        public Monster(Color foreground, Color background) : base(foreground, background, 'M')
        {
            //Sets base stats and Name of Monster
            //For the pupose of this investigation all monsters 
            //have the same stats
            Attack = 10;
            AttackChance = 20;
            Defense = 5;
            DefenseChance = 20;
            Health = 5;
            Name = "Goblin";
        }
    }
}
