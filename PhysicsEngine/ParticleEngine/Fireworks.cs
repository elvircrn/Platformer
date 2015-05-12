using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using PhysicsEngine;
using PhysicsEngine.ParticleEngine;

namespace PhysicsEngine.ParticleEngine
{
    struct FireworkRule
    {
        int Type;
        float MinAge, MaxAge;
        Vector2 MinVelocity, MaxVelocity;

        public struct Payload
        {
            int Type, Count;
        }

        public List<Payload> Payloads;

        public int PayloadCount
        {
            get { return Payloads.Count; }
        }

        public void SetParameters(int type, float minAge, float maxAge, Vector2 minVelocity, Vector2 maxVelocity)
        {
            Type = type;
            MinAge = minAge;
            MaxAge = maxAge;
            MinVelocity = minVelocity;
            MaxVelocity = maxVelocity;
        }

        public void Create(ref Firework firework, Firework parentFirework)
        {
            Random rand = new Random();
            float randomFloat = (float)rand.NextDouble();

            firework.Type = 0;
            firework.Age = (MaxAge - MinAge) * randomFloat;

            if (parentFirework != null)
            {
                firework.Position = parentFirework.Position;
                firework.Velocity = parentFirework.Velocity;
            }

            Vector2 vel;

            if (parentFirework != null)
                vel = parentFirework.Velocity;
            else
                vel = Vector2.Zero;

            vel += Rand.RandomVector2(MinVelocity, MaxVelocity);
            firework.Velocity = vel;

            firework.InverseMass = 1;

            //firework.Acceleration = new Vector2(100, 0);
        }
    }

    class Firework : Particle
    {
        public int Type;
        public float Age;

        public Firework() { }
          
        public bool Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Age -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            return (Age < 0);
        }
    }
}
