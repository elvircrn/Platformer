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

namespace PhysicsEngine.ParticleEngine
{
    public class Particle
    {
        public static Texture2D Texture;

        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;

        public Vector2 ForceAccum;

        public Color DrawColor;

        public float InverseMass;

        public Particle() 
        {
            DrawColor = Color.White;//new Color(1f, Rand.RandomFloat(), 0f, 255);
            MouseState mouseState = Mouse.GetState();
            this.Position = /*new Vector2(300, 400);*/new Vector2(mouseState.X, mouseState.Y); 
        }

        public Particle(float Mass, Vector2 Velocity, Vector2 Acceleration)
        {
            DrawColor = Color.White;
            MouseState mouseState = Mouse.GetState();
            this.InverseMass = 1.0f / Mass;
            this.Velocity = Velocity;
            this.Acceleration = Acceleration;
            this.Position = new Vector2(mouseState.X, mouseState.Y);
        }

        public void Update(GameTime gameTime)
        {
            float duration = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += (Velocity * duration);
            //Vector2 resultingAcc = Acceleration;
            //resultingAcc += (forceAccum * inverseMass);

            Velocity += (Acceleration * duration);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, DrawColor);
        }

        public void ClearAccumulation()
        {
            ForceAccum = Vector2.Zero;
        }
    }
}
