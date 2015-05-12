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
    public class Animation
    {
        #region Declaratios

        public bool IsLooping = true, IsActive = false;
        public int NumberOfFrames;
        public int currentFrame;
        public float FrameDuration;
        public string Name;
        public int FrameHeight, FrameWidth, LoopStart = 0;
        public bool Flipped;
        public string NextAnimation;
        public Vector2 Location;
        public bool CounterFlipped;
        public bool PlayerAnimation;
        public bool Finished;

        public int DrawWidth;
        public int DrawHeight;

        private float cooldown;

        public List<Rectangle> frames = new List<Rectangle>();

        private Texture2D texture;

        #endregion

        #region Constructor

        public Animation() { Finished = false; PlayerAnimation = true; }
        public Animation(string Name, Vector2 Location, int NumberOfFrames, int Width, int Height, float FrameDuration)
        {
            PlayerAnimation = false;

            Finished = false;
            this.Name = Name;
            this.Location = Location;
            this.NumberOfFrames = NumberOfFrames;
            this.FrameWidth = Width;
            this.FrameHeight = Height;
            this.FrameDuration = FrameDuration;

            currentFrame = 0;

            this.DrawWidth  =  Width;
            this.DrawHeight = Height;

            for (int i = 0; i < NumberOfFrames; i++)
            {
                frames.Add(new Rectangle((int)Location.X + Width * i, (int)Location.Y, Width, Height));
            }
        }

        //<summary>
        //Use this constructor
        //</summary>
        public Animation(string Name, Vector2 Location, int NumberOfFrames, int Width, int Height, float FrameDuration, int DrawWidth, int DrawHeight)
        {
            PlayerAnimation = false;

            Finished = false;
            this.Name = Name;
            this.Location = Location;
            this.NumberOfFrames = NumberOfFrames;
            this.FrameWidth = Width;
            this.FrameHeight = Height;
            this.FrameDuration = FrameDuration;

            currentFrame = 0;

            CounterFlipped = false;

            if (!CounterFlipped)
                this.currentFrame = 0;
            else
                this.currentFrame = NumberOfFrames - 1;

            this.CounterFlipped = false;

            this.LoopStart = 0;

            this.IsLooping = true;

            this.DrawWidth = DrawWidth;
            this.DrawHeight = DrawHeight;

            for (int i = 0; i < NumberOfFrames; i++)
            {
                frames.Add(new Rectangle((int)Location.X + this.FrameWidth * i, (int)Location.Y, this.FrameWidth, this.FrameHeight));
            }
        }

        #endregion

        #region Properties

        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle((int)Location.X + FrameWidth * currentFrame, (int)Location.Y, FrameWidth, FrameHeight);
            }
        }

        public Texture2D Texture
        {
            get
            {
                if (PlayerAnimation)
                {
                    return Player.Texture;
                }
                else
                {
                    return texture;
                }
            }
            set
            {
                if (PlayerAnimation)
                {
                    Player.Texture = value;
                }
                else
                {
                    texture = value;
                }
            }
        }

        #endregion

        #region Public Methods

        public void Update(GameTime gameTime)
        {
            cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (cooldown < 0.0f)
            {
                cooldown = FrameDuration;
                currentFrame++;

                if (currentFrame >= NumberOfFrames && IsLooping == true)
                    currentFrame = 0;
                else if (currentFrame >= NumberOfFrames && IsLooping == false)
                {
                    currentFrame = NumberOfFrames - 1;
                    Finished = true;
                }
            }
        }

        public void Play()
        {
            IsActive = true;
            currentFrame = 0;
            Finished = false;
        }

        public void Stop()
        {
            IsActive = false;
            currentFrame = 0;
        }

        #endregion
    }
}







