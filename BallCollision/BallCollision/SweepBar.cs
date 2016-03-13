using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BallCollision
{
    public class SweepBar
    {
        public Vector2 Position;
        public Vector2 startingPosition;
        public Texture2D Texture;
        public float Speed;
        public Rectangle BoundingBox;

        public bool Move;
        


        public SweepBar()
        {
            startingPosition = new Vector2(0, 750);
            Texture = null;
            Move = false;
        }

        public void LoadContent(ContentManager content)
        {
            Position = startingPosition;
            Texture = content.Load<Texture2D>("sweepBar");

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Update()
        {
            if (Move == true)
            {
                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
                //BoundingBox.X = (int)Position.X;
                //BoundingBox.Y = (int)Position.Y;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
