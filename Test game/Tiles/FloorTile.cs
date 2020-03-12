using System;
using SadConsole;
using Microsoft.Xna.Framework;

namespace Test_game.Tiles
{
    public class FloorTile : BaseTile
    {
        //Floor tile
        //Doesnt block movement
        public FloorTile(bool blockMovement = false) : base(Color.White, Color.Teal, '.', blockMovement)
        {
            TileName = "Floor";
        }
    }
}
