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

namespace PhysicsEngine.ParticleEngine
{
    public class Emitter
    {
        /*Random random = new Random();

        Texture2D ParticleBase;
        public int MaxNumber;
        float NextSpawnIn;
        float SecPassed;
        LinkedList<Particle> ActiveParticles;

        public Emitter() { }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            LinkedListNode<Particle> node = ActiveParticles.First;

            while (node != null)
            {
                bool isAlive = node.Value.Update(gameTime);
                node = node.Next;

                if (!isAlive && node == null)
                    ActiveParticles.RemoveLast();
                else if (!isAlive && node == null)
                    ActiveParticles.Remove(node.Previous);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var node = ActiveParticles.First;

            while (node != null)
            {
                //node.Value.Draw(spriteBatch);
                node = node.Next;
            }
        }*/
    }
}
