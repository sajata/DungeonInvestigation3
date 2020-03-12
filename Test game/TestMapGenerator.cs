using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    /// <summary>
    /// Acts as a defense
    /// In case a wrong map type has been passed into the World.LoadMap or World.CreateMap function
    /// Prevents the program from crashing
    /// </summary>
    public class TestMapGenerator
    {
        //stores the map whilst its being generated
        private Map _map;

        public TestMapGenerator()
        {

        }
        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            _map = new Map(mapWidth, mapHeight); // Temporarilly stores the map that is being generated
            
            Rectangle newRoom = new Rectangle(15, 15, 30, 30);

            //This is a dungeon, so start by filling the enitre map with walls
            FloodWalls();

            //Draws the room on the map
            CreateRoom(newRoom);
             

            return _map;
        }

        /// <summary>
        /// Draws the room on the map
        /// </summary>
        /// <param name="room"></param>
        private void CreateRoom(Rectangle room)
        {
            //Place floors in the interior area
            for (int x = room.Left + 1; x < room.Right - 1; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    CreateFloor(new Point(x, y));
                }
            }
        }

        /// <summary>
        /// Draws a floor tile at the given position
        /// </summary>
        /// <param name="postion"></param>
        private void CreateFloor(Point postion)
        {
            _map.Tiles[postion.ToIndex(_map.Width)] = new FloorTile();
        }

        /// <summary>
        /// Fills the enitre map with walls
        /// </summary>
        private void FloodWalls()
        {
            for (int i = 0; i < _map.Tiles.Length; i++)
            {
                _map.Tiles[i] = new WallTile();
            }
        }
    }
}

