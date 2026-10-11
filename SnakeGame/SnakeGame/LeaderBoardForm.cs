using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;

namespace SnakeGame 
{
    public partial class LeaderBoardForm : Form
    {
        public class NguoiChoi
        {
            public string Ten { get; set; }
            public int Diem { get; set; }
        }

        string filePath = "highscore.txt"; 

        public LeaderBoardForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            CapNhatBangXepHang();
        }

        private void LeaderBoardForm_Load(object sender, EventArgs e)
        {
            CapNhatBangXepHang();
        }

        private void CapNhatBangXepHang()
        {
            BangDiem.Items.Clear();

            List<NguoiChoi> danhSach = new List<NguoiChoi>()
            {
                new NguoiChoi { Ten = "Pro", Diem = 67 },
                new NguoiChoi { Ten = "J97", Diem = 36 },
                new NguoiChoi { Ten = "Mr Đàm", Diem = 22 },
                new NguoiChoi { Ten = "Đại boàng", Diem = 17 },
                new NguoiChoi { Ten = "Kiwi", Diem = 3}
            };

            int playerScore = readScore();

            danhSach.Add(new NguoiChoi { Ten = "Bạn", Diem = playerScore });

            // Sắp xếp điểm
            for (int i = 0; i < danhSach.Count - 1; i++)
            {
                for (int j = i + 1; j < danhSach.Count; j++)
                {
                    if (danhSach[i].Diem < danhSach[j].Diem)
                    {
                        NguoiChoi temp = danhSach[i];
                        danhSach[i] = danhSach[j];
                        danhSach[j] = temp;
                    }
                }
            }

            int rank = 1;
            foreach (var p in danhSach)
            {
                BangDiem.Items.Add(" " + rank + ". " + p.Ten + " - " + p.Diem + " điểm");
                rank++;
            }

            BangDiem.Items.Add("--------------------------------");
            BangDiem.Items.Add($" Điểm cao nhất: " + playerScore + " điểm");
        }

        private int readScore()
        {
            if (!File.Exists(filePath)) return 0;

            string line = File.ReadAllText(filePath);
            int diem = 0;
            int.TryParse(line, out diem);
            return diem;
        }

        public static void Save(int newScore)
        {
            string path = "highscore.txt";
            int oldScore = 0;

            if (File.Exists(path))
            {
                int.TryParse(File.ReadAllText(path), out oldScore);
            }

            if (newScore > oldScore)
            {
                File.WriteAllText(path, newScore.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BangDiem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}