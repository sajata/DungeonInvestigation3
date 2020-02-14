using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;

namespace Test_game
{
    public class MessyBSPNode
    {
        public MessyBSPNode Left;
        public MessyBSPNode Right;
        public Rectangle Leaf;
        public Rectangle Room;
        public List<Point> Halls = new List<Point>();
        private bool HorizontalSplit;


        public MessyBSPNode(Rectangle leaf)
        {
            Leaf = leaf;
        }

        public bool Partition(ref Random r, int MinLeafSize)
        {
            int MaxSplit = 0;
            int Split = 0;
            HorizontalSplit = true;
            if (Left != null || Right != null)
            {
                return false;
            }
            if (this.Leaf.Width / this.Leaf.Height >= 1.05)
            {
                HorizontalSplit = false;
            }
            else if (this.Leaf.Height / this.Leaf.Width >= 1.05)
            {
                HorizontalSplit = true;
            }
            else
            {
                if (r.Next(0, 2) == 1)
                {
                    HorizontalSplit = false;
                }
                else
                {
                    HorizontalSplit = true;
                }
            }

            MaxSplit = (HorizontalSplit ? Leaf.Height : Leaf.Width) - MinLeafSize;


            if (MaxSplit <= MinLeafSize)
            {
                return false;
            }

            Split = r.Next(MinLeafSize, MaxSplit);

            if (HorizontalSplit)
            {
                Left = new MessyBSPNode(new Rectangle(this.Leaf.X, this.Leaf.Y, this.Leaf.Width, Split));
                Right = new MessyBSPNode(new Rectangle(this.Leaf.X, this.Leaf.Y + Split, this.Leaf.Width, this.Leaf.Height - Split));
            }
            else
            {
                Left = new MessyBSPNode(new Rectangle(this.Leaf.X, this.Leaf.Y, Split, this.Leaf.Height));
                Right = new MessyBSPNode(new Rectangle(this.Leaf.X + Split, this.Leaf.Y, this.Leaf.Width - Split, this.Leaf.Height));
            }

            return true;
        }

        public void MakeRoom(ref Random r, int MinRoomSize)
        {
            int width, height, x, y;

            width = r.Next(MinRoomSize, this.Leaf.Width);
            height = r.Next(MinRoomSize, this.Leaf.Height);

            x = r.Next(this.Leaf.Left, this.Leaf.Right - width);
            y = r.Next(this.Leaf.Top, this.Leaf.Bottom - height);

            this.Room = new Rectangle(x, y, width, height);
        }

        public Rectangle getRoom(ref Random r)
        {
            Rectangle LeftRoom = new Rectangle();
            Rectangle RightRoom = new Rectangle();
            Rectangle nullRec = new Rectangle();

            if (this.Room != nullRec)
            {
                return this.Room;
            }
            else
            {
                if (this.Left != null)
                {
                    LeftRoom = this.Left.getRoom(ref r);
                }
                if (this.Right != null)
                {
                    RightRoom = this.Right.getRoom(ref r);
                }
                if (LeftRoom == null && RightRoom == null)
                {
                    return nullRec;
                }
                else if (RightRoom == null)
                {
                    return LeftRoom;
                }
                else if (LeftRoom == null)
                {
                    return RightRoom;
                }
                else if (r.Next(0, 2) == 1)
                {
                    return LeftRoom;
                }
                else
                {
                    return RightRoom;
                }

            }
        }

        public void createHall(Rectangle leftRoom, Rectangle rightRoom, ref Random r, int mapWidth, int mapHeight)
        {
            this.Halls = new List<Point>();
            Point FinishPoint = leftRoom.Center;
            Point Drunkard = rightRoom.Center;
            //ensure that the finish point is always to the left
            

            //int DistanceBetweenRoomsX = leftRoom.Center.X - rightRoom.Center.X;
            //int DistanceBetweenRoomsY = leftRoom.Center.Y - rightRoom.Center.Y;
            
            //if(DistanceBetweenRoomsX > 0)
            //{
            //    FinishPoint = leftRoom.Center;
            //    Drunkard = rightRoom.Center;
            //}
            //else if(DistanceBetweenRoomsX < 0)
            //{
            //    FinishPoint = rightRoom.Center;
            //    Drunkard = leftRoom.Center;
            //}
            //else
            //{
            //    if(DistanceBetweenRoomsY > 0)
            //    {
            //        Drunkard = leftRoom.Center;
            //        FinishPoint = rightRoom.Center;
            //    }
            //    else
            //    {
            //        Drunkard = rightRoom.Center;
            //        FinishPoint = leftRoom.Center;
            //    }
            //}

            int dx = 0;
            int dy = 0;

            double choice = 0.0;
            double total = 0.0;
            double north = 1.0;
            double south = 1.0;
            double east = 1.0;
            double west = 1.0;
            double weight = 0.85;



            while (!(leftRoom.Contains(Drunkard)))
            {
                north = 1.0;
                south = 1.0;
                east = 1.0;
                west = 1.0;
                total = 0;
                //weight radnom walsk agains edges
                if (Drunkard.X < FinishPoint.X)
                {
                    east += weight;
                }
                else if (Drunkard.X > FinishPoint.X)
                {
                    west += weight;
                }
                if (Drunkard.Y < FinishPoint.Y)
                {
                    south += weight;
                }
                else if (Drunkard.Y > FinishPoint.Y)
                {
                    north += weight;
                }

                total += north + south + east + west;
                north /= total;
                south /= total;
                east /= total;
                west /= total;

                choice = r.NextDouble();

                if (0 <= choice && choice < north)
                {
                    dx = 0;
                    dy = -1;
                }
                else if (choice >= north && choice < (north + south))
                {
                    dy = 1;
                    dx = 0;
                }
                else if (choice >= (north + south) && choice < (north + south + east))
                {
                    dx = 1;
                    dy = 0;
                }
                else
                {
                    dx = -1;
                    dy = 0;
                }

                if ((Drunkard.X + dx > 0 && Drunkard.X + dx < mapWidth - 1) && (Drunkard.Y + dy > 0 && Drunkard.Y + dy < mapHeight - 1))
                {
                    Drunkard.X += dx;
                    Drunkard.Y += dy;
                    this.Halls.Add(Drunkard);
                }
            }
            
            
        }


        public bool IAmLeaf()
        {
            return (this.Left == null && this.Right == null);
        }

    }
}
