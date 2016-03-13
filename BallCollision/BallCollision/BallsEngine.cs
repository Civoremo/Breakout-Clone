using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BallCollision
{
    public class BallsEngine
    {
        public List<Balls> ballsList;
        public Texture2D ballTexture;
        public Vector2 startBallPosition;
        public Vector2 ballPosition;
        public Vector2 previousBallPosition;
        public Rectangle TopRec, BottomRec, LeftRec, RightRec, WholeRec;

        public float Speed;
        public Vector2 Direction;
        public bool Move = false;

        public int Lives;



        public BallsEngine(Texture2D texture)
        {
            this.ballTexture = texture;
            this.ballsList = new List<Balls>();

            ballPosition = Vector2.Zero;
            Speed = 8f;
            Direction = new Vector2(1f, -1) * Speed;
            Lives = 3;
            
        }

        public Balls BallsGenerator(Vector2 position, bool active, Vector2 direction)
        {
            ballPosition = position;
            bool Active = active;
            Vector2 Direction = direction;
            
            TopRec = new Rectangle(((int)ballPosition.X + (ballTexture.Width) / 2), (int)ballPosition.Y, 1, 1);
            BottomRec = new Rectangle(((int)ballPosition.X + ((ballTexture.Width) / 2)), (int)ballPosition.Y + ballTexture.Height, 1, 1);
            LeftRec = new Rectangle((int)ballPosition.X, (int)ballPosition.Y + (ballTexture.Height / 2), 1, 1);
            RightRec = new Rectangle((int)ballPosition.X + (ballTexture.Width), (int)ballPosition.Y + (ballTexture.Width / 2), 1, 1);
            WholeRec = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, ballTexture.Width, ballTexture.Height);

            return new Balls(ballTexture, ballPosition, TopRec, BottomRec, RightRec, LeftRec, WholeRec, Active, Direction);
        }


        public void Update()
        {
                for (int num = 0; num < ballsList.Count; num++)
                {
                    ballsList[num].Position += (ballsList[num].Direction);

                    ballsList[num].TopRec = new Rectangle((int)ballsList[num].Position.X + (ballsList[num].Texture.Width / 2), (int)ballsList[num].Position.Y, 1, 1);

                    ballsList[num].BottomRec = new Rectangle((int)ballsList[num].Position.X + (ballsList[num].Texture.Width / 2), (int)ballsList[num].Position.Y + ballsList[num].Texture.Height, 1, 1);

                    ballsList[num].LeftRec = new Rectangle((int)ballsList[num].Position.X, (int)ballsList[num].Position.Y + (ballsList[num].Texture.Height / 2), 1, 1);

                    ballsList[num].RightRec = new Rectangle((int)ballsList[num].Position.X + (ballsList[num].Texture.Width), (int)ballsList[num].Position.Y + (ballsList[num].Texture.Width / 2), 1, 1);

                    ballsList[num].WholeRec = new Rectangle((int)ballsList[num].Position.X, (int)ballsList[num].Position.Y, ballsList[num].Texture.Width, ballsList[num].Texture.Height);
                }

                for (int y = 0; y < ballsList.Count; y++)
                {
                    if (ballsList[y].Position.X < 0)
                    {
                        ballsList[y].Direction *= new Vector2(-1, 1);

                    }
                    if (ballsList[y].Position.X > (700 - ballsList[y].Texture.Width))
                    {
                        ballsList[y].Direction *= new Vector2(-1, 1);

                    }
                    if (ballsList[y].Position.Y < 0)
                    {
                        ballsList[y].Direction *= new Vector2(1, -1);
                    }
                    if (ballsList[y].Position.Y > 800)
                    {
                        ballsList.RemoveAt(y);
                    }
                }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < ballsList.Count; x++)
            {
                ballsList[x].Draw(spriteBatch);
            }
        }


        public void BallsBuild(Vector2 position, int n)
        {
            int numOfBalls;
            numOfBalls = n;
            startBallPosition = position;
            
                for (int x = 0; x < numOfBalls; x++)
                {
                    if (x == 0)
                    {
                        Vector2 direction = new Vector2(-.69f, -.89f);
                        bool ballMove = true;
                        ballsList.Add(BallsGenerator(startBallPosition, ballMove, direction));
                    }
                    if (x == 1)
                    {
                        Vector2 direction = new Vector2(-.54f, -.68f);
                        bool ballMove = true;
                        ballsList.Add(BallsGenerator(startBallPosition, ballMove, direction));
                    }
                    if (x == 2)
                    {
                        Vector2 direction = new Vector2(.7f, -.77f);
                        bool ballMove = true;
                        ballsList.Add(BallsGenerator(startBallPosition, ballMove, direction));
                    }
                }
        }
    }
}
