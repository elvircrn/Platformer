using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public class Player : MoveableEntity
    {
        #region Declarations

        public bool Dead = false;
        public float Speed = 4.0f;
        public string CurrentAnimation;

        bool AnimationFlipped;

        public Vector2 jump = new Vector2(0, -7);
        public bool Jumping = false;
        public bool Dying;

        public static Texture2D Texture;

        public Rectangle PrevState;

        public int Points = 0;

        private bool DisplayCollisionRectangle = true;
            
        protected Vector2 forceAccum = Vector2.Zero;

        public Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();

        private List<String> AnimationNames;

        #endregion

        #region Constructor

        public Player(ContentManager Content)
        {
            Texture2D texture = Content.Load<Texture2D>(@"Textures\default");
            AnimationNames = new List<String>();

            #region Zero

            /*Animations.Add("default", new Animation("default", texture, 1, 1000f));
            
            Animations.Add("Idle", new Animation("Idle", Content.Load<Texture2D>(@"Textures\Zero\Idle"), 3, 0.5f));
            Animations.Add("Run", new Animation("Run", Content.Load<Texture2D>(@"Textures\Zero\Run"), 10, Speed / 40.0f));
            Animations.Add("Jump", new Animation("Jump", Content.Load<Texture2D>(@"Textures\Zero\Jump"), 7, 0.11f));
            */

            #endregion

            #region Indie Wolf

            int W = 16, H = 16, DW = 45, DH = 45;

            Animations.Add("Idle", new Animation("Idle", Vector2.Zero, 1, W, H, 0.9f, DW, DH));
            Animations.Add("Jump", new Animation("Jump", new Vector2(0, 16), 4, W, H, 0.3f, DW, DH)); Animations["Jump"].IsLooping = false; //Animations["Jump"].Flipped ^= true;
            Animations.Add("Run", new Animation("Run", new Vector2(0, 32), 10, W, H, 0.1f, DW, DH));  // Animations["Run"].CounterFlipped = true; //Animations["Run"].Flipped ^= true;
            Animations.Add("Death", new Animation("Death", new Vector2(0, 96), 3, W, H, 0.5f, DW, DH)); Animations["Death"].IsLooping = false;

            CurrentAnimation = "Idle";

            AnimationNames.Add("Idle");
            AnimationNames.Add("Jump");
            AnimationNames.Add("Run");
            AnimationNames.Add("Death");

            foreach (String name in AnimationNames)
            {
                Animations[name].PlayerAnimation = true;
            }

            CurrentAnimation = "Idle";

            CollisionRectagleDisplacement = new Vector2(-5, 0);
            CollisionRectangle = new Vector2(36, DH);

            #endregion
        }

        #endregion

        #region Properties

        public Rectangle WorldRectangle
        {   
            get { return new Rectangle((int)WorldLocation.X + (int)CollisionRectagleDisplacement.X, (int)WorldLocation.Y + (int)CollisionRectagleDisplacement.Y, Animations[CurrentAnimation].DrawWidth, Animations[CurrentAnimation].DrawHeight); }
        }

        public Rectangle CameraRectangle
        {
            get { return new Rectangle((int)Camera.WorldToScreen(WorldLocation).X, (int)Camera.WorldToScreen(WorldLocation).Y, WorldCollisionRectangle.Width, WorldCollisionRectangle.Height); }
        }

        public Animation cAnimation
        {
            get { return Animations[CurrentAnimation]; }
        }

        public List<Vector2> KeyPoints
        {
            get
            {
                List<Vector2> ret = new List<Vector2>();
                ret.Add(WorldLocation);
                ret.Add(WorldLocation + new Vector2(CollisionRectangle.X, 0));
                ret.Add(WorldLocation + new Vector2(0, CollisionRectangle.Y));
                ret.Add(WorldLocation + CollisionRectangle);

                return ret;
            }
        }

        #endregion

        #region Public Methods

        public void Init()
        {
            Points = 0;
            CurrentAnimation = "Idle";
            WorldLocation = TileMap.PlayerStartPosition;
            Camera.WorldLocation = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (Animations[CurrentAnimation].Flipped == true)
                effect = SpriteEffects.FlipHorizontally;

            #region old
            /*if (CurrentAnimation == "Jump")
            {*/
            #endregion

            Rectangle DrawRectangle = Camera.WorldToScreen(WorldRectangle);
            spriteBatch.Draw(
                 Animations[CurrentAnimation].Texture,
                 DrawRectangle,
                 Animations[CurrentAnimation].FrameRectangle,
                 Color.White, 0.0f, Vector2.Zero, effect, 0.79f);
            
            #region old
            /*}
            else
            {
                spriteBatch.Draw(
                     Animations[CurrentAnimation].Texture,
                     Camera.WorldToScreen(WorldRectangle),
                     Animations[CurrentAnimation].FrameRectangle,
                     Color.White, 0.0f, Vector2.Zero, effect, 0.9f);
            }*/
            #endregion
            
            if (DisplayCollisionRectangle)
                Primitives.DrawRectangle(spriteBatch, Camera.WorldToScreen(WorldCollisionRectangle), 0.00001f, Color.White);
         }

        public void Update(GameTime gameTime)
        {
            bool debug = false;
            PrevState = WorldCollisionRectangle;
            if (!Dying)
            {
                #region Collision With Gems

                foreach (Gem gem in LevelManager.Gems)
                {
                    if (!gem.PickedUp && WorldCollisionRectangle.Intersects(gem.WorldCollisionRectangle))
                    {
                        if (Gem.PickUpInstance.State == SoundState.Stopped ||
                            Gem.PickUpInstance.State == SoundState.Paused)
                            Gem.PickUpInstance.Play();
                        else
                        {
                            Gem.PickUpInstance.Stop();
                            Gem.PickUpInstance.Play();
                        }

                        Gem.LastPickUp = gem.WorldLocation;
                        gem.PickedUp = true;
                        Points += 100;

                        if (LevelManager.Gems.Count == Points / 100)
                            LevelManager.Signs.Add("You won!", WorldLocation + new Vector2(0.0f, -50.0f));
                    }
                }

                #endregion

                #region Collision With Portals

                bool PortalCollision = false;

                for (int i = 1; i <= LevelManager.PortalCount; i++)
                {
                    if (WorldCollisionRectangle.Intersects(LevelManager.Portals[i].OneRectangle))
                        PortalCollision = true;
                    else if (WorldCollisionRectangle.Intersects(LevelManager.Portals[i].TwoRectangle))
                        PortalCollision = true;
                }

                #endregion

                #region End of Level Collision

                if (WorldCollisionRectangle.Intersects(TileMap.EndSquare))
                {
                    if (LevelManager.currentLevel != 1)
                        LevelManager.LoadLevel(LevelManager.currentLevel + 1);
                    else
                        LevelManager.LoadLevel(0);

                    Init();

                    return;
                }

                #endregion

                #region Physics

                Vector2 test = forceAccum + (Constants.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);

                forceAccum.Y = verticalTest(test).Y;

                if (forceAccum.Y == 0)
                    OnGround = true;

                if (Game1.keyState.IsKeyDown(Keys.Up) && OnGround && Game1.prevKeyState.IsKeyUp(Keys.Up))
                    Jump(jump);

                forceAccum.X = 0;

                if (Game1.keyState.IsKeyDown(Keys.Left))
                {
                    forceAccum.X = horizontalTest(new Vector2(-Speed, 0f)).X;
                    GetCurrentAnimation.Flipped = false;
                    AnimationFlipped = false;

                    if (OnGround)
                    {
                        if (CurrentAnimation != "Run")
                            Animations[CurrentAnimation].Play();
                        CurrentAnimation = "Run";
                    }
                }
                
                if (Game1.keyState.IsKeyDown(Keys.Right))
                {
                    forceAccum.X = horizontalTest(new Vector2(Speed, 0f)).X;
                    GetCurrentAnimation.Flipped = true;
                    AnimationFlipped = true;

                    if (OnGround)
                    {
                        if (CurrentAnimation != "Run")
                            Animations[CurrentAnimation].Play();
                        CurrentAnimation = "Run";

                    }
                }

                WorldLocation += forceAccum;

                #endregion


                if (WorldLocation.Y < -46)
                    debug = true;

                if (OnGround && forceAccum.X == 0)
                    CurrentAnimation = "Idle";

                if (Math.Abs(forceAccum.X) > 32)
                    forceAccum.X = 32 * ((forceAccum.X < 0) ? -1 : 1);
                if (Math.Abs(forceAccum.Y) > 32)
                    forceAccum.Y = 32 * ((forceAccum.Y < 0) ? -1 : 1);

                #region Update the Camera


                //Left
                if (Camera.Left.Intersects(new Rectangle((int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).X,
                                                    (int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).Y,
                                                    WorldRectangle.Width, WorldRectangle.Height
                    )))
                {

                    if (forceAccum.X < 0)
                    {
                        Camera.Move(new Vector2(forceAccum.X, 0));
                        if (Camera.WorldLocation.X > 0)
                            Game1.Mountain.Move(-1);
                    }
                }
                //Right
                else if (Camera.Right.Intersects(new Rectangle((int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).X,
                                                    (int)Camera.WorldToScreen(new Vector2(WorldLocation.X, WorldLocation.Y)).Y,
                                                    WorldRectangle.Width, WorldRectangle.Height
                    )))
                {
                    if (forceAccum.X > 0)
                    {
                        Camera.Move(new Vector2(forceAccum.X, 0));
                        Game1.Mountain.Move(1);
                    }
                }
                
                
                //Up
                if (forceAccum.Y < 0.0f && CameraRectangle.Intersects(Camera.Up))
                    Camera.Move(new Vector2(0.0f, forceAccum.Y));
                //Down
                else if (forceAccum.Y > 0 && CameraRectangle.Intersects(Camera.Down))
                    Camera.Move(new Vector2(0.0f, forceAccum.Y));
                
                #endregion

                if (forceAccum.Y != 0 && !Jumping)
                    CurrentAnimation = "Jump";

                if (OnGround)
                    Jumping = false;

                #region Collision With Portals

                for (int i = 1; i <= LevelManager.PortalCount; i++)
                {
                    if (WorldCollisionRectangle.Intersects(LevelManager.Portals[i].OneRectangle))
                    {
                        if (!PortalCollision)
                        {
                            WorldLocation = LevelManager.Portals[i].Two;
                            //if (!Camera.ViewPortRectangle.Contains(LevelManager.Portals[i].OneRectangle))
                            Camera.Move(LevelManager.Portals[i].Two - LevelManager.Portals[i].One);
                        }
                    }
                    else if (WorldCollisionRectangle.Intersects(LevelManager.Portals[i].TwoRectangle))
                    {
                        if (!PortalCollision)
                        {
                            WorldLocation = LevelManager.Portals[i].One;
                            //if (!Camera.ViewPortRectangle.Contains(LevelManager.Portals[i].TwoRectangle))
                            Camera.Move(LevelManager.Portals[i].One - LevelManager.Portals[i].Two);
                        }
                    }
                }

                #endregion

                #region Collision With "DEAD" Squares

                foreach (Rectangle rec in LevelManager.DeathSquares)
                    if (WorldCollisionRectangle.Intersects(rec))
                        Die();

                #endregion

            }

            Animations[CurrentAnimation].Flipped = AnimationFlipped;
            Animations[CurrentAnimation].Update(gameTime);

            if (Dying && Animations[CurrentAnimation].Finished)
            {
                Dying = false;
                LevelManager.LoadLevel(0);
            }

            #region Fix the Camera

            if (!Camera.ViewPortRectangle.Contains(WorldCollisionRectangle))
                Camera.Move(new Vector2(WorldLocation.X - Camera.Width / 2 - Camera.WorldLocation.X, 0));

            #endregion 

            if (debug == true)
            {
                int a = 2 + 2;
            }
        }

        public void Die()
        {
            if (Dying)
                return;
            
            forceAccum = Vector2.Zero;

            Dying = true;
            
            CurrentAnimation = "Death";
            Animations["Death"].Play();
        }

        public void Jump(Vector2 amount)
        {
            forceAccum = Vector2.Zero;
            forceAccum += amount;
            OnGround = false;
            Jumping = true;
            CurrentAnimation = "Jump";
        }

        #region Collision Detection

        public Animation GetCurrentAnimation
        {
            get { return Animations[CurrentAnimation]; }
        }

        #endregion

        #endregion
    }
}
