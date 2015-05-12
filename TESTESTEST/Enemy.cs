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
    public class Enemy : MoveableEntity
    {
        public string CurrentAnimation;
        public Dictionary<String, Animation> Animations;

        bool Direction = false;
        Vector2 forceAccum = Vector2.Zero;

        public Vector2 jump = new Vector2(0, -3);

        public float Speed = 2.0f;

        public float Mass = 11.0f;

        public static Texture2D DefaultTexture;
        public static Texture2D Move;

        public bool IsDead = false;

        public Enemy(Vector2 location)
        {
            OnGround = true;

            WorldLocation = location;
            Animations = new Dictionary<String, Animation>();
            CurrentAnimation = "Move";

            CollisionRectangle = new Vector2(48, 48);

            Animations.Add("DefaultAnimation", new Animation("DefaultAnimation", Vector2.Zero, 1, 32, 32, 10.0f));
            Animations.Add("Move", new Animation("Move", Vector2.Zero, 4, 35, 35, 0.15f, 48, 48));

            Animations["DefaultAnimation"].Texture = DefaultTexture;
            Animations["Move"].Texture = Move;
        }

        public void LoadAssets(ContentManager Content)
        {
            Animations = new Dictionary<String, Animation>();
            CurrentAnimation = "DefaultAnimation";

            Animations.Add("DefaultAnimation", new Animation("DefaultAnimation", Vector2.Zero, 1, 48, 48, 10.0f));
            Animations["DefaultAnimation"].Texture = DefaultTexture;
        }

        public void Update(GameTime gameTime)
        {
            if (Animations[CurrentAnimation].currentFrame == 0)
            {
                if (forceAccum.Y == 0)
                {
                    forceAccum.Y = jump.Y;
                    OnGround = false;
                }
                
            }

            Animations[CurrentAnimation].Update(gameTime);

            Vector2 Move = new Vector2(Speed, 0.0f);

            Vector2 test = forceAccum + (Constants.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            forceAccum.Y = verticalTest(test).Y;

            if ((int)forceAccum.Y == 0)
                OnGround = true;
            else
                OnGround = false;

            if (!Direction)
                Move.X *= -1;

            Move = horizontalTest(Move);

            Vector2 CurrentLocation = WorldLocation + Move;
            Tile CurrentTile = TileMap.GetTileAtPixel(CurrentLocation);

            if (CurrentTile.CodeValue == "TURN")
                Direction ^= true;
            
            forceAccum.X = Move.X;

            WorldLocation += forceAccum;

            if (WorldCollisionRectangle.Intersects(Game1.player.WorldCollisionRectangle))
            {
                if (Game1.player.PrevState.Y < Game1.player.WorldCollisionRectangle.Y &&
                    (WorldCollisionRectangle.X < Game1.player.PrevState.X && Game1.player.PrevState.X < (WorldCollisionRectangle.X + WorldCollisionRectangle.Width) ||
                    WorldCollisionRectangle.X < Game1.player.PrevState.X + Game1.player.PrevState.Width && Game1.player.PrevState.X + Game1.player.PrevState.Width < (WorldCollisionRectangle.X + WorldCollisionRectangle.Width)))
                {
                    IsDead = true;
                    Game1.player.Jump(new Vector2(0, -7));
                }
                else
                    Game1.player.Die();
            }

            if ((int)Move.X == 0)
                Direction ^= true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect;

            if (Direction)
                effect = SpriteEffects.FlipHorizontally;
            else
                effect = SpriteEffects.None;
            
            Rectangle WorldDraw = Camera.WorldToScreen(WorldRectangle);

            Rectangle DrawRectangle = Camera.WorldToScreen(WorldRectangle);
            spriteBatch.Draw(
                 Animations[CurrentAnimation].Texture,
                 DrawRectangle,
                 Animations[CurrentAnimation].FrameRectangle,
                 Color.White, 0.0f, Vector2.Zero, effect, 0.79f);
        }

        public Rectangle WorldRectangle
        {
            get { return new Rectangle((int)WorldLocation.X + (int)CollisionRectagleDisplacement.X, (int)WorldLocation.Y + (int)CollisionRectagleDisplacement.Y, Animations[CurrentAnimation].DrawWidth, Animations[CurrentAnimation].DrawHeight); }
        }

        public Animation GetCurrentAnimation
        {
            get { return Animations[CurrentAnimation]; }
        }
    }
}
