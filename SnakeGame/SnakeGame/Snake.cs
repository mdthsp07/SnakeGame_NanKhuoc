using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Snake : Form
    {
        int cols = 50, rows = 25, score = 0, highScore = 0, dx = 0, dy = 0, front = 0, back = 0;
        Piece[] snake = new Piece[1250];
        List<int> available = new List<int>();
        bool[,] visit;

        // Chướng ngại vật & Tốc độ
        List<Piece> obstacles = new List<Piece>();
        bool hasObstacles = false;
        int gameSpeed = 120; // Default: Easy (Slow)

        Random rand = new Random();
        Timer timer = new Timer();
        SoundManager sound = new SoundManager();

        // Model / Images
        Image imgHead, imgBody, imgDeadHead, imgDeadBody, imgRock;
        List<Image> foodImages = new List<Image>();

        // Controls
        private PictureBox picFood;
        private Button btnInGameSettings;

        // UI Panels
        private Panel pnlMenu;
        private Label lblTitle;
        private Label lblSpeedOption;
        private ComboBox cboSpeed;
        private CheckBox chkObstacles;
        private Button btnStart;
        private Button btnTutorial;

        private Panel pnlGameOver;
        private Label lblGameOverTitle;
        private Label lblFinalScore;
        private Label lblHighScore;
        private Button btnRestart;
        private Button btnMenu;

        private Panel pnlTutorial;
        private Label lblTutTitle;
        private Label lblTutContent;
        private Button btnCloseTut;

        private Panel pnlSettings;
        private Label lblSetTitle;
        private CheckBox chkBgm;
        private CheckBox chkSfx;
        private Button btnCloseSet;

        public Snake()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            InitFoodControl();
            InitInGameUI();
            LoadGameAssets();

            CreateMenuPanel();
            CreateGameOverPanel();
            CreateTutorialPanel();
            CreateSettingsPanel();

            sound.PlayBackground("background.wav");
        }

        private void InitFoodControl()
        {
            if (lblFood != null) lblFood.Visible = false;

            picFood = new PictureBox
            {
                Size = new Size(20, 20),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Visible = false
            };
            this.Controls.Add(picFood);
        }

        private void InitInGameUI()
        {
            btnInGameSettings = new Button
            {
                Text = "⚙ SETTINGS",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(100, 28),
                Location = new Point(this.ClientSize.Width - 110, 5),
                BackColor = Color.FromArgb(180, 40, 40, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TabStop = false,
                Visible = false
            };
            btnInGameSettings.FlatAppearance.BorderSize = 0;
            btnInGameSettings.Click += (s, e) =>
            {
                timer.Stop(); // Tạm dừng game khi mở Settings
                pnlSettings.Visible = true;
                pnlSettings.BringToFront();
            };
            this.Controls.Add(btnInGameSettings);
        }

        // Xoay ảnh đầu rắn chuẩn theo ảnh Head.png có 2 mắt hướng LÊN TRÊN
        private Image GetRotatedHeadImage(Image sourceImg, int moveX, int moveY)
        {
            if (sourceImg == null) return null;

            Bitmap bmp = new Bitmap(sourceImg);

            if (moveX == 0 && moveY == -20)
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipNone); // Lên trên
            else if (moveX == 0 && moveY == 20)
                bmp.RotateFlip(RotateFlipType.Rotate180FlipNone); // Xuống dưới
            else if (moveX == 20 && moveY == 0)
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);  // Sang phải
            else if (moveX == -20 && moveY == 0)
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone); // Sang trái

            return bmp;
        }

        // Tìm đường dẫn file, ưu tiên kiểm tra thư mục "Asset"
        private string FindAssetPath(string fileName)
        {
            string[] searchPaths = new string[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Asset", fileName), // Ưu tiên 1: bin/Debug/Asset/
                Path.Combine("Asset", fileName),                                        // Ưu tiên 2: Thư mục Asset tương đối
                fileName,                                                                // Ưu tiên 3: Thư mục gốc
                Path.Combine("..", "Asset", fileName)                                   // Ưu tiên 4: Thư mục project gốc/Asset
            };

            foreach (string path in searchPaths)
            {
                if (File.Exists(path)) return path;
            }
            return null;
        }

        private Image LoadImageSafe(string fileName)
        {
            string path = FindAssetPath(fileName);
            if (path != null)
            {
                try { return Image.FromFile(path); } catch { }
            }
            return null;
        }

        private void LoadGameAssets()
        {
            imgHead = LoadImageSafe("Head.png");
            imgBody = LoadImageSafe("Body.png");
            imgDeadHead = LoadImageSafe("DeadHead.png");
            imgDeadBody = LoadImageSafe("DeadBody.png");

            string[] foodFiles = new string[]
            {
                "apple_17.png", "apple_04.png", "px_apple_02.png", "apple_06.png",
                "Food.png", "apple_17_2.png", "apple_04_2.png"
            };

            foreach (string foodFile in foodFiles)
            {
                Image img = LoadImageSafe(foodFile);
                if (img != null) foodImages.Add(img);
            }

            Image customRock = LoadImageSafe("rock.png");
            imgRock = customRock ?? CreateRockTexture();
        }

        private Image CreateRockTexture()
        {
            Bitmap bmp = new Bitmap(20, 20);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(110, 110, 115));
                using (Pen darkPen = new Pen(Color.FromArgb(70, 70, 75), 1))
                {
                    g.DrawRectangle(darkPen, 0, 0, 19, 19);
                    g.DrawLine(darkPen, 3, 3, 8, 12);
                    g.DrawLine(darkPen, 8, 12, 16, 6);
                }
            }
            return bmp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int tileSize = 20;

            Color grassLight = Color.FromArgb(162, 209, 73);
            Color grassDark = Color.FromArgb(170, 215, 81);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Brush grassBrush = ((r + c) % 2 == 0)
                        ? new SolidBrush(grassLight)
                        : new SolidBrush(grassDark);

                    g.FillRectangle(grassBrush, c * tileSize, r * tileSize, tileSize, tileSize);
                    grassBrush.Dispose();
                }
            }
        }

        private void launchTimer()
        {
            timer.Interval = gameSpeed;
            timer.Tick -= move;
            timer.Tick += move;
            timer.Start();
        }

        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (dx != -20) { dx = 20; dy = 0; }
                    break;
                case Keys.Left:
                    if (dx != 20) { dx = -20; dy = 0; }
                    break;
                case Keys.Up:
                    if (dy != 20) { dx = 0; dy = -20; }
                    break;
                case Keys.Down:
                    if (dy != -20) { dx = 0; dy = 20; }
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                    if (dx != -20) { dx = 20; dy = 0; }
                    return true;
                case Keys.Left:
                    if (dx != 20) { dx = -20; dy = 0; }
                    return true;
                case Keys.Up:
                    if (dy != 20) { dx = 0; dy = -20; }
                    return true;
                case Keys.Down:
                    if (dy != -20) { dx = 0; dy = 20; }
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void move(object sender, EventArgs e)
        {
            int x = snake[front].Location.X, y = snake[front].Location.Y;

            if (dx == 0 && dy == 0) return;

            if (game_over(x + dx, y + dy))
            {
                ShowGameOver();
                return;
            }

            Image rotatedHead = GetRotatedHeadImage(imgHead, dx, dy);

            if (collisionFood(x + dx, y + dy))
            {
                score += 1;
                lblScore.Text = "Score: " + score.ToString();
                sound.PlayEat("eat.wav");

                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                if (snake[front] != null && imgBody != null) snake[front].Image = imgBody;

                Piece head = new Piece(x + dx, y + dy, rotatedHead);
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                randomFood();
            }
            else
            {
                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                if (snake[front] != null && imgBody != null) snake[front].Image = imgBody;

                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);

                if (rotatedHead != null) snake[front].Image = rotatedHead;

                back = (back - 1 + 1250) % 1250;
                visit[(y + dy) / 20, (x + dx) / 20] = true;
            }
        }

        private void randomFood()
        {
            available.Clear();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (!visit[i, j]) available.Add(i * cols + j);

            if (available.Count == 0) return;

            int idx = rand.Next(available.Count);
            int foodX = (available[idx] % cols) * 20;
            int foodY = (available[idx] / cols) * 20;

            picFood.Location = new Point(foodX, foodY);
            picFood.Visible = true;
            picFood.BringToFront();

            if (foodImages.Count > 0)
            {
                picFood.Image = foodImages[rand.Next(foodImages.Count)];
            }
        }

        private bool hits(int x, int y)
        {
            if (visit[x, y])
            {
                ShowGameOver();
                return true;
            }
            return false;
        }

        private bool collisionFood(int x, int y)
        {
            return picFood.Visible && x == picFood.Location.X && y == picFood.Location.Y;
        }

        private bool game_over(int x, int y)
        {
            return x < 0 || y < 0 || x > 980 || y > 480;
        }

        private void intial()
        {
            foreach (var obs in obstacles)
            {
                Controls.Remove(obs);
                obs.Dispose();
            }
            obstacles.Clear();

            visit = new bool[rows, cols];
            available.Clear();

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }

            if (hasObstacles)
            {
                GenerateObstacles(15);
            }

            int startCol = rand.Next(cols);
            int startRow = rand.Next(rows);

            while (visit[startRow, startCol])
            {
                startCol = rand.Next(cols);
                startRow = rand.Next(rows);
            }

            Piece head = new Piece(startCol * 20, startRow * 20, imgHead);

            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            available.Remove((head.Location.Y / 20) * cols + (head.Location.X / 20));

            Controls.Add(head);
            snake[front] = head;

            randomFood();
        }

        private void GenerateObstacles(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int r = rand.Next(2, rows - 2);
                int c = rand.Next(2, cols - 2);

                if (!visit[r, c])
                {
                    Piece obs = new Piece(c * 20, r * 20, imgRock);
                    visit[r, c] = true;
                    available.Remove(r * cols + c);

                    obstacles.Add(obs);
                    Controls.Add(obs);
                }
            }
        }

        // --- GIAO DIỆN MENUS ---

        private void CreateMenuPanel()
        {
            pnlMenu = new Panel
            {
                Size = new Size(420, 330),
                BackColor = Color.FromArgb(235, 30, 30, 45),
                BorderStyle = BorderStyle.Fixed3D
            };

            pnlMenu.Location = new Point(
                (this.ClientSize.Width - pnlMenu.Width) / 2,
                (this.ClientSize.Height - pnlMenu.Height) / 2
            );

            lblTitle = new Label
            {
                Text = "SNAKE GAME",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.LimeGreen,
                AutoSize = false,
                Size = new Size(400, 45),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblSpeedOption = new Label
            {
                Text = "Difficulty Level:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(45, 80),
                AutoSize = true
            };

            cboSpeed = new ComboBox
            {
                Location = new Point(220, 75),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TabStop = false
            };
            cboSpeed.Items.AddRange(new object[] { "Easy (Slow)", "Medium", "Hard (Fast)" });
            cboSpeed.SelectedIndex = 0; // Mặc định là Easy (Slow)

            chkObstacles = new CheckBox
            {
                Text = "Enable Obstacles (Rocks)",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(45, 125),
                AutoSize = true,
                TabStop = false
            };

            btnStart = new Button
            {
                Text = "PLAY GAME",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Size = new Size(160, 45),
                Location = new Point(45, 240),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += BtnStart_Click;

            btnTutorial = new Button
            {
                Text = "HOW TO PLAY",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(150, 45),
                Location = new Point(225, 240),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnTutorial.FlatAppearance.BorderSize = 0;
            btnTutorial.Click += (s, e) =>
            {
                pnlTutorial.Visible = true;
                pnlTutorial.BringToFront();
            };

            pnlMenu.Controls.Add(lblTitle);
            pnlMenu.Controls.Add(lblSpeedOption);
            pnlMenu.Controls.Add(cboSpeed);
            pnlMenu.Controls.Add(chkObstacles);
            pnlMenu.Controls.Add(btnStart);
            pnlMenu.Controls.Add(btnTutorial);

            this.Controls.Add(pnlMenu);
            pnlMenu.BringToFront();
        }

        private void CreateTutorialPanel()
        {
            pnlTutorial = new Panel
            {
                Size = new Size(380, 260),
                BackColor = Color.FromArgb(240, 25, 25, 35),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            pnlTutorial.Location = new Point(
                (this.ClientSize.Width - pnlTutorial.Width) / 2,
                (this.ClientSize.Height - pnlTutorial.Height) / 2
            );

            lblTutTitle = new Label
            {
                Text = "HOW TO PLAY",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                AutoSize = false,
                Size = new Size(360, 35),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblTutContent = new Label
            {
                Text = "• Use Arrow Keys (↑ ↓ ← →) to control snake.\n" +
                       "• Eat fruits to gain points and grow longer.\n" +
                       "• Avoid hitting walls, rocks, or yourself!\n" +
                       "• Use Settings (⚙) in-game to toggle BGM/SFX.",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(25, 60),
                Size = new Size(330, 120)
            };

            btnCloseTut = new Button
            {
                Text = "GOT IT!",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(120, 35),
                Location = new Point(130, 200),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnCloseTut.FlatAppearance.BorderSize = 0;
            btnCloseTut.Click += (s, e) => pnlTutorial.Visible = false;

            pnlTutorial.Controls.Add(lblTutTitle);
            pnlTutorial.Controls.Add(lblTutContent);
            pnlTutorial.Controls.Add(btnCloseTut);

            this.Controls.Add(pnlTutorial);
        }

        private void CreateSettingsPanel()
        {
            pnlSettings = new Panel
            {
                Size = new Size(320, 220),
                BackColor = Color.FromArgb(240, 30, 35, 45),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            pnlSettings.Location = new Point(
                (this.ClientSize.Width - pnlSettings.Width) / 2,
                (this.ClientSize.Height - pnlSettings.Height) / 2
            );

            lblSetTitle = new Label
            {
                Text = "SETTINGS",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Gold,
                AutoSize = false,
                Size = new Size(300, 35),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };

            chkBgm = new CheckBox
            {
                Text = "Background Music (BGM)",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.White,
                Checked = sound.IsMusicOn,
                Location = new Point(45, 65),
                AutoSize = true,
                TabStop = false
            };
            chkBgm.CheckedChanged += (s, e) =>
            {
                sound.IsMusicOn = chkBgm.Checked;
                if (sound.IsMusicOn) sound.PlayBackground("background.wav");
                else sound.StopBackground();
            };

            chkSfx = new CheckBox
            {
                Text = "Sound Effects (SFX)",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.White,
                Checked = sound.IsSoundOn,
                Location = new Point(45, 105),
                AutoSize = true,
                TabStop = false
            };
            chkSfx.CheckedChanged += (s, e) => sound.IsSoundOn = chkSfx.Checked;

            btnCloseSet = new Button
            {
                Text = "RESUME",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Size = new Size(120, 35),
                Location = new Point(100, 160),
                BackColor = Color.Gold,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnCloseSet.FlatAppearance.BorderSize = 0;
            btnCloseSet.Click += (s, e) =>
            {
                pnlSettings.Visible = false;
                if (!pnlMenu.Visible && !pnlGameOver.Visible)
                {
                    timer.Start(); // Tiếp tục chơi game khi đóng Settings
                    this.Focus();
                }
            };

            pnlSettings.Controls.Add(lblSetTitle);
            pnlSettings.Controls.Add(chkBgm);
            pnlSettings.Controls.Add(chkSfx);
            pnlSettings.Controls.Add(btnCloseSet);

            this.Controls.Add(pnlSettings);
        }

        private void CreateGameOverPanel()
        {
            pnlGameOver = new Panel
            {
                Size = new Size(360, 260),
                BackColor = Color.FromArgb(235, 40, 40, 50),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            pnlGameOver.Location = new Point(
                (this.ClientSize.Width - pnlGameOver.Width) / 2,
                (this.ClientSize.Height - pnlGameOver.Height) / 2
            );

            lblGameOverTitle = new Label
            {
                Text = "GAME OVER",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Crimson,
                AutoSize = false,
                Size = new Size(340, 40),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblFinalScore = new Label
            {
                Text = "Score: 0",
                Font = new Font("Segoe UI", 13, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(340, 28),
                Location = new Point(10, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblHighScore = new Label
            {
                Text = "High Score: 0",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.Gold,
                AutoSize = false,
                Size = new Size(340, 28),
                Location = new Point(10, 95),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnRestart = new Button
            {
                Text = "PLAY AGAIN",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(135, 40),
                Location = new Point(30, 180),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnRestart.FlatAppearance.BorderSize = 0;
            btnRestart.Click += BtnRestart_Click;

            btnMenu = new Button
            {
                Text = "MAIN MENU",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(135, 40),
                Location = new Point(195, 180),
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.Click += BtnMenu_Click;

            pnlGameOver.Controls.Add(lblGameOverTitle);
            pnlGameOver.Controls.Add(lblFinalScore);
            pnlGameOver.Controls.Add(lblHighScore);
            pnlGameOver.Controls.Add(btnRestart);
            pnlGameOver.Controls.Add(btnMenu);

            this.Controls.Add(pnlGameOver);
            pnlGameOver.BringToFront();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            switch (cboSpeed.SelectedIndex)
            {
                case 0: gameSpeed = 120; break;
                case 1: gameSpeed = 75; break;
                case 2: gameSpeed = 45; break;
            }

            hasObstacles = chkObstacles.Checked;

            pnlMenu.Visible = false;
            btnInGameSettings.Visible = true;
            btnInGameSettings.BringToFront();

            StartNewGame();
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            pnlGameOver.Visible = false;
            btnInGameSettings.Visible = true;
            btnInGameSettings.BringToFront();

            StartNewGame();
        }

        private void BtnMenu_Click(object sender, EventArgs e)
        {
            pnlGameOver.Visible = false;
            btnInGameSettings.Visible = false;
            pnlMenu.Visible = true;
            pnlMenu.BringToFront();
        }

        private void StartNewGame()
        {
            for (int i = 0; i < 1250; i++)
            {
                if (snake[i] != null)
                {
                    this.Controls.Remove(snake[i]);
                    snake[i].Dispose();
                    snake[i] = null;
                }
            }

            score = 0;
            dx = 0;
            dy = 0;
            front = 0;
            back = 0;
            lblScore.Text = "Score: 0";

            intial();
            launchTimer();

            if (sound.IsMusicOn) sound.PlayBackground("background.wav");

            this.Focus();
        }

        private void ShowGameOver()
        {
            timer.Stop();
            btnInGameSettings.Visible = false;

            sound.PlayGameOver("gameover.wav");
            sound.StopBackground();

            for (int i = 0; i < 1250; i++)
            {
                if (snake[i] != null)
                {
                    if (i == front && imgDeadHead != null)
                        snake[i].Image = GetRotatedHeadImage(imgDeadHead, dx, dy);
                    else if (imgDeadBody != null)
                        snake[i].Image = imgDeadBody;
                }
            }

            if (score > highScore)
            {
                highScore = score;
            }

            lblFinalScore.Text = "Score: " + score;
            lblHighScore.Text = "High Score: " + highScore;

            pnlGameOver.Visible = true;
            pnlGameOver.BringToFront();
        }
    }
}