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
            
            List<BSPNode> Leafs = new List<BSPNode>();
            BSPNode BSPTree = new BSPNode(new Rectangle(0, 0, mapWidth - 1, mapHeight - 1));
            Leafs.Add(BSPTree);
            if (Seed == 0)
            {
                Random SeedGen = new Random();
                Seed = SeedGen.Next();
            }

            _map = new Map(mapWidth, mapHeight);
            Random r = new Random(Seed);

            FloodWalls();

            CreateTree(ref BSPTree, ref r);

            DrawLeaf(ref BSPTree);

            return _map;
        }
       
        private void DrawLeaf(ref BSPNode root)
        {
            if(!root.IAmLeaf())
            {
                DrawLeaf(ref root);
            }
            else
            {
                CreateRoom(root.Leaf);
            }
        }

        private void CreateTree(ref BSPNode Root, ref Random r)
        {
            bool splitSuccesfuly = true;
            int MaxLeafSize = 25;
            int MinLeafSize = 13;

            while (splitSuccesfuly)
            {
                splitSuccesfuly = false;
                if(Root.IAmLeaf())
                {
                    if(Root.Leaf.Width >  MaxLeafSize && Root.Leaf.Height > MaxLeafSize)
                    {
                        if (Root.Partition(ref r, MinLeafSize))
                        {
                            splitSuccesfuly = true;
                            CreateTree(ref Root.Left, ref r);
                            CreateTree(ref Root.Right, ref r);
                        }
                    }
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
