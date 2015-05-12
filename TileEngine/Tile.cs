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
    [Serializable]
    public class Tile
    {
        #region Declarations

        private string codeValue = "";
        public bool Passable = true, pivotPoint = false;
        public int skyTile = 2;
        public int[] Layers = new int[3];

        #endregion

        public string CodeValue
        {
            get
            {
                return codeValue;
            }
            set
            {
                codeValue = value;
            }
        }

        #region Constructor

        public Tile() { }
        public Tile(int background, int interactive, int foreground, string CodeValue, bool Passable)
        {
            this.CodeValue = CodeValue;
            this.Passable = Passable;
            Layers[0] = background;
            Layers[1] = interactive;
            Layers[2] = foreground;
        }

        #endregion

        #region Public Methods

        public void TogglePassable()
        {
            Passable = !Passable;
        }

        public void MakeItPassable()
        {
            Passable = true;
        }

        public void MakeItUnpassable()
        {
            Passable = false;
        }

        #endregion
    }
}
