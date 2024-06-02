using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace TETRIS
{
    public partial class main : Form
    {
        SoundPlayer clickSound = new SoundPlayer();
        home home;
        public main(home f)
        {
            home = f;
            clickSound.SoundLocation = Application.StartupPath + @"\\sound\\mixkit\\click.wav";
            clickSound.Load();
            
            InitializeComponent();
        }

        void buttonClickSound()
        {
            clickSound.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            home.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(buttonClickSound)).Start();
            home.showform(new game(home, 1));
        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(buttonClickSound)).Start();
            home.showform(new Scoreboard(home));
        }
    }
}
