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
        //Nodes children
        //Since this is a binary tree algorithm
        //each node has 2 children
        public BSPNode Left;
        public BSPNode Right;

        //stores the dimension of the leaf size
        public Rectangle Leaf;
        //stores the dimensions of the room
        public Rectangle Room;
        //stores each corridor this node has
        public List<Rectangle> Halls = new List<Rectangle>();
        //determines wether this node will be split horizontally of vertically
        private bool HorizontalSplit;
        

        public BSPNode(Rectangle leaf)           
        {
            Leaf = leaf;
        }

        /// <summary>
        /// Reuturns wether its possible to partition this node
        /// If it can partition the node it does so
        /// creating the right and left child nodes for it
        /// </summary>
        /// <param name="r"></param>
        /// <param name="MinLeafSize"></param>
        /// <returns></returns>
        public bool Partition(ref Random r, int MinLeafSize)
        {                   
            //stores the maximum
            int MaxSplit = 0;
            int Split = 0;            
            HorizontalSplit = true;

            //if this node has been partioned exit
            if(Left != null || Right != null)
            {
                return false;
            }

            //if the leaf is too wide split it veritcally
            if (this.Leaf.Width / this.Leaf.Height >= 1.65)
            {
                HorizontalSplit = false;
            }
          
            //if the leaf isn't too wide choose the split orientation randomly
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

            //MaxSplit = (HorizontalSplit ? Leaf.Height : Leaf.Width) - MinLeafSize;

            //If splitting horizontally set the max split position 
            //within the leafs height
            if(HorizontalSplit)
            {
                MaxSplit = Leaf.Height - MinLeafSize;
            }
            //If splitting veritcally set the max split position
            //within the widths width
            else
            {
                MaxSplit = Leaf.Width - MinLeafSize;
            }

            //if the leaf is too small too partition
            //exit
            if(MaxSplit <= MinLeafSize)
            {
                return false;
            }

            //picks a random split position
            //between the max split position 
            //and minimum leaf size
            Split = r.Next(MinLeafSize, MaxSplit);

            //Partitions this node into the two child nodes
            //according to the split orientation
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

        /// <summary>
        /// Randomly generates a room within the confines of that nodes leaf
        /// </summary>
        /// <param name="r"></param>
        /// <param name="MinRoomSize"></param>
        public void MakeRoom(ref Random r, int MinRoomSize)
        {
            int width, height, x, y;

            //sets the size of the room randomly
            width = r.Next(MinRoomSize, this.Leaf.Width);
            height = r.Next(MinRoomSize, this.Leaf.Height);
            //sets the position of the room randomly
            x = r.Next(this.Leaf.Left, this.Leaf.Right - width);
            y = r.Next(this.Leaf.Top, this.Leaf.Bottom - height);

            this.Room = new Rectangle(x, y, width, height);
        }


        /// <summary>
        /// Returns the room from the ndoes respective leaf at the end of the branch
        /// If the end of the branch has two leafs randomly pick a room between the two leafs
        /// Essentially a depth first search of the binary tree
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Rectangle getRoom(ref Random r)
        {
            Rectangle LeftRoom = new Rectangle();
            Rectangle RightRoom = new Rectangle();
            //workaround non nullable structures
            Rectangle nullRec = new Rectangle();

            // if this node is a leaf
            // return its room
            if (this.Room != nullRec)
            {
                return this.Room;
            }
            else
            {
                //if the left chil isnt null
                //try and gets it room recursively
                if(this.Left != null)
                {
                    LeftRoom = this.Left.getRoom(ref r);
                }
                //if the right child isn't null
                //try and get its room recursively
                if(this.Right != null)
                {
                    RightRoom = this.Right.getRoom(ref r);
                }
                //if both children's rooms are null
                //there is no room to get so return the null rectangle
                if(LeftRoom == null && RightRoom == null)
                {
                    return nullRec;
                }
                //if the right childs room is null
                //return the left child's room
                else if(RightRoom == null)
                {
                    return LeftRoom;
                }
                //if the left child's room is null
                //return the right child's room
                else if(LeftRoom == null)
                {
                    return RightRoom;
                }
                //if both the left child and right child have a room
                //randomly pick a room
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

        /// <summary>
        /// Creates a random L shaped hallway between the two rooms
        /// </summary>
        /// <param name="leftRoom"></param>
        /// <param name="rightRoom"></param>
        /// <param name="r"></param>
        public void createHall(Rectangle leftRoom, Rectangle rightRoom, ref Random r)
        {            
            this.Halls = new List<Rectangle>();

            //picks a random point within in each room
            Point LeftRoomPoint = new Point(r.Next(leftRoom.Left + 1, leftRoom.Right - 2), r.Next(leftRoom.Top + 1, leftRoom.Bottom - 2));
            Point RightRoomPoint = new Point(r.Next(rightRoom.Left + 1, rightRoom.Right - 2), r.Next(rightRoom.Top + 1, rightRoom.Bottom - 2));
         
            //to simplify the code, the left point will always be on the left
            if(LeftRoomPoint.X > RightRoomPoint.X)
            {
                Point temp = LeftRoomPoint;
                LeftRoomPoint = RightRoomPoint;
                RightRoomPoint = temp;
            }

            //Veritcal and Horizontal distance between the two points within the two rooms being conncected
            int DistanceX = LeftRoomPoint.X - RightRoomPoint.X;
            int DistanceY = LeftRoomPoint.Y - RightRoomPoint.Y;

            //if the points are not alligned horizontally
            if (DistanceX != 0)
            {
                //flip a coin to decide wether to go vertically and then horizontally 
                //or the opposite
                //if 1 create a horiontal corridor first : "---: shaped tunnel 
                //                                             |
                if(r.Next(0,2) == 1)
                {
                    //add corridor to from the leftpoint to right point
                    Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, Math.Abs(DistanceX) +1, 1));

                    //builds left point is above right point build a corridor downwards and vice versa
                    // build down
                    if(DistanceY <0)
                    {
                        Halls.Add(new Rectangle(RightRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                    //build up
                    else
                    {
                        Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                }
                //if 0 create verticla corridor first : "L" shaped tunnel
                else
                {
                    //add a vertical corridor
                    //build down
                    if (DistanceY < 0)
                    {
                        Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }
                    //build up
                    else
                    {
                        Halls.Add(new Rectangle(LeftRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                    }

                    //then go to the right
                    Halls.Add(new Rectangle(LeftRoomPoint.X, RightRoomPoint.Y, Math.Abs(DistanceX) +1 , 1));
                }
            }

            //if the points are alligned horizontally
            //just go up or down depending on their positions
            else
            {
                //add a vertical corridor
                //build down
                if (DistanceY < 0)
                {
                    Halls.Add(new Rectangle(LeftRoomPoint.X, LeftRoomPoint.Y, 1, Math.Abs(DistanceY)));
                }
                //build up
                else
                {
                    Halls.Add(new Rectangle(RightRoomPoint.X, RightRoomPoint.Y, 1, Math.Abs(DistanceY)));
                }
            }
        }

        // a node is a leaf if it doesn't have any children 
        public bool IAmLeaf()
        {
            return (this.Left == null && this.Right == null);
        }
    }
}