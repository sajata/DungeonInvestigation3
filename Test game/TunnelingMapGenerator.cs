using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    public class TunnelingMapGenerator
    {
        private Map _map;
        public int Seed = 0;

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
            Random randNum = new Random(Seed);
            // store a list of the rooms created so far             
            List<Rectangle> Rooms = new List<Rectangle>();
            // create up to (maxRooms) rooms on the map             
            // and make sure the rooms do not overlap with each other            
            for (int i = 0; i < maxRooms; i++)
            {
                // set the room's (width, height) as a random size between (minRoomSize, maxRoomSize)                 
                int newRoomWidth = randNum.Next(minRoomSize, maxRoomSize);
                int newRoomHeight = randNum.Next(minRoomSize, maxRoomSize);
                // sets the room's X/Y Position at a random point between the edges of the map                
                int newRoomX = randNum.Next(0, mapWidth - newRoomWidth - 1);
                int newRoomY = randNum.Next(0, mapHeight - newRoomHeight - 1);
                // create a Rectangle representing the room's perimeter                 
                Rectangle newRoom = new Rectangle(newRoomX, newRoomY, newRoomWidth, newRoomHeight);

                // Checks if the new room intersects with any other rooms
                //newRoom.Intersects checks if any rectangles overlap 
                bool newRoomIntersects = Rooms.Any(room => newRoom.Intersects(room)); // WHAT IS LAMBDA???????????????
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
            for (int r = 1; r < Rooms.Count; r++)
            {
                //for all remaining rooms get center of current room and previous room
                Point previousRoomCenter = Rooms[r - 1].Center;
                Point currentRoomCenter = Rooms[r].Center;

                //create a 50% chance of which type of L shaped tunnel will be carved
                if (randNum.Next(1, 2) == 1)
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
    

        // sets X coordinate between right and left edges of map
        // to prevent any out-of-bounds errors
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

        // sets Y coordinate between top and bottom edges of map
        // to prevent any out-of-bounds errors
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
            // OR using ternary conditional operators: return (y < 0) ? 0 : (y > _map.Height - 1) ? _map.Height - 1 : y;
            //this is how it works
            // condition ? first expression : second expression
            // if true set first expr as result ; if false set the second condition as false
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
