using System;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Test_game.Tiles
{
    /// <summary>
    /// Template for the tiles used in the game
    /// </summary>
    public abstract class BaseTile : SadConsole.Cell
    {
        //Bools accounting for tiles 
        //and movement allowance
        public bool IsBlockingMove;
        
        //Tiles Name
        protected string TileName;

        //Sets blockingMove and Sight to false by default
        //Sets a blank name by default
        //If no name is passed through as a parameter
        public BaseTile(Color foreground, Color background, int glyph, bool blockingMove = false, String Name = "") : base(foreground, background, glyph)
        {
            TileName = Name;
            IsBlockingMove = blockingMove;            
        }
    }
}

