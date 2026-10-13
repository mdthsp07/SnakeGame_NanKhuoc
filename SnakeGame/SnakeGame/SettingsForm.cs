using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class SettingsForm : Form
    {
        private Color selectedColor = Color.FromArgb(187, 143, 206);
        private Color unselectedColor = Color.FromArgb(100, 100, 100);

        public SettingsForm()
        {
            InitializeComponent();
            UpdateDifficultyButtons();
            UpdateSoundButton();
        }

        private void UpdateDifficultyButtons()
        {
            btnEasy.BackColor = (GameSettings.Difficulty == 0) ? selectedColor : unselectedColor;
            btnMedium.BackColor = (GameSettings.Difficulty == 1) ? selectedColor : unselectedColor;
            btnHard.BackColor = (GameSettings.Difficulty == 2) ? selectedColor : unselectedColor;
        }

        private void UpdateSoundButton()
        {
            if (SoundManager.Instance.IsMasterMuted)
            {
                btnSoundToggle.Text = "🔇 SOUND: OFF";
                btnSoundToggle.BackColor = Color.FromArgb(120, 30, 30);
            }
            else
            {
                btnSoundToggle.Text = "🔊 SOUND: ON";
                btnSoundToggle.BackColor = Color.FromArgb(76, 175, 80);
            }
        }

        private void btnEasy_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            GameSettings.Difficulty = 0;
            UpdateDifficultyButtons();
        }

        private void btnMedium_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            GameSettings.Difficulty = 1;
            UpdateDifficultyButtons();
        }

        private void btnHard_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            GameSettings.Difficulty = 2;
            UpdateDifficultyButtons();
        }

        private void btnSoundToggle_Click(object sender, EventArgs e)
        {
            bool newMuteState = !SoundManager.Instance.IsMasterMuted;
            SoundManager.Instance.SetMasterMute(newMuteState);

            if (!newMuteState)
                SoundManager.Instance.PlayClick();

            UpdateSoundButton();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            this.Close();
        }

    }
}