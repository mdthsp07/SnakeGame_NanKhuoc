using System;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    class Piece : PictureBox
    {
        public Piece(int x, int y, Image img = null)
        {
            Location = new Point(x, y);
            Size = new Size(20, 20);
            SizeMode = PictureBoxSizeMode.StretchImage; // Co giãn hình vừa khít ô 20x20
            BackColor = Color.Transparent;             // Nền trong suốt để lộ bản đồ cỏ

            if (img != null)
            {
                Image = img;
            }
            else
            {
                BackColor = Color.Green; // Màu mặc định nếu không có ảnh
            }
            Enabled = false;
        }
    }
}