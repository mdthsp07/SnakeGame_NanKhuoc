using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Snake()
        {
            InitializeComponent();
            intial();
            StartCountdown();
            sound.PlayBackground("background.wav");
        }

        private void launchTimer()
        {
            timer.Interval = 100;
            timer.Tick -= move;  
            timer.Tick += move;
            timer.Start();
        }

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

            if (collisionFood(x + dx, y + dy))
            {
                score += 1;
                lblScore.Text = "Score: " + score.ToString();
                sound.PlayEat("eat.wav");

                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                Piece head = new Piece(x + dx, y + dy);
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                randomFood();
            }
            else
            {
                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
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

            if (available.Count == 0) return;   // ← Tránh crash nếu hết ô trống

            int idx = rand.Next(available.Count) % available.Count;
            lblFood.Left = (available[idx] * 20) % Width;
            lblFood.Top = (available[idx] * 20) / Width * 20;
        }

        private bool hits(int x, int y)
        {
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

        private bool collisionFood(int x, int y)
        {
            return x == lblFood.Location.X && y == lblFood.Location.Y;
        }

        private bool game_over(int x, int y)
        {
            return x < 0 || y < 0 || x > 980 || y > 480;
        }

        private void intial()
        {
            visit = new bool[rows, cols];
            Piece head = new Piece((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);
            lblFood.Location = new Point((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }

            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            available.Remove(head.Location.Y / 20 * cols + head.Location.X / 20);
            Controls.Add(head);
            snake[front] = head;
        }

        // ==================== GAME OVER ====================
        private void ShowGameOver()
        {
            int highScore = score;   // Tạm thời, bạn khác sẽ làm phần đọc file

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
                            // HighScoreForm hsForm = new HighScoreForm();
                            // hsForm.ShowDialog();
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

        private void StartCountdown()
        {
            // Tạo label đếm ngược ở giữa màn hình
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

                if (count > 0)
                {
                    lblCountdown.Text = count.ToString();
                }
                else if (count == 0)
                {
                    lblCountdown.Text = "GO!";
                }
                else
                {
                    // Kết thúc đếm ngược
                    countdownTimer.Stop();
                    countdownTimer.Dispose();
                    this.Controls.Remove(lblCountdown);
                    lblCountdown.Dispose();

                    // Nếu người chơi CHƯA bấm phím nào → chọn hướng mặc định an toàn
                    if (nextDx == 0 && nextDy == 0)
                    {
                        // Nếu rắn ở nửa phải bàn → đi trái, ngược lại đi phải
                        if (snake[front].Location.X > 490)
                        {
                            nextDx = -20; nextDy = 0;
                        }
                        else
                        {
                            nextDx = 20; nextDy = 0;
                        }
                    }

                    // Bắt đầu chạy
                    launchTimer();
                }
            };
            countdownTimer.Start();
        }
        // ==================== CHƠI LẠI ====================
        private void RestartGame()
        {
           
            timer.Stop();
            timer.Tick -= move;

            
            score = 0;
            dx = 0;
            dy = 0;
            nextDx = 0;   
            nextDy = 0;
            front = 0;
            back = 0;
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

            
            intial();
            sound.PlayBackground("background.wav");
            StartCountdown();
        }
    }
}