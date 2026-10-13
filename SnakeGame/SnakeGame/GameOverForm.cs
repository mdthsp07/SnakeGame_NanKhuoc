using System;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class GameOverForm : Form
    {
        public enum GameAction { PlayAgain, ViewHighScore, MainMenu }
        public GameAction SelectedAction { get; private set; } = GameAction.MainMenu;
        public int FinalScore { get; private set; }
        public int HighScore { get; private set; }
        private Timer fadeTimer;
        public GameOverForm(int score, int highScore)
        {
            InitializeComponent();
            this.FinalScore = score;
            this.HighScore = highScore;
            lblScore.Text = "Your Score: " + score;
            lblHighScore.Text = "High Score: " + highScore;

            AttachButtonEffects();
            SetupFadeIn();
            SetupShake(lblTitle);

            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(Handle, 0xA1, 0x2, 0);
                }
            };
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
       
            if (fadeTimer != null)
            {
                fadeTimer.Stop();
                fadeTimer.Dispose();
                fadeTimer = null;
            }
            base.OnFormClosing(e);
        }
       
        private void SetupFadeIn()
        {
            this.Opacity = 0;                         
            fadeTimer = new Timer { Interval = 15 };   
            fadeTimer.Tick += (s, e) =>
            {           
                if (this.IsDisposed)
                {
                    fadeTimer.Stop();
                    fadeTimer.Dispose();
                    return;
                }

                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                {
                    this.Opacity = 1;
                    fadeTimer.Stop();
                    fadeTimer.Dispose();
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
                
                int offsetX = (tick % 2 == 0) ? 2 : -2;
                lbl.Location = new Point(originalX + offsetX, originalY);

              
                if (tick >= 8)
                {
                    shake.Stop();
                    shake.Dispose();
                    lbl.Location = new Point(originalX, originalY);
                }
            };


            Timer delay = new Timer { Interval = 200 };
            delay.Tick += (s, e) =>
            {
                delay.Stop();
                delay.Dispose();
                shake.Start();
            };
            delay.Start();
        }

        private void AttachButtonEffects()
        {
            AttachEffectToButton(btnPlayAgain, Color.FromArgb(76, 175, 80));
            AttachEffectToButton(btnViewHighScore, Color.FromArgb(33, 150, 243));
            AttachEffectToButton(btnMainMenu, Color.FromArgb(120, 120, 140));
        }

        private void AttachEffectToButton(Button btn, Color backColor)
        {
            Color hoverColor = ControlPaint.Light(backColor, 0.3f);
            Color pressedColor = ControlPaint.Dark(backColor, 0.2f);
            Point originalLocation = btn.Location;

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
        }
        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            SelectedAction = GameAction.PlayAgain;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnViewHighScore_Click(object sender, EventArgs e)
        {
            SelectedAction = GameAction.ViewHighScore;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            SelectedAction = GameAction.MainMenu;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lblHighScore_Click(object sender, EventArgs e)
        {

        }
    }
    //
    //Hỗ Trợ Kéo Form
    //
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}