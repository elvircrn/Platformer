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
    public class MoveableEntity : GameObject
    {
        public bool OnGround = true;

        public virtual Vector2 horizontalTest(Vector2 displacement)
        {
            int precision = 10;
            if (displacement.X == 0)
                return displacement;

            List<Vector2> corners = new List<Vector2>();

            Rectangle newRectangle = WorldCollisionRectangle;

            newRectangle.X += (int)displacement.X;

            int unit = (newRectangle.Bottom - newRectangle.Top) / precision;

            if (displacement.X > 0)
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(newRectangle.Right, newRectangle.Top + (unit * i)) + displacement);
            }
            else
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(newRectangle.Left, newRectangle.Top + (unit * i)) + displacement);
            }

            if (displacement.X > 0)
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);

                        WorldLocation = new Vector2(tile.X - newRectangle.Width, newRectangle.Y);

                        return Vector2.Zero;
                    }
                }
            }
            else
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);

                        WorldLocation = new Vector2(tile.X + TileMap.TileWidth + 2, newRectangle.Y);

                        return Vector2.Zero;
                    }
                }
            }

            return displacement;
        }

        public virtual Vector2 verticalTest(Vector2 displacement)
        {
            int precision = 10;
            if (displacement.Y == 0)
                return displacement;

            List<Vector2> corners = new List<Vector2>();

            Rectangle newRectangle = WorldCollisionRectangle;

            newRectangle.Y += (int)displacement.Y;

            int unit = (newRectangle.Width) / precision;

            if (displacement.Y > 0)
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(newRectangle.Left + (unit * i) + 1, newRectangle.Y + newRectangle.Height));
            }
            else
            {
                for (int i = 0; i < precision; i++)
                    corners.Add(new Vector2(newRectangle.Left + (unit * i) + 1, newRectangle.Top));
            }

            if (displacement.Y > 0)
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);

                        Vector2 prev = WorldLocation;

                        WorldLocation = new Vector2(WorldLocation.X, tile.Y - newRectangle.Height);

                        return Vector2.Zero;
                    }
                }

                OnGround = false;
            }
            else
            {
                for (int i = 0; i < precision; i++)
                {
                    Vector2 mapCell = TileMap.PixelToTile(corners[i]);

                    if (!TileMap.TileIsPassable((int)mapCell.X, (int)mapCell.Y))
                    {
                        Rectangle tile = TileMap.TileWorldRectangle(mapCell);
                        WorldLocation = new Vector2(newRectangle.X, tile.Y + TileMap.TileHeight);
                        return Vector2.Zero;
                    }
                }
            }

            if (displacement.Y != 0)
                OnGround = false;

            return displacement;
        }
    }
}
