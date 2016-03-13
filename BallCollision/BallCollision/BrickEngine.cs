using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BallCollision
{
    public class BrickEngine
    {
        private Random random;
        public List<Brick> bricks;
        private List<Texture2D> textures;
        public Rectangle BoundingBox;

        private Vector2 StartingPosition = new Vector2(60, 100);
        private Vector2 Position;
        private int OffSetXDirection;
        private int OffSetYDirection;


        public List<PowerUpBrick> powerUps;
        public Rectangle PowerBoundBox;
        public Vector2 PowerPosition;
        private List<Texture2D> powerTextures;

        private float PowerSpeed;


        public List<WavePowerup> wavePowerUps;
        public Texture2D WaveTexture;
        public Rectangle WaveBoundingBox;
        public Vector2 WavePosition;

        private float waveSpeed;


        public List<PermaBrick> permamentBricks;
        public Texture2D PermaBrickTexture;
        public Rectangle PermaBrickBoundingBox;
        public Vector2 PermaBrickPosition;


        public BrickEngine(List<Texture2D> textures, List<Texture2D> powerTex, Texture2D waveTex, Texture2D permaTex)
        {
            this.textures = textures;
            this.powerTextures = powerTex;
            this.WaveTexture = waveTex;
            this.PermaBrickTexture = permaTex;
            this.bricks = new List<Brick>();
            this.powerUps = new List<PowerUpBrick>();
            this.wavePowerUps = new List<WavePowerup>();
            this.permamentBricks = new List<PermaBrick>();

            random = new Random();

            Position = Vector2.Zero;

            PowerSpeed = 3f;
            waveSpeed = 4f;
        }

        private Brick BrickGenerator(Vector2 position, bool Active)
        {
            Position = position;
            Texture2D texture = null;
            int Hits;

            if (random.Next(1, 25) == 5)
            {
                texture = textures[3];  // exploding brick
                Hits = 1;
            }
            else if (random.Next(1, 20) == 7)
            {
                texture = textures[2];
                Hits = 3;
            }
            else
            {
                texture = textures[random.Next(0, 2)];
                Hits = 1;
            }

            OffSetXDirection = texture.Width;
            OffSetYDirection = texture.Height;

            bool active = Active;

            if (active == false)
            {
                Hits = 0;
            }
            

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);

            return new Brick(texture, Position, active, BoundingBox, Hits);
            

        }

        private PowerUpBrick PowerUpGenerator(Vector2 position, bool active)
        {
            PowerPosition = position;
            bool Active = active;

            Texture2D Textures = powerTextures[random.Next(powerTextures.Count)];

            PowerBoundBox = new Rectangle((int)PowerPosition.X, (int)PowerPosition.Y, Textures.Width, Textures.Height);

            return new PowerUpBrick(Textures, PowerPosition, Active, PowerBoundBox);
        }


        private WavePowerup WavePowerupGenerator(Vector2 position, bool active)
        {
            WavePosition = position;
            bool Active = active;

            Texture2D Texture = WaveTexture;

            WaveBoundingBox = new Rectangle((int)WavePosition.X, (int)WavePosition.Y, Texture.Width, Texture.Height);

            return new WavePowerup(Texture, WavePosition, Active, WaveBoundingBox);
        }


        private PermaBrick PermamentBrickGenerator(Vector2 position, bool active)
        {
            PermaBrickPosition = position;
            bool Active = active;

            Texture2D Texture = PermaBrickTexture;

            PermaBrickBoundingBox = new Rectangle((int)PermaBrickPosition.X, (int)PermaBrickPosition.Y, Texture.Width, Texture.Height);

            return new PermaBrick(Texture, PermaBrickPosition, Active, PermaBrickBoundingBox);
        }



        public void Update(int number)
        {
            int Number = number;

            for (int n = 0; n < Number; n++)
            {
                BrickBuild();
            }
            
            for (int brick = 0; brick < bricks.Count; brick++)
            {
                //bricks[brick].Update();
                if (bricks[brick].HitCount <= 0)
                {
                    for (int powIndex = 0; powIndex < powerUps.Count; powIndex++)
                    {
                        if (bricks[brick].Position == powerUps[powIndex].Position)
                        {
                            powerUps[powIndex].startMoving = true;
                        }
                    }
                    bricks.RemoveAt(brick);
                }
            }

            for (int j = 0; j < powerUps.Count; j++)
            {
                if (powerUps[j].startMoving == true)
                {
                    powerUps[j].Position = new Vector2(powerUps[j].Position.X, powerUps[j].Position.Y + PowerSpeed);
                    powerUps[j].BoundingBox = new Rectangle((int)powerUps[j].Position.X, (int)powerUps[j].Position.Y, powerUps[j].Texture.Width, powerUps[j].Texture.Height);
                }
                if (powerUps[j].Position.Y > 800 || powerUps[j].isActive == false)
                {
                    powerUps.RemoveAt(j);
                }

            }


            for (int wave = 0; wave < wavePowerUps.Count; wave++)
            {
                wavePowerUps[wave].Update();
                if (wavePowerUps[wave].isActive == true)
                {
                    wavePowerUps[wave].Position = new Vector2(wavePowerUps[wave].Position.X, wavePowerUps[wave].Position.Y - waveSpeed);
                    wavePowerUps[wave].BoundingBox = new Rectangle((int)wavePowerUps[wave].Position.X, (int)wavePowerUps[wave].Position.Y, wavePowerUps[wave].Texture.Width, wavePowerUps[wave].Texture.Height);
                }
                if (wavePowerUps[wave].Position.Y < -10 || wavePowerUps[wave].isActive == false)
                {
                    wavePowerUps.RemoveAt(wave);
                }
            }

        }



        public void Draw(SpriteBatch spriteBatch)
        {
            
                for (int m = 0; m < bricks.Count; m++)
                {
                    for (int perma = 0; perma < permamentBricks.Count; perma++)
                    {
                            permamentBricks[perma].Draw(spriteBatch);
                    }
                    for (int n = 0; n < powerUps.Count; n++)
                    {
                        if (powerUps[n].startMoving == true)
                        {
                            powerUps[n].Draw(spriteBatch);
                        }
                        else
                        {
                            bricks[m].Draw(spriteBatch);
                        }
                    }
                    bricks[m].Draw(spriteBatch);
                }
            
            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {

                
                for (int index = 0; index < bricks.Count; index++)
                {
                    bricks[index].Draw(spriteBatch);
                }
                for (int n = 0; n < powerUps.Count; n++)
                {
                    powerUps[n].Draw(spriteBatch);
                }
                for (int perma = 0; perma < permamentBricks.Count; perma++)
                {
                    permamentBricks[perma].Draw(spriteBatch);
                }
                 
            }

            for (int index = 0; index < wavePowerUps.Count; index++)
            {
                wavePowerUps[index].Draw(spriteBatch);
            }
        }


        public void BrickBuild()
        {
            int Column = 9;
            int Row = 9;
            int countOffset = 0;
            int randomNum;
            int randomPowNum;
            int randomPermaNum;

            Position = StartingPosition;

            for (int i = 0; i < Column; i++)
            {
                countOffset++;

                for (int a = 0; a < Row; a++)
                {
                    randomNum = random.Next(1, 5);
                    randomPowNum = random.Next(1, 15);

                    if (randomNum != 1)
                    {
                        bool Active = true;

                            bricks.Add(BrickGenerator(Position, Active));

                            if (randomPowNum == 7)
                            {
                                bool pActive = true;
                                powerUps.Add(PowerUpGenerator(Position, pActive));
                            }
                    }
                    else 
                    {
                        bool Active = false;
                        bricks.Add(BrickGenerator(Position, Active));

                        randomPermaNum = random.Next(1, 7);
                        if (randomPermaNum == 2)
                        {
                            bool permaActive = true;
                            permamentBricks.Add(PermamentBrickGenerator(Position, permaActive));
                        }
                    }

                    Position += new Vector2(OffSetXDirection, 0);
                }
                    Position = StartingPosition;
                    Position += new Vector2(0, (OffSetYDirection * countOffset));
            }
        }


        public void WaveMaker(int w)
        {
            int W = 0;
            W += w;
            int numWaves = 12;
            Vector2 wavePos = new Vector2(0, 800);

            if (W > 0)
            {
                for (int index = 0; index < numWaves; index++)
                {
                    bool active = true;
                    wavePowerUps.Add(WavePowerupGenerator(wavePos, active));
                    wavePos += new Vector2(OffSetXDirection, 0);
                }
                W--;
                
            }
        }
    }
}
