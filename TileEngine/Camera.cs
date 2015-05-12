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

namespace TileEngine
{
    public class Camera
    {
        #region Declarations

        public static int ScreenWidth;
        public static int ScreenHeight;

        private static Vector2 location = Vector2.Zero;                                    
        private static Vector2 viewPortSize = Vector2.Zero;
        private static Rectangle worldRectangle = new Rectangle(0, 0, ScreenWidth, ScreenHeight);

        public static Point BaseResolution;

        public static int Scale = 1;           

        #endregion

        #region Properties

        public static Vector2 WorldLocation
        {
            get { return location; }
            set
            {
                location = new Vector2(
                    MathHelper.Clamp(value.X,
                        worldRectangle.X, worldRectangle.Width -
                        Width),
                    MathHelper.Clamp(value.Y,
                        worldRectangle.Y, worldRectangle.Height -
                        Height));
            }
        }

        public static int Height
        {
            get { return (int)viewPortSize.Y; }
            set { viewPortSize.Y = value; }
        }

        public static int Width
        {
            get { return (int)viewPortSize.X; }
            set { viewPortSize.X = value; }
        }

        public static Rectangle WorldRectangle
        {
            get { return worldRectangle; }
            set { worldRectangle = value; }
        }

        public static Rectangle ViewPortRectangle
        {
            get { return new Rectangle((int)location.X, (int)location.Y, Width, Height); }
        }

        #endregion

        #region Public Methods

        public static void Move(Vector2 offset)
        {
            WorldLocation += offset;
        }

        public static bool ObjectIsVisible(Rectangle Object)
        {
            return ViewPortRectangle.Intersects(Object);
        }

        public static Vector2 WorldToScreen(Vector2 worldLocation)
        {
            return worldLocation - location;
        }

        public static Rectangle WorldToScreen(Rectangle rectangle)
        {
            return new Rectangle(rectangle.Left - (int)location.X, rectangle.Top - (int)location.Y, rectangle.Width, rectangle.Height);
        }

        public static Vector2 ScreenToWorld(Vector2 screenLocation)
        {
            return screenLocation + location;
        }

        public static Rectangle ScreenToWorld(Rectangle rectangle)
        {
            return new Rectangle(rectangle.X + (int)location.X, rectangle.Y + (int)location.Y, rectangle.Width, rectangle.Height);
        }

        protected static float      _zoom = 1.0f; // Camera Zoom
        public static Matrix   _transform; // Matrix Transform
        protected static float  _rotation = 0.0f; // Camera Rotation
        public static Vector2 ScreenLocation = Vector2.Zero;

        public static float Zoom
        {
            get { return _zoom; }
            set { _zoom = (value < 0.1f) ?  0.1f : value; } // Negative zoom will flip image
        }

        public static float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public static void MoveScreenCamera(Vector2 move)
        {
            ScreenLocation += move;
        }

        // Get set position

        public static Rectangle Left
        {
            get { return new Rectangle(0, 0, (2 * (Camera.Width / 5)), Camera.Height); }
        }

        public static Rectangle Middle
        {
            get { return new Rectangle((2 * (Camera.Width / 5)), 0, Camera.Width / 5, Camera.Height); }
        }

        public static Rectangle Right
        {
            get { return new Rectangle((3 * (Camera.Width / 5)), 0, 2 * (Camera.Width / 5), Camera.Height); }
        }

        public static Rectangle Up
        {
            get { return new Rectangle(0, 0, Camera.Width, Camera.Height / 2); }
        }

        public static Rectangle Down
        {
            get { return new Rectangle(0, Camera.Height / 2, Camera.Width, Camera.Height); }
        }

        public static Matrix get_transformation(GraphicsDevice graphicsDevice)
         {
            _transform =  Matrix.CreateTranslation(new Vector3(-ScreenLocation.X, -ScreenLocation.Y, 0)) *
                          Matrix.CreateRotationZ(Rotation) *
                          Matrix.CreateScale(Zoom) *
                          Matrix.CreateTranslation(new Vector3(ScreenWidth * 0.5f, ScreenHeight * 0.5f, 0));


            return _transform;
        }

        #endregion

    }
}
