namespace SnakeGame
{
    partial class Snake
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblScore = new System.Windows.Forms.Label();
            this.picFood = new System.Windows.Forms.PictureBox();
            this.picGift = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picFood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGift)).BeginInit();
            this.SuspendLayout();
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.BackColor = System.Drawing.Color.Transparent;
            this.lblScore.ForeColor = System.Drawing.Color.White;
            this.lblScore.Location = new System.Drawing.Point(12, 9);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(124, 27);
            this.lblScore.TabIndex = 0;
            this.lblScore.Text = "Score: 0";
            this.lblScore.Click += new System.EventHandler(this.lblScore_Click);
            // 
            // picFood
            // 
            this.picFood.BackColor = System.Drawing.Color.Transparent;
            this.picFood.Image = global::SnakeGame.Properties.Resources.apple;
            this.picFood.Location = new System.Drawing.Point(433, 197);
            this.picFood.Name = "picFood";
            this.picFood.Size = new System.Drawing.Size(20, 20);
            this.picFood.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFood.TabIndex = 2;
            this.picFood.TabStop = false;
            // 
            // picGift
            // 
            this.picGift.BackColor = System.Drawing.Color.Transparent;
            this.picGift.Image = global::SnakeGame.Properties.Resources.gift_box;
            this.picGift.Location = new System.Drawing.Point(578, 323);
            this.picGift.Name = "picGift";
            this.picGift.Size = new System.Drawing.Size(20, 20);
            this.picGift.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGift.TabIndex = 3;
            this.picGift.TabStop = false;
            this.picGift.Visible = false;
            // 
            // Snake
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SnakeGame.Properties.Resources.grass_bg;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.picGift);
            this.Controls.Add(this.picFood);
            this.Controls.Add(this.lblScore);
            this.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "Snake";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Snake_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picFood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGift)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.PictureBox picFood;
        private System.Windows.Forms.PictureBox picGift;
    }
}

