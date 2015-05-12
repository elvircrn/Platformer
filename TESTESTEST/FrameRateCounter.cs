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

namespace OpenGLPlatformer
{
    public class FrameRateCounter
    {
        protected static int frameCount = 0;
        protected static int elapsed = 0;
        protected static int fps;

        public static void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            frameCount++;

            if (elapsed >= 1000)
            {
                fps = frameCount;
                frameCount = 0;
                elapsed = 0;
            }
        }

        public static int FrameRate
        {
            get { return fps; }
        }
    }
}
