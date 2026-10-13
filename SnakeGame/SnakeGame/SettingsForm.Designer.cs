namespace SnakeGame
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDiffTitle = new System.Windows.Forms.Label();
            this.lblSoundTitle = new System.Windows.Forms.Label();
            this.btnEasy = new System.Windows.Forms.Button();
            this.btnMedium = new System.Windows.Forms.Button();
            this.btnHard = new System.Windows.Forms.Button();
            this.btnSoundToggle = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(29, 62);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(370, 57);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "⚙ SETTINGS";
            // 
            // lblDiffTitle
            // 
            this.lblDiffTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblDiffTitle.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiffTitle.ForeColor = System.Drawing.Color.White;
            this.lblDiffTitle.Location = new System.Drawing.Point(32, 147);
            this.lblDiffTitle.Name = "lblDiffTitle";
            this.lblDiffTitle.Size = new System.Drawing.Size(400, 30);
            this.lblDiffTitle.TabIndex = 1;
            this.lblDiffTitle.Text = "Difficulty:";
            // 
            // lblSoundTitle
            // 
            this.lblSoundTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSoundTitle.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoundTitle.ForeColor = System.Drawing.Color.White;
            this.lblSoundTitle.Location = new System.Drawing.Point(32, 277);
            this.lblSoundTitle.Name = "lblSoundTitle";
            this.lblSoundTitle.Size = new System.Drawing.Size(400, 30);
            this.lblSoundTitle.TabIndex = 2;
            this.lblSoundTitle.Text = "Sound:";
            // 
            // btnEasy
            // 
            this.btnEasy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnEasy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEasy.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEasy.ForeColor = System.Drawing.Color.White;
            this.btnEasy.Location = new System.Drawing.Point(37, 202);
            this.btnEasy.Name = "btnEasy";
            this.btnEasy.Size = new System.Drawing.Size(130, 50);
            this.btnEasy.TabIndex = 3;
            this.btnEasy.Text = "EASY";
            this.btnEasy.UseVisualStyleBackColor = false;
            this.btnEasy.Click += new System.EventHandler(this.btnEasy_Click);
            // 
            // btnMedium
            // 
            this.btnMedium.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnMedium.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMedium.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMedium.ForeColor = System.Drawing.Color.White;
            this.btnMedium.Location = new System.Drawing.Point(182, 202);
            this.btnMedium.Name = "btnMedium";
            this.btnMedium.Size = new System.Drawing.Size(130, 50);
            this.btnMedium.TabIndex = 4;
            this.btnMedium.Text = "MEDIUM";
            this.btnMedium.UseVisualStyleBackColor = false;
            this.btnMedium.Click += new System.EventHandler(this.btnMedium_Click);
            // 
            // btnHard
            // 
            this.btnHard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnHard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHard.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHard.ForeColor = System.Drawing.Color.White;
            this.btnHard.Location = new System.Drawing.Point(327, 202);
            this.btnHard.Name = "btnHard";
            this.btnHard.Size = new System.Drawing.Size(130, 50);
            this.btnHard.TabIndex = 5;
            this.btnHard.Text = "\tHARD";
            this.btnHard.UseVisualStyleBackColor = false;
            this.btnHard.Click += new System.EventHandler(this.btnHard_Click);
            // 
            // btnSoundToggle
            // 
            this.btnSoundToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnSoundToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSoundToggle.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSoundToggle.ForeColor = System.Drawing.Color.White;
            this.btnSoundToggle.Location = new System.Drawing.Point(37, 330);
            this.btnSoundToggle.Name = "btnSoundToggle";
            this.btnSoundToggle.Size = new System.Drawing.Size(420, 50);
            this.btnSoundToggle.TabIndex = 6;
            this.btnSoundToggle.Text = "🔊 SOUND: ON";
            this.btnSoundToggle.UseVisualStyleBackColor = false;
            this.btnSoundToggle.Click += new System.EventHandler(this.btnSoundToggle_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(37, 412);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(420, 50);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "EXIT";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SettingsForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.BackgroundImage = global::SnakeGame.Properties.Resources.daaece34463f6b599008ced4b7c0c280;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(494, 500);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSoundToggle);
            this.Controls.Add(this.btnHard);
            this.Controls.Add(this.btnMedium);
            this.Controls.Add(this.btnEasy);
            this.Controls.Add(this.lblSoundTitle);
            this.Controls.Add(this.lblDiffTitle);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDiffTitle;
        private System.Windows.Forms.Label lblSoundTitle;
        private System.Windows.Forms.Button btnEasy;
        private System.Windows.Forms.Button btnMedium;
        private System.Windows.Forms.Button btnHard;
        private System.Windows.Forms.Button btnSoundToggle;
        private System.Windows.Forms.Button btnClose;
    }
}