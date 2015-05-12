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

using TileEngine;

namespace OpenGLPlatformer
{
    public class ParallaxBackground
    {
        public List<BackgroundElement> BackgroundElements;

        public ParallaxBackground()
        {
            BackgroundElements = new List<BackgroundElement>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (BackgroundElement backgroundElement in BackgroundElements)
            {
                backgroundElement.Update(gameTime);
            }
        }

        public void Move(int X)
        {
            foreach (BackgroundElement backgroundElement in BackgroundElements)
            {
                if (X < 0)
                    backgroundElement.MoveRight();
                else if (X > 0)
                    backgroundElement.MoveLeft();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float currentLayer = 0.40f, levelIterator = 0.01f;
            foreach (BackgroundElement backgroundElement in BackgroundElements)
            {
                currentLayer -= levelIterator;
                backgroundElement.Draw(spriteBatch, currentLayer);
            }
        }
    }
}
