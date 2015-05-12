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
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using TileEngine;

namespace OpenGLPlatformer
{
    public class BigLetterManager
    {
        public List<BigLetter> Words;

        public BigLetterManager() { Words = new List<BigLetter>();  }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BigLetter word in Words)
                word.Draw(spriteBatch);
        }

        public void Add(string What, Vector2 position)
        {
            Words.Add(new BigLetter(What, position));
        }

        public void Add(BigLetter Sign)
        {
            Words.Add(Sign);
        }
    }
}
