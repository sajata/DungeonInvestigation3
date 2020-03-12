using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    //FOR THIS CELLULAR AUTOMATION
    //A DEAD CELL = FLOOR TILE
    //ALIVE CELL  = WALL TILE
    public class CellAutoGenerator
    {
        //stores the map whilst its being generated
        private Map _map;
        //stores the map seed
        private int _seed;

        //the random number generator used for
        //generating the dungeon
        private Random r;

        //number of times the cellular automation runs for
        private const int _numberOfGenerations = 7;

        //probability a cells starts alive in the initial state of the automation
        private const int _percentWalls = 48;

        //threshold number of cells required for a cell to stay alive
        private const int _deathLimit = 4;

        //threshold number of cells for a dead cell to become alive
        private const int _birthLimit = 5;

        public int Seed { get => _seed; set => _seed = value; }

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {             

            //If no seed has been passed by the user
            //Generate a random map seed
            if (Seed == 0)
            {
                Random SeedGen = new Random();
                Seed = SeedGen.Next();
            }
            //creates an empty map of size (mapWidth x mapHeight)
            _map = new Map(mapWidth, mapHeight);

            //creates the random number generator with the given map seed
             r = new Random(Seed);

            //start by filling the map with dead cells(floor tiles)
            FloodFloors();
            //randomly distribute alive cells(wall tiles) accross the map
            RandomFillWalls(ref r);
            
            //executes the cellular automation for the number of generations
            for(int i =0; i < _numberOfGenerations; i++)
            {
                GenerateCaves();
            }

            //roughly tries to connect the caves
            ConnectCaves();

            return _map;
        }

        /// <summary>
        /// A crude attempt at connecting the caves
        /// By drawing a grid of floor tiles
        /// in the central part of the map
        /// </summary>
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
        /// <summary>
        /// Randomly distributes walls accross the map
        /// With the probability of each cell being alive or dead
        /// </summary>
        /// <param name="r"></param>
        private void RandomFillWalls(ref Random r)
        {
            //iterates through each tile in the map
            //randomly decides wether a cell is alive or dead
            //according to the probility of the cell being initially alive (_percentWalls)
            for(int i =0; i < _map.Tiles.Length; i++)
            {
                if(r.Next(1,101) < _percentWalls)
                {
                    _map.Tiles[i] = new WallTile();
                }
                else
                {
                    _map.Tiles[i] = new FloorTile();
                }
            }
        }
        /// <summary>
        /// Applies the rules of the cellular automation
        /// to each cell on the map
        /// and changes the cells state accordingly
        /// </summary>
        private void GenerateCaves()
        {
            Point position;
            //number of neighbours the cell has
            int AdjacentWalls = 0;
            //stores the new cells and their states in a new map
            //so the new changes don't affect the other cells
            Map tempMap = new Map(_map.Width, _map.Height);

            for(int y = 0; y < _map.Height; y++)
            {
                for(int x = 0; x< _map.Width; x++)
                {
                    position = new Point(x, y);
                    //gets number of neighbours
                    AdjacentWalls = GetAdjacentWalls(position);
                    //if tile is wall/alive
                    if(_map.Tiles[position.ToIndex(_map.Width)].IsBlockingMove)
                    {
                        //if cell has enough neigbours it stays alive 
                        if(AdjacentWalls >= _deathLimit) 
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new WallTile();
                        }
                        //if cell too few it dies
                        else
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new FloorTile();
                        }
                    }
                    //if tile is dead/ floor tile
                    else
                    {
                        //if it has enough alive neighbours it becomes alive
                        if (AdjacentWalls >= _birthLimit) 
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new WallTile();
                        }
                        //if it doesn't have enough alive neighbours it stays dead
                        else
                        {
                            tempMap.Tiles[position.ToIndex(_map.Width)] = new FloorTile();
                        }
                    }
                }
                
            }
            //saves the changes made to the map
            _map = tempMap;
        }

        /// <summary>
        /// Gets the number of neighbours the cell has
        /// Using Moore's neighbourhood
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
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
                        //cells outside the bounds of the map are coonsidered to be alive (wall tiles)
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
        /// <summary>
        /// Determines wether a cell is outside the bounds of the map
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool isOutOfBound(Point position)
        {
            return (position.X < 0 || position.Y < 0 || position.X >= _map.Width || position.Y >= _map.Height);
        }

        /// <summary>
        /// Fills the entire map with floor tiles
        /// </summary>
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
        /// <summary>
        /// Draws a floor tile at the given position
        /// </summary>
        /// <param name="postion"></param>
        private void CreateFloor(Point postion)
        {
            //EXCEPTION HANDLING to make sure the position isn't outside the bounds of the map tiles 
            //array
            try
            {
                _map.Tiles[postion.ToIndex(_map.Width)] = new FloorTile();
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR POSITION OUSTIDE OF THE BOUNDS OF THE MAP");
                return;
            }
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
