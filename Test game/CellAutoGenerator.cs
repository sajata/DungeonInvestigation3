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

            FloodFloors();

            RandomFillWalls(ref r);
            
            for(int i =0; i < numberOfGenerations; i++)
            {
                GenerateCaves();
            }

            ConnectCaves();

            return _map;
        }

        private void ConnectCaves()
        {
            for(int x =5; x < _map.Width - 1; x++)
            {
                for(int y = 5; y < _map.Height - 1; y++)
                {
                    if(y % 18 == 0)
                    {
                        CreateFloor(new Point(x, y));
                    }
                    if(x % 12 == 0)
                    {
                        CreateFloor(new Point(x, y));
                    }
                }
            }
        }

        private void RandomFillWalls(ref Random r)
        {
            int PercentWalls = 48;

            for(int i =0; i < _map.Tiles.Length; i++)
            {
                if(r.Next(1,101) < PercentWalls)
                {
                    _map.Tiles[i] = new WallTile();
                }
                else
                {
                    _map.Tiles[i] = new FloorTile();
                }
            }
        }

        private void GenerateCaves()
        {
            Point position;
            int AdjacentWalls = 0;
            Map tempMap = new Map(_map.Width, _map.Height);

            for(int y = 0; y < _map.Height; y++)
            {
                for(int x = 0; x< _map.Width; x++)
                {
                    position = new Point(x, y);
                    AdjacentWalls = GetAdjacentWalls(position);
                    //if tile is walls
                    if(_map.Tiles[position.ToIndex(_map.Width)].IsBlockingMove)
                    {
                        if(AdjacentWalls >= 4) // death limit
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new WallTile();
                        }
                        else
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new FloorTile();
                        }
                    }
                    else
                    {
                        if (AdjacentWalls >= 5) // birth limit
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new WallTile();
                        }
                        else
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new FloorTile();
                        }
                    }
                }
                
            }
            _map = tempMap;
        }


        private int GetAdjacentWalls(Point position)
        {
            int AdjacentWalls = 0;
            Point temppos = new Point();
            for(int x = -1; x <2; x++)
            {
                for(int y = -1; y < 2; y++)
                {                    
                    if(x!= 0 || y != 0)
                    {
                        temppos.X = position.X + x;
                        temppos.Y = position.Y + y;
                        if(isOutOfBound(temppos))
                        {
                            AdjacentWalls++;
                        }
                        else if(_map.Tiles[temppos.ToIndex(_map.Width)].IsBlockingMove)
                        {
                            AdjacentWalls++;
                        }
                    }
                }
            }

            return AdjacentWalls++;
        }

        private bool isOutOfBound(Point position)
        {
            return (position.X < 0 || position.Y < 0 || position.X >= _map.Width || position.Y >= _map.Height);
        }

        private void FloodFloors()
        {
            int MaxX = _map.Width-1;
            int MaxY = _map.Height-1;

            for (int x = 1; x < MaxX; x++)
            {
                for (int y = 1; y < MaxY; y++)
                {
                    CreateFloor(new Point(x, y));
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
