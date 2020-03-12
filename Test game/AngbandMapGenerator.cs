using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    public class AngbandMapGenerator
    {
        //stores the map whilst its being generated
        private Map _map;
        //stores the map seed
        private int _seed;

        //the random number generator used for
        //generating the dungeon
        private Random r;

        public int Seed { get => _seed; set => _seed = value; }

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            if(Seed == 0)
            {
                Random SeedGen = new Random();
                Seed = SeedGen.Next();
            }
            // create an empty map of size (mapWidth x mapHeight)             
            _map = new Map(mapWidth, mapHeight);
            // Create a random number generator            
            r = new Random(Seed);
            // store a list of the rooms created so far             
            List<Rectangle> Rooms = new List<Rectangle>();
            // create up to (maxRooms) rooms on the map             
            // and make sure the rooms do not overlap with each other            
            for (int i = 0; i < maxRooms; i++)
            {
                // set the room's (width, height) as a random size between (minRoomSize, maxRoomSize)                 
                int newRoomWidth = r.Next(minRoomSize, maxRoomSize);
                int newRoomHeight = r.Next(minRoomSize, maxRoomSize);
                // sets the room's X/Y Position at a random point between the edges of the map                
                int newRoomX = r.Next(0, mapWidth - newRoomWidth - 1);
                int newRoomY = r.Next(0, mapHeight - newRoomHeight - 1);
                // create a Rectangle representing the room's perimeter                 
                Rectangle newRoom = new Rectangle(newRoomX, newRoomY, newRoomWidth, newRoomHeight);

                // Checks if the new room intersects with any other rooms
                //newRoom.Intersects checks if any rectangles overlap 
                //adds to list of rooms if doesn't 
                bool newRoomIntersects = CheckIntersection(newRoom, Rooms);
                if (!newRoomIntersects)
                {
                    Rooms.Add(newRoom);
                }
            }


            // This is a dungeon, so begin by flooding the map with walls.             
            FloodWalls();
            // carve out rooms for every room in the Rooms list             
            foreach (Rectangle room in Rooms)
            {
                CreateRoom(room);
            }

            //carve out tunnels between all rooms
            //based on the position of their centers
            for (int i = 1; i < Rooms.Count; i++)
            {
                //for all remaining rooms get center of current room and previous room
                Point previousRoomCenter = Rooms[i - 1].Center;
                Point currentRoomCenter = Rooms[i].Center;

                //create a 50% chance of which type of L shaped tunnel will be carved
                if (r.Next() == 1)
                {
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, previousRoomCenter.Y);
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, currentRoomCenter.X);
                }
                else
                {
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, previousRoomCenter.X);
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, currentRoomCenter.Y);
                }
                //loop runs through the entire list of rooms storing the center of each room in the list
                //flips a coin to decide ¬ or L shaped tunnel to carve out
            }

            

            return _map;
        }

        /// <summary>
        /// Checks wether the newRoom intersects with any other room currently generated
        /// </summary>
        /// <param name="newRoom"></param>
        /// <param name="Rooms"></param>
        /// <returns></returns>
        private bool CheckIntersection(Rectangle newRoom, List<Rectangle> Rooms)
        {
            bool intersects = false;
            foreach(Rectangle room in Rooms)
            {
                if (newRoom.Intersects(room))
                {
                    intersects = true;
                    break;
                }
            }
            return intersects;
        }

        /// <summary>
        ///  sets X coordinate between right and left edges of map
        // to prevent any out-of-bounds errors
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private int ClampX(int x)
        {
            if (x < 0)
            {
                x = 0;
            }
            else if (x > _map.Width - 1)
            {
                x = _map.Width - 1;

            }
            return x;
        }

        /// <summary>
        /// sets Y coordinate between top and bottom edges of map
        // to prevent any out-of-bounds errors
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private int ClampY(int y)
        {
            if (y < 0)
            {
                y = 0;

            }
            else if (y > _map.Height - 1)
            {
                y = _map.Height - 1;
            }
            return y;           
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

        //carve a tunnel parallel to the x axis
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                CreateFloor(new Point(x, yPosition));
            }
        }
        //carve a tunnel parallel to y axis
        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                CreateFloor(new Point(xPosition, y));
            }
        }

    }
}
