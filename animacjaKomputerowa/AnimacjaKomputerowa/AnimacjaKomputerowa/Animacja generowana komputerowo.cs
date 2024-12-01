using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimacjaKomputerowa
{
    public partial class Form1 : Form
    {
        int ilosc;
        Graphics graphics;
        Pen pen;
        float height;
        int interval = 10;
        float gravity = 0.2f;
        private bool animationRunning = false;

        List<Ball> balls = new List<Ball>();

        public Form1()
        {
            InitializeComponent();
            pen = new Pen(Color.Black);
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int ilosc = trackBar1.Value;
            label1.Text = "ilość piłek: " + ilosc.ToString();
        }
        private void start_Click(object sender, EventArgs e)
        {
            ilosc = trackBar1.Value;
            if (animationRunning)
            {
                return;
            }
            balls.Clear();

            Random random = new Random();
            
            // Dodajemy kilka piłek do listy
            for (int i = 0; i < ilosc; i++)
            {
                float x0 = random.Next(0, pictureBox1.Width - 35); 
                float y0 = random.Next(0, pictureBox1.Height / 2); 
                float angle = (float)(random.NextDouble() * Math.PI / 2); 
                float speed = random.Next(2, 5); 

                float velocityX = speed * (float)Math.Cos(angle);
                float velocityY = speed * (float)Math.Sin(angle);

                Color color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)); 

                Ball ball = new Ball(x0, y0, velocityX, velocityY, color); 
                balls.Add(ball);
            }

            height = 35f;
            graphics = pictureBox1.CreateGraphics();
            animationRunning = true;

            Task.Run(() =>
            {
                do
                {
                    bool allBallsStopped = true; 

                    foreach (Ball ball in balls)
                    {
                        Brush brush = new SolidBrush(ball.Color);
                        graphics.FillEllipse(brush, ball.X, ball.Y, 35f, 35f);
                        ball.VelocityY += gravity;
                        ball.Y += ball.VelocityY;

                        // Sprawdzamy kolizje z krawędziami
                        if (ball.Y >= pictureBox1.Height - height - 3) 
                        {
                            ball.Y = pictureBox1.Height - height - 3;
                            ball.VelocityY *= -0.8f; 
                        }
                        else if (ball.Y <= 0) 
                        {
                            ball.Y = 0;
                            ball.VelocityY *= -0.8f; 
                        }

                        if (ball.X <= 0 || ball.X >= pictureBox1.Width - 35)
                        {
                            ball.VelocityX *= -1; 
                        }
                        if (Math.Abs(ball.VelocityY) > 0.1f || Math.Abs(ball.VelocityX) > 0.1f)
                        {
                            allBallsStopped = false; 
                        }
                    }

                    graphics.DrawLine(pen, 4, pictureBox1.Height - 3, pictureBox1.Width - 5, pictureBox1.Height - 3);
                    System.Threading.Thread.Sleep(interval);
                    graphics.Clear(pictureBox1.BackColor);

                    if (allBallsStopped)
                    {
                        animationRunning = false;
                        break;
                    }

                } while (animationRunning);
            });
        }
        // Klasa reprezentująca piłkę
        public class Ball
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float VelocityX { get; set; }
            public float VelocityY { get; set; }
            public Color Color { get; set; }

            public Ball(float x, float y, float velocityX, float velocityY, Color color)
            {
                X = x;
                Y = y;
                VelocityX = velocityX;
                VelocityY = velocityY;
                Color = color;
            }
        }


        private void restart_Click(object sender, EventArgs e)
        {
            animationRunning = false;
            balls.Clear();
            graphics.Clear(pictureBox1.BackColor);
        }


    }
}