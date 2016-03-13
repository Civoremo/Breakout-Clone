using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BallCollision
{
    public class Brick
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public int TexWidth, TexHeight;

        public bool isActive { get; set; }
        public int HitCount { get; set; }

        public Rectangle BoundingBox { get; set; }


        public Brick(Texture2D texture, Vector2 position, bool active, Rectangle boundingBox, int hitCount)
        {
            Texture = texture;
            Position = position;
            isActive = active;
            HitCount = hitCount;

            TexWidth = Texture.Width;
            TexHeight = Texture.Height;

            BoundingBox = boundingBox;

            
        }



        public void Update()
        {
                      

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);


        }



    }
}
