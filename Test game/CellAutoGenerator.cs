using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    public class CellAutoGenerator
    {
        private Map _map;
        public int Seed;

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            int numberOfGenerations = 7;
            if(Seed == 0)
            {
                Random SeedGen = new Random();
                Seed = SeedGen.Next();
            }

            _map = new Map(mapWidth, mapHeight);
            Random r = new Random(Seed);

            FloodWalls();
         

            FloodFloors();

            RandomFillWalls(ref r);

            for (int i = 0; i < numberOfGenerations; i++)
            {
                MakeCaverns();
            }

            ConnectRooms();
            return _map;
        }

        private void FloodFloors()
        {
            int MaxX = _map.Width - 1;
            int MaxY = _map.Height - 1;

            for (int x = 1; x < MaxX; x++)
            {
                for (int y = 1; y < MaxY; y++)
                {
                    CreateFloor(new Point(x, y));
                }
            }
        }

        private void ConnectRooms()
        {
            int MaxX = _map.Width - 1;
            int MaxY = _map.Height - 1;

            for (int x = 1; x < MaxX; x++)
            {
                for (int y = 1; y < MaxY; y++)
                {
                    if(x % 25 == 0)
                    {
                        CreateFloor(new Point(x, y));
                    }
                    if(y % 13 == 0)
                    {
                        CreateFloor(new Point(x, y));
                    }
                }
            }
        }

        private void MakeCaverns()
        {
            int MaxX = _map.Width;
            int MaxY = _map.Height;

            for (int x = 1; x < MaxX; x++)
            {
                for (int y = 1; y < MaxY; y++)
                {
                    PlaceWallLogic(x, y);
                }
            }
        }

        private void PlaceWallLogic(int x , int y)
        {
            int numWalls = GetAdjacentWalls(x,y);
            Point position = new Point(x,y);

            if(!_map.IsTileWalkable(position))
            {
               
                if(numWalls < 2)
                {
                    _map.Tiles[position.ToIndex(_map.Width)] = new FloorTile();
                }
            }
            else
            {
                if(numWalls >= 4)
                {
                    _map.Tiles[position.ToIndex(_map.Width)] = new WallTile();
                }
            }
        }

        private int GetAdjacentWalls(int x, int y)
        {
            int startX = x - 1;
            int startY = y - 1;
            int endX = x + 1;
            int endY = y + 1;
            int wallCounter = 0;
            Point position;

            for(int iY = startY; iY <= endY; iY++)
            {
                for(int iX = startX; iX <= endX; iX++)
                {
                    if(iX != x && iY != y)
                    {
                        position = new Point(iX, iY);
                        if(!_map.IsTileWalkable(position))
                        {
                            wallCounter++;
                        }
                    }
                }
            }
            return wallCounter;
        }

        private void RandomFillWalls(ref Random r)
        {
            int MaxX = _map.Width - 1;
            int MaxY = _map.Height - 1;
            int PercentWalls = 55;
            for(int x = 1; x< MaxX; x++)
            {
                for(int y = 1;y < MaxY; y++)
                {
                    if(PercentWalls >= r.Next(1,101))
                    {
                        CreateWall(new Point(x, y));
                    }
                }
            }

        }

        private void CreateWall(Point postion)
        {
            _map.Tiles[postion.ToIndex(_map.Width)] = new WallTile();
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
