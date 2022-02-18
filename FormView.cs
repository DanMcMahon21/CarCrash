/**************************************************************************/
/* Program Name:     Dan_McMahon_exercise1.cs                             */
/* Date:             1/20/2021                                            */
/* Programmer:       Dan McMahon                                          */
/**************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Dan_McMahon_exercise2
{
    public partial class Form1 : Form
    {
        enum Position
        {
            Left, Right, Forward, Stop, Crash
        }
        enum LightColor
        {
            Green, Yellow, Red
        }
        enum Direction
        {
            Left, Right, Forward, Stop
        }

        private int _x;
        private int _y;
        private int _px;
        private int _py;
        private int _light = 0;
        private bool _start = false;
        private LightColor _trafficLight;
        private Position _truckPosition;
        private Position _pintoPosition;
        private bool _directionChosen = false;
        private Direction _chosenDirection = Direction.Stop;
        private bool _waiting = true;

        public Form1()
        {
            InitializeComponent();
            label1.Text = "Direction Not Chosen";
            label3.Text = "";

            _x = 625;
            _y = 650;
            _px = 1200;
            _py = 225;

            _trafficLight = LightColor.Green;
            _truckPosition = Position.Forward;
            _pintoPosition = Position.Left;
        }

        /// <summary>
        /// This section displays images in the form
        /// </summary>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Dispose();

            // Color in the road
            SolidBrush road = new SolidBrush(Color.Gray);
            e.Graphics.FillRectangle(road, 0, 200, 1200, 200);
            e.Graphics.FillRectangle(road, 500, 0, 200, 800);

            // Color in grass areas
            Image grass = new Bitmap("grass.jfif");
            TextureBrush tBrush = new TextureBrush(grass);
            tBrush.Transform = new Matrix(
               75.0f / 640.0f,
               0.0f,
               0.0f,
               75.0f / 480.0f,
               0.0f,
               0.0f);
            e.Graphics.FillRectangle(tBrush, new Rectangle(0, 0, 500, 200));
            e.Graphics.FillRectangle(tBrush, new Rectangle(0, 400, 500, 600));
            e.Graphics.FillRectangle(tBrush, new Rectangle(700, 400, 1200, 600));
            tBrush.Dispose();

            // Draw in curbs
            Pen pen1 = new Pen(Color.FromArgb(255, 0, 0, 0), 10);
            e.Graphics.DrawLine(pen1, 0, 200, 500, 200);            
            e.Graphics.DrawLine(pen1, 0, 400, 500, 400);            
            e.Graphics.DrawLine(pen1, 700, 200, 1200, 200);            
            e.Graphics.DrawLine(pen1, 700, 400, 1200, 400);            
            e.Graphics.DrawLine(pen1, 500, 0, 500, 205);            
            e.Graphics.DrawLine(pen1, 700, 0, 700, 205);            
            e.Graphics.DrawLine(pen1, 500, 395, 500, 800);            
            e.Graphics.DrawLine(pen1, 700, 395, 700, 800);

            // Draw in lane separators
            Pen pen2 = new Pen(Color.Yellow, 10);
            e.Graphics.DrawLine(pen2, 0, 300, 50, 300);
            e.Graphics.DrawLine(pen2, 100, 300, 150, 300);
            e.Graphics.DrawLine(pen2, 200, 300, 250, 300);
            e.Graphics.DrawLine(pen2, 300, 300, 350, 300);
            e.Graphics.DrawLine(pen2, 400, 300, 450, 300);

            e.Graphics.DrawLine(pen2, 750, 300, 800, 300);
            e.Graphics.DrawLine(pen2, 850, 300, 900, 300);
            e.Graphics.DrawLine(pen2, 950, 300, 1000, 300);
            e.Graphics.DrawLine(pen2, 1050, 300, 1100, 300);
            e.Graphics.DrawLine(pen2, 1150, 300, 1200, 300);

            e.Graphics.DrawLine(pen2, 600, 0, 600, 50);
            e.Graphics.DrawLine(pen2, 600, 100, 600, 150);

            e.Graphics.DrawLine(pen2, 600, 450, 600, 500);
            e.Graphics.DrawLine(pen2, 600, 550, 600, 600);
            e.Graphics.DrawLine(pen2, 600, 650, 600, 700);
            e.Graphics.DrawLine(pen2, 600, 750, 600, 800);

            // Images for traffic light
            if (_trafficLight == LightColor.Green)
            {
                e.Graphics.DrawImage(new Bitmap("green.png"), 1100, 50, 50, 100);
            }
            else if (_trafficLight == LightColor.Yellow)
            {
                e.Graphics.DrawImage(new Bitmap("yellow.png"), 1100, 50, 50, 100);
            }
            else if (_trafficLight == LightColor.Red)
            {
                e.Graphics.DrawImage(new Bitmap("red.png"), 1100, 50, 50, 100);
                e.Graphics.DrawImage(new Bitmap("LeftTurn.png"), 1050, 110, 40, 40);
            }

            // Images for Pinto
            if(_pintoPosition == Position.Left)
            {
                e.Graphics.DrawImage(new Bitmap("Pinto_Left.png"), _px, _py, 120, 50);
            }
            else if(_pintoPosition == Position.Right)
            {
                e.Graphics.DrawImage(new Bitmap("Pinto_Right.png"), _px, _py, 120, 50);
            }
            else if (_pintoPosition == Position.Crash)
            {
                e.Graphics.DrawImage(new Bitmap("Pinto_Crash.png"), _px, _py, 120, 50);
            }

            // Images for SUV
            if (_truckPosition == Position.Right)
            {
                if(_y > 325)
                {
                    e.Graphics.DrawImage(new Bitmap("SUV_Forward.png"), _x, _y, 60, 150);
                }
                else
                {
                    e.Graphics.DrawImage(new Bitmap("SUV_Right.png"), _x, _y, 150, 60);
                }
            }
            else if (_truckPosition == Position.Left)
            {
                if(_y > 225)
                {
                    e.Graphics.DrawImage(new Bitmap("SUV_Forward.png"), _x, _y, 60, 150);
                }
                else
                {
                    e.Graphics.DrawImage(new Bitmap("SUV_Left.png"), _x - 150, _y, 150, 60);
                }
            }
            else if (_truckPosition == Position.Forward)
            {
                e.Graphics.DrawImage(new Bitmap("SUV_Forward.png"), _x, _y, 60, 150);
            }
            else if (_truckPosition == Position.Stop)
            {
                e.Graphics.DrawImage(new Bitmap("SUV_Forward.png"), _x, _y, 60, 150);
            }
            else if (_truckPosition == Position.Crash)
            {
                e.Graphics.DrawImage(new Bitmap("SUV_Forward.png"), _x, _y, 60, 150);
            }

            // Images for direction tile
            if (_chosenDirection == Direction.Right)
            {
                pictureBox1.Image = new Bitmap("RightArrow.png");
            }
            else if (_chosenDirection == Direction.Left)
            {
                pictureBox1.Image = new Bitmap("LeftArrow.png");
            }
            else if (_chosenDirection == Direction.Forward)
            {
                pictureBox1.Image = new Bitmap("ForwardArrow.png");
            }
            else if(_chosenDirection == Direction.Stop)
            {
                pictureBox1.Image = new Bitmap("stop.png");
            }

            // Images for waiting tile
            if (_waiting == true)
            {
                pictureBox2.Image = new Bitmap("waiting.png");
            }
            else
            {
                pictureBox2.Image = null;
            }
        }

        /// <summary>
        /// This section controls what happens with the position of the SUV with each tick of timerMoving
        /// </summary>
        private void timerMoving_Tick(object sender, EventArgs e)
        {
            // Execute if right position chosen
            if (_truckPosition == Position.Right)
            {
                if (_x < 1350)
                {
                    if (_y > 325)
                    {
                        _y -= 8;
                    }
                    else
                    {
                        _x += 20;
                    }
                }
                else
                {
                    if (timerPinto.Enabled == true)
                    {
                        timerMoving.Stop();
                    }
                    else
                    {
                        _truckPosition = Position.Forward;
                        label1.Text = "Direction Not Chosen";
                        label3.Text = "";
                        _trafficLight = LightColor.Green;
                        _light = 0;
                        _directionChosen = false;
                        _chosenDirection = Direction.Forward;
                        _waiting = true;
                        _x = 625;
                        _y = 800;
                        _start = false;
                    }
                }
            }

            // Execute if left position chosen
            else if (_truckPosition == Position.Left)
            {
                if (_x > -150)
                {
                    if (_y > 225)
                    {
                        _y -= 8;
                    }
                    else
                    {
                        _x -= 20;
                    }
                }
                else
                {
                    if (timerPinto.Enabled == true)
                    {
                        timerMoving.Stop();
                    }
                    else
                    {
                        _truckPosition = Position.Forward;
                        label1.Text = "Direction Not Chosen";
                        label3.Text = "";
                        _trafficLight = LightColor.Green;
                        _light = 0;
                        _directionChosen = false;
                        _chosenDirection = Direction.Forward;
                        _waiting = true;
                        _x = 625;
                        _y = 800;
                        _start = false;
                    }
                }
            }

            // Execute if forward position chosen
            else if (_truckPosition == Position.Forward)
            {
                if (_y > -150)
                {
                    if (_start == true)
                    {
                        _y -= 8;
                    }
                    else
                    {
                        if (_y > 650)
                        {
                            _y -= 10;
                        }
                        else
                        {
                            _pintoPosition = Position.Left;
                            _px = 1200;
                            _py = 225;
                            timerMoving.Stop();
                            timerLight.Stop();
                            _chosenDirection = Direction.Stop;
                        }
                    }
                }
                else
                {
                    if (timerPinto.Enabled == true)
                    {
                        timerMoving.Stop();
                    }
                    else
                    {
                        label1.Text = "Direction Not Chosen";
                        label3.Text = "";
                        _trafficLight = LightColor.Green;
                        _light = 0;
                        _directionChosen = false;
                        _chosenDirection = Direction.Forward;
                        _waiting = true;
                        _x = 625;
                        _y = 800;
                        _start = false;
                    }
                }
            }

            // Execute if crash position chosen
            else if (_truckPosition == Position.Crash)
            {
                if (_y > -150)
                {
                    _y -= 20;

                    if(_y < 225 && (_px < 700 && _px > 520))
                    {
                        timerPinto.Stop();
                        _pintoPosition = Position.Crash;
                        label3.Text = "Wrecked It!";
                    }
                    else if (_y < 50 && _pintoPosition != Position.Crash)
                    {
                        label3.Text = "Missed It!";
                    }
                }
                else
                {
                    if (_pintoPosition == Position.Crash)
                    {
                        if (timerPinto.Enabled == true)
                        {
                            timerMoving.Stop();
                        }
                        else
                        {
                            _truckPosition = Position.Forward;
                            label1.Text = "Direction Not Chosen";
                            label3.Text = "";
                            _trafficLight = LightColor.Green;
                            _light = 0;
                            _directionChosen = false;
                            _chosenDirection = Direction.Forward;
                            _waiting = true;
                            _x = 625;
                            _y = 800;
                            _start = false;
                        }
                    }
                    else
                    {
                        _truckPosition = Position.Forward;

                        if (_px != 1200)
                        {
                            timerPinto.Start();
                            timerMoving.Stop();
                        }
                    }
                }
            }

            Invalidate();
        }

        /// <summary>
        /// This section determines what happens with the commands keyed in by the user
        /// </summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Execute if simulation has not started yet
            if (_start == false)
            {
                if (e.KeyCode == Keys.Up)
                {
                    _start = true;
                    timerLight.Start();
                    timerMoving.Start();
                    timerPinto.Start();
                    _truckPosition = Position.Forward;
                    _chosenDirection = Direction.Forward;
                    label3.Text = "";
                }
                else
                {
                    label3.Text = "Must Start By Moving Forward!";
                }
            }

            // Execute if simulation has already started
            else
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (_directionChosen == true)
                    {
                        label3.Text = "Direction Already Chosen!";
                    }
                    else if (_truckPosition == Position.Stop)
                    {
                        label3.Text = "Can Only Move Forward!";
                    }
                    else if (_y < 225)
                    {
                        label3.Text = "Too Late To Turn Left!";
                    }
                    else
                    {
                        if (_trafficLight != LightColor.Red)
                        {
                            label3.Text = "No Left Turn Signal!";
                        }
                        else
                        {
                            _truckPosition = Position.Left;
                            label1.Text = "Direction Chosen";
                            label3.Text = "";
                            _directionChosen = true;
                            _chosenDirection = Direction.Left;
                            _waiting = false;
                        }
                    }
                }

                if (e.KeyCode == Keys.Right)
                {
                    if (_directionChosen == true)
                    {
                        label3.Text = "Direction Already Chosen!";
                    }
                    else if (_truckPosition == Position.Stop)
                    {
                        label3.Text = "Can Only Move Forward!";
                    }
                    else if (_y < 325)
                    {
                        label3.Text = "Too Late To Turn Right!";
                    }

                    else
                    {
                        _truckPosition = Position.Right;
                        label1.Text = "Direction Chosen";
                        label3.Text = "";
                        _directionChosen = true;
                        _chosenDirection = Direction.Right;
                        _waiting = false;
                    }
                }

                if (e.KeyCode == Keys.Up)
                {
                    if (_truckPosition == Position.Stop)
                    {
                        timerMoving.Start();
                        _truckPosition = Position.Forward;
                        label1.Text = "Direction Not Chosen";
                        label3.Text = "";
                        _light = 0;
                        _directionChosen = false;
                        _chosenDirection = Direction.Forward;
                    }
                    else
                    {
                        if (_truckPosition == Position.Forward)
                        {
                            label3.Text = "Already Moving Forward!";
                        }
                        else if (_directionChosen == true)
                        {
                            label3.Text = "Direction Already Chosen!";
                        }
                        else
                        {
                            _truckPosition = Position.Forward;
                            label1.Text = "Direction Chosen";
                            _directionChosen = true;
                            _chosenDirection = Direction.Forward;
                            _waiting = false;
                        }
                    }
                }

                if (e.KeyCode == Keys.Down)
                {
                    if (_truckPosition == Position.Stop)
                    {
                        label3.Text = "Can Only Move Forward!";
                    }
                    else if (_truckPosition == Position.Crash)
                    {
                        label3.Text = "Committed To Crashing!";
                    }
                    else if (_x == 625 && _y > -150)
                    {
                        timerMoving.Stop();
                        _truckPosition = Position.Stop;
                        label1.Text = "Direction Not Chosen";
                        label3.Text = "";
                        _light = 0;
                        _directionChosen = false;
                        _chosenDirection = Direction.Stop;
                        _waiting = true;
                    }
                    else
                    {
                        label3.Text = "Too Late To Stop!";
                    }
                }

                if (e.KeyCode == Keys.Space)
                {
                    if (_directionChosen == true)
                    {
                        label3.Text = "Direction Already Chosen!";
                    }
                    else if (_truckPosition != Position.Forward)
                    {
                        label3.Text = "Must Be Moving Forward!";
                    }
                    else if (_x == 625)
                    {
                        _truckPosition = Position.Crash;
                        label1.Text = "Crash Course";
                        label3.Text = "";
                        _light = 0;
                        _directionChosen = true;
                        _chosenDirection = Direction.Forward;
                        _waiting = false;
                    }
                    else
                    {
                        label3.Text = "Already Turned!";
                    }
                }

            }
        }

        /// <summary>
        /// This section controls the traffic light color with each tick of timerLight
        /// </summary>
        private void timerLight_Tick(object sender, EventArgs e)
        {
            _light++;

            if(_light < 3)
            {
                _trafficLight = LightColor.Green;
                Invalidate();
            }
            else if (_light >= 3 && _light < 4)
            {
                _trafficLight = LightColor.Yellow;
                Invalidate();
            }
            else if (_light > 4)
            {
                _trafficLight = LightColor.Red;
                Invalidate();
            }

            if(_light == 10)
            {
                _light = 0;
            }
        }

        /// <summary>
        /// This section controls what happens with the position of the Pinto with each tick of timerPinto
        /// </summary>
        private void timerPinto_Tick(object sender, EventArgs e)
        {
            if (_pintoPosition == Position.Left)
            {
                if (_px > -120)
                {
                    if ((_px > 700 && _px < 720) && _truckPosition == Position.Crash)
                    {
                        if (_y > 125)
                        {
                            timerPinto.Stop();
                        }
                    }
                    else
                    {
                        if (_y < 350 && _truckPosition != Position.Crash)
                        {
                            _px -= 40;
                        }
                        else if (_px < 710)
                        {
                            _px -= 20;
                        }
                        else
                        {
                            _px -= 10;
                        }
                    }
                }
                else
                {
                    if (_y < 125 || _x != 625)
                    {
                        _pintoPosition = Position.Right;
                        _px = -120;
                        _py = 325;
                    }
                }
            }
            else if (_pintoPosition == Position.Right)
            {
                if (_px < 1200)
                {
                    _px += 40;
                }
                else
                {
                    _px = 1200;
                    _py = 225;
                    _pintoPosition = Position.Left;
                    timerPinto.Stop();
                    timerMoving.Start();
                    if (_y < -150)
                    {
                        label1.Text = "Direction Not Chosen";
                        label3.Text = "";
                        _light = 0;
                        _directionChosen = false;
                        _chosenDirection = Direction.Forward;
                    }
                }
            }

            Invalidate();
        }
    }
}
