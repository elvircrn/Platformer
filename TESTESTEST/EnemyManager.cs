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

namespace OpenGLPlatformer
{
    public class EnemyManager
    {
        public List<Enemy> Enemies;

        public EnemyManager()
        {
            Enemies = new List<Enemy>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in Enemies)
            {
                if (!enemy.IsDead)
                    enemy.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in Enemies)
            {
                if (!enemy.IsDead)
                    enemy.Update(gameTime);
            }
        }

        public void Add(Enemy enemy)
        {
            Enemies.Add(enemy);
        }
    }
}
