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

namespace OpenGLPlatformer
{
    public class Gem : Item
    {
        public Gem(Vector2 location) : base(location) { }

        public static SoundEffect PickUp;
        public static Vector2 LastPickUp;
        public static SoundEffectInstance PickUpInstance;
    }
}
