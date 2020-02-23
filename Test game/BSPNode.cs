using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Test_game
{
    public class BSPNode
    {
        public BSPNode Left;
        public BSPNode Right;
        public Rectangle Leaf;
        public Rectangle Room;
        public List<Rectangle> Halls = new List<Rectangle>();
        private bool HorizontalSplit;
        

        public BSPNode(Rectangle leaf)           
        {
            Leaf = leaf;
        }

        public bool Partition(ref Random r, int MinLeafSize)
        {         
            int MaxSplit = 0;
            int Split = 0;            
            HorizontalSplit = true;

            if(Left != null || Right != null)
            {
                return false;
            }

            if (this.Leaf.Width / this.Leaf.Height >= 1.65)
            {
                HorizontalSplit = false;
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


            if(MaxSplit <= MinLeafSize)
            {
                return false;
            }

            Split = r.Next(MinLeafSize, MaxSplit);

            if(HorizontalSplit)
            {
                Left = new BSPNode(new Rectangle(this.Leaf.X, this.Leaf.Y, this.Leaf.Width, Split));
                Right = new BSPNode(new Rectangle(this.Leaf.X, this.Leaf.Y + Split, this.Leaf.Width, this.Leaf.Height - Split));
            }
            else
            {
                Left = new BSPNode(new Rectangle(this.Leaf.X, this.Leaf.Y, Split, this.Leaf.Height));
                Right = new BSPNode(new Rectangle(this.Leaf.X + Split, this.Leaf.Y, this.Leaf.Width - Split, this.Leaf.Height));
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
                if(this.Left != null)
                {
                    LeftRoom = this.Left.getRoom(ref r);
                }
                if(this.Right != null)
                {
                    RightRoom = this.Right.getRoom(ref r);
                }
                if(LeftRoom == null && RightRoom == null)
                {
                    return nullRec;
                }
                else if(RightRoom == null)
                {
                    return LeftRoom;
                }
                else if( LeftRoom == null)
                {
                    return RightRoom;
                }
                else if(r.Next(0, 2) == 1)
                {
                    return LeftRoom;
                }
                else
                {
                    return RightRoom;
                }

            }
        }

        public void createHall(Rectangle leftRoom, Rectangle rightRoom, ref Random r)
        {
            this.Halls = new List<Rectangle>();
            Point LeftRoomPoint = new Point(r.Next(leftRoom.Left + 1, leftRoom.Right - 2), r.Next(leftRoom.Top + 1, leftRoom.Bottom - 2));
            Point RightRoomPoint = new Point(r.Next(rightRoom.Left + 1, rightRoom.Right - 2), r.Next(rightRoom.Top + 1, rightRoom.Bottom - 2));

            //Veritcal and Horizontal distance between the two points within the two rooms being conncected
            int DistanceX = RightRoomPoint.X - LeftRoomPoint.X;
            int DistanceY = RightRoomPoint.Y - LeftRoomPoint.Y;

            if(DistanceX < 0) // the right room is to the left of the left room
            {
                if (DistanceY < 0)
                {
                    if (r.Next(0, 2) == 1)
                    {
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, LeftRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                    else
                    {
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                }
                else if (DistanceY > 0)
                {
                    if (r.Next(0, 2) == 1)
                    {
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, LeftRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                    else
                    {
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                }
                else // h == 0
                {
                    this.Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, Math.Abs(DistanceX), 1));
                }
            }

            else if(DistanceX > 0)
            {
                if(DistanceY < 0)
                {
                    if (r.Next(0, 2) == 1)
                    {
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, RightRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                    else
                    {
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                }
                else if(DistanceY > 0)
                {
                    if (r.Next(0, 2) == 1)
                    {
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(RightRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                    else
                    {
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, RightRoomPoint.Y, Math.Abs(DistanceX), 1));
                        this.Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                }
                else // DIstance Y ===0;
                {
                    this.Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, Math.Abs(DistanceX), 1));
                }
            }
            else //Distance X == 0
            {
                if (DistanceY < 0)
                {
                    this.Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                }
                else
                {
                    this.Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                }
            }
        }


        public bool IAmLeaf()
        {
            return (this.Left == null && this.Right == null);
        }
    }
}