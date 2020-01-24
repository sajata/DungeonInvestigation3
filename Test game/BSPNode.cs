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
        private enum Orientation { Horizontal, Veritcal };
        private const double MaxSplitRatio = 1.45;
        private const double splitPaddingPercent = 0.2;
        private int splitPaddingSize;

        public BSPNode(Rectangle leaf)           
        {
            Leaf = leaf;
        }

        public bool Partition(ref Random r, int MinLeafSize)
        {
            Orientation SplitOrientation = new Orientation();
            int splitSize = 0;

            if(this.Leaf.Width / this.Leaf.Height >= MaxSplitRatio)
            {
                SplitOrientation = Orientation.Veritcal;
            }
            else if(this.Leaf.Height / this.Leaf.Width >= MaxSplitRatio)
            {
                SplitOrientation = Orientation.Horizontal;
            }
            else
            {
                if(r.Next(0,2) == 1)
                {
                    SplitOrientation = Orientation.Horizontal;
                }
                else
                {
                    SplitOrientation = Orientation.Veritcal;
                }
            }

            if(SplitOrientation == Orientation.Veritcal)
            {
                splitPaddingSize = (int)(this.Leaf.Width * splitPaddingPercent);
                splitSize = r.Next(this.Leaf.Left + splitPaddingSize, this.Leaf.Right - splitPaddingSize);
                this.Left = new BSPNode(new Rectangle(this.Leaf.Left, this.Leaf.Top , splitSize , this.Leaf.Height));
                this.Right = new BSPNode(new Rectangle(splitSize, this.Leaf.Top, this.Leaf.Right - splitSize, this.Leaf.Height));
            }
            else if (SplitOrientation == Orientation.Horizontal)
            {
                splitPaddingSize = (int)(this.Leaf.Height * splitPaddingPercent);
                splitSize = r.Next(this.Leaf.Top + splitPaddingSize, this.Leaf.Bottom - splitPaddingSize);
                this.Left = new BSPNode(new Rectangle(this.Leaf.Left, this.Leaf.Top, this.Leaf.Width, splitSize));
                this.Right = new BSPNode(new Rectangle(this.Leaf.Left, splitSize, this.Leaf.Width, this.Leaf.Height - splitSize));
            }

            return true; 
        }
        public bool IAmLeaf()
        {
            return (this.Left == null && this.Right == null);
        }
    }
}
