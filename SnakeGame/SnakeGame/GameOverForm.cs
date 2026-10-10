using System;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class GameOverForm : Form
    {
      
        public enum GameAction { PlayAgain, ViewHighScore, MainMenu }
        public GameAction SelectedAction { get; private set; } = GameAction.MainMenu;

        // Giữ lại để truyền cho HighScoreForm sau này (bạn khác làm)
        public int FinalScore { get; private set; }
        public int HighScore { get; private set; }

        public GameOverForm(int score, int highScore)
        {
            InitializeComponent();
            this.FinalScore = score;
            this.HighScore = highScore;

           
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 550);   // ← Đã giảm từ 620 → 550
            this.BackColor = Color.FromArgb(30, 30, 45);
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            SetupUI();
            SetupFadeIn();

            //============= Cho phép kéo form bằng chuột================//
            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(Handle, 0xA1, 0x2, 0);
                }
            };
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Vẽ ảnh nền nếu có
            try
            {
                Image bg = Image.FromFile("gameover_bg.jpg");
                e.Graphics.DrawImage(bg, this.ClientRectangle);
                bg.Dispose();
            }
            catch
            {
                // Nếu không tìm thấy ảnh → dùng màu đặc
                e.Graphics.Clear(Color.FromArgb(30, 30, 45));
            }
        }
        private void SetupUI()
        {
            // ===== TIÊU ĐỀ "GAME OVER" =====
            Label lblTitle = new Label
            {
                Text = "GAME OVER",
                Font = new Font("Courier New", 42, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 20, 20),
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(500, 90),
                Location = new Point(0, 100)
            };
            this.Controls.Add(lblTitle);

            SetupShake(lblTitle);

            // ===== NÚT PLAY AGAIN =====
            Button btnPlayAgain = CreateButton(
                "▶  PLAY AGAIN", Color.FromArgb(76, 175, 80),
                new Point(55, 260)
            );
            btnPlayAgain.Click += (s, e) =>
            {
                SelectedAction = GameAction.PlayAgain;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnPlayAgain);

            // ===== NÚT VIEW HIGH SCORE =====
            Button btnViewHigh = CreateButton(
                "🏆  VIEW HIGH SCORE", Color.FromArgb(33, 150, 243),
                new Point(55, 340)
            );
            btnViewHigh.Click += (s, e) =>
            {
                SelectedAction = GameAction.ViewHighScore;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnViewHigh);

            // ===== NÚT MAIN MENU =====
            Button btnMainMenu = CreateButton(
                "🏠  MAIN MENU", Color.FromArgb(120, 120, 140),
                new Point(55, 420)
            );
            btnMainMenu.Click += (s, e) =>
            {
                SelectedAction = GameAction.MainMenu;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            this.Controls.Add(btnMainMenu);
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
            Color pressedColor = ControlPaint.Dark(backColor, 0.2f);
            Point originalLocation = location;

            // Hover: đổi màu + dịch lên 2px
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

            // Ấn xuống: nút lún xuống 3px + màu tối hơn
            btn.MouseDown += (s, e) =>
            {
                btn.BackColor = pressedColor;
                btn.Location = new Point(originalLocation.X, originalLocation.Y + 3);
            };
            btn.MouseUp += (s, e) =>
            {
                btn.BackColor = hoverColor;
                btn.Location = new Point(originalLocation.X, originalLocation.Y - 2);
            };

            // Vẽ chữ không anti-alias (giữ style 8-bit cho chữ)
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
        private void SetupFadeIn()
        {
            this.Opacity = 0;                          // Bắt đầu từ mờ hoàn toàn
            Timer fadeTimer = new Timer { Interval = 15 };   // 15ms mỗi lần cập nhật
            fadeTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;              // Mỗi tick tăng 5% độ rõ
                else
                {
                    this.Opacity = 1;                  // Đảm bảo dừng ở 100%
                    fadeTimer.Stop();
                    fadeTimer.Dispose();               // Giải phóng timer
                }
            };
            fadeTimer.Start();
        }

        private void SetupShake(Label lbl)
        {
            int originalX = lbl.Location.X;
            int originalY = lbl.Location.Y;
            int tick = 0;

            Timer shake = new Timer { Interval = 50 };
            shake.Tick += (s, e) =>
            {
                tick++;
                // Rung NHẸ: chỉ 2 pixel mỗi bên
                int offsetX = (tick % 2 == 0) ? 2 : -2;
                lbl.Location = new Point(originalX + offsetX, originalY);

                // Rung 8 lần (khoảng 0.4 giây) rồi dừng
                if (tick >= 8)
                {
                    shake.Stop();
                    shake.Dispose();
                    lbl.Location = new Point(originalX, originalY);
                }
            };

            // Đợi 200ms cho Fade In xong rồi mới rung
            Timer delay = new Timer { Interval = 200 };
            delay.Tick += (s, e) =>
            {
                delay.Stop();
                delay.Dispose();
                shake.Start();
            };
            delay.Start();
        }
    }

    // Hỗ trợ kéo form không viền
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}