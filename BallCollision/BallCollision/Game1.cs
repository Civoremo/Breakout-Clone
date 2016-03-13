using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BallCollision
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public int score = 0;
        public int scoreMultiply = 0;
        public int basePoints = 50;
        public int points;
        public float currentTime;

        public int X = 1;
        public int Y = 1;
        public Vector2 StartingPosition;

        Ball ball = new Ball();
        BallsEngine BallsList;
        Texture2D ballsTexture;
        Player player = new Player();
        BrickEngine BrickLvl;

        public double radian;
        public int angle;
        public Vector2 angleDir;

        SpriteFont scoreFont;

        List<Texture2D> brickTexture = new List<Texture2D>();
        List<Texture2D> powerTexture = new List<Texture2D>();

        int currentTex;
        bool isSweepBar = false;
        Texture2D SweepBarTexture;

        public Texture2D playerTextureSmall;
        public Texture2D playerTextureBig;
        public Texture2D playerTextureNormal;

        public Texture2D textureWave;
        public Texture2D permaBrickTex;

        float playerTimer = 0;
        bool playerClock = false;

        bool ballCreateActive = false;

        Texture2D MainMenuTexture;
        Texture2D GameOverTexture;

        bool isExploding = false;
        Texture2D ExplosionTexture;
        Vector2 ExplosionPosition;

        bool isMainMenu;
        bool isGameOver;
        bool isGamePlaying;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 700;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.LoadContent(Content);
            ball.LoadContent(Content);

            scoreFont = Content.Load<SpriteFont>("Verdana");
            
            playerTextureSmall = Content.Load<Texture2D>("smallpaddleBlu");
            playerTextureNormal = Content.Load<Texture2D>("paddleBlu");
            playerTextureBig = Content.Load<Texture2D>("bigpaddleBlu");

            textureWave = Content.Load<Texture2D>("WavePU");
            permaBrickTex = Content.Load<Texture2D>("SolidBrick");
            ExplosionTexture = Content.Load<Texture2D>("explosion");
            SweepBarTexture = Content.Load<Texture2D>("sweepBar");

            brickTexture.Add(Content.Load<Texture2D>("greenBrick"));
            brickTexture.Add(Content.Load<Texture2D>("redBrick"));
            brickTexture.Add(Content.Load<Texture2D>("purpleBrick"));
            brickTexture.Add(Content.Load<Texture2D>("BoomBrick"));
            brickTexture.Add(Content.Load<Texture2D>("purpleBrickCrack"));
            brickTexture.Add(Content.Load<Texture2D>("purpleBrickBroken"));

            powerTexture.Add(Content.Load<Texture2D>("powerupTime"));   // 0    safety net
            powerTexture.Add(Content.Load<Texture2D>("powerupLife"));   // 1    extra life
            powerTexture.Add(Content.Load<Texture2D>("powerupWave"));   // 2    wave destroying the firt thing it collides with
            powerTexture.Add(Content.Load<Texture2D>("powerupBig"));    // 3    make paddle bigger
            powerTexture.Add(Content.Load<Texture2D>("powerupSmall"));  // 4    make paddle smaller
            powerTexture.Add(Content.Load<Texture2D>("ExtraBallBrick"));    // 5 add extra balls into play


            ballsTexture = Content.Load<Texture2D>("ballGrey");
            BrickLvl = new BrickEngine(brickTexture, powerTexture, textureWave, permaBrickTex);
            BallsList = new BallsEngine(ballsTexture);


            MainMenuTexture = Content.Load<Texture2D>("MainMenu");
            GameOverTexture = Content.Load<Texture2D>("GameOver");

            isMainMenu = true;
            isGameOver = false;
            isGamePlaying = false;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // managing gamestates
            if (isMainMenu)
            {
                UpdateMainMenu();
            }
            else if (isGamePlaying)
            {
                UpdateGamePlaying();
            }
            else if (isGameOver)
            {
                UpdateGameOver();
            }

            // Timers and such
                

                if (playerClock == true)
                {
                    playerTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (playerTimer <= 0f)
                    {
                        player.Texture = playerTextureNormal;
                        playerClock = false;
                    }
                }

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (isMainMenu)
            {
                DrawMainMenu();
            }
            else if (isGamePlaying)
            {
                DrawGamePlaying();
            }
            else if (isGameOver)
            {
                DrawGameOver();
            }

            if (isExploding == true)
            {
                spriteBatch.Draw(ExplosionTexture, new Vector2(ExplosionPosition.X - 96, ExplosionPosition.Y - 48), Color.White);
            }

            if (isSweepBar == true)
            {
                spriteBatch.Draw(SweepBarTexture, new Vector2(0, 750), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        // SweepSafetyNet
        public void SweepBarNet()
        {
            if (isSweepBar == true)
            {
                Rectangle SweepBoundingBox = new Rectangle(0, 750, SweepBarTexture.Width, SweepBarTexture.Height);
                if (ball.WholeRec.Intersects(SweepBoundingBox))
                {
                    ball.Direction *= new Vector2(1, -1);
                    isSweepBar = false;
                }

                    for (int x = 0; x < BallsList.ballsList.Count; x++)
                    {
                        if (BallsList.ballsList[x].WholeRec.Intersects(SweepBoundingBox))
                        {
                            BallsList.ballsList[x].Direction *= new Vector2(1, -1);
                            isSweepBar = false;
                            break;
                        }
                    }
            }
        }

        // exploding power up brick
        public void ExplodingBrick()
        {
            Vector2 Position = ExplosionPosition;

            Rectangle explodingBox = new Rectangle((int)Position.X - 96, (int)Position.Y - 48, 256, 128);

            if (isExploding == true)
            {

                for (int x = 0; x < BrickLvl.bricks.Count; x++)
                {
                    if (explodingBox.Intersects(BrickLvl.bricks[x].BoundingBox))
                    {
                        if (BrickLvl.bricks[x].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[x].Position;
                        }
                        BrickLvl.bricks.RemoveAt(x);
                        for (int y = 0; y < BrickLvl.bricks.Count; y++)
                        {
                            if (explodingBox.Intersects(BrickLvl.bricks[y].BoundingBox))
                            {
                                if (BrickLvl.bricks[y].Texture == brickTexture[3])
                                {
                                    isExploding = true;
                                    ExplosionPosition = BrickLvl.bricks[y].Position;
                                }
                                BrickLvl.bricks.RemoveAt(y);
                            }
                            //BrickLvl.Update(0);
                        }
                    }
                    //BrickLvl.Update(0);
                }
                BrickLvl.Update(0);
                isExploding = false;
            }
        }

        // collision with premamanet bricks
        public void BallPermaBrickCollision()
        {
            if (ball.Move == true)
            {
                for (int num = 0; num < BrickLvl.permamentBricks.Count; num++)
                {
                        if (ball.WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && ball.LeftRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                        {
                            while (ball.CurrentPos.X < BrickLvl.permamentBricks[num].Position.X + BrickLvl.permamentBricks[num].Texture.Width)
                            {
                                ball.CurrentPos.X += .1f;
                            }

                            ball.Direction *= new Vector2(-1, 1);
                            break;
                        }
                        if (ball.WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && ball.RightRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                        {
                            while (ball.CurrentPos.X + ball.Texture.Width > BrickLvl.permamentBricks[num].Position.X)
                            {
                                ball.CurrentPos.X -= .1f;
                            }

                            ball.Direction *= new Vector2(-1, 1);
                            break;
                        }

                        if (ball.WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && ball.TopRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                        {
                            while (ball.CurrentPos.Y < BrickLvl.permamentBricks[num].Position.Y + BrickLvl.permamentBricks[num].Texture.Height)
                            {
                                ball.CurrentPos.Y += .1f;
                            }

                            ball.Direction *= new Vector2(1, -1);
                            break;
                        }

                        if (ball.WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && ball.BottomRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                        {
                            while (ball.CurrentPos.Y + ball.Texture.Height > BrickLvl.permamentBricks[num].Position.Y)
                            {
                                ball.CurrentPos.Y -= .1f;
                            }

                            ball.Direction *= new Vector2(1, -1);
                            break;
                        }
                }
                
            }

            if (BallsList.ballsList.Count > 0)
            {
                for (int num = 0; num < BrickLvl.permamentBricks.Count; num++)
                {
                    for (int x = 0; x < BallsList.ballsList.Count; x++)
                    {

                            if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && BallsList.ballsList[x].LeftRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                            {
                                while (BallsList.ballsList[x].Position.X < BrickLvl.permamentBricks[num].Position.X + BrickLvl.permamentBricks[num].Texture.Width)
                                {
                                    BallsList.ballsList[x].Position += new Vector2(.1f, 0);
                                }

                                BallsList.ballsList[x].Direction *= new Vector2(-1, 1);
                                break;
                            }
                            if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && BallsList.ballsList[x].RightRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                            {
                                while (BallsList.ballsList[x].Position.X + ball.Texture.Width > BrickLvl.permamentBricks[num].Position.X)
                                {
                                    BallsList.ballsList[x].Position -= new Vector2(.1f, 0);
                                }

                                BallsList.ballsList[x].Direction *= new Vector2(-1, 1);
                                break;
                            }

                            if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && BallsList.ballsList[x].TopRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                            {
                                while (BallsList.ballsList[x].Position.Y < BrickLvl.permamentBricks[num].Position.Y + BrickLvl.permamentBricks[num].Texture.Height)
                                {
                                    BallsList.ballsList[x].Position += new Vector2(0, .1f);
                                }

                                BallsList.ballsList[x].Direction *= new Vector2(1, -1);
                                break;
                            }

                            if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox) && BallsList.ballsList[x].BottomRec.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                            {
                                while (BallsList.ballsList[x].Position.Y + ball.Texture.Height > BrickLvl.permamentBricks[num].Position.Y)
                                {
                                    BallsList.ballsList[x].Position -= new Vector2(0, .1f);
                                }

                                BallsList.ballsList[x].Direction *= new Vector2(1, -1);
                                break;
                            }
                    }
                }
            }
        }

        // collision with extra balls
        public void PowerBallsCollision()
        {
            for (int x = 0; x < BallsList.ballsList.Count; x++)
            {

                for (int y = 0; y < BrickLvl.bricks.Count; y++)
                {
                    BrickLvl.bricks[y].Update();

                    if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.bricks[y].BoundingBox) && BallsList.ballsList[x].LeftRec.Intersects(BrickLvl.bricks[y].BoundingBox))
                    {
                        while (x != 0 && BallsList.ballsList[x].Position.X < BrickLvl.permamentBricks[y].Position.X + BrickLvl.permamentBricks[y].Texture.Width)
                        {
                            BallsList.ballsList[x].Position += new Vector2(.1f, 0);
                        }

                        BallsList.ballsList[x].Direction *= new Vector2(-1, 1);

                        if (BrickLvl.bricks[y].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[y].Position;
                        }

                        BrickLvl.bricks[y].HitCount -= 1;

                        if (BrickLvl.bricks[y].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[y].Texture == brickTexture[4] && BrickLvl.bricks[y].HitCount == 1)
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
                    if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.bricks[y].BoundingBox) && BallsList.ballsList[x].RightRec.Intersects(BrickLvl.bricks[y].BoundingBox))
                    {
                        while (x != 0 && BallsList.ballsList[x].Position.X + ball.Texture.Width > BrickLvl.permamentBricks[y].Position.X)
                        {
                            BallsList.ballsList[x].Position -= new Vector2(.1f, 0);
                        }

                        BallsList.ballsList[x].Direction *= new Vector2(-1, 1);

                        if (BrickLvl.bricks[y].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[y].Position;
                        }

                        BrickLvl.bricks[y].HitCount -= 1;

                        if (BrickLvl.bricks[y].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[y].Texture == brickTexture[4] && BrickLvl.bricks[y].HitCount == 1)
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
                    if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.bricks[y].BoundingBox) && BallsList.ballsList[x].TopRec.Intersects(BrickLvl.bricks[y].BoundingBox))
                    {
                        while (x != 0 && BallsList.ballsList[x].Position.Y < BrickLvl.permamentBricks[y].Position.Y + BrickLvl.permamentBricks[y].Texture.Height)
                        {
                            BallsList.ballsList[x].Position += new Vector2(0, .1f);
                        }

                        BallsList.ballsList[x].Direction *= new Vector2(1, -1);

                        if (BrickLvl.bricks[y].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[y].Position;
                        }

                        BrickLvl.bricks[y].HitCount -= 1;

                        if (BrickLvl.bricks[y].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[y].Texture == brickTexture[4] && BrickLvl.bricks[y].HitCount == 1)
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
                    if (BallsList.ballsList[x].WholeRec.Intersects(BrickLvl.bricks[y].BoundingBox) && BallsList.ballsList[x].BottomRec.Intersects(BrickLvl.bricks[y].BoundingBox))
                    {
                        while (x != 0 && BallsList.ballsList[x].Position.Y + ball.Texture.Height > BrickLvl.permamentBricks[y].Position.Y)
                        {
                            BallsList.ballsList[x].Position -= new Vector2(0, .1f);
                        }

                        BallsList.ballsList[x].Direction *= new Vector2(1, -1);

                        if (BrickLvl.bricks[y].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[y].Position;
                        }

                        BrickLvl.bricks[y].HitCount -= 1;

                        if (BrickLvl.bricks[y].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[y].Texture == brickTexture[4] && BrickLvl.bricks[y].HitCount == 1)
                        {
                            BrickLvl.bricks[y].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
                }
            }
        }

        // collision between ball and brick
        public void BallBrickCollision()
        {
            for (int i = 0; i < BrickLvl.bricks.Count; i++)
            {
                BrickLvl.bricks[i].Update();

                    if (ball.WholeRec.Intersects(BrickLvl.bricks[i].BoundingBox) && ball.LeftRec.Intersects(BrickLvl.bricks[i].BoundingBox))
                    {
                        while (ball.CurrentPos.X < BrickLvl.bricks[i].Position.X + BrickLvl.bricks[i].Texture.Width)
                        {
                            ball.CurrentPos.X += .1f;
                        }

                        ball.Direction *= new Vector2(-1, 1);

                        if (BrickLvl.bricks[i].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[i].Position;
                        }

                        BrickLvl.bricks[i].HitCount -= 1;

                        if (BrickLvl.bricks[i].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[i].Texture == brickTexture[4] && BrickLvl.bricks[i].HitCount == 1)
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
                    if (ball.WholeRec.Intersects(BrickLvl.bricks[i].BoundingBox) && ball.RightRec.Intersects(BrickLvl.bricks[i].BoundingBox))
                    {
                        while (ball.CurrentPos.X + ball.Texture.Width > BrickLvl.bricks[i].Position.X)
                        {
                            ball.CurrentPos.X -= .1f;
                        }
                        
                        ball.Direction *= new Vector2(-1, 1);

                        if (BrickLvl.bricks[i].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[i].Position;
                        }

                        BrickLvl.bricks[i].HitCount -= 1;

                        if (BrickLvl.bricks[i].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[i].Texture == brickTexture[4] && BrickLvl.bricks[i].HitCount == 1)
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
                    if (ball.WholeRec.Intersects(BrickLvl.bricks[i].BoundingBox) && ball.TopRec.Intersects(BrickLvl.bricks[i].BoundingBox))
                    {
                        while (ball.CurrentPos.Y < BrickLvl.bricks[i].Position.Y + BrickLvl.bricks[i].Texture.Height)
                        {
                            ball.CurrentPos.Y += .1f;
                        }
                        
                        ball.Direction *= new Vector2(1, -1);

                        if (BrickLvl.bricks[i].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[i].Position;
                        }

                        BrickLvl.bricks[i].HitCount -= 1;

                        if (BrickLvl.bricks[i].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[i].Texture == brickTexture[4] && BrickLvl.bricks[i].HitCount == 1)
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
                    if (ball.WholeRec.Intersects(BrickLvl.bricks[i].BoundingBox) && ball.BottomRec.Intersects(BrickLvl.bricks[i].BoundingBox))
                    {
                        while (ball.CurrentPos.Y + ball.Texture.Height > BrickLvl.bricks[i].Position.Y)
                        {
                            ball.CurrentPos.Y -= .1f;
                        }
                        
                        ball.Direction *= new Vector2(1, -1);

                        if (BrickLvl.bricks[i].Texture == brickTexture[3])
                        {
                            isExploding = true;
                            ExplosionPosition = BrickLvl.bricks[i].Position;
                        }

                        BrickLvl.bricks[i].HitCount -= 1;

                        if (BrickLvl.bricks[i].Texture == brickTexture[2])
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[4];
                        }
                        if (BrickLvl.bricks[i].Texture == brickTexture[4] && BrickLvl.bricks[i].HitCount == 1)
                        {
                            BrickLvl.bricks[i].Texture = brickTexture[5];
                        }

                        score += points;
                        break;
                    }
             }

        }

        // collision between ball and player paddle
        public void BallPaddleCollision()
        {
            if (ball.Move == true)
            {
                if (ball.WholeRec.Intersects(player.BoundingBox))
                {
                    while (ball.CurrentPos.Y > player.Position.Y)
                    {
                        ball.CurrentPos.Y -= .1f;
                    }

                    ball.Direction = angleDir * ball.Speed;
                }
            }

            for (int s = 0; s < BallsList.ballsList.Count; s++)
            {
                if (BallsList.ballsList[s].WholeRec.Intersects(player.BoundingBox))
                {
                    while (BallsList.ballsList[s].Position.Y > player.Position.Y)
                    {
                        BallsList.ballsList[s].Position -= new Vector2(0, .1f);
                    }

                    BallsList.ballsList[s].Direction = angleDir * BallsList.Speed;
                }
            }
        }

        // sets the initial positions and waits for player to press button to start ACTION
        public void StartButton()
        {
                StartingPosition = new Vector2((player.Origin.X - ball.Texture.Width / 2), (player.Position.Y - ball.Texture.Height));

                if (ball.Move == false && Keyboard.GetState().IsKeyDown(Keys.Space) || (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 0.5f))
                {
                    ball.Direction = angleDir * ball.Speed;
                    ball.Move = true;
                }

            if (BrickLvl.bricks.Count <= 0)
            {
                
                for (int b = 0; b < BrickLvl.powerUps.Count; b++)
                {
                    if (BrickLvl.powerUps[b].isActive == true)
                        BrickLvl.powerUps.RemoveAt(b);
                }
                for (int c = 0; c < BrickLvl.wavePowerUps.Count; c++)
                {
                    if (BrickLvl.wavePowerUps[c].isActive == true)
                        BrickLvl.wavePowerUps.RemoveAt(c);
                }
                for (int d = 0; d < BrickLvl.permamentBricks.Count; d++)
                {
                    if (BrickLvl.permamentBricks[d].isActive == true)
                        BrickLvl.permamentBricks.RemoveAt(d);
                }
                for (int e = 0; e < BallsList.ballsList.Count; e++)
                {
                        BallsList.ballsList.RemoveAt(e);
                }
                StartingPosition = new Vector2((player.Origin.X - ball.Texture.Width / 2), (player.Position.Y - ball.Texture.Height));
                ball.Move = false;
                X = 1;
                scoreMultiply++;
                points = basePoints * scoreMultiply;
                ball.Speed += .5f;
                player.Speed += .3f;

                /*
                if (scoreMultiply <= 5)
                {
                    ball.Speed += 0.5f;
                }
                if (scoreMultiply > 5 && scoreMultiply <= 10)
                {
                    ball.Speed += 0.2f;
                }
                if (scoreMultiply > 10 && scoreMultiply <= 15)
                {
                    ball.Speed += 0.1f;
                }

                /*if (ball.Speed <= 9f)
                {
                    ball.Speed += 1f;
                }
                if (player.Speed <= 10f)
                { 
                    player.Speed += .5f;
                }*/
            }
        }

        // did the ball miss the player and hit the bottom of the screen
        public void HitTheBottom()
        {
            // declares a position, which is stored in the variable STARTINGPOSITION
            //StartingPosition = new Vector2((player.Position.X + player.Texture.Width / 2) - newBall.Texture.Width / 2, (player.Position.Y - newBall.Texture.Height));

            // if the ball touches the bottom, reset ball position to player position and wait for Space Bar input
            if (ball.CurrentPos.Y > (800 - ball.Texture.Height) && BallsList.ballsList.Count == 0)
            {
                    ball.Move = false;
                    ball.Lives -= 1;
                    StartingPosition = new Vector2((player.Origin.X - ball.Texture.Width / 2), (player.Position.Y - ball.Texture.Height));
            }
        }

        // what is the aim direction for the ball after it collides with the player paddle
        public void AngleToVector()
        {
            angleDir = new Vector2(player.aimOrigin.X - (player.Origin.X), player.aimPosition.Y - player.Origin.Y);
            angleDir.Normalize();
            /*
            radian = Math.Atan2((player.aimPosition.Y - player.Position.Y ) , ((player.Position.X + player.Texture.Width /2) - player.aimPosition.X));
            angle = (int)(radian * (180 / Math.PI));
            angleDir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
             */
        }

        // is the falling power brick colliding with the player paddle
        public void PowerPaddleCollide()
        {
                for (int s = 0; s < BrickLvl.powerUps.Count; s++)
                {
                    BrickLvl.powerUps[s].Update();
                    if (BrickLvl.powerUps[s].BoundingBox.Intersects(player.BoundingBox))
                    {
                        BrickLvl.powerUps[s].isActive = false;

                        for (int num = 0; num < powerTexture.Count; num++)
                        {
                            if (BrickLvl.powerUps[s].Texture == powerTexture[num])
                            {
                                currentTex = num;

                                // Life powerup
                                if (num == 1)
                                {
                                    ball.Lives += 1;
                                }

                                if (num == 0)
                                {
                                    isSweepBar = true;
                                }

                                // small paddle powerup
                                if (num == 4)
                                {
                                    player.Texture = playerTextureSmall;
                                    playerTimer = 10f;
                                    playerClock = true;
                                }

                                // big paddle powerup
                                if (num == 3)
                                {
                                    player.Texture = playerTextureBig;
                                    playerTimer = 10f;
                                    playerClock = true;
                                }

                                // wave powerup
                                if (num == 2)
                                {
                                    BrickLvl.WaveMaker(1);
                                }

                                // extra balls powerup
                                if (num == 5)
                                {
                                    ballCreateActive = true;
                                }
                            }
                        }
                    }
                }
        }

 
        // wave collision detection
        public void WaveCollide()
        {
            for (int x = 0; x < BrickLvl.wavePowerUps.Count; x++)
            {
                for (int y = 0; y < BrickLvl.bricks.Count; y++)
                {
                    if (BrickLvl.wavePowerUps[x].BoundingBox.Intersects(BrickLvl.bricks[y].BoundingBox))
                    {
                        BrickLvl.bricks[y].HitCount -= 1;
                        BrickLvl.wavePowerUps.RemoveAt(x);
                    }
                    
                }

                for (int num = 0; num < BrickLvl.permamentBricks.Count; num++)
                {
                    if (BrickLvl.wavePowerUps[x].BoundingBox.Intersects(BrickLvl.permamentBricks[num].BoundingBox))
                    {
                        BrickLvl.wavePowerUps.RemoveAt(x);
                    }
                }
            }
            
        }


        // GameState Update State
        private void UpdateMainMenu()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isMainMenu = false;
                isGameOver = false;
                isGamePlaying = true;
                return;
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // close game
                // code missing
                return;

            }
        }

        private void UpdateGamePlaying()
        {
           
            if (ball.Lives == 0)
            {
                isMainMenu = false;
                isGamePlaying = false;
                isGameOver = true;
            }
            if (ball.Lives > 0)
            {
                AngleToVector();
                player.Update();
                StartButton();
                HitTheBottom();

                //PowerBallsCollision();

                ball.Update(StartingPosition);
                BallsList.Update();
                SweepBarNet();
                ExplodingBrick();
                BallPermaBrickCollision();
                BallBrickCollision();
                BallPaddleCollision();
                PowerPaddleCollide();
                WaveCollide();

                BrickLvl.Update(X);
                if (X != 0)
                { X--; }

                if (ballCreateActive == true)
                {
                    BallsList.BallsBuild(StartingPosition, 3);
                    ballCreateActive = false;
                }
            }
            
        }

        private void UpdateGameOver()
        {
            // resets game board
            for (int a = 0; a < BrickLvl.bricks.Count; a++)
            {
                if (BrickLvl.bricks[a].isActive == true)
                    BrickLvl.bricks.RemoveAt(a);
            }
            for (int b = 0; b < BrickLvl.powerUps.Count; b++)
            {
                if (BrickLvl.powerUps[b].isActive == true)
                    BrickLvl.powerUps.RemoveAt(b);
            }
            for (int c = 0; c < BrickLvl.wavePowerUps.Count; c++)
            {
                if (BrickLvl.wavePowerUps[c].isActive == true)
                    BrickLvl.wavePowerUps.RemoveAt(c);
            }
            for (int d = 0; d < BrickLvl.permamentBricks.Count; d++)
            {
                if (BrickLvl.permamentBricks[d].isActive == true)
                    BrickLvl.permamentBricks.RemoveAt(d);
            }
            for (int e = 0; e < BallsList.ballsList.Count; e++)
            {
                if (BallsList.ballsList[e].Move == true)
                    BallsList.ballsList.RemoveAt(e);
            }

            X = 1;
            score = 0;
            scoreMultiply = 0;
            ball.Lives = 3;
            ball.Speed = 8f;
            player.Texture = playerTextureNormal;
            

            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                
                isMainMenu = false;
                isGameOver = false;
                isGamePlaying = true;
                return;
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                isMainMenu = true;
                isGameOver = false;
                isGamePlaying = false;
                return;

            }
        }

        // GameState Draw State
        private void DrawMainMenu()
        {
            spriteBatch.Draw(MainMenuTexture, Vector2.Zero, Color.White);
        }

        private void DrawGamePlaying()
        {
            ball.Draw(spriteBatch);
            player.Draw(spriteBatch);
            BrickLvl.Draw(spriteBatch);
            BallsList.Draw(spriteBatch);

            spriteBatch.DrawString(scoreFont, "LIFES: " + ball.Lives, new Vector2(50, 730), Color.Black);
            spriteBatch.DrawString(scoreFont, "SCORE: " + score, new Vector2(50, 750), Color.Black);
            spriteBatch.DrawString(scoreFont, "Cluster: " + scoreMultiply, new Vector2(50, 770), Color.Black);

            spriteBatch.DrawString(scoreFont, "Ball Count: " + BallsList.ballsList.Count, new Vector2(player.Position.X + 50, player.Position.Y + 50), Color.Black);
            //spriteBatch.DrawString(scoreFont, "direction " + angleDir , new Vector2(player.aimPosition.X, player.aimPosition.Y - 55), Color.Black);

            //spriteBatch.DrawString(scoreFont, "powerups remaining " + BrickLvl.powerUps.Count, new Vector2(player.Position.X, player.Position.Y + 50), Color.Black);
            //spriteBatch.DrawString(scoreFont, "current Texture: " + currentTex, new Vector2(player.Position.X, player.Position.Y + 80), Color.Black);

        }

        private void DrawGameOver()
        {
            spriteBatch.Draw(GameOverTexture, new Vector2(200, 250), Color.White);
        }
    }
}
