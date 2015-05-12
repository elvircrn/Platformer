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

using TileEngine;

namespace OpenGLPlatformer
{
    public class Item : GameObject
    {
        public static Texture2D Texture;
        public bool PickedUp;

        public Item() { }
        public Item(Vector2 _loc)
        {
            PickedUp = false;
            worldLocation = _loc;
            CollisionRectangle = new Vector2(Texture.Width, Texture.Height);
        }

        public void PickUp()
        {
            PickedUp = false;
        }

        public void Draw(SpriteBatch spriteBatch, float Layer) //0.8
        {
            if (!PickedUp && WorldCollisionRectangle.Intersects(Camera.WorldRectangle))
                spriteBatch.Draw(Texture,
                                 Camera.WorldToScreen(WorldLocation),
                                 null,
                                 Color.White,
                                 0.0f,
                                 Vector2.Zero,
                                 1f,
                                 SpriteEffects.None,
                                 Layer);
        }
    }
}
