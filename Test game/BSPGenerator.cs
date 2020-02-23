using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    public class BSPGenerator
    {
        private Map _map;
        public int Seed;

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            int MaxLeafSize = maxRoomSize + 5;
            int MinLeafSize = minRoomSize + 5;
            bool splitSuccesfuly = true;
            List<BSPNode> Nodes = new List<BSPNode>();
            BSPNode Root = new BSPNode(new Rectangle(0, 0, mapWidth - 1, mapHeight - 1));
            Nodes.Add(Root);
            if (Seed == 0)
            { 
                Random SeedGen = new Random();
                Seed = SeedGen.Next();
            }

            _map = new Map(mapWidth, mapHeight);
            Random r = new Random(Seed);

            FloodWalls();

            while (splitSuccesfuly)
            {
                splitSuccesfuly = false;
                
                foreach(BSPNode node in Nodes)
                {
                    if(GetNumberOfLeafs(Nodes) < maxRooms)
                    {
                        if(node.Partition(ref r, MinLeafSize))
                        {
                            Nodes.Add(node.Left);
                            Nodes.Add(node.Right);
                            splitSuccesfuly = true;
                            break;
                        }
                    }
                }
            }

            DrawRooms(ref Root, ref r, minRoomSize);

            DrawHalls(Nodes);

            return _map;
        }

        private int GetNumberOfLeafs(List<BSPNode> Nodes)
        {
            int NumberLeafs = 0;

            foreach(BSPNode node in Nodes)
            {
                if(node.IAmLeaf())
                {
                    NumberLeafs++;
                }
            }
            return NumberLeafs;
        }

        private void DrawHalls(List<BSPNode> Nodes)
        {
            foreach(BSPNode node in Nodes)
            {
                if (node.Halls.Count > 0)
                {
                    for(int i = 0; i < node.Halls.Count(); i++)
                    {
                        CreateHall(node.Halls[i]);
                    }
                }
            }
            
        }

        private void DrawRooms(ref BSPNode root, ref Random r, int MinRoomSize)
        {
            if(root.Left != null)
            {
                DrawRooms(ref root.Left, ref r, MinRoomSize);
               
            }
            if(root.Right != null)
            {
                 DrawRooms(ref root.Right, ref r, MinRoomSize);
            }
            if(root.Left != null && root.Right != null)
            {
                root.createHall(root.Left.getRoom(ref r), root.Right.getRoom(ref r), ref r);
            }
            else
            {
                root.MakeRoom(ref r, MinRoomSize);
                CreateRoom(root.Room);
            }
        }

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
