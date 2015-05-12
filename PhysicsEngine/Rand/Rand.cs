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
    public static class Rand
    {
        private static Random rand = new Random();

        public static float RandomFloat()
        {
            return (float)rand.NextDouble();
        }

        public static float RandomFloat(float Maximum)
        {
            return RandomFloat() * Maximum;
        }

        public static float RandomFloat(float Minimum, float Maximum)
        {
            return Minimum + ((Maximum - Minimum) * RandomFloat());
        }

        public static Vector2 RandomVector2()
        {
            return new Vector2(RandomFloat(), RandomFloat());
        }

        public static Vector2 RandomVector2(Vector2 MaxVector2)
        {
            return new Vector2(RandomFloat(MaxVector2.X), RandomFloat(MaxVector2.Y));
        }

        public static Vector2 RandomVector2(Vector2 MinVector2, Vector2 MaxVector2)
        {
            return new Vector2(RandomFloat(MinVector2.X, MaxVector2.X),
                               RandomFloat(MinVector2.Y, MaxVector2.Y));
        }
    }
}
