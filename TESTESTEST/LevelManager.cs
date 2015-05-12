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
    public class LevelManager
    {
        public static BigLetterManager Signs;
        public static List<Gem> Gems;
        public static Portal[] Portals;

        public static List<Rectangle> DeathSquares;

        public static EnemyManager Enemies;

        public static int PortalCount;

        public static int currentLevel = 0;
        public static void LoadLevel(int levelIndex)
        {
            PortalCount = 0;
            Portals = new Portal[100];
            Gems = new List<Gem>();
            DeathSquares = new List<Rectangle>();
            Enemies = new EnemyManager();
            Signs = new BigLetterManager();

            currentLevel = levelIndex;
            
            //FileStream fileStream = new FileStream(@"Content\Maps\level" + levelIndex.ToString() + ".MAP", FileMode.Open);
            FileStream fileStream = new FileStream(@"C:\Users\elvircrn\Documents\why hello there\maturski ako se sta desi - Copy\Platformer\OpenGLPlatformerContent\OpenGLPlatformerContentContent\Maps\big.MAP", FileMode.Open);
            TileMap.Load(fileStream);
            
            for (int i = 0; i < TileMap.MapWidth; i++)
            {
                for (int j = 0; j < TileMap.MapHeight; j++)
                {
                    if (TileMap.Map[i, j].CodeValue == null)
                        continue;

                    if (TileMap.Map[i, j].CodeValue == "START")
                    {
                        //Game1.player.WorldLocation = TileMap.;
                        TileMap.PlayerStartPosition = new Vector2(i * TileMap.TileWidth, j * TileMap.TileHeight);
                        
                    }
                    else if (TileMap.Map[i, j].CodeValue == "END")
                    {
                        TileMap.EndSquare = TileMap.TileWorldRectangle(i, j);
                    }
                    else if (TileMap.Map[i, j].CodeValue == "GEM")
                    {
                        Gems.Add(new Gem(new Vector2(i * TileMap.TileWidth + 8, j * TileMap.TileHeight)));
                    }
                    else if (TileMap.Map[i, j].CodeValue == "DEAD")
                    {
                        DeathSquares.Add(new Rectangle(i * TileMap.TileWidth, j * TileMap.TileHeight, TileMap.TileWidth, TileMap.TileHeight));
                    }
                    else if (TileMap.Map[i, j].CodeValue == "ENEMY")
                    {
                        Enemies.Add(new Enemy(new Vector2(i * TileMap.TileWidth + 8, j * TileMap.TileHeight)));
                    }
                    else if (TileMap.Map[i, j].CodeValue != "" && Char.IsDigit(TileMap.Map[i, j].CodeValue[0]))
                    {
                        int door_index = Convert.ToInt32(TileMap.Map[i, j].CodeValue);

                        if (Portals[door_index] == null)
                        {
                            Portals[door_index] = new Portal();
                            Portals[door_index].One = new Vector2(i * TileMap.TileWidth, j * TileMap.TileHeight);
                            PortalCount++;
                        }
                        else
                            Portals[door_index].Two = new Vector2(i * TileMap.TileWidth, j * TileMap.TileHeight);
                    }
                    else if (TileMap.Map[i, j].CodeValue != "")
                    {
                        Signs.Add(new BigLetter(TileMap.Map[i, j].CodeValue, new Vector2((float)(i * TileMap.TileWidth), (float)(j * TileMap.TileHeight))));
                    }
                }
            }

            Signs.Add("Find all the gems", TileMap.PlayerStartPosition + new Vector2(0, -20.0f));
            Game1.player.Init();
        }

        public static void DrawLevel(SpriteBatch spriteBatch)
        {
            //Draw the map
            TileMap.Draw(spriteBatch);
        
            //Draw the items
            foreach (Gem gem in Gems)
            {
                gem.Draw(spriteBatch, 0.7f);
            }

            //Draw the portals
            for (int i = 1; i <= PortalCount; i++)
            {
                Portals[i].Draw(spriteBatch); 
            }

            Signs.Draw(spriteBatch);

            //Draw the enemies
            Enemies.Draw(spriteBatch);
        }

        public static void Update(GameTime gameTime)
        {
            Enemies.Update(gameTime);
        }
    }
}
/*

 * 
 * 10111
 *  1010
 * _____
 *  1110
*/