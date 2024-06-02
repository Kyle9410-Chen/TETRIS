using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Threading;

namespace TETRIS
{
    public partial class Scoreboard : Form
    {
        home home;
        SoundPlayer clickSound = new SoundPlayer();
        public Scoreboard(home f)
        {
            home = f;
            clickSound.SoundLocation = Application.StartupPath + @"\\sound\\mixkit\\click.wav";
            clickSound.Load();

            InitializeComponent();
        }

        class scoreData
        {
            public DateTime time;
            public int score;
            public string gameTime;


            public void Set(string txtdata)
            {

                var data = txtdata.Split('-');
                time = DateTime.Parse(data[0]);
                score = int.Parse(data[1]);
                gameTime = data[2];
            }
        }

        void buttonClickSound()
        {
            clickSound.Play();
        }

        private void Scoreboard_Load(object sender, EventArgs e)
        {
            int count = 0;
            string txtdata;
            StreamReader sr = new StreamReader(Application.StartupPath + "\\scoreboard.txt");
            txtdata = sr.ReadToEnd();
            sr.Close();
            List<string> scoredata = new List<string>();
            scoredata = txtdata.Split(new char[2] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries).ToList();

            List<scoreData> scoreboard = new List<scoreData>();
            foreach(var i in scoredata)
            {
                scoreData s = new scoreData();
                s.Set(i);
                scoreboard.Add(s);
            }
            
            scoreboard = scoreboard.OrderByDescending(x=>x.score).ToList();

            foreach(var data in scoreboard)
            {
                if(count == 10)
                {
                    break;
                }

                GroupBox g = new GroupBox();
                g.SetBounds(200, 160 + count*75, 766, 50);
                g.BackColor = Color.FromArgb(100, 255, 255, 255);
                Controls.Add(g);

                Label no = new Label();
                no.SetBounds(10, 12, 100, 30);
                no.Text = (++count).ToString();
                no.Font = new Font("微軟正黑體", 16, FontStyle.Bold);
                no.BackColor = Color.FromArgb(0, 0, 0, 0);
                no.ForeColor = Color.FromArgb(105, 160, 225);
                g.Controls.Add(no);

                Label time = new Label();
                time.SetBounds(110, 12, 300, 30);
                time.Text = "遊戲時間:" + (data.time).ToString("yyyMMdd-HHmmss");
                time.Font = new Font("微軟正黑體", 16, FontStyle.Bold);
                time.BackColor = Color.FromArgb(0, 0, 0, 0);
                time.ForeColor = Color.FromArgb(105, 160, 225);
                g.Controls.Add(time);

                Label score = new Label();
                score.SetBounds(410, 12, 150, 30);
                score.Text = "分數:" + (data.score).ToString();
                score.Font = new Font("微軟正黑體", 16, FontStyle.Bold);
                score.BackColor = Color.FromArgb(0, 0, 0, 0);
                score.ForeColor = Color.FromArgb(105, 160, 225);
                g.Controls.Add(score);

                Label gameTime = new Label();
                gameTime.SetBounds(560, 12, 196, 30);
                gameTime.Text = "遊戲時長:" + (data.gameTime).ToString();
                gameTime.Font = new Font("微軟正黑體", 16, FontStyle.Bold);
                gameTime.BackColor = Color.FromArgb(0, 0, 0, 0);
                gameTime.ForeColor = Color.FromArgb(105, 160, 225);
                g.Controls.Add(gameTime);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(buttonClickSound)).Start();
            home.showform(new main(home));
        }
    }
}
