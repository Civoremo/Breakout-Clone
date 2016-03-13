using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BallCollision
{
    public class Ball
    {
        public Vector2 StartingPosition;
        public Vector2 CurrentPos;
        public Vector2 PreviousPos;
        //public Vector2 Origin;
        public Texture2D Texture;
        public Rectangle TopRec, BottomRec, LeftRec, RightRec, WholeRec;

        public float Speed;
        public Vector2 Direction;
        public bool Move = false;

        public int Lives;
        

        public Ball()
        {

            Texture = null;
            CurrentPos = new Vector2(400, 700);
            PreviousPos = new Vector2(400, 700);            
            //Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Speed = 8f;
            Direction = new Vector2(1f, 1) * Speed;

            Lives = 3;
        }


        public void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("ballGrey");

            TopRec = new Rectangle(((int)CurrentPos.X + (Texture.Width) / 2), (int)CurrentPos.Y, 1, 1);
            BottomRec = new Rectangle(((int)CurrentPos.X + ((Texture.Width) / 2)), (int)CurrentPos.Y + Texture.Height, 1, 1 );
            LeftRec = new Rectangle((int)CurrentPos.X, (int)CurrentPos.Y + (Texture.Height / 2), 1, 1);
            RightRec = new Rectangle((int)CurrentPos.X + (Texture.Width), (int)CurrentPos.Y + (Texture.Width / 2), 1, 1);
            WholeRec = new Rectangle((int)CurrentPos.X, (int)CurrentPos.Y, Texture.Width, Texture.Height);
        }

        public void Update(Vector2 startingPos)
        {
            StartingPosition = startingPos;

                if (Move == false)
                {
                    CurrentPos = StartingPosition;
                }
                else if (Move == true)
                {
                    CurrentPos += Direction;

                    TopRec.X = (int)CurrentPos.X + (Texture.Width / 2);
                    TopRec.Y = (int)CurrentPos.Y;

                    BottomRec.X = (int)CurrentPos.X + (Texture.Width / 2);
                    BottomRec.Y = (int)CurrentPos.Y + Texture.Height;

                    LeftRec.X = (int)CurrentPos.X;
                    LeftRec.Y = (int)CurrentPos.Y + (Texture.Height / 2);

                    RightRec.X = (int)CurrentPos.X + (Texture.Width);
                    RightRec.Y = (int)CurrentPos.Y + (Texture.Width / 2);

                    WholeRec.X = (int)CurrentPos.X;
                    WholeRec.Y = (int)CurrentPos.Y;

                    if (CurrentPos.X < 0)
                    {
                        PreviousPos = CurrentPos;
                        Direction *= new Vector2(-1, 1);

                    }
                    if (CurrentPos.X > (700 - Texture.Width))
                    {
                        PreviousPos = CurrentPos;
                        Direction *= new Vector2(-1, 1);

                    }
                    if (CurrentPos.Y < 0)
                    {
                        PreviousPos = CurrentPos;
                        Direction *= new Vector2(1, -1);
                    }
                }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, CurrentPos, Color.White);
        }

                
    }
}
