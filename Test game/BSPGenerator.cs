using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    public class BSPGenerator
    {
        //stores the map whilst its being generated
        private Map _map;
        //stores the map seed
        private int _seed;

        //the random number generator used for
        //generating the dungeon
        private Random r;

        //test map generator for the exception handler
        private TestMapGenerator failSafe = new TestMapGenerator();

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


            //setting the leaf sizes to be slightly larger than the room sizes
            //to make sure each room can fit within each leaf
            int MaxLeafSize = maxRoomSize + 5;
            int MinLeafSize = minRoomSize + 5;

           
            
            //creates the root node which contains the entire map
            //slightly smaller than the entire map to allow for a border of walls aroundthe dungeon
            BSPNode Root = new BSPNode(new Rectangle(0, 0, mapWidth - 1, mapHeight - 1));
             
            //creates an empty map of size (mapWidth x mapHeight)
            _map = new Map(mapWidth, mapHeight);
            //cretae a random number gnerator which starts on the map seed
            r = new Random(Seed);

            //This is a dungeon, so start by filling the enitre map with walls
            FloodWalls();

            //Since this is a recursive function 
            //Exception handling for stack overflow
            try
            {
                //Partitions the dungeon
                CreateBSP(Root, maxRoomSize, MinLeafSize);
            }
            catch(Exception)
            {
                //Rather than crashing
                //generate a failsafe map using the testmapgenerator
                Debug.WriteLine("ERROR STACK OVERFLOW HAS OCCURED");
                _map = failSafe.GenerateMap(mapWidth, mapHeight, maxRooms, minRoomSize, maxRoomSize);
            }

            try
            {
                //creates rooms and hallways between neighbourin rooms
                DrawRooms(ref Root, ref r, minRoomSize);                
            }
            catch(Exception)
            {
                //Rather than crashing
                //generate a failsafe map using the testmapgenerator
                Debug.WriteLine("ERROR STACK OVERFLOW HAS OCCURED");
                _map = failSafe.GenerateMap(mapWidth, mapHeight, maxRooms, minRoomSize, maxRoomSize);
            }

            try
            {
                //draws the hallways on the map
                DrawHalls(Root);               
            }
            catch(Exception)
            {
                //Rather than crashing
                //generate a failsafe map using the testmapgenerator
                Debug.WriteLine("ERROR STACK OVERFLOW HAS OCCURED");
                _map = failSafe.GenerateMap(mapWidth, mapHeight, maxRooms, minRoomSize, maxRoomSize);
            }

            return _map;
        }

        /// <summary>
        /// Partitions the root node and creates the Binary Tree to store the dungeon
        /// </summary>
        /// <param name="node"></param>
        /// <param name="maxRoomSize"></param>
        /// <param name="MinLeafSize"></param>        
        private void CreateBSP(BSPNode node, int maxRoomSize, int MinLeafSize)
        {            
            //Add exception 
            //Only Partition Leaf nodes
            //i.e. only nodes which haven't been partioned
            if(node.IAmLeaf())
            {
                // if partition is too large
                if(node.Leaf.Width > maxRoomSize || node.Leaf.Height > maxRoomSize)
                {
                    //if the node can be partiotned
                    if(node.Partition(ref r, MinLeafSize))
                    {
                        //recursively partition the children of this node
                        CreateBSP(node.Left, maxRoomSize, MinLeafSize);
                        CreateBSP(node.Right, maxRoomSize, MinLeafSize);
                    }
                }
            }
        }


        /// <summary>
        /// Draws the halls within each leaf node
        /// </summary>
        /// <param name="Root"></param>
        private void DrawHalls(BSPNode Root)
        {
           //if the node is null breakout of this function and return to previous function call on stack
           //which would be a leaf
           if(Root == null)
            {
                return;
            }
           //recursively draw the room of both children
            DrawHalls(Root.Left);
            DrawHalls(Root.Right);  

            //draw each hall that the node has
            foreach(Rectangle hall in Root.Halls)
            {
                CreateHall(hall);
            }


        }
        
        /// <summary>
        /// Draws all the rooms and creates hallways between rooms in neighbouring nodes
        /// </summary>
        /// <param name="root"></param>
        /// <param name="r"></param>
        /// <param name="MinRoomSize"></param>
        private void DrawRooms(ref BSPNode root, ref Random r, int MinRoomSize)
        {
            // if the node's left child isn't a leaf
            if(root.Left != null)
            {
                DrawRooms(ref root.Left, ref r, MinRoomSize);
               
            }
            // if the node's right child isn't a leaf
            if(root.Right != null)
            {
                 DrawRooms(ref root.Right, ref r, MinRoomSize);
            }
            //if both of the nodes children aren't leafs 
            //create a tunnel between the rooms at the end of their respective branches
            if(root.Left != null && root.Right != null)
            {
                root.createHall(root.Left.getRoom(ref r), root.Right.getRoom(ref r), ref r);
            }

            //if this node is a a leaf
            //create a room and draw it
            else
            {
                root.MakeRoom(ref r, MinRoomSize);
                CreateRoom(root.Room);
            }
        }

        /// <summary>
        /// Draws the hallway on the map
        /// </summary>
        /// <param name="hall"></param>
        private void CreateHall(Rectangle hall)
        {
            
            for (int x = hall.Left; x < hall.Right; x++)
            {
                for (int y = hall.Top; y < hall.Bottom; y++)
                {
                    CreateFloor(new Point(x, y));
                }
            }
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
            catch(Exception)
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
