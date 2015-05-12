using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PhysicsEngine
{
    public static class Primitives
    {
        #region Private Members

        private static Texture2D pixel;

        #endregion

        #region Private Methods

        private static void CreatePixel(SpriteBatch spriteBatch)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        #endregion

        #region Public Methods

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color)
        {
            if (pixel == null)
                CreatePixel(spriteBatch);

            float angle = (float)Math.Atan2((point2.Y - point1.Y), (point2.X - point1.X));

            spriteBatch.Draw(pixel, point1, null, color, angle, Vector2.Zero, new Vector2(Vector2.Distance(point1, point2), 1), SpriteEffects.None, 0);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, float layer, Color color)
        {
            if (pixel == null)
                CreatePixel(spriteBatch);

            float angle = (float)Math.Atan2((point2.Y - point1.Y), (point2.X - point1.X));

            spriteBatch.Draw(pixel, point1, null, color, angle, Vector2.Zero, new Vector2(Vector2.Distance(point1, point2), 1), SpriteEffects.None, layer);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, float layer, Color color)
        {
            DrawLine(spriteBatch, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), layer, color);
            DrawLine(spriteBatch, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), layer, color);
            DrawLine(spriteBatch, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), layer, color);
            DrawLine(spriteBatch, new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), layer, color);
        }

        #endregion
    }
}
