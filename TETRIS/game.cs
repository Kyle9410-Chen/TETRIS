using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using WMPLib;

namespace TETRIS    
{
    public partial class game : Form
    {
        Point nowPoint = new Point(4, 1);
        Point lastPoint = new Point(4, 1);
        Point nextPoint = new Point(4, 2);
        Random r = new Random(Guid.NewGuid().GetHashCode());
        string[] blockStyles = new string[7] { "I", "J", "L", "O", "S", "T", "Z" };
        List<List<PictureBox>> listPictureBox = new List<List<PictureBox>>();
        List<List<PictureBox>> nextBlockBox = new List<List<PictureBox>>();
        List<List<PictureBox>> holdBlockBox = new List<List<PictureBox>>();
        block nowblock;
        block holdBlock;
        block nextblock;
        Label scoreLabel;
        bool gameEnd = true;
        bool canHold = true;
        home home;
        int score = 0;
        int second = 0, minute = 0;
        int mode;
        int startTimer = 3;
        int gameTimer = 120;

        SoundPlayer TurnSound = new SoundPlayer();
        SoundPlayer MoveSound = new SoundPlayer();
        SoundPlayer clickSound = new SoundPlayer();
        SoundPlayer fastDownSound = new SoundPlayer();
        WindowsMediaPlayer bgm = new WindowsMediaPlayer();

        public game(home f, int m)
        {
            mode = m;
            home = f;
            clickSound.SoundLocation = Application.StartupPath + @"\\sound\\mixkit\\click.wav";
            clickSound.Load();
            fastDownSound.SoundLocation = Application.StartupPath + @"\\sound\\pixabay\\blockfastdown.wav";
            fastDownSound.Load();
            TurnSound.SoundLocation = Application.StartupPath + @"\\sound\\pixabay\\blockturn.wav";
            TurnSound.Load();
            MoveSound.SoundLocation = Application.StartupPath + @"\\sound\\pixabay\\blockmove.wav";
            MoveSound.Load();
            bgm.URL = Application.StartupPath + $"\\sound\\pixabay\\bgm{new Random(Guid.NewGuid().GetHashCode()).Next(1,1)}.wav";
            bgm.controls.stop();
            InitializeComponent();
        }

        void summonScoreLabel()
        {
            scoreLabel = new Label();
            scoreLabel.Text = "Score: 0";
            scoreLabel.SetBounds(500, 800, 400, 40);
            scoreLabel.Font = new Font("Console", 30);
            scoreLabel.ForeColor = Color.FromArgb(0, 105, 160, 225);
            Controls.Add(scoreLabel);
        }

        void summonBlock()
        {
            for (int i = 0; i < 20; i++)
            {
                List<PictureBox> queue = new List<PictureBox>();
                for (int j = 0; j < 10; j++)
                {
                    PictureBox p = new PictureBox();
                    p.Name = $"Block_{j}_{i}";
                    p.SetBounds(44*j+60, 44*i+10, 40, 40);
                    p.SizeMode = PictureBoxSizeMode.StretchImage;
                    p.Image = Image.FromFile(Application.StartupPath + "\\Images\\blocks\\block_0.jpg");
                    p.Tag = "0";
                    Controls.Add(p);
                    queue.Add(p);
                }
                listPictureBox.Add(queue);
            }
        }

        void summonNextBlock()
        {
            Label l = new Label();
            l.SetBounds(500, 10,100,40);
            l.Text = "Next:";
            l.Font = new Font("Console", 22);
            l.ForeColor = Color.FromArgb(0, 105, 160, 225);
            Controls.Add(l);

            for (int i = 0; i < 4; i++)
            {
                List<PictureBox> queue = new List<PictureBox>();
                for (int j = 0; j < 4; j++)
                {
                    PictureBox p = new PictureBox();
                    p.Name = $"Block_{j}_{i}";
                    p.SetBounds(44 * j + 600, 44 * i + 10, 40, 40);
                    p.SizeMode = PictureBoxSizeMode.StretchImage;
                    p.Image = Image.FromFile(Application.StartupPath + "\\Images\\blocks\\block_0.jpg");
                    p.Tag = "0";
                    Controls.Add(p);
                    queue.Add(p);
                }
                nextBlockBox.Add(queue);
            }
        }

        void summonHoldBlock()
        {
            Label l = new Label();
            l.SetBounds(500, 200, 100, 40);
            l.Text = "Hold:";
            l.Font = new Font("Console", 22);
            l.ForeColor = Color.FromArgb(0, 105, 160, 225);
            Controls.Add(l);

            for (int i = 0; i < 4; i++)
            {
                List<PictureBox> queue = new List<PictureBox>();
                for (int j = 0; j < 4; j++)
                {
                    PictureBox p = new PictureBox();
                    p.Name = $"Block_{j}_{i}";
                    p.SetBounds(44 * j + 600, 44 * i + 200, 40, 40);
                    p.SizeMode = PictureBoxSizeMode.StretchImage;
                    p.Image = Image.FromFile(Application.StartupPath + "\\Images\\blocks\\block_0.jpg");
                    p.Tag = "0";
                    Controls.Add(p);
                    queue.Add(p);
                }
                holdBlockBox.Add(queue);
            }
        }

        void setBlockTag() {
            foreach (var i in nowblock.blockPoint)
            {
                listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X].Tag = nowblock.blocktype;
            }
        }

        void setNextBlcok()
        {
            foreach(var i in nextblock.blockPoint)
            {
                nextBlockBox[1 + i.Y][1+i.X].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_{nextblock.blocktype}.jpg");
            }
        }

        void setHoldBlock()
        {
            foreach (var i in holdBlock.blockPoint)
            {
                holdBlockBox[1 + i.Y][1 + i.X].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_{holdBlock.blocktype}.jpg");
            }
        }

        void clearBlock()
        {
            try
            {
                foreach (var i in nowblock.blockPoint)
                {
                    listPictureBox[lastPoint.Y + i.Y][lastPoint.X + i.X].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_0.jpg");
                    listPictureBox[lastPoint.Y + i.Y][lastPoint.X + i.X].Tag = "0";
                }
            }
            catch
            {

            }
        }

        void clearNextBlock()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    nextBlockBox[i][j].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_0.jpg");
                }
            }
        }

        void clearHoldBlock()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    holdBlockBox[i][j].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_0.jpg");
                }
            }
        }

        void lineCheck()
        {
            int count = 0;
            for(int index = 0;index<20;index++)
            {
                var i = listPictureBox[index];
                if(!i.Any(x=>x.Tag.ToString() == "0"))
                {
                    foreach(PictureBox j in i)
                    {
                        j.Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_0.jpg");
                        j.Tag = "0";
                    }

                    foreach(var data in listPictureBox[0])
                    {
                        data.Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_0.jpg");
                        data.Tag = "0";
                    }

                    for (int j = index; j >= 1; j--)
                    {
                        for(int k = 0; k < 10; k++)
                        {
                            listPictureBox[j][k].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_{listPictureBox[j - 1][k].Tag}.jpg");
                            listPictureBox[j][k].Tag = listPictureBox[j - 1][k].Tag;
                        }
                    }
                    count++;
                }
            }
            switch (count)
            {
                case 1:
                    score += 100;
                    break;
                case 2:
                    score += 300;
                    break;
                case 3:
                    score += 500;
                    break;
                case 4:
                    score += 800;
                    break;
                default:
                    break;
            }
            scoreLabel.Text = $"Score: {score}";
        }

        void turnClearBlock(block lastblock, Point clearPoint)
        {
            try
            {
                foreach (var i in lastblock.blockPoint)
                {
                    listPictureBox[clearPoint.Y + i.Y][clearPoint.X + i.X].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_0.jpg");
                    listPictureBox[clearPoint.Y + i.Y][clearPoint.X + i.X].Tag = "0";
                }
            }
            catch {}
        }

        void blockDown()
        {
            if (nowPoint.Y + nowblock.blockPoint.Max(x => x.Y) >= listPictureBox.Count)
            {
                lineCheck();
                canHold = true;
                nowblock = nextblock;
                nextblock = new block();
                clearNextBlock();
                r = new Random(Guid.NewGuid().GetHashCode());
                nextblock.setPoint(blockStyles[r.Next(0, 7)], 1);
                setNextBlcok();
                nowPoint = new Point(4, 1);
                lastPoint = new Point(4, 1);
                nextPoint = new Point(4, 2);
                blockDown();
                return;
            }


            var data = from x in nowblock.blockPoint
                       group x by x.X into y
                       select new
                       {
                           Y = y.Max(z => z.Y),
                           X = y.Key
                       };

            foreach (var i in data.ToList())
            {
                try
                {
                    if (listPictureBox[nextPoint.Y + i.Y][nextPoint.X + i.X].Tag.ToString() != "0")
                    {
                        lineCheck();
                        canHold = true;
                        nowblock = nextblock;
                        nextblock = new block();
                        clearNextBlock();
                        r = new Random(Guid.NewGuid().GetHashCode());
                        nextblock.setPoint(blockStyles[r.Next(0, 7)], 1);
                        setNextBlcok();
                        nowPoint = new Point(4, 1);
                        lastPoint = new Point(4, 1);
                        nextPoint = new Point(4, 2);
                        if (listPictureBox[2][4].Tag.ToString() != "0")
                        {
                            gameOver();
                            return;
                        }
                        blockDown();
                        return;
                    }
                }
                catch { }
            }

            clearBlock();

            foreach (var i in nowblock.blockPoint)
            {
                listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_{nowblock.blocktype}.jpg");
                setBlockTag();
            }
            lastPoint = nowPoint;
            nextPoint = new Point(nowPoint.X, nowPoint.Y + 1);
        }

        void blockMoveSound()
        {
            MoveSound.Play();
        }

        void blockFastDown()
        {
            while (true)
            {
                nowPoint.Y++;
                if (nowPoint.Y + nowblock.blockPoint.Max(x => x.Y) >= listPictureBox.Count)
                {
                    lineCheck();
                    canHold = true;
                    nowblock = nextblock;
                    nextblock = new block();
                    clearNextBlock();
                    r = new Random(Guid.NewGuid().GetHashCode());
                    nextblock.setPoint(blockStyles[r.Next(0, 7)], 1);
                    setNextBlcok();
                    nowPoint = new Point(4, 1);
                    if (listPictureBox[1][4].Tag.ToString() != "0")
                    {
                        gameOver();
                        return;
                    }
                    lastPoint = new Point(4, 1);
                    nextPoint = new Point(4, 2);
                    blockDown();
                    return;
                }


                var data = from x in nowblock.blockPoint
                           group x by x.X into y
                           select new
                           {
                               Y = y.Max(z => z.Y),
                               X = y.Key
                           };
                foreach (var i in data.ToList())
                {
                    try
                    {
                        if (listPictureBox[nextPoint.Y + i.Y][nextPoint.X + i.X].Tag.ToString() != "0")
                        {
                            lineCheck();
                            canHold = true;
                            nowblock = nextblock;
                            nextblock = new block();
                            clearNextBlock();
                            r = new Random(Guid.NewGuid().GetHashCode());
                            nextblock.setPoint(blockStyles[r.Next(0, 7)], 1);
                            setNextBlcok();
                            nowPoint = new Point(4, 1);
                            lastPoint = new Point(4, 1);
                            nextPoint = new Point(4, 2);
                            blockDown();
                            return;
                        }
                    }
                    catch { }
                }

                clearBlock();

                foreach (var i in nowblock.blockPoint)
                {
                    listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_{nowblock.blocktype}.jpg");
                    setBlockTag();
                }
                lastPoint = nowPoint;
                nextPoint = new Point(nowPoint.X, nowPoint.Y + 1);
            }
        }

        void blockTurnSound()
        {
            TurnSound.PlaySync();
        }

        void blockTurn()
        {
            new Thread(new ThreadStart(blockTurnSound)).Start();
            foreach (var i in nowblock.blockPoint)
            {
                listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X].Image = Image.FromFile(Application.StartupPath + $"\\Images\\blocks\\block_{nowblock.blocktype}.jpg");
                setBlockTag();
            }
            lastPoint = nowPoint;
        }

        bool checkBlock(Point checkPoint)
        {
            foreach (var i in nowblock.blockPoint)
            {
                try
                {
                    if (listPictureBox[checkPoint.Y + i.Y][checkPoint.X + i.X].Tag.ToString() != "0")
                    {
                        return false;
                    }
                }
                catch { return false; }
            }
            return true;
        }

        bool TTurn(int mode)
        {
            Point checkPoint = new Point();
            checkPoint.X = nowPoint.X;
            checkPoint.Y = nowPoint.Y;
            bool can = false;
            switch (nowblock.blocktype)
            {
                case "I":
                    switch (mode)
                    {
                        case 1:
                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y + 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 2:
                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 3:
                            checkPoint.X = nowPoint.X + 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 2;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 4:
                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 5:
                            checkPoint.X = nowPoint.X + 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 2;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 6:
                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 7:
                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 2;
                            checkPoint.Y = nowPoint.Y + 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 8:
                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 2;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 2;
                            checkPoint.Y = nowPoint.Y + 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;
                    }
                    break;
                default:
                    switch (mode)
                    {
                        case 1:
                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 2:
                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y + 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 3:
                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 4:
                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y + 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 5:
                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y + 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 6:
                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 7:
                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y + 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X - 1;
                            checkPoint.Y = nowPoint.Y - 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;


                        case 8:
                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y - 1;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }

                            checkPoint.X = nowPoint.X + 1;
                            checkPoint.Y = nowPoint.Y + 2;
                            can = checkBlock(checkPoint);
                            if (can)
                            {
                                nowPoint = checkPoint;
                                return can;
                            }
                            break;
                    }
                    break;
            }
            return can;
        }

        void gameOver()
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            bgm.controls.stop();

            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\scoreboard.txt",true);
            sw.WriteLine($"{DateTime.Now}-{score}-{minute.ToString("d2")}:{second.ToString("d2")}");
            sw.Close();

            groupBox1.Visible = true;
            label1.Text = $"分數: {score}";
            label2.Text = $"遊戲時長: {minute.ToString("d2")}:{second.ToString("d2")}";
            gameEnd = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            home.KeyDown += main_KeyDown;
            home.KeyUp += main_KeyUp;

            

            nowblock = new block();
            r = new Random(Guid.NewGuid().GetHashCode());
            nowblock.setPoint(blockStyles[r.Next(0, 7)], 1);
            nextblock = new block();
            r = new Random(Guid.NewGuid().GetHashCode());
            nextblock.setPoint(blockStyles[r.Next(0, 7)], 1);

            summonScoreLabel();

            summonNextBlock();
            setNextBlcok();

            summonBlock();
            setBlockTag();

            summonHoldBlock();

            blockDown();

            groupBox2.Visible = true;
            timer4.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            nowPoint.Y++;
            blockDown();
        }

        private void main_KeyDown(object sender, KeyEventArgs e)
        {

            if (gameEnd) return;
            if(e.KeyCode == Keys.D)
            {

                bool canTurn = true;
                int mode;
                int style = nowblock.blockstyle == 4 ? 1 : nowblock.blockstyle + 1;
                mode = nowblock.blockstyle;
                block lastblock = new block();
                lastblock.setPoint(nowblock.blocktype, nowblock.blockstyle);
                nowblock.setPoint(nowblock.blocktype, style);
                turnClearBlock(lastblock, nowPoint);
                foreach (var i in nowblock.blockPoint)
                {
                    try
                    {
                        if (listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X].Tag.ToString() != "0" && !lastblock.blockPoint.Contains(i))
                        {
                            canTurn = TTurn(mode);
                            break;
                        }
                    }
                    catch
                    {
                        canTurn = TTurn(mode);
                    }
                }
                nextPoint = new Point(nowPoint.X, nowPoint.Y + 1);
                if (!canTurn)
                {
                    nowblock = lastblock;

                }
                blockTurn();
            }
            if (e.KeyCode == Keys.A)
            {
                bool canTurn = true;
                int mode;
                int style = nowblock.blockstyle == 1 ? 4 : nowblock.blockstyle - 1;
                mode = nowblock.blockstyle + 4;
                block lastblock = new block();
                lastblock.setPoint(nowblock.blocktype, nowblock.blockstyle);
                nowblock.setPoint(nowblock.blocktype, style);
                turnClearBlock(lastblock, nowPoint);
                foreach (var i in nowblock.blockPoint)
                {

                    try
                    {
                        if (listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X].Tag.ToString() != "0" && !lastblock.blockPoint.Contains(i))
                        {
                            canTurn = TTurn(mode);
                            break;
                        }
                    }
                    catch
                    {
                        canTurn = TTurn(mode);
                    }
                }
                nextPoint = new Point(nowPoint.X, nowPoint.Y + 1);
                if (!canTurn)
                {
                    nowblock = lastblock;
                }
                blockTurn();
            }
            if (e.KeyCode == Keys.Right)
            {
                foreach (var i in nowblock.blockPoint.Where(x => x.X == nowblock.blockPoint.Max(y => y.X)).ToList())
                {
                    try
                    {
                        if (listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X + 1].Tag.ToString() != "0") return;
                    }
                    catch {}
                }
                new Thread(new ThreadStart(blockMoveSound)).Start();

                if (nowPoint.X + nowblock.blockPoint.Max(x => x.X) < 9)
                {
                    nowPoint.X++;
                }
                blockDown();
            }
            if (e.KeyCode == Keys.Left)
            {
                foreach (var i in nowblock.blockPoint.Where(x => x.X == nowblock.blockPoint.Min(y => y.X)).ToList())
                {
                    try
                    {
                        if (listPictureBox[nowPoint.Y + i.Y][nowPoint.X + i.X - 1].Tag.ToString() != "0") return;
                    }
                    catch {}
                }
                new Thread(new ThreadStart(blockMoveSound)).Start();
                if (nowPoint.X + nowblock.blockPoint.Min(x => x.X) > 0)
                {
                    nowPoint.X--;
                }
                blockDown();
            }
            if(e.KeyCode == Keys.Space)
            {
                new Thread(new ThreadStart(blockFastDownSound)).Start();
                blockFastDown();
            }
            if(e.KeyCode == Keys.Down)
            {
                timer1.Interval = 100;
            }
            if(e.KeyCode == Keys.ShiftKey)
            {
                if (!canHold) return;
                if (holdBlock == null)
                {
                    clearBlock();
                    holdBlock = new block();
                    holdBlock.setPoint(nowblock.blocktype, 1);
                    setHoldBlock();
                    nowblock = nextblock;
                    nextblock = new block();
                    clearNextBlock();
                    r = new Random(Guid.NewGuid().GetHashCode());
                    nextblock.setPoint(blockStyles[r.Next(0, 7)], 1);
                    setNextBlcok();
                    nowPoint = new Point(4, 1);
                    if (listPictureBox[1][4].Tag.ToString() != "0")
                    {
                        gameOver();
                        return;
                    }
                    lastPoint = new Point(4, 1);
                    nextPoint = new Point(4, 2);
                    blockDown();
                }
                else
                {
                    clearHoldBlock();
                    clearBlock();
                    block temp = new block();
                    temp.setPoint(holdBlock.blocktype, 1);
                    holdBlock.setPoint(nowblock.blocktype, 1);
                    nowblock.setPoint(temp.blocktype, 1);
                    nowPoint = new Point(4, 1);
                    lastPoint = new Point(4, 1);
                    nextPoint = new Point(4, 2);
                    setHoldBlock();
                    blockDown();
                    canHold = false;
                }

            }
        }

        private void main_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                timer1.Interval = 800;
            }
        }

        void buttonClickSound()
        {
            clickSound.Play();
        }
        void blockFastDownSound()
        {
            fastDownSound.Play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(buttonClickSound)).Start();
            home.showform(new game(home,mode));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(buttonClickSound)).Start();
            home.showform(new main(home));
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            gameTimer--;
            if(gameTimer == 0)
            {
                gameOver();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            startTimer--;
            if(startTimer == 0)
            {
                gameEnd = false;
                timer1.Start();
                timer2.Start();
                groupBox2.Visible = false;
                bgm.controls.play();
                if (mode == 1)
                {
                    timer3.Start();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            second++;
            if (second == 60)
            {
                second = 0;
                minute++;
                if(mode == 1)
                {
                    if(minute == 2)
                    {
                        gameOver();
                    }
                }
            }
        }
    }
}
