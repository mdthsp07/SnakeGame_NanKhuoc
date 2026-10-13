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
            SetupFadeIn();
            AttachButtonEffects();
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

        
        private void AttachButtonEffects()
        {
            AttachEffectToButton(btnPlay, Color.FromArgb(76, 175, 80));
            AttachEffectToButton(btnTutorial, Color.FromArgb(255, 152, 0));
            AttachEffectToButton(btnSettings, Color.FromArgb(187, 143, 206));
            AttachEffectToButton(btnExit, Color.FromArgb(180, 50, 50));
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

        // ===== XỬ LÝ SỰ KIỆN CLICK =====
        private void btnPlay_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            Snake game = new Snake();
            game.Show();
            this.Hide();
            game.FormClosed += (ss, ee) =>
            {
                this.Show();
                this.Focus();
            };
        }

        private void btnTutorial_Click(object sender, EventArgs e)
        {
            // MỞ FORM HƯỚNG DẪN Ở ĐÂY
            SoundManager.Instance.PlayClick();
            TutorialForm tut = new TutorialForm();
            tut.ShowDialog();
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            Application.Exit();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            SettingsForm settings = new SettingsForm();
            settings.ShowDialog();
        }
    }
}