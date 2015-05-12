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

namespace Platformer
{
    public class Animation
    {
        #region Declaratios

        public bool IsLooping = true, IsActive = false;
        public int NumberOfFrames;
        public int currentFrame;
        public float FrameDuration;
        public string Name;
        public Texture2D Texture;
        public int FrameHeight, FrameWidth, LoopStart = 0;
        public bool Flipped = true;
        public string NextAnimation;

        private float cooldown;

        public List<Rectangle> frames = new List<Rectangle>();

        #endregion

        #region Constructor

        public Animation() { }
        public Animation(string Name, Texture2D Texture, int NumberOfFrames, int Width, int Height)
        {
            this.Name = Name;
            this.Texture = Texture;
            this.NumberOfFrames = NumberOfFrames;
            this.FrameWidth = Width;
            this.FrameHeight = Height;

            for (int i = 0; i < NumberOfFrames; i++)
            {
                frames.Add(new Rectangle(Width * i, 0, Width, Height));
            }
        }
        public Animation(string Name, Texture2D Texture, int NumberOfFrames, int Width, int Height, float FrameDuration)
        {
            this.Name = Name;
            this.Texture = Texture;
            this.NumberOfFrames = NumberOfFrames;
            this.FrameWidth = Width;
            this.FrameHeight = Height;
            this.FrameDuration = FrameDuration;

            for (int i = 0; i < NumberOfFrames; i++)
            {
                frames.Add(new Rectangle(Width * i, 0, Width, Height));
            }
        }
        public Animation(string Name, Texture2D Texture, int NumberOfFrames, float FrameDuration)
        {
            this.Name = Name;
            this.Texture = Texture;
            this.NumberOfFrames = NumberOfFrames;
            this.FrameWidth = Texture.Width / NumberOfFrames;
            this.FrameHeight = Texture.Height;
            this.FrameDuration = FrameDuration;
            this.Flipped = false;

            for (int i = 0; i < NumberOfFrames; i++)
            {
                frames.Add(new Rectangle(this.FrameWidth * i, 0, this.FrameWidth, this.FrameHeight));
            }
        }

        #endregion

        #region Properties

        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(FrameWidth * currentFrame, 0, FrameWidth, FrameHeight);
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
                    currentFrame = LoopStart;
            }
        }

        public void Play()
        {
            IsActive = true;
            currentFrame = 0;
        }

        public void Stop()
        {
            IsActive = false;
            currentFrame = 0;
        }

        #endregion
    }
}







