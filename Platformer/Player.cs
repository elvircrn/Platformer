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

namespace Platformer
{
    public class Player : GameObject
    {
        #region Declarations

        public bool Dead = false;
        public int Layer = 1;
        public float Speed = 4.0f;
        public bool OnGround = true;
        public string CurrentAnimation;
        public Vector2 gravity = new Vector2(0, 15f);
        public Vector2 jump = new Vector2(0, -7);

        private bool DisplayCollisionRectangle = true;
            
        protected Vector2 forceAccum = Vector2.Zero;

        public Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();

        #endregion

        #region Constructor

        public Player(ContentManager Content)
        {
            Texture2D texture = Content.Load<Texture2D>(@"Textures\default");

            Animations.Add("default", new Animation("default", texture, 1, 1000f));
            
            Animations.Add("Idle", new Animation("Idle", Content.Load<Texture2D>(@"Textures\Zero\Idle"), 3, 0.5f));
            Animations.Add("Run", new Animation("Run", Content.Load<Texture2D>(@"Textures\Zero\Run"), 10, Speed / 40.0f));
            Animations.Add("Jump", new Animation("Jump", Content.Load<Texture2D>(@"Textures\Zero\Jump"), 7, 0.11f));

            CurrentAnimation = "Idle";

            CollisionRectagleDisplacement = new Vector2(-7, 0);
            CollisionRectangle = new Vector2(48, 48);
        }

        #endregion

        #region Properties

        #region Screen displacement

        public static Rectangle Left
        {
            get { return new Rectangle(0, 0, (Camera.ScreenWidth / 4), Camera.ScreenHeight); }
        }

        public static Rectangle Middle
        {
            get { return new Rectangle((Camera.ScreenWidth / 4), 0, (Camera.ScreenWidth / 2), Camera.ScreenHeight); }
        }

        public static Rectangle Right
        {
            get { return new Rectangle((Camera.ScreenWidth * 3) / 4, 0, (Camera.ScreenWidth / 4), Camera.ScreenHeight); }
        }

        #endregion

        public Rectangle WorldRectangle
        {
            get { return new Rectangle((int)WorldLocation.X + (int)CollisionRectagleDisplacement.X, (int)WorldLocation.Y + (int)CollisionRectagleDisplacement.Y, Animations[CurrentAnimation].FrameWidth, Animations[CurrentAnimation].FrameHeight); }
        }

        public Animation cAnimation
        {
            get { return Animations[CurrentAnimation]; }
        }

        #endregion

        #region Public Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (Animations[CurrentAnimation].Flipped == true)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(
                     Animations[CurrentAnimation].Texture,
                     Camera.WorldToScreen(WorldRectangle),
                     Animations[CurrentAnimation].FrameRectangle,
                     Color.White, 0.0f, Vector2.Zero, effect, 0.9f);
            
            if (DisplayCollisionRectangle)
                Primitives.DrawRectangle(spriteBatch, Camera.WorldToScreen(WorldCollisionRectangle), 0.00001f, Color.White);
         }

        public void Update(GameTime gameTime)
        {
            if (WorldCollisionRectangle.Intersects(TileMap.EndSquare))
            {
                //TODO: Implement end of level action

                LevelManager.LoadLevel(LevelManager.currentLevel + 1);
            }

            forceAccum.X = 0;
            if (Game1.keyState.IsKeyDown(Keys.Left))
            {
                forceAccum.X = horizontalTest(new Vector2(-Speed, 0f)).X;
                GetCurrentAnimation.Flipped = true;
                if (OnGround)
                    CurrentAnimation = "Run";
            }
            else if (Game1.keyState.IsKeyDown(Keys.Right))
            {
                forceAccum.X = horizontalTest(new Vector2(Speed, 0f)).X;
                GetCurrentAnimation.Flipped = false;
                if (OnGround)
                    CurrentAnimation = "Run";
            }

            Vector2 test = forceAccum + (gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            forceAccum.Y = verticalTest(test).Y;

            if (Game1.keyState.IsKeyDown(Keys.Up) && OnGround && Game1.prevKeyState.IsKeyUp(Keys.Up))
            {
                forceAccum.Y = jump.Y;
                OnGround = false;
                CurrentAnimation = "Jump";
            }

            WorldLocation += forceAccum;

            if (OnGround && forceAccum.X == 0)
                CurrentAnimation = "Idle";

            #region Update the Camera


            /*if (Left.Intersects(new Rectangle((int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).X,
                                                (int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).Y,
                                                WorldRectangle.Width, WorldRectangle.Height
                )))
            {
                
                if (forceAccum.X < 0)
                    Camera.Move(new Vector2(forceAccum.X, 0));
            }
            else if (Right.Intersects(new Rectangle((int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).X,
                                                (int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).Y,
                                                WorldRectangle.Width, WorldRectangle.Height
                )))
            {
                if (forceAccum.X > 0)
                    Camera.Move(new Vector2(forceAccum.X, 0));
            }*/

            Camera.Move(new Vector2(forceAccum.X, 0));

            #endregion

            Animations[CurrentAnimation].Update(gameTime);
        }

        #region Collision Detection

        public Vector2 horizontalTest(Vector2 displacement)
        {
            int precision = 10;
            if (displacement.X == 0)
                return displacement;

            List<Vector2> corners = new List<Vector2>();

            //newRectangle.X += (int)displacement.X;

            int unit = (WorldCollisionRectangle.Bottom - WorldCollisionRectangle.Top) / precision;

            if (displacement.X > 0)
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(WorldCollisionRectangle.Right, WorldCollisionRectangle.Top + (unit * i)) + displacement);
            }
            else
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(WorldCollisionRectangle.Left, WorldCollisionRectangle.Top + (unit * i)) + displacement);
            }
            
            if (displacement.X > 0)
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);

                        WorldLocation = new Vector2(tile.X - WorldCollisionRectangle.Width, WorldCollisionRectangle.Y);

                        return Vector2.Zero;
                    }
                }
            }
            else
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);

                        WorldLocation = new Vector2(tile.X + TileMap.TileWidth + 2, WorldCollisionRectangle.Y);

                        return Vector2.Zero;
                    }
                }
            }

            return displacement; 
        }

        public Vector2 verticalTest(Vector2 displacement)
        {
            int precision = 10;
            if (displacement.Y == 0)
                return displacement;

            List<Vector2> corners = new List<Vector2>();

            Rectangle newRectangle = WorldCollisionRectangle;

            newRectangle.Y += (int)displacement.Y;

            int unit = (WorldCollisionRectangle.Right - WorldCollisionRectangle.Left) / precision;

            if (displacement.Y > 0)
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(WorldCollisionRectangle.Left + (unit * i) + 1, WorldCollisionRectangle.Bottom));
            }
            else
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(WorldCollisionRectangle.Left + (unit * i) + 1, WorldCollisionRectangle.Top));
            }

            if (displacement.Y > 0)
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);

                        if (displacement.Y > 0)
                            OnGround = true;

                        Vector2 prev = WorldLocation;

                        WorldLocation = new Vector2(WorldLocation.X, tile.Y - WorldCollisionRectangle.Height);

                        OnGround = true;
                        return Vector2.Zero;
                    }
                }

                OnGround = false;
            }
            else
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);

                        WorldLocation = new Vector2(WorldCollisionRectangle.X, tile.Y + TileMap.TileHeight);

                        return Vector2.Zero;
                    }
                }
            }

            if (displacement.Y != 0)
                OnGround = false;

            return displacement; 
        }

        public Animation GetCurrentAnimation
        {
            get { return Animations[CurrentAnimation]; }
        }

        #endregion

        #endregion
    }
}
