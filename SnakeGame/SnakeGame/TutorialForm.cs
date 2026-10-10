using System;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class TutorialForm : Form
    {
        public TutorialForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(450, 400);
            this.BackColor = Color.FromArgb(25, 15, 40);
            this.DoubleBuffered = true;

            SetupUI();
        }

        private void SetupUI()
        {
            Label lblTitle = new Label
            {
                Text = "HOW TO PLAY",
                Font = new Font("Courier New", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 255),
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(450, 50),
                Location = new Point(0, 30)
            };
            this.Controls.Add(lblTitle);

            Label lblContent = new Label
            {
                Text = "• Use ARROW KEYS to control the snake.\n\n" +
                       "• Eat fruits to grow and gain points.\n\n" +
                       "• Avoid walls, rocks, and yourself!\n\n" +
                       "• Use ⚙ SETTINGS to toggle music.",
                Font = new Font("Courier New", 11, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(400, 200),
                Location = new Point(30, 100),
                TextAlign = ContentAlignment.TopLeft
            };
            this.Controls.Add(lblContent);

            Button btnClose = new Button
            {
                Text = "GOT IT!",
                Font = new Font("Courier New", 12, FontStyle.Bold),
                Size = new Size(180, 45),
                Location = new Point(135, 320),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }
    }
}