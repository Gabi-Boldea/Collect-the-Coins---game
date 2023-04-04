using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P_Boldea_Gabriel
{
    public partial class Form1 : Form
    {
        SoundPlayer sunet = new SoundPlayer(Properties.Resources.smrpg_coin);
        public Form1()
        {
            InitializeComponent();
            door.Visible = false;
            player.Parent = background;
            platform1.Parent = background;
            platform2.Parent = background;
            platform3.Parent = background;
            platform4.Parent = background;
            platform5.Parent = background;
            platform6.Parent = background;
            coin1.Parent = background;
            coin2.Parent = background;
            coin3.Parent = background;
            coin4.Parent = background;
            coin5.Parent = background;
            coin6.Parent = background;
            door.Parent = background;
            label1.Parent = background;
            puncte.Parent = background;
        }

        #region .. Double Buffered function ..
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }
        #endregion

        #region .. code for Flickering ..
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        #endregion

        bool sus=false, dreapta=false, stanga=false;
        int viteza = 15;
        int cadere = 5;
        int pozitie = 1;
        int pu = 6; //puctaj
        //int forta=8;
        bool ok = false;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Up)
            {
                sus = true;
            }
            
            if (e.KeyCode == Keys.Left)
                stanga = true;
            if (e.KeyCode == Keys.Right)
                dreapta = true;
            /*
            if (e.KeyCode == Keys.Up && !sus)
                sus = true;
            */
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                sus = false;
            if (e.KeyCode == Keys.Left)
                stanga = false;
            if (e.KeyCode == Keys.Right)
                dreapta = false;
            /*
            if (sus)
                sus = false;
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (player.Right + background.Left > 550 && background.Left > -600)
            {
                background.Left--;
                label1.Left++;
                puncte.Left++;
            }

            while (player.Left + background.Left < 60 && background.Left < 0)
            {
                background.Left++;
                label1.Left--;
                puncte.Left--;
            }
            
            if (player.Top < this.Height - player.Height - 50)
            {
                player.Top += cadere;
            }
            /*
            //player.Top += viteza;
            if (sus && forta < 0)
                sus = false;
            */
            if (sus)
            {
                player.Top -= viteza;
                //viteza = -12;
                //forta -= 1;
            }
            else
                viteza = 12;
            if (dreapta)
            {
                //player.Left += 5;
                player.Left += viteza;
                if (pozitie == 0)
                {
                    player.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                    pozitie = 1;
                }
                if (player.Left > background.Right - player.Width - 10)
                    player.Left = background.Right - player.Width -5;
            }
            if (stanga)
            {
                //player.Left -= 5;
                player.Left -= viteza;
                if (pozitie == 1)
                {
                    player.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                    pozitie = 0;
                }
                if (player.Left < 5)
                    player.Left = player.Left + 15;
            }
            foreach (Control x in background.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "platform")
                {
                    if (x.Bounds.IntersectsWith(player.Bounds) && !sus)
                    {
                        cadere = 8;
                        //forta = 8;
                        player.Top = x.Top - player.Height;
                    }
                }
            }
            //banuti
            foreach (Control x in background.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "coin")
                {
                    if (x.Bounds.IntersectsWith(player.Bounds) && x.Visible == true)
                    {
                        sunet.Play();
                        x.Visible = false;
                        pu--;
                        puncte.Text = Convert.ToString(pu);
                        if (pu == 0)
                        {
                            door.Visible = true;
                        }
                    }
                }
                if (door.Bounds.IntersectsWith(player.Bounds) && door.Visible == true)
                {
                    if (ok == false)
                    {
                        //this.Visible = false;
                        //F2 g = new F2(pu);
                        //g.ShowDialog();
                        //this.Close();
                        ok = true;
                        timer1.Stop();
                        MessageBox.Show("Ai castigat!");
                    }
                }
            }
        }
    }
}