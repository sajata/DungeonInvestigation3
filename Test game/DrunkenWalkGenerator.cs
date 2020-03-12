using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Test_game.Tiles;


namespace Test_game
{
    public class DrunkenWalkGenerator
    {
        //stores the map whilst its being generated
        private Map _map;
        //stores the map seed
        private int _seed;

        //the random number generator used for
        //generating the dungeon
        private Random r;

        //the percentage of the map to be filled by floor tiles
        private const double _percentFilled = 0.45;       
        private const double _weightTowardCentre = 0.15;
        private const double _weightTowardsPreviousDirection = 0.40;
        //maximum number of moves the drunkard can make 
        private const int _walkIterations = 25000;

        public int Seed { get => _seed; set => _seed = value; }

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            //instatiating the drunkard
            Point Drunkard = new Point(0, 0);
            string PreviousDirection = "";
            //stores the number of filled walls
            int filledWalls = 0;
            //
            int filledGoal = 0;
            if (Seed == 0)
            {
                Random SeedGen = new Random();
                Seed = SeedGen.Next();
            }
            // create an empty map of size (mapWidth x mapHeight)             
            _map = new Map(mapWidth, mapHeight);
            // Create a random number generator with the given seed          
            r = new Random(Seed);

            //this is a dungeon so start by filling 
            //the map with walls
            FloodWalls();          

            //Picks a random starting point for the drunkard
            Drunkard.X = r.Next(2, mapWidth - 2);
            Drunkard.Y = r.Next(2, mapHeight - 2);

            //creates a floor tile at that starting position
            CreateFloor(Drunkard);

            //calculates the number of tiles to be filled in by the drunkard
            //ideally 
            filledGoal =(int) (mapWidth * mapHeight * _percentFilled);

            //loops while the drunkard hasn't exceeded the max number of moves
            for(int i =0; i < _walkIterations; i++)
            {
                
               Walk(ref PreviousDirection,ref Drunkard, mapWidth, mapHeight, ref r, ref filledWalls);
                //if the drunkard has filled in enough walls 
                //stop looping
               if(filledWalls >= filledGoal)
               {
                    break;
               }
            }

            return _map;
        }
        /// <summary>
        /// Sets the new position the drunkard has to walk to and creates a floor tile in that position
        /// Drunkard is biased towards the centre of the map and to the previous direction it moved in
        /// </summary>
        /// <param name="previousDirection"></param>
        /// <param name="Drunkard"></param>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="r"></param>
        /// <param name="filled"></param>
        private void Walk(ref string previousDirection,ref Point Drunkard, int mapWidth, int mapHeight, ref Random r, ref int filled)
        {
            //the value added to the drunkards coordinates
            int dx = 0;
            int dy = 0;
            //Direction Probabilities
            double north = 1.0;
            double south = 1.0;
            double east = 1.0;
            double west = 1.0;
            double total = 0.0;
            double choice = 0.0;
            //weight random walk against edges
            //if too far to the right (west) weight eastwards
            if (Drunkard.X < mapWidth * 0.25)
            {
                east += _weightTowardCentre;
            }
            //if too far to the left(east) weight westwards
            else if (Drunkard.X > mapWidth *  0.75)
            {
                west += _weightTowardCentre;
            }
            //if too far up (north) weight southwards
            if(Drunkard.Y < mapHeight * 0.25)
            {
                south += _weightTowardCentre;
            }
            //if too far down(south) weight northwards
            else if(Drunkard.Y > mapHeight * 0.75)
            {
                north += _weightTowardCentre;
            }

            //weights the drunkard according to their previous direction
            if(previousDirection == "north")
            {
                north += _weightTowardsPreviousDirection;
            }
            if(previousDirection == "south")
            {
                south += _weightTowardsPreviousDirection;
            }
            if(previousDirection == "east")
            {
                east += _weightTowardsPreviousDirection;
            }
            if(previousDirection == "west")
            {
                west += _weightTowardsPreviousDirection;
            }

            //normalising the values so they all range from 0 to 1
            total = north + south + east + west;
            north /= total;
            south /= total;
            east /= total;
            west /= total;

            //generates a random number between 1 and 0
            choice = r.NextDouble();
            //if random number falls in the north range of values
            //driection is north
            if(choice >= 0 && choice < north)
            {
                dx = 0;
                dy = -1;
                previousDirection = "north";
            }
            //if random number falls in the south range of values
            //direction is south
            else if (choice >= north && choice < (north+south))
            {
                dx = 0;
                dy = 1;
                previousDirection = "south";
            }
            //if random number falls in the east range of values
            //direction is east
            else if (choice >= (north+south) && choice < (north+south+east))
            {
                dx = 1;
                dy = 0;
                previousDirection = "east";
            }
            //if random number falls in the west range of values
            //direction is west
            else
            {
                dx = -1;
                dy = 0;
                previousDirection = "west";
            }

            //checks collision at edges and walks
            if((Drunkard.X + dx > 0 && Drunkard.X + dx < mapWidth-1) && (Drunkard.Y + dy > 0 && Drunkard.Y + dy < mapHeight - 1))
            {
                Drunkard.X += dx;
                Drunkard.Y += dy;
                if(_map.Tiles[Drunkard.ToIndex(mapWidth)].IsBlockingMove)
                {
                    CreateFloor(Drunkard);
                    filled += 1;
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
