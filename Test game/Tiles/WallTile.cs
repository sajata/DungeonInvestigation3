using System;
using SadConsole;
using Microsoft.Xna.Framework;

namespace Test_game.Tiles
{
    public class WallTile : BaseTile
    {
        public WallTile(bool blockMovement = true, bool blockSight = true) : base(Color.Blue, Color.Navy, '#', blockMovement, blockSight)
        {
            TileName = "Wall";
        }
    }
}
