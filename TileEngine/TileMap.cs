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

using PhysicsEngine;

namespace TileEngine
{
    [Serializable]
    public static class TileMap
    {
        #region Declarations

        private static int tileWidth = 50;
        private static int tileHeight = 50;

        public static int TileSourceWidth = 48;
        public static int TileSourceHeight = 48;

        public static bool[] Visible = new bool[3];

        public static int MapHeight = 40;
        public static int MapWidth = 160;

        public static bool displayBoxes = true;

        public static string TextureName = "PlatformTiles";

        public static bool DisplayPassable;

        public static bool EditorMode = false;

        public const int MapLayers = 3;

        public static Tile[,] Map = new Tile[MapWidth, 100];

        public static Texture2D Texture;

        public static SpriteFont defaultFont;

        public static Vector2 LevelOffset = new Vector2(0, 100);

        public static Vector2 PlayerStartPosition = Vector2.Zero;

        public static Rectangle EndSquare = new Rectangle();

        #endregion

        #region Initialization and Stuff

        public static void Initialize(Texture2D Texture)
        {
            TileMap.Texture = Texture;
            Map = new Tile[MapWidth, MapHeight];

            Clear();
        }

        public static void Clear()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    for (int k = 0; k < MapLayers; k++)
                    {
                        Map[i, j] = new Tile(0, 0, 0, " ", true);
                    }
                }
            }
        }

        #endregion

        #region Load and Save functions

        public static void Save(FileStream fileStream)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, Map);
                fileStream.Close();
            }
            catch
            {
                throw new Exception("Faield to save the map");
            }
        }

        public static void Load(FileStream fileStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Map = (Tile[,])formatter.Deserialize(fileStream);

            fileStream.Close();
        }

        #endregion

        #region Tile and Tile Sheet Handling

        public static int TilesPerRow
        {
            get { return Texture.Width / TileSourceWidth; }
        }

        public static int TilesPerColumn
        {
            get { return Texture.Height / TileSourceHeight; }
        }

        public static Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle(
                (tileIndex % TilesPerColumn) * TileSourceWidth,
                (tileIndex / TilesPerRow) * TileSourceHeight,
                TileSourceWidth,
                TileSourceHeight);
        }

        #endregion

        #region Pixel and Tile Conversions

        public static int TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public static int TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public static int PixelToTileX(int pixelX)
        {
            return pixelX / (TileWidth * Camera.Scale);
        }

        public static int PixelToTileY(int pixelY)
        {
            return pixelY / (TileHeight * Camera.Scale);
        }

        public static Vector2 PixelToTile(Vector2 pixel)
        {
            return new Vector2((int)PixelToTileX((int)pixel.X), (int)PixelToTileY((int)pixel.Y));
        }

        public static Vector2 GetTileCenter(int cellX, int cellY)
        {
            return new Vector2(cellX + TileWidth / 2, cellY + TileHeight / 2);
        }

        public static Vector2 GetTileCenter(Vector2 cell)
        {
            return GetTileCenter((int)cell.X, (int)cell.Y);
        }

        public static Rectangle TileWorldRectangle(int cellX, int cellY)
        {
            return new Rectangle(cellX * TileWidth * Camera.Scale, cellY * TileHeight * Camera.Scale, TileWidth * Camera.Scale, TileHeight * Camera.Scale);
        }

        public static Rectangle TileWorldRectangle(Vector2 cell)
        {
            return TileWorldRectangle((int)cell.X, (int)cell.Y);
        }

        public static Rectangle TileScreenRectangle(int cellX, int cellY)
        {
            return Camera.WorldToScreen(TileWorldRectangle(cellX, cellY));
        }

        public static Rectangle TileScreenRectangle(Vector2 cell)
        {
            return TileScreenRectangle((int)cell.X, (int)cell.Y);
        }

        public static bool TileIsPassable(int tileX, int tileY)
        {
            if (tileX < 0 || tileY < 0)
                return true;

            Tile tile = Map[tileX, tileY];

            if (tile == null)
                return false;
            else
                return tile.Passable;
        }

        public static bool TileIsPassableByPixel(int pixelX, int pixelY)
        {
            Tile tile = Map[PixelToTileX(pixelX), PixelToTileY(pixelY)];

            if (tile == null)
                return false;
            else
                return tile.Passable;
        }

        public static string TileCodeValue(int tileX, int tileY)
        {
            Tile tile = Map[tileX, tileY];

            if (tile == null)
                return "";
            else
                return tile.CodeValue;
        }

        public static string TileCodeValue(Vector2 tile)
        {
            return TileCodeValue((int)tile.X, (int)tile.Y);
        }

        #endregion

        #region Information about MapSquare objects

        public static Tile GetTileAtSquare(int tileX, int tileY)
        {
            if (tileX >= 0 && tileY >= 0 && tileX < MapWidth && tileY < MapHeight)
            {
                Tile tile = Map[tileX, tileY];
                return tile;
            }
            else
                return new Tile(0, 0, 0, "", false);
        }

        public static void SetTileAtCell(int tileX, int tileY, int layer, int tileIndex)
        {
            if (tileX >= 0 && tileX < MapWidth && tileY >= 0 && tileY < MapHeight)
            {
                Map[tileX, tileY].Layers[layer] = tileIndex;
            }
        }

        public static Tile GetTileAtPixel(int pixelX, int pixelY)
        {
            return Map[PixelToTileX(pixelX), PixelToTileY(pixelY)];
        }

        public static Tile GetTileAtPixel(Vector2 pixel)
        {
            return Map[(int)PixelToTile(pixel).X, (int)PixelToTile(pixel).Y];
        }

        #endregion

        #region Draw Stuff

        public static void Draw(SpriteBatch spriteBatch)
        {
            int startX = PixelToTileX((int)Camera.WorldLocation.X);
            int startY = PixelToTileY((int)Camera.WorldLocation.Y);

            int endX = PixelToTileX((int)Camera.WorldLocation.X + Camera.Width);
            int endY = PixelToTileY((int)Camera.WorldLocation.Y + Camera.Height);

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        if (i < 0 || j < 0 || i >= MapWidth || j >= MapHeight)
                            continue;
                        if (Visible[z] || (!EditorMode && Map[i, j].Layers[z] != 0))
                            spriteBatch.Draw(Texture,
                                             TileScreenRectangle(i, j),
                                             TileSourceRectangle(Map[i, j].Layers[z]),
                                             Color.White,
                                             0.0f,
                                             Vector2.Zero,
                                             SpriteEffects.None,
                                             1f - ((float)(z + 1) * 0.1f));
                    }

                    if (EditorMode)
                        DrawEditModeItems(spriteBatch, i, j);
                }
            }
        }

        public static void DrawEditModeItems(SpriteBatch spriteBatch, int x, int y)
        {
            if ((x < 0) || (y < 0) || (x >= MapWidth) || (y >= MapHeight))
                return;

            if (!TileIsPassable(x, y) && DisplayPassable == true)
                spriteBatch.Draw(Texture, TileScreenRectangle(x, y), TileSourceRectangle(1), new Color(255, 0, 0, 80), 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            if (Map[x, y].CodeValue != " ")
                spriteBatch.DrawString(defaultFont, Map[x, y].CodeValue, new Vector2(TileScreenRectangle(x, y).X, TileScreenRectangle(x, y).Y), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        }

        #endregion
    }
}