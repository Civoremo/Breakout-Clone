using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BallCollision
{
    public class Aim
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Origin;
        public float Speed;

        public Aim()
        {
            Texture = null;
            Position = new Vector2(0, 0);
            Speed = 3f;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("ballBlue");

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public void Update(Vector2 position)
        {
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
                        
    }
}
