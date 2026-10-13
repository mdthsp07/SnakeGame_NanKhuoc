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
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayClick();
            this.Close();
        }

        private void lblContent_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
