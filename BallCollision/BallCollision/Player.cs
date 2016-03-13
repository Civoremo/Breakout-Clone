using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BallCollision
{
    public class Player
    {
        // player variables
        public Vector2 Position;
        public Vector2 Origin;
        public float Speed;
        public Texture2D Texture;

        public int SmallSize;
        public int SideSize;
        public int MiddleSize;

        public Rectangle BoundingBox;

        // aim variables
        public Texture2D aimTexture;
        public Vector2 aimPosition;
        public Vector2 aimOrigin;
        public float aimSpeed;


        public Player()
        {
            // player
            Position = new Vector2(300, 700);
            Speed = 7f;
            Texture = null;

            // aim
            aimTexture = null;
            aimPosition = new Vector2(300, Position.Y - 100);
            aimSpeed = 3f;
            
        }

        public void LoadContent(ContentManager Content)
        {
            // player 
            Texture = Content.Load<Texture2D>("paddleBlu");
            Origin = new Vector2(Position.X + Texture.Width / 2, Position.Y);        // finds the center of player and sets it to variable ORIGIN 

            SmallSize = Texture.Width / 12;
            SideSize = Texture.Width / 4;
            MiddleSize = Texture.Width / 3;

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            // aim
            aimTexture = Content.Load<Texture2D>("ballBlue");
            aimOrigin = new Vector2(aimPosition.X + aimTexture.Width / 2, Position.Y - 100);

        }

        public void Update()
        {
            PlayerInput();
            AimInput();

            Origin = new Vector2(Position.X + Texture.Width / 2, Position.Y);
            aimOrigin = new Vector2(aimPosition.X + aimTexture.Width / 2, Position.Y - 100);
            
            //BoundingBox.X = (int)Position.X;
            //BoundingBox.Y = (int)Position.Y;

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.Draw(aimTexture, aimPosition, Color.White);
        }


        // takes player input for player controlled paddle
        public void PlayerInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -0.1f))
            {
                Position.X -= Speed;
                if (Position.X > 0)
                aimPosition.X -= Speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 0.1f))
            {
                Position.X += Speed;
                if (Position.X + Texture.Width < 700)
                aimPosition.X += Speed;
            }

            // if the player paddle is leaving the screen
            if (Position.X <= 0)
            {
                Position.X = 0;
            }
            if (Position.X >= (700 - Texture.Width))
            {
                Position.X = (700 - Texture.Width);
            }

            if (aimOrigin.X <= Position.X - 50)
            {
                aimOrigin.X = Position.X - 50 + aimTexture.Width / 2;
            }
            if (aimOrigin.X >= Position.X + Texture.Width + 50)
            {
                aimOrigin.X = Position.X + Texture.Width + 50 + aimTexture.Width / 2;
            }
        }


        // aim input
        public void AimInput()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.A) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X <= -0.5f))
            {
                if (aimPosition.X >= Position.X - 50)
                aimPosition.X -= aimSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X >= 0.5f))
            {
                if (aimPosition.X <= Position.X + Texture.Width + 50)
                aimPosition.X += aimSpeed;
            }
        }
    }
}
