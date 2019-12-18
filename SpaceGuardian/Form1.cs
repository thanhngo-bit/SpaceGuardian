using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace SpaceGuardian
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootMedia;
        WindowsMediaPlayer explosion;

        PictureBox[] stars;
        int playerSpeed;

        PictureBox[] munitions;
        int munitionSpeed;

        PictureBox[] enemies;
        int enemiSpeed;

        PictureBox[] enemiesMunition;
        int ememiesMunitionSpeed;

        Random rnd;
        int backgroundspeed;

        //detail for remain part of the game
        int score;
        int level;
        int difficulty;
        bool pause;
        bool gameIsOver;

        public Form1()
        {
            InitializeComponent();
        }


        //public string[] enemiOptions = { "enemi1", "enemi2", "enemi3", "boss1", "boss2" };
        private void Form1_Load(object sender, EventArgs e)
        {
            //for remain part of the game
            pause = false;
            gameIsOver = false;
            score = 0;
            level = 1;
            difficulty = 9; //the more number decrease the more difficult will be

            //speed
            backgroundspeed = 4;
            playerSpeed = 4;
            enemiSpeed = 4;
            ememiesMunitionSpeed = 4;

            munitionSpeed = 10;
            munitions = new PictureBox[5]; //default 3

            //Load enemies Picture
            Image enemi1 = Image.FromFile(@"..\..\img\E1.png");
            Image enemi2 = Image.FromFile(@"..\..\img\E2.png");
            Image enemi3 = Image.FromFile(@"..\..\img\E3.png");
            Image boss1 = Image.FromFile(@"..\..\img\Boss1.png");
            Image boss2 = Image.FromFile(@"..\..\img\Boss2.png");

            int numberEnemi = 12;
            enemies = new PictureBox[numberEnemi];

            //InitialiseEnemis PictureBoxes
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);

            }


            //Call random enemi Picturebox with specific number
            //
            //EX: enemies[0].Image = boss1;
            //
            Image[] enemiOptions = { enemi1, enemi2, enemi3, boss1, boss2 };
            Random r = new Random();
            int index;
            for (int i = 0; i < numberEnemi; i++)
            {
                index = r.Next(enemiOptions.Length);
                enemies[i].Image = enemiOptions[index];
            }

            //Load ammunition
            Image munitionimg = Image.FromFile(@"..\..\img\munition.png");
            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munitionimg;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }

            //create object WMP
            gameMedia = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //Load sound through object WMP
            // full physical link
            //"D:\Workspace of coding\Network programming\ProjectSpaceGuardian\SpaceGuardian\SpaceGuardian\sound\shoot.mp3"
            //"D:\Workspace of coding\Network programming\ProjectSpaceGuardian\SpaceGuardian\SpaceGuardian\sound\GameSong.mp3"
            gameMedia.URL = @"D:\\Workspace of coding\\Network programming\\ProjectSpaceGuardian\\SpaceGuardian\\SpaceGuardian\\sound\\GameSong.mp3" ;
            shootMedia.URL = @"D:\\Workspace of coding\\Network programming\\ProjectSpaceGuardian\\SpaceGuardian\\SpaceGuardian\\sound\\shoot.mp3" ;
            explosion.URL =  @"D:\\Workspace of coding\\Network programming\\ProjectSpaceGuardian\\SpaceGuardian\\SpaceGuardian\\sound\\boom.mp3";

            //Setup sound setting through WMP
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 60;
            shootMedia.settings.volume = 10;
            explosion.settings.volume = 70;

            stars = new PictureBox[10];
            rnd = new Random();
            
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(20, 500), rnd.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }
                this.Controls.Add(stars[i]);
            }

            //enemies Munition
            enemiesMunition = new PictureBox[10];
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i] = new PictureBox();
                enemiesMunition[i].Size = new Size(2, 25);
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10);
                enemiesMunition[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemiesMunition[i]);
            }

            //play media
            gameMedia.controls.play();
        }

        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                stars[i].Top += backgroundspeed;
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }

            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundspeed - 2;
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            { 
                Player.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 675)
            {
                Player.Left += playerSpeed;
            }
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }

        //Key down
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause) //game not pause then...
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }
            

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();

            //for pause game and game over --------- process how button and label act
            if (e.KeyCode == Keys.Space)
            {
                if (!gameIsOver)
                {
                    if (pause)
                    {
                        StartTimer();
                        label1.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label1.Location = new Point(this.Width / 2 - 50, 150);
                        label1.Text = "Pause";
                        label1.Visible = true;
                        gameMedia.controls.pause();
                        StopTimer();
                        pause = true;
                    }
                }
            }
        }

        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            //shooting sound
            shootMedia.controls.play();

            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= munitionSpeed;
                    //call collision function - from object picturebox to object picturebox
                    Collision();
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiSpeed);
        }

        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;
                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -180);
                }
            }
        }

        //for collision sound working
        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.controls.play();

                    //for counting score and level


                    //labelScore.Text = (score < 10) ? "0" + score.ToString() : level.ToString();
                    // How score counts
                    score += 1;
                    if (score<10)
                    {
                        labelScore.Text = "0" + score.ToString();
                    }
                    else
                    {
                        labelScore.Text = score.ToString();
                    }

                    //How level counts

                    if (score % 30 == 0)
                    {
                        level += 1;
                        if (level<10)
                        {
                            labelLvl.Text = "0" + level.ToString();
                        }
                        else
                        {
                            labelLvl.Text = level.ToString();
                        }
                        //
                        if (enemiSpeed <= 10 && ememiesMunitionSpeed <= 10 && difficulty >=0)
                        {
                            difficulty -= 1;
                            enemiSpeed += 1;
                            ememiesMunitionSpeed += 1;
                        }
                        if (level ==10)
                        {
                            GameOver("NICE DONE");
                        }
                    }
                    //level
                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }
                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 80;
                    explosion.controls.play();
                    Player.Visible = false;
                    //
                    GameOver("GAME OVER");
                }
            }
        }
        private void GameOver(string str)
        {
            //when game over
            label1.Text = str;
            //label1.Location = new Point(20, 20);
            label1.Visible = true;
            //label1.Enabled = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;
            //former
            gameMedia.controls.stop();
            StopTimer();
        }

        //stop timer
        private void StopTimer()
        {
            MoveBgTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveMunitionTimer.Stop();
            EnemiesMunitionTimer.Stop();
            //
            MoveEnemiesTimer.Enabled = false;
        }

        //start timer
        private void StartTimer()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveMunitionTimer.Start();
            //
            EnemiesMunitionTimer.Start();
            MoveEnemiesTimer.Enabled = true;
        }

        //enemies Munition
        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemiesMunition.Length - difficulty); i++)
            {
                if (enemiesMunition[i].Top < this.Height)
                {
                    enemiesMunition[i].Visible = true;
                    enemiesMunition[i].Top += munitionSpeed;
                    //call collision with enemies munition
                    CollisionWithEnemiesMunition();
                }
                else
                {
                    enemiesMunition[i].Visible = false;
                    int x = rnd.Next(0, 12);
                    enemiesMunition[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
                
            }
        }

        //collision with enemies Munition
        private void CollisionWithEnemiesMunition()
        {
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                if (enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMunition[i].Visible = false;
                    explosion.settings.volume = 70;   //explosion song volume
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("GAME OVER");
                }
            }
        }

       

        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
