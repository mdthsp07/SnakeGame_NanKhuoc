using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Snake : Form
    {
        int cols = 50, rows = 25, score = 0, dx = 0, dy = 0, front = 0, back = 0;
        int nextDx = 0, nextDy = 0;

        Piece[] snake = new Piece[1250];
        List<int> available = new List<int>();
        bool[,] visit;

        Random rand = new Random();
        Timer timer = new Timer();
        SoundManager sound = SoundManager.Instance;

        // ===== ẢNH =====
        private Image headLeft, headRight, headUp, headDown;
        private Image bodyImage;
        private Image tailImage;
        private Image foodImage;
        private Image bombImage;
        private Image giftImage;

        // ===== HỘP QUÀ =====
        private Timer giftTimer;
        private bool isGiftActive = false;
        private int giftLifetime = 0;
        private const int GIFT_LIFETIME_MS = 5000;
        private const int GIFT_BONUS = 5;
        private const int GIFT_SPAWN_CHANCE = 15;
        private const int BOMB_SPAWN_CHANCE = 15;
        private bool isBombActive = false;

        public Snake()
        {
            InitializeComponent();
            LoadImages();
            InitializeGift();
            intial();
            StartCountdown();
            sound.PlayBackground("background.wav");
        }

        // ===== LOAD ẢNH =====
        private void LoadImages()
        {
            try
            {
                bodyImage = Properties.Resources.snake_body;
                tailImage = Properties.Resources.snake_tail;
                foodImage = Properties.Resources.apple;
                bombImage = Properties.Resources.daaece34463f6b599008ced4b7c0c280;
                giftImage = Properties.Resources.gift_box;

                Image origHead = Properties.Resources.snake_head;

                // Ảnh gốc hướng trái
                headLeft = origHead;
                headRight = Flip(origHead);           // lật ngang
                headUp = Rotate(origHead, RotateFlipType.Rotate90FlipNone);
                headDown = Rotate(origHead, RotateFlipType.Rotate270FlipNone);
            }
            catch { }
        }

        private Image Flip(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return bmp;
        }

        private Image Rotate(Image img, RotateFlipType type)
        {
            Bitmap bmp = new Bitmap(img);
            bmp.RotateFlip(type);
            return bmp;
        }

        private Image GetHeadImage(int dx, int dy)
        {
            if (dx < 0) return headLeft;
            if (dx > 0) return headRight;
            if (dy < 0) return headUp;
            if (dy > 0) return headDown;
            return headLeft; // mặc định
        }

        // ===== KHỞI TẠO HỘP QUÀ =====
        private void InitializeGift()
        {
            giftTimer = new Timer { Interval = 100 };
            giftTimer.Tick += GiftTimer_Tick;
        }

        private void GiftTimer_Tick(object sender, EventArgs e)
        {
            giftLifetime -= giftTimer.Interval;
            if (giftLifetime <= 0)
            {
                picGift.Visible = false;
                isGiftActive = false;
                giftTimer.Stop();
            }
        }

        // ===== ĐỒNG HỒ ĐẾM NGƯỢC =====
        private void launchTimer()
        {
            timer.Interval = GameSettings.GetSnakeInterval();
            timer.Tick -= move;
            timer.Tick += move;
            timer.Start();
        }

        // ===== ĐIỀU KHIỂN =====
        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (dx != -20) { nextDx = 20; nextDy = 0; }
                    break;
                case Keys.Left:
                    if (dx != 20) { nextDx = -20; nextDy = 0; }
                    break;
                case Keys.Up:
                    if (dy != 20) { nextDx = 0; nextDy = -20; }
                    break;
                case Keys.Down:
                    if (dy != -20) { nextDx = 0; nextDy = 20; }
                    break;
            }
        }

        // ===== DI CHUYỂN =====
        private void move(object sender, EventArgs e)
        {
            if (nextDx != 0 || nextDy != 0)
            {
                dx = nextDx;
                dy = nextDy;
            }

            int x = snake[front].Location.X, y = snake[front].Location.Y;
            if (dx == 0 && dy == 0) return;

            if (game_over(x + dx, y + dy))
            {
                timer.Stop();
                sound.PlayGameOver("gameover.wav");
                sound.StopBackground();
                ShowGameOver();
                return;
            }

            // If moving into a bomb, immediate game over
            if (collisionBomb(x + dx, y + dy))
            {
                timer.Stop();
                sound.PlayGameOver("gameover.wav");
                sound.StopBackground();
                ShowGameOver();
                return;
            }

            bool ateFood = collisionFood(x + dx, y + dy);
            bool ateGift = isGiftActive &&
                           (x + dx) == picGift.Location.X &&
                           (y + dy) == picGift.Location.Y;

            if (ateFood || ateGift)
            {
                if (ateFood) score += 1;
                if (ateGift)
                {
                    score += GIFT_BONUS;
                    picGift.Visible = false;
                    isGiftActive = false;
                    giftTimer.Stop();
                }
                lblScore.Text = "Score: " + score.ToString();
                sound.PlayEat("eat.wav");

                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                // Đầu cũ chuyển thành thân (chỉ khi có nhiều hơn 1 đốt)
                if (front != back)
                {
                    snake[front].Image = bodyImage;
                }

                Piece head = new Piece(x + dx, y + dy, GetHeadImage(dx, dy));
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                head.BringToFront();

                if (ateFood) randomFood();

                // After food spawn, possibly spawn a bomb at a different available cell
                if (!isBombActive && rand.Next(100) < BOMB_SPAWN_CHANCE)
                {
                    // pick a free cell different from current food
                    List<int> free = new List<int>(available);
                    if (free.Count > 0)
                    {
                        // remove current food position (just in case)
                        int foodPos = (picFood.Top / 20) * cols + (picFood.Left / 20);
                        free.RemoveAll(v => v == foodPos);
                        if (free.Count > 0)
                        {
                            int bidx = rand.Next(free.Count);
                            int pos = free[bidx];
                            picBomb.Left = (pos * 20) % Width;
                            picBomb.Top = (pos * 20) / Width * 20;
                            picBomb.Image = bombImage;
                            picBomb.Visible = true;
                            picBomb.BringToFront();
                            isBombActive = true;
                        }
                    }
                }

                // 15% cơ hội sinh hộp quà sau khi ăn thức ăn thường
                if (ateFood && !isGiftActive && rand.Next(100) < GIFT_SPAWN_CHANCE)
                {
                    SpawnGift();
                }

                UpdateSnakeImages();
            }
            else
            {
                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                Piece oldHead = snake[front];

                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
                back = (back - 1 + 1250) % 1250;
                visit[(y + dy) / 20, (x + dx) / 20] = true;

                UpdateSnakeImages();
            }
        }

        // ===== CẬP NHẬT ẢNH ĐẦU/THÂN/ĐUÔI =====
        private void UpdateSnakeImages()
        {
            if (snake[front] == null) return;

            // Chỉ 1 đốt
            if (front == back)
            {
                snake[front].Image = GetHeadImage(dx, dy);
                return;
            }

            // Đầu
            snake[front].Image = GetHeadImage(dx, dy);
            // Đuôi
            snake[back].Image = tailImage;

            // Thân (các đốt ở giữa)
            int i = (front + 1) % 1250;
            while (i != back)
            {
                if (snake[i] != null) snake[i].Image = bodyImage;
                i = (i + 1) % 1250;
                if (i == front) break;
            }
        }

        // ===== SINH HỘP QUÀ =====
        private void SpawnGift()
        {
            available.Clear();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (!visit[i, j]) available.Add(i * cols + j);

            if (available.Count == 0) return;

            int idx = rand.Next(available.Count);
            int gx = (available[idx] * 20) % Width;
            int gy = (available[idx] * 20) / Width * 20;

            picGift.Location = new Point(gx, gy);
            picGift.Image = giftImage;
            picGift.Visible = true;
            picGift.BringToFront();
            isGiftActive = true;
            giftLifetime = GIFT_LIFETIME_MS;
            giftTimer.Start();
        }

        // ===== SINH THỨC ĂN =====
        private void randomFood()
        {
            available.Clear();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (!visit[i, j]) available.Add(i * cols + j);

            if (available.Count == 0) return;

            int idx = rand.Next(available.Count);
            picFood.Left = (available[idx] * 20) % Width;
            picFood.Top = (available[idx] * 20) / Width * 20;
            picFood.Image = foodImage;
            picFood.Visible = true;
            picFood.BringToFront();
        }

        // ===== KIỂM TRA VA CHẠM =====
        private bool collisionFood(int x, int y)
        {
            return x == picFood.Location.X && y == picFood.Location.Y;
        }

        private bool collisionBomb(int x, int y)
        {
            return isBombActive && x == picBomb.Location.X && y == picBomb.Location.Y;
        }

        private bool hits(int x, int y)
        {
            if (x < 0 || y < 0 || x >= rows || y >= cols) return false;
            if (visit[x, y])
            {
                timer.Stop();
                sound.PlayGameOver("gameover.wav");
                sound.StopBackground();
                ShowGameOver();
                return true;
            }
            return false;
        }

        private bool game_over(int x, int y)
        {
            return x < 0 || y < 0 || x > 980 || y > 480;
        }

        private void lblScore_Click(object sender, EventArgs e) { }

        // ===== KHỞI TẠO BAN ĐẦU =====
        private void intial()
        {
            visit = new bool[rows, cols];
            Piece head = new Piece((rand.Next() % cols) * 20, (rand.Next() % rows) * 20, headLeft);
            picFood.Location = new Point((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);
            picFood.Image = foodImage;
            picFood.Visible = true;
            picFood.BringToFront();

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }

            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            available.Remove(head.Location.Y / 20 * cols + head.Location.X / 20);
            Controls.Add(head);
            head.BringToFront();
            snake[front] = head;
        }

        // ===== GAME OVER =====
        private void ShowGameOver()
        {
            LeaderBoardForm.Save(score);
            int highScore = LeaderBoardForm.GetHighScore();

            using (GameOverForm gameOver = new GameOverForm(score, highScore))
            {
                DialogResult result = gameOver.ShowDialog();

                if (result == DialogResult.OK)
                {
                    switch (gameOver.SelectedAction)
                    {
                        case GameOverForm.GameAction.PlayAgain:
                            RestartGame();
                            break;
                        case GameOverForm.GameAction.ViewHighScore:
                            LeaderBoardForm leaderboard = new LeaderBoardForm(score);
                            leaderboard.ShowDialog();
                            ShowGameOver();
                            break;
                        case GameOverForm.GameAction.MainMenu:
                            this.Close();
                            break;
                    }
                }
                else
                {
                    this.Close();
                }
            }
        }

        // ===== ĐẾM NGƯỢC =====
        private void StartCountdown()
        {
            Label lblCountdown = new Label
            {
                Text = "3",
                Font = new Font("Courier New", 100, FontStyle.Bold),
                ForeColor = Color.Gold,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height),
                Location = new Point(0, 0)
            };
            this.Controls.Add(lblCountdown);
            lblCountdown.BringToFront();

            int count = 3;

            Timer countdownTimer = new Timer { Interval = 1000 };
            countdownTimer.Tick += (s, e) =>
            {
                count--;

                if (count > 0) lblCountdown.Text = count.ToString();
                else if (count == 0) lblCountdown.Text = "GO!";
                else
                {
                    countdownTimer.Stop();
                    countdownTimer.Dispose();
                    this.Controls.Remove(lblCountdown);
                    lblCountdown.Dispose();

                    if (nextDx == 0 && nextDy == 0)
                    {
                        if (snake[front].Location.X > 490) { nextDx = -20; nextDy = 0; }
                        else { nextDx = 20; nextDy = 0; }
                    }

                    launchTimer();
                }
            };
            countdownTimer.Start();
        }

        // ===== CHƠI LẠI =====
        private void RestartGame()
        {
            timer.Stop();
            timer.Tick -= move;

            score = 0; dx = 0; dy = 0; nextDx = 0; nextDy = 0;
            front = 0; back = 0;
            lblScore.Text = "Score: 0";

            for (int i = 0; i < 1250; i++)
            {
                if (snake[i] != null)
                {
                    Controls.Remove(snake[i]);
                    snake[i] = null;
                }
            }

            available.Clear();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }

            // Reset hộp quà
            picGift.Visible = false;
            isGiftActive = false;
            giftTimer.Stop();

            intial();
            sound.PlayBackground("background.wav");
            StartCountdown();
        }
    }
}