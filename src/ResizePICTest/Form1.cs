using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResizePICTest
{
    public partial class Form1 : Form
    {
        private bool IsDragging = false;
        private int GripGap = 10;
        private Point BeginPoint = new Point();
        private DIRECTION Dir = DIRECTION.UNKNOWN;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateCursor(Point current)
        {
            if (current.X - pictureBox2.ClientRectangle.Left <= GripGap)
                Dir = DIRECTION.WEST;
            else if (pictureBox2.ClientRectangle.Right - current.X <= GripGap)
                Dir = DIRECTION.EAST;
            else if (current.Y - pictureBox2.ClientRectangle.Top <= GripGap)
                Dir = DIRECTION.NORTH;
            else if (pictureBox2.ClientRectangle.Bottom - current.Y <= GripGap)
                Dir = DIRECTION.SOUTH;

            if (Dir == DIRECTION.WEST || Dir == DIRECTION.EAST)
                pictureBox2.Cursor = Cursors.SizeWE;
            else if (Dir == DIRECTION.NORTH || Dir == DIRECTION.SOUTH)
                pictureBox2.Cursor = Cursors.SizeNS;
        }

        private void North_Resize(Point old, Point current) 
        {
            var height = pictureBox2.Size.Height;
            var newsize = pictureBox2.Size;
            newsize.Height += (old.Y - current.Y);
            pictureBox2.Size = newsize;

            var newposition = pictureBox2.Location;
            newposition.Y += (current.Y-old.Y);
            pictureBox2.Location = newposition;

            pictureBox2.Invalidate();
        }
        private void South_Resize(Point old, Point current) 
        {
            var height = pictureBox2.Size.Height;
            var newsize = pictureBox2.Size;
            newsize.Height += (current.Y - old.Y);
            pictureBox2.Size = newsize;
            pictureBox2.Invalidate();
        }
        private void West_Resize(Point old, Point current) 
        {
            var width = pictureBox2.Size.Width;
            var newsize = pictureBox2.Size;
            newsize.Width += (old.X - current.X);
            pictureBox2.Size = newsize;

            var newposition = pictureBox2.Location;
            newposition.X += (current.X - old.X);
            pictureBox2.Location = newposition;
            pictureBox2.Invalidate();
        }
        private void East_Resize(Point old, Point current) 
        {
            var width = pictureBox2.Size.Width;
            var newsize = pictureBox2.Size;
            newsize.Width += (current.X - old.X);
            pictureBox2.Size = newsize;
            pictureBox2.Invalidate();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                IsDragging = true;
                BeginPoint = pictureBox2.PointToScreen(e.Location);
                //BeginPoint = e.Location;
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                IsDragging = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                var new_point = pictureBox2.PointToScreen(e.Location);

                switch(Dir)
                {
                    case DIRECTION.WEST:
                        West_Resize(BeginPoint, new_point);
                        break;
                    case DIRECTION.EAST:
                        East_Resize(BeginPoint, new_point);
                        break;
                    case DIRECTION.NORTH:
                        North_Resize(BeginPoint, new_point);
                        break;
                    case DIRECTION.SOUTH:
                        South_Resize(BeginPoint, new_point);
                        break;
                }

                BeginPoint = pictureBox2.PointToScreen(e.Location);
            }
            else
            {
                UpdateCursor(e.Location);
            }
        }
        
    }

    public enum DIRECTION
    { 
        UNKNOWN = 0,
        NORTH = 1,
        WEST = 2,
        SOUTH = 3,
        EAST = 4
    }
}
