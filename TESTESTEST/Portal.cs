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

using PhysicsEngine;
using TileEngine;

namespace OpenGLPlatformer
{
    public class Portal
    {
        public static Texture2D PortalBlue;
        public static Texture2D PortalOrange;
        public Vector2 One, Two;

        public Portal()
        { 
            One = new Vector2(-1, -1); 
            Two = new Vector2(-1, -1); 
        }

        Portal(Vector2 one, Vector2 two)
        {
            One = one;
            Two = two;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PortalBlue, Camera.WorldToScreen(One)/*new Rectangle(Camera.WorldToScreen(One).X, Camera.WorldToScreen(One).Y, 48, 48)*/,
                null, null, Vector2.Zero, 0f, null, Color.White, SpriteEffects.None, 0.78888f);
            spriteBatch.Draw(PortalOrange, Camera.WorldToScreen(Two),
                null, null, Vector2.Zero, 0f, null, Color.White, SpriteEffects.None, 0.78888f);
        }

        public Rectangle OneRectangle
        {
            get { return new Rectangle((int)One.X, (int)One.Y, PortalBlue.Width, PortalBlue.Height); }
        }

        public Rectangle TwoRectangle
        {
            get { return new Rectangle((int)Two.X, (int)Two.Y, PortalOrange.Width, PortalOrange.Width); }
        }
        
    }
}
