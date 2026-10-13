using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public class Piece : PictureBox
    {
        public Piece(int x, int y, Image image = null)
        {
            Location = new Point(x, y);
            Size = new Size(20, 20);
            BackColor = Color.Transparent;
            SizeMode = PictureBoxSizeMode.Zoom;
            Enabled = false;
            if (image != null)
                Image = image;
        }
    }
}