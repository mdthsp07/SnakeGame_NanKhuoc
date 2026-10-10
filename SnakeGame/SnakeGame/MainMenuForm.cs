using System;
using System.Drawing;
using System.IO;
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
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;

            SetupUI();
            SetupFadeIn();
        }

        // ===== ẢNH NỀN =====
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Image bg = Image.FromFile("menu_bg.jpg");
                e.Graphics.DrawImage(bg, this.ClientRectangle);
                bg.Dispose();
            }
            catch
            {
                e.Graphics.Clear(Color.Black);
            }
        }

        private void SetupUI()
        {
            // ===== LOGO ẢNH =====
            PictureBox picLogo = new PictureBox
            {
                Size = new Size(400, 200),
                Location = new Point(50, 40),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            try
            {
                string[] paths = new string[]
                {
                    "logo.png",
                    "Resources/Images/logo.png",
                    "Assets/logo.png",
                    "Asset/logo.png",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png")
                };

                foreach (string path in paths)
                {
                    if (File.Exists(path))
                    {
                        picLogo.Image = Image.FromFile(path);
                        break;
                    }
                }
            }
            catch { }

            this.Controls.Add(picLogo);

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

        // ===== NÚT BẤM VỚI HIỆU ỨNG HOVER + CLICK =====
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
            Color pressedColor = ControlPaint.Dark(backColor, 0.2f);
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

           
            btn.MouseDown += (s, e) =>
            {
                SoundManager.Instance.PlayClick();
                btn.BackColor = pressedColor;
                btn.Location = new Point(originalLocation.X, originalLocation.Y + 3);
            };
            btn.MouseUp += (s, e) =>
            {
                btn.BackColor = hoverColor;
                btn.Location = new Point(originalLocation.X, originalLocation.Y - 2);
            };

           
            btn.Paint += (s, e) =>
            {
                e.Graphics.Clear(btn.BackColor);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                TextRenderer.DrawText(
                    e.Graphics, btn.Text, btn.Font,
                    btn.ClientRectangle, btn.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            };

            return btn;
        }

        // ===== HIỆU ỨNG FADE IN =====
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
    }
}