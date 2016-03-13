using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BallCollision
{
    public class Balls
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public int TexWidth, TexHeight;
        
        public Rectangle TopRec { get; set; }
        public Rectangle BottomRec { get; set; }
        public Rectangle LeftRec { get; set; }
        public Rectangle RightRec { get; set; }
        public Rectangle WholeRec { get; set; }

        public float Speed = 8f;
        public Vector2 Direction;
        public bool Move = false;


        public Balls(Texture2D texture, Vector2 position, Rectangle top, Rectangle bottom, Rectangle left, Rectangle right, Rectangle whole, bool active, Vector2 direction)
        {
            Texture = texture;
            Position = position;

            TexWidth = Texture.Width;
            TexHeight = Texture.Height;

            Move = active;
            Direction = direction * Speed;

            TopRec = top;
            BottomRec = bottom;
            LeftRec = left;
            RightRec = right;
            WholeRec = whole;
            
            
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
