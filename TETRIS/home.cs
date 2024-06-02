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
    public partial class home : Form
    {
        SoundPlayer closeSound = new SoundPlayer();
        SoundPlayer enterSound = new SoundPlayer();
        public home()
        {
            closeSound.SoundLocation = Application.StartupPath + @"\\sound\\pixabay\\closegame.wav";
            closeSound.Load();
            enterSound.SoundLocation = Application.StartupPath + @"\\sound\\pixabay\\entergame.wav";
            enterSound.Load();
            
            InitializeComponent();
        }

        public void showform(Form form)
        {
            panel1.Controls.Clear();
            form.FormBorderStyle = FormBorderStyle.None;
            form.TopLevel = false;
            panel1.Controls.Add(form);
            form.Location = new Point(0, 0);
            Focus();
            form.Show();
        }


        private void home_Load(object sender, EventArgs e)
        {
            enterSound.Play();
            showform(new main(this));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void blockturn_sound_Enter(object sender, EventArgs e)
        {

        }

        private void home_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeSound.Play();
            Thread.Sleep(1500);
        }

        private void home_Enter(object sender, EventArgs e)
        {
        }
    }
}
