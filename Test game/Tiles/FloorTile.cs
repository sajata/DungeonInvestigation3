using System;
using SadConsole;
using Microsoft.Xna.Framework;

namespace Test_game.Tiles
{
    public class FloorTile : BaseTile
    {
        //Creates a floor tile
        //Doesnt block sight or movement
        public FloorTile(bool blockMovement = false, bool blockSight = false) : base(Color.White, Color.Teal, '.', blockMovement, blockSight)
        {
            TileName = "Floor";
        }
    }
}
