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

namespace Platformer
{
    public class LevelManager
    {
        public static int currentLevel = 0;
        public static void LoadLevel(int levelIndex)
        {
            FileStream fileStream = new FileStream(@"C:\Users\elvircrn\Desktop\level" + levelIndex.ToString() + ".MAP", FileMode.Open);
            TileMap.Load(fileStream);
            for (int i = 0; i < TileMap.MapWidth; i++)
            {
                for (int j = 0; j < TileMap.MapHeight; j++)
                {
                    if (TileMap.Map[i, j].CodeValue == "START")
                    {
                        Game1.player.WorldLocation = new Vector2(i * TileMap.TileWidth, j * TileMap.TileHeight);
                    }
                    else if (TileMap.Map[i, j].CodeValue == "END")
                    {
                        TileMap.EndSquare = TileMap.TileWorldRectangle(i, j);
                    }
                }
            }
        }

        public static void DrawLevel(SpriteBatch spriteBatch)
        {
            TileMap.Draw(spriteBatch);
        }

        public static void Update(GameTime gameTime)
        {

        }
    }
}
