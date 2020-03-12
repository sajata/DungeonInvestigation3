using System;
using Microsoft.Xna.Framework;

//Abstract class for all actors in the game
//Eg monsters and players

namespace Test_game.Entities
{
    public abstract class Actor : Entity
    {
        public int Health { get; set; } //current health
        public int Attack { get; set; } // attack strength i.e how many times it can attack in one go
        public int AttackChance { get; set; } // percent chance of successful hit
        public int Defense { get; set; } // defensive strength i.e how many times it can defend in one go
        public int DefenseChance { get; set; } // percent chance of successfully blocking a hit        

        protected Actor(Color foreground, Color background, int glyph, int width = 1, int length = 1) : base(foreground, background, width, length, glyph)
        {
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;
        }

        //Moves actor the BY the positionCHange value in any X/Y direction
        //returns true if able to move, false if cant
        public bool MoveBy(Point positionChange)
        {
            //Check the current map if actor can move to the new postion
            if (GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange))
            {
                Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + positionChange); // gets the monster at the postion actor wants to move to

                //if there is a monster on that tile
                if (monster != null)
                {
                    GameLoop.CommandManager.Attack(this, monster); // executes Attack method of the command manager with this actor(attacker) and the monster(defender)
                    return true;
                }

                Position += positionChange;
                return true;
            }
            else
            {
                return false;
            }
        }

        //Moves actor TO the newPosition location
        public bool MoveTo(Point newPosition)
        {
            Position = newPosition;
            return true;
        }
    }
}
