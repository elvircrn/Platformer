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
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using MonogameLinuxSpecifics;

using PhysicsEngine;

using TileEngine;

//20x12

namespace OpenGLPlatformer
{
    class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static KeyboardState keyState;
        public static KeyboardState prevKeyState = Keyboard.GetState();
        public static SpriteFont defaultFont;
        public static Vector3 _screenScale;

        public static ParallaxBackground Mountain;
        public static Rectangle MoonRectangle;

        public static Texture2D Moon;

        public static Player player;
        //public static Song main_theme;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            #region Window Init

            Camera.BaseResolution = new Point(1280, 720);

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

            GameWindowExtensions.SetPosition(Window, new Point(0, 0));
            //raphicsDevice.PreferedFilter = All.Linear;

            Window.IsBorderless = true;

            graphics.PreferMultiSampling = true;

            var scaleX = (float)graphics.PreferredBackBufferWidth / (float)Camera.BaseResolution.X;
            var scaleY = (float)graphics.PreferredBackBufferHeight / (float)Camera.BaseResolution.Y;
            _screenScale = new Vector3(scaleX, scaleY, 1.0f);

            #endregion

            #region Camera Init

            Camera.ScreenLocation = Vector2.Zero;
            Camera.WorldLocation = Vector2.Zero;

            Camera.Width = Camera.BaseResolution.X;
            Camera.Height = Camera.BaseResolution.Y;

            Camera.ScreenWidth = Camera.BaseResolution.X;
            Camera.ScreenHeight = Camera.BaseResolution.Y;

            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.WorldLocation = Vector2.Zero;

            Camera.ScreenLocation += new Vector2(Camera.ScreenWidth / 2, Camera.ScreenHeight / 2);

            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Gem.Texture = Content.Load<Texture2D>(@"Textures\Gem");

            Gem.PickUp = Content.Load<SoundEffect>(@"Sounds\gempickup");

            Gem.PickUpInstance = Gem.PickUp.CreateInstance();

            Portal.PortalOrange = Content.Load<Texture2D>(@"Textures\portal1");
            Portal.PortalBlue = Content.Load<Texture2D>(@"Textures\portal2");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>(@"Fonts\myFont");
            
            #region Enemies

            Enemy.Move = Content.Load<Texture2D>(@"Textures\enemy_move");
            Enemy.DefaultTexture = Content.Load<Texture2D>(@"Textures\enemy_default");

            #endregion

            #region Player

            Player.Texture = Content.Load<Texture2D>(@"Textures\IndieWolf\IndieWolf");
            player = new Player(Content);
            player.Init();

            #endregion

            #region Level

            TileEngine.TileMap.Initialize(Content.Load<Texture2D>(@"Textures\PlatformTiles"));
            LevelManager.LoadLevel(0);

            #endregion

            #region Background Stuff

            Mountain = new ParallaxBackground();

            Mountain.BackgroundElements.Add(new BackgroundElement(Content.Load<Texture2D>(@"Textures\Backgrounds\Mountain\mountain1"), true, 1f));
            //Mountain.BackgroundElements.Add(new BackgroundElement(Content.Load<Texture2D>(@"Textures\Backgrounds\Mountain\mountain2"), true, 0.0f));

            Moon = Content.Load<Texture2D>(@"Textures\Backgrounds\Mountain\mountain2");

            float moonScale = ((float)Camera.BaseResolution.Y / (float)Moon.Height);

            int moonWidth = (int)(moonScale * (float)Moon.Width);

            MoonRectangle = new Rectangle(Camera.BaseResolution.X / 2 - (moonWidth / 2), 0, (int)(moonScale * (float)Moon.Width), Camera.BaseResolution.Y);

            #endregion

            //player.CollisionRectangle = new Vector2(player.Animations["default"].FrameWidth, player.Animations["default"].FrameHeight);

            player.WorldLocation = TileMap.PlayerStartPosition;

            BinaryFormatter formatter = new BinaryFormatter();
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            FrameRateCounter.Update(gameTime);

            keyState = Keyboard.GetState();

            LevelManager.Update(gameTime);

            #region Parse Movement

            if (keyState.IsKeyDown(Keys.W))
            {
                Camera.MoveScreenCamera(new Vector2(0, -5));
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                Camera.MoveScreenCamera(new Vector2(0, 5));
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                Camera.MoveScreenCamera(new Vector2(-5, 0));
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                Camera.MoveScreenCamera(new Vector2(5, 0));
            }
            if (keyState.IsKeyDown(Keys.E))
            {
                Camera.Rotation += 0.01f;
            }
            if (keyState.IsKeyDown(Keys.Q))
            {
                Camera.Rotation -= 0.01f;
            }
            if (keyState.IsKeyDown(Keys.L))
            {
                LevelManager.LoadLevel(0);
            }

            player.Update(gameTime);

            #endregion

            prevKeyState = keyState;

            #region Update the Sound

            if (Gem.PickUpInstance.State == SoundState.Playing)
            {
                float distance = (player.WorldLocation - Gem.LastPickUp).Length();
                float volume = 100.0f / distance;

                Gem.PickUpInstance.Volume = MathHelper.Clamp(volume, 0.0f, 1.0f);
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOrchid);

            var scaleMatrix = Matrix.CreateScale(_screenScale);
            

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Camera.get_transformation(graphics.GraphicsDevice));
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, scaleMatrix);


            spriteBatch.Draw(Moon, MoonRectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            Mountain.Draw(spriteBatch);
            LevelManager.DrawLevel(spriteBatch);

            player.Draw(spriteBatch);

            spriteBatch.DrawString(defaultFont, FrameRateCounter.FrameRate.ToString(), Vector2.Zero, Color.White);

            /*
            #region DEBUG
            spriteBatch.DrawString(defaultFont, "Level " + LevelManager.currentLevel.ToString() + "\nPoints: " + player.Points + "\nlocation: " + player.WorldLocation.X.ToString() + " " + player.WorldLocation.Y.ToString(), new Vector2(1, 1), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            #endregion
            */

            #region UI
            spriteBatch.DrawString(defaultFont, "Gems found: " + (player.Points / 100).ToString(), new Vector2(1, 1), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            #endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
