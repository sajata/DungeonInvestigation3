using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    public class TestMapGenerator
    {
        private Map _map;

        public TestMapGenerator()
        {

        }
        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            _map = new Map(mapWidth, mapHeight); // Temporarilly stores the map that is being generated
            List<Rectangle> Rooms = new List<Rectangle>();
            Rectangle newRoom = new Rectangle(15, 15, 30, 30);

            FloodWalls();

            for (int i = 0; i < maxRooms; i++)
            {
                Rooms.Add(newRoom);
            }
            foreach (Rectangle room in Rooms)
            {
                CreateRoom(room);
            }

            return _map;
        }

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

        private void CreateFloor(Point postion)
        {
            _map.Tiles[postion.ToIndex(_map.Width)] = new FloorTile();
        }

        //Fills the map with walls
        private void FloodWalls()
        {
            for (int i = 0; i < _map.Tiles.Length; i++)
            {
                _map.Tiles[i] = new WallTile();
            }
        }
    }
}

