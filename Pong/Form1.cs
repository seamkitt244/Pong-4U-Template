/*
 * Description:     A basic PONG simulator
 * Author:          Seamus Kittmer    
 * Date:            Febuary 4
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values
        //random values
        Random randGen = new Random();
        int value;

        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.White);
        SolidBrush powerBrush = new SolidBrush(Color.Pink);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean aKeyDown, zKeyDown, jKeyDown, mKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = false;
        const int BALL_SPEED = 4;
        Rectangle ball;

        //Power up vars
        Rectangle power;
        Boolean veiw = false;
        Boolean powerMoveRight = false;
        Boolean powerMoveDown = false;
        const int POWER_SPEED = 2;

        //paddle speeds and rectangles
        int paddle1Speed = 4;
        int paddle2Speed = 4;
        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 5;  // number of points needed to win game

        int paddle1Edge = 20;
        int paddle2Edge = 20;  // buffer distance between screen edge and paddle            
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.Z:
                    zKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.M:
                    mKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.Z:
                    zKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.M:
                    mKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
                veiw = false;
                //paddle2Edge=paddle1Edge =20
            }
            #region
            paddle1Speed = paddle2Speed = 4;
            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            p1.X = 20;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - 20 - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // TODO set Width and Height of ball
            ball.Width = 10;
            ball.Height = 10;
            // TODO set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2;
            // TODO set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2;

            //pwer up start location
            power.Width = 10;
            power.Height = 10;

            power.X = this.Width / 2;
            power.Y = this.Height / 2;

            this.Refresh();
            #endregion
        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region power tick
            //Power tick 
            if (powerMoveRight)
            {
                power.X = power.X + POWER_SPEED;
            }
            else
            {
                power.X = power.X - POWER_SPEED;
            }
            if (powerMoveDown)
            {
                power.Y = power.Y - POWER_SPEED;
            }
            else
            {
                power.Y = power.Y + POWER_SPEED;
            }
            #endregion

            #region update ball position
            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED

            if (ballMoveRight)
            {
                ball.X = ball.X + BALL_SPEED;
            }
            else
            {
                ball.X = ball.X - BALL_SPEED;
            }
            //TODO code to move ball either down or up based on ballMoveDown and using BALL_SPEED
            if (ballMoveDown)
            {
                ball.Y = ball.Y - BALL_SPEED;
            }
            else
            {
                ball.Y = ball.Y + BALL_SPEED;
            }
            #endregion

            #region update paddle positions

            if (aKeyDown == true && p1.Y > 4)
            { p1.Y = p1.Y - paddle1Speed; }

            // TODO create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED

            if (zKeyDown == true && p1.Y < (this.Height - p1.Height - 4))
            { p1.Y = p1.Y + paddle1Speed; }

            // TODO create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED

            if (jKeyDown == true && p2.Y > 4)
            { p2.Y = p2.Y - paddle2Speed; }

            // TODO create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED
            if (mKeyDown == true && p2.Y < (this.Height - p2.Height - 4))
            { p2.Y = p2.Y + paddle2Speed; }

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0)
            {
                ballMoveDown = false;
                collisionSound.Play();
                // this.BackColor = Color.FromArgb(randGen.Next(1, 250), randGen.Next(1, 250), randGen.Next(1, 250));
                this.Refresh();
            }

            if (ball.Y > this.Height - ball.Height)
            {
                ballMoveDown = true;
                collisionSound.Play();
                // this.BackColor = Color.FromArgb(randGen.Next(1, 250), randGen.Next(1, 250), randGen.Next(1, 250));
                this.Refresh();
            }

            // TODO In an else if statement use ball.Y, this.Height, and ball.Width to check for collision with bottom line
            // If true use ballMoveDown down boolean to change direction

            #endregion

            #region Power up collision
            if (power.Y < 0)
            {
                powerMoveDown = false;
                // this.BackColor = Color.FromArgb(randGen.Next(1, 250), randGen.Next(1, 250), randGen.Next(1, 250));
                this.Refresh();
            }

            if (power.Y > this.Height - ball.Height)
            {
                powerMoveDown = true;
                // this.BackColor = Color.FromArgb(randGen.Next(1, 250), randGen.Next(1, 250), randGen.Next(1, 250));
                this.Refresh();
            }
            #endregion

            #region ball collision with paddles

            // TODO create if statment that checks p1 collides with ball and if it does
            //// TODO create if statment that checks p2 collides with ball and if it does
            if (ball.IntersectsWith(p1) || p2.IntersectsWith(ball) || power.IntersectsWith(p1) || power.IntersectsWith(p2))
            {
                if (ball.IntersectsWith(p1))
                {
                    ballMoveRight = true;
                }
                else if (p2.IntersectsWith(ball))
                {
                    ballMoveRight = false;
                }
                value = randGen.Next(1, 5);
                if (power.IntersectsWith(p1))
                {
                    veiw = false;
                    if (value == 1)
                    {
                        p2.Height = 20;
                        p1.Height = 40;
                    }
                    else if (value == 2)
                    {
                        p1.Height = 80;
                    }
                    else if (value == 3)
                    {
                        paddle1Speed = paddle1Speed * 2;
                        paddle2Speed = paddle2Speed - 2;
                    }
                    //else if (value == 4)
                    //{
                    //    paddle2Edge = 30;
                    //}

                }
                if (power.IntersectsWith(p2))
                {
                    veiw = false;
                    if (value == 1)
                    {
                        p1.Height = 20;
                        p2.Height = 40;
                    }
                    else if (value == 2)
                    {
                        p2.Height = 80;
                    }
                    else if (value == 3)
                    {
                        paddle1Speed = paddle1Speed - 2;
                        paddle2Speed = paddle2Speed * 2;
                    }
                    //else if (value == 4)
                    //{
                    //    paddle1Edge = 30;
                    //}
                }
                collisionSound.Play();
                this.BackColor = Color.FromArgb(randGen.Next(1, 250), randGen.Next(1, 250), randGen.Next(1, 250));
                this.Refresh();
            }
            #endregion

            #region ball collision with side walls (point scored)
            //    // TODO
            if (ball.X < 0 || ball.X > this.Width)
            {
                if (ball.X < 0) //left wall
                {
                    player2Score = player2Score + 1;
                    ballMoveRight = true;
                    powerMoveRight = true;
                }
                if (ball.X > this.Width)//right wall
                {
                    player1Score = player1Score + 1;
                    ballMoveRight = false;
                    powerMoveRight = false;
                }
                veiw = true;
                scoreSound.Play();
                this.ForeColor = Color.FromArgb(randGen.Next(1, 250), randGen.Next(1, 250), randGen.Next(1, 250));
                gameUpdateLoop.Stop();
                startLabel.Visible = true;
                startLabel.Text = player1Score + " : " + player2Score;
                this.Refresh();
                Thread.Sleep(2000);
                startLabel.Visible = false;
                gameUpdateLoop.Start();
                SetParameters();
            }

            if (player1Score == gameWinScore)
            {
                GameOver("p1");
            }
            if (player2Score == gameWinScore)
            {
                GameOver("p2");
            }

            //  TODO use if statement to check to see if player 1 has won the game. If true run
            //  GameOver method.Else change direction of ball and call SetParameters method.

            this.Refresh();
            #endregion
        }

        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            // TODO create game over logic
            // --- stop the gameUpdateLoop
            newGameOk = false;
            gameUpdateLoop.Stop();
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).
            startLabel.Visible = true;
            if (winner == "p1")
            { startLabel.Text = "Player 1 wins"; }

            else
            { startLabel.Text = "Player 2 wins"; }

            // --- pause for two seconds 
            this.Refresh();
            Thread.Sleep(2000);
            // --- use the startLabel to ask the user if they want to play again

            newGameOk = true;
            startLabel.Text = "Press space bar to play again, or press n to exit";
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw paddles using FillRectangle
            e.Graphics.FillRectangle(drawBrush, p1);
            e.Graphics.FillRectangle(drawBrush, p2);
            // TODO draw ball using FillRectangle
            e.Graphics.FillRectangle(drawBrush, ball);

            //drawing power up 
            if (veiw == true)
            {
                e.Graphics.FillRectangle(powerBrush, power);
            }
        }
    }
}