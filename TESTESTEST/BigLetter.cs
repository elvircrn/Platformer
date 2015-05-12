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

using TileEngine;

namespace OpenGLPlatformer
{
    public class BigLetter : GameObject
    {
        public string What;

        public BigLetter() { }
        public BigLetter(string What, Vector2 location) { this.What = What; WorldLocation = location;  }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString (Game1.defaultFont, What, Camera.WorldToScreen(WorldLocation), Color.White);
            spriteBatch.DrawString(Game1.defaultFont, What, Camera.WorldToScreen(WorldLocation), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                //Camera.WorldToScreen(WorldCollisionRectangle), Color.White);
        }
    }
}
