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
    public class GameObject
    {
        #region Declarations
        
        protected Vector2 worldLocation;
        protected Vector2 collisionRectangle;
        protected Vector2 collisionRectangleDisplacement;
        
        #endregion

        #region Properties

        public Vector2 WorldLocation 
        {
            get { return worldLocation; } 
            set { worldLocation = value; } 
        }
        protected virtual Vector2 CollisionRectangle 
        {
            get { return collisionRectangle; } 
            set { collisionRectangle = value; } 
        }

        public virtual Rectangle WorldCollisionRectangle
        {
            get { return new Rectangle((int)WorldLocation.X/* + (int)CollisionRectagleDisplacement.X*/, (int)WorldLocation.Y/* + (int)CollisionRectagleDisplacement.Y*/, (int)CollisionRectangle.X, (int)CollisionRectangle.Y); }
        }

        public virtual Vector2 CollisionRectagleDisplacement
        {
            get { return collisionRectangleDisplacement; }
            set { collisionRectangleDisplacement = value; }
        }

        #endregion

        #region Public Methods


        #endregion

        #region Don't Touch!!!
        /*#region Declarations

        protected Vector2 worldLocation;
        protected Vector2 velocity;
        protected int frameWidth, frameHeight;

        protected bool enabled;
        protected bool flipped = false;
        protected bool onGround;

        protected Rectangle collisionRectangle;
        protected int collisionWidth, collisionHeight;
        protected bool codeBasedBlocks = true;

        protected float drawDepth = 0.85f;
        protected Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();
        protected string currentAnimation = "";

        #endregion

        #region Properties

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Vector2 WorldLocation
        {
            get { return worldLocation; }
            set { worldLocation = value; }
        }

        public Vector2 WorldCentre
        {
            get
            {
                return new Vector2((int)worldLocation.X + (frameWidth / 2), (int)worldLocation.Y + (frameHeight / 2));
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle((int)worldLocation.X, (int)worldLocation.Y, frameWidth, frameHeight);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)worldLocation.X + collisionRectangle.X, 
                                     (int)worldLocation.Y + collisionRectangle.Y, 
                                     collisionRectangle.Width, 
                                     collisionRectangle.Height);
            }
        }

        #endregion

        protected void updateAnimation(GameTime gameTime)
        {
            if (Animations.ContainsKey(currentAnimation))
            {
                Animations[currentAnimation].Update(gameTime);
            }
        }

        #region Public Methods

        public void PlayAnimation(string name)
        {
            if (name != null && Animations.ContainsKey(name))
            {
                currentAnimation = name;
                Animations[name].Play();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!enabled)
                return;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            updateAnimation(gameTime);
            if (velocity.Y != 0)
            {
                onGround = false;
            }

            Vector2 moveAmount = velocity * elapsed;

            moveAmount = horizontalCollisionTest(moveAmount);
            moveAmount = verticalCollisionTest(moveAmount);

            Vector2 newPosition = worldLocation + moveAmount;

            newPosition = new Vector2(MathHelper.Clamp(newPosition.X, 0, Camera.WorldRectangle.Width - frameWidth), 
                                      MathHelper.Clamp(newPosition.Y, 2*(-TileMap.TileHeight), Camera.WorldRectangle.Height - frameHeight));

            worldLocation = newPosition;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!enabled)
                return;
            if (Animations.ContainsKey(currentAnimation))
            {
                SpriteEffects effect = SpriteEffects.None;
                if (flipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }

                spriteBatch.Draw(Animations[currentAnimation].Texture, Camera.WorldToScreen(Animations[currentAnimation].
            }
        }
        #endregion*/
        #endregion
    }
}
