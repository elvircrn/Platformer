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

using PhysicsEngine.ParticleEngine;

namespace PhysicsEngine
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Firework> fireworks = new List<Firework>();

        SpriteFont spriteFont;
        Particle Bullet;

        FireworkRule[] rules = new FireworkRule[10];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Bullet = new Particle(2.0f, new Vector2(350.0f, 0), new Vector2(10.0f, 0));
            rules[0].SetParameters(1, 0.5f, 1.4f, new Vector2(-80, -400), new Vector2(80, -400));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            spriteFont = Content.Load<SpriteFont>("Fonts\\myFont");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.N))
            {
                Firework firework = new Firework();
                rules[0].Create(ref firework, null);

                fireworks.Add(firework);
            }

            //Bullet.Update(gameTime);
            foreach (Firework firework in fireworks)
            {
                firework.Update(gameTime);
            } 

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);
            //Bullet.Draw(spriteBatch);
            foreach (Firework firework in fireworks)
            {
                firework.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
