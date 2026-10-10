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
                Text = "𝓗𝓞𝓦 𝓣𝓞 𝓟𝓛𝓐𝓨",
                Font = new Font("Courier New", 25, FontStyle.Bold),
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
                Text =  "𝓤𝓼𝓮 𝓐𝓡𝓡𝓞𝓦 𝓚𝓔𝓨𝓢 𝓽𝓸 𝓬𝓸𝓷𝓽𝓻𝓸𝓵 𝓽𝓱𝓮 𝓼𝓷𝓪𝓴𝓮.\n\n" +
                        "𝓔𝓪𝓽 𝓯𝓻𝓾𝓲𝓽𝓼 𝓽𝓸 𝓰𝓻𝓸𝔀 𝓪𝓷𝓭 𝓰𝓪𝓲𝓷 𝓹𝓸𝓲𝓷𝓽𝓼.\n\n" +
                        "𝓐𝓿𝓸𝓲𝓭 𝔀𝓪𝓵𝓵𝓼 𝓪𝓷𝓭 𝔂𝓸𝓾𝓻𝓼𝓮𝓵𝓯! 𝓰𝓸𝓸𝓭 𝓵𝓾𝓬𝓴!",
                Font = new Font("Courier New", 15, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(430, 230),
                Location = new Point(10, 105),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblContent);

            Button btnClose = new Button
            {
                Text = "GOT IT!",
                Font = new Font("Courier New", 12, FontStyle.Bold),
                Size = new Size(180, 45),
                Location = new Point(135, 340),
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