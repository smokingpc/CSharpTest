using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureBoxTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void InitUI()
        {
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Parent = pictureBox1;
            label1.Dock = DockStyle.Fill;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Parent = label1;
            label2.Dock = DockStyle.Fill;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox2.BackgroundImage, new Rectangle(new Point(0, 0), pictureBox2.Size));

            
            Pen p = new Pen(new SolidBrush(Color.GreenYellow), 10.0f);
            e.Graphics.DrawLine(p, new Point(pictureBox2.Size.Width / 2, 0),
                                    new Point(pictureBox2.Size.Width / 2, pictureBox2.Size.Height));
            e.Graphics.DrawLine(p, new Point(0, pictureBox2.Size.Height / 2),
                                    new Point(pictureBox2.Size.Width, pictureBox2.Size.Height / 2));

            //e.Graphics.tr
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(new SolidBrush(Color.GreenYellow), 3.0f);

            Point[] begin = new Point[]
            {
                new Point(0, 0),
                new Point(pictureBox1.Size.Width/4 * 1, 0),
                new Point(pictureBox1.Size.Width/4 * 2, 0),
                new Point(pictureBox1.Size.Width/4 * 3, 0),
                new Point(0, 0),
                new Point(0, pictureBox1.Size.Height/4 *1),
                new Point(0, pictureBox1.Size.Height/4 *2),
                new Point(0, pictureBox1.Size.Height/4 *3),
            };

            Point[] end = new Point[]
            {
                new Point(0, pictureBox1.Size.Height),
                new Point(pictureBox1.Size.Width/4 * 1, pictureBox1.Size.Height),
                new Point(pictureBox1.Size.Width/4 * 2, pictureBox1.Size.Height),
                new Point(pictureBox1.Size.Width/4 * 3, pictureBox1.Size.Height),
                new Point(pictureBox1.Size.Width, 0),
                new Point(pictureBox1.Size.Width, pictureBox1.Size.Height/4 *1),
                new Point(pictureBox1.Size.Width, pictureBox1.Size.Height/4 *2),
                new Point(pictureBox1.Size.Width, pictureBox1.Size.Height/4 *3),
            };

            for (int i = 0; i < begin.Length; i++)
            {
                e.Graphics.DrawLine(p, begin[i], end[i]);
            }
        }
    }
}
