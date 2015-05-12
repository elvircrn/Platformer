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
    public class BackgroundElement
    {
        public Texture2D Texture;
        public int DisplayWidth, DisplayHeight;
        public float Speed;
        public Vector2 Origin;

        public BackgroundElement() { }
        public BackgroundElement(Texture2D Texture, int DisplayWidth, int DisplayHeight, float Speed)
        {
            this.Origin = Vector2.Zero;
            this.Texture = Texture;
            this.DisplayWidth = DisplayWidth;
            this.DisplayHeight = DisplayHeight;
            this.Speed = Speed;
        }
        public BackgroundElement(Texture2D Texture, float TextureScale)
        {
            this.Texture = Texture;
            DisplayWidth = (int)(TextureScale * Texture.Width);
            DisplayHeight = (int)(TextureScale * Texture.Height);
        }
        public BackgroundElement(Texture2D Texture, bool Scale, float Speed)
        {
            this.Texture = Texture;
            this.Speed = Speed;
            DisplayWidth = Texture.Width;
            DisplayHeight = Texture.Height;

            if (Scale)
                ScaleToScreenHeight(Camera.BaseResolution.Y);
        }
        public BackgroundElement(Texture2D Texture)
        {
            this.Texture = Texture;
            DisplayWidth = Texture.Width;
            DisplayHeight = Texture.Height;
        }

        public void ScaleToScreenHeight(int DesiredHeight)
        {
            float Scale = (DesiredHeight / DisplayHeight);
            DisplayWidth  = (int)((float)DisplayWidth * Scale);
            DisplayHeight = DesiredHeight;
        }

        public void MoveRight()
        {
            Origin.X += Speed;

            if (Origin.X > 0)
                Origin.X -= DisplayWidth;
        }

        public void MoveLeft()
        {
            Origin.X -= Speed;

            if (Origin.X < -DisplayWidth)
            {
                Origin.X += DisplayWidth;
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        private Rectangle GetDiplayRectangle(Vector2 DisplayLocation)
        {
            return new Rectangle((int)DisplayLocation.X, (int)DisplayLocation.Y, DisplayWidth, DisplayHeight);
        }

        public void Draw(SpriteBatch spriteBatch, float Layer)
        {
            Vector2 DisplayLocation = Origin;

            while (DisplayLocation.X < (float)Camera.BaseResolution.X)
            {
                spriteBatch.Draw(Texture, 
                                GetDiplayRectangle(DisplayLocation), 
                                null,
                                Color.White,
                                0f, 
                                Vector2.Zero, 
                                SpriteEffects.None, 
                                Layer);
                DisplayLocation.X += DisplayWidth;
            }
        }
    }
}