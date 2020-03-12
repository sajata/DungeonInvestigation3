using System;
using SadConsole;
using Microsoft.Xna.Framework;

namespace Test_game.Tiles
{
    /// <summary>
    /// Wall tile does block movement
    /// </summary>
    public class WallTile : BaseTile
    {
        public WallTile(bool blockMovement = true) : base(Color.Blue, Color.Navy, '#', blockMovement)
        {
            TileName = "Wall";
        }
    }
}
