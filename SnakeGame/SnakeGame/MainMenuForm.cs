using System;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 550);
            this.BackColor = Color.FromArgb(20, 10, 35);
            this.DoubleBuffered = true;

            SetupUI();
            SetupFadeIn();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(70, 10, 90),
                Color.FromArgb(10, 5, 20),
                90f))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        private void SetupUI()
        {
            // ===== TIÊU ĐỀ "SNAKE GAME" =====
            Label lblTitle = new Label
            {
                Text = "SNAKE GAME",
                Font = new Font("Courier New", 38, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 255, 120),
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(500, 120),
                Location = new Point(0, 80)
            };
            this.Controls.Add(lblTitle);

            // ===== NÚT PLAY GAME =====
            Button btnPlay = CreateButton(
                "▶  PLAY GAME", Color.FromArgb(76, 175, 80),
                new Point(55, 260)
            );
            btnPlay.Click += (s, e) =>
            {
                Snake game = new Snake();
                game.Show();
                this.Hide();
                game.FormClosed += (ss, ee) =>
                {
                    this.Show();
                    this.Focus();
                };
            };
            this.Controls.Add(btnPlay);

            // ===== NÚT HOW TO PLAY =====
            Button btnTutorial = CreateButton(
                "📖  HOW TO PLAY", Color.FromArgb(255, 152, 0),
                new Point(55, 340)
            );
            btnTutorial.Click += (s, e) =>
            {
                TutorialForm tut = new TutorialForm();
                tut.ShowDialog();
            };
            this.Controls.Add(btnTutorial);

            // ===== NÚT EXIT =====
            Button btnExit = CreateButton(
                "✕  EXIT", Color.FromArgb(180, 50, 50),
                new Point(55, 420)
            );
            btnExit.Click += (s, e) =>
            {
                Application.Exit();
            };
            this.Controls.Add(btnExit);
        }

        private void SetupFadeIn()
        {
            this.Opacity = 0;
            Timer fadeTimer = new Timer { Interval = 15 };
            fadeTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1) this.Opacity += 0.05;
                else
                {
                    this.Opacity = 1;
                    fadeTimer.Stop();
                    fadeTimer.Dispose();
                }
            };
            fadeTimer.Start();
        }

        private Button CreateButton(string text, Color backColor, Point location)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Courier New", 15, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(390, 60),
                Location = location,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;

            Color hoverColor = ControlPaint.Light(backColor, 0.3f);
            Point originalLocation = location;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = hoverColor;
                btn.Location = new Point(originalLocation.X, originalLocation.Y - 2);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = backColor;
                btn.Location = originalLocation;
            };

            return btn;
        }
    }
}