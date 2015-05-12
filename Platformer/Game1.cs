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

using PhysicsEngine;

using TileEngine;

namespace Platformer
{
    class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static KeyboardState keyState;
        public static KeyboardState prevKeyState = Keyboard.GetState();
        public static Vector2 baseResolution;
        public static SpriteFont defaultFont;
        public static Vector3 _screenScale;

        public static Player player;
        //public static Song main_theme;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            baseResolution = new Vector2(1024, 768);

            //Camera.ScreenWidth = graphics.baseResolution.X;
            //Camera.ScreenHeight = graphics.baseResolution.Y;

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

            graphics.ApplyChanges(); 

            var scaleX = (float)GraphicsDevice.Viewport.Width / (float)baseResolution.X;
            var scaleY = (float)GraphicsDevice.Viewport.Height / (float)baseResolution.Y;
            _screenScale = new Vector3(scaleX, scaleY, 1.0f);

            Camera.ScreenLocation = Vector2.Zero;
            Camera.WorldLocation = Vector2.Zero;

            Camera.Width = (int)baseResolution.X;
            Camera.Height = (int)baseResolution.Y;

            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.WorldLocation = Vector2.Zero;

            graphics.PreferMultiSampling = true;

            //main_theme = Content.Load<Song>(@"Sounds\main_theme");

            Camera.ScreenLocation += new Vector2(Camera.ScreenWidth / 2, Camera.ScreenHeight / 2);

            Window.IsBorderless = true;
            //MediaPlayer.Play(main_theme);
            //MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("Fonts\\myFont");

            

            TileEngine.TileMap.Initialize(Content.Load<Texture2D>(@"Textures\PlatformTiles"));
            player = new Player(Content);

            //player.CollisionRectangle = new Vector2(player.Animations["default"].FrameWidth, player.Animations["default"].FrameHeight);

            BinaryFormatter formatter = new BinaryFormatter();
            LevelManager.LoadLevel(0);
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            keyState = Keyboard.GetState();

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOrchid);

            var scaleMatrix = Matrix.CreateScale(_screenScale);

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Camera.get_transformation(graphics.GraphicsDevice));
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, scaleMatrix);
            LevelManager.DrawLevel(spriteBatch);
            
            player.Draw(spriteBatch);

            spriteBatch.DrawString(defaultFont, Camera.ScreenLocation.X.ToString() + " " + Camera.ScreenLocation.Y.ToString() + "\n" + graphics.PreferredBackBufferWidth.ToString() + " " + graphics.PreferredBackBufferHeight.ToString(), Vector2.Zero, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
