using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BallCollision
{
    public class PowerUp
    {

        public Texture2D PowerUpTexture;
        public bool PowActive;
        public Rectangle PowerBox;
        public List<PowerUp> powerUps;
        public float PowerSpeed;

        public Vector2 PowerPosition;
        
        // public float PowerTime;


        public PowerUp()
        {
            this.powerUps = new List<PowerUp>();
            PowerUpTexture = null;
            
            PowerPosition = Vector2.Zero;           

        }

        public PowerUpBrick PowerUpGenerate(Vector2 position, bool active)
        {
            Texture2D texture = PowerUpTexture;
            PowerPosition = position;
            PowActive = active;
            PowerSpeed = 3f;

            PowerBox = new Rectangle((int)PowerPosition.X, (int)PowerPosition.Y, PowerUpTexture.Width, PowerUpTexture.Height);
            
            return new PowerUpBrick(texture, PowerPosition, PowActive, PowerBox);
        }


        public void LoadContent(ContentManager Content)
        {
            PowerUpTexture = Content.Load<Texture2D>("powerup");
        }

        public void Update()
        {
            for (int i = 0; i < powerUps.Count; i++)
            {
                if (powerUps[i].PowActive == true)
                {
                    powerUps[i].PowerPosition.Y += powerUps[i].PowerSpeed;
                    //PowerBox.Y = (int)PowerPosition.Y;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
                
    }
}
