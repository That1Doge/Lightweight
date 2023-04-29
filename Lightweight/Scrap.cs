using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Class that creates a scrap
    /// </summary>
    public class Scrap
    {
        //Fields used in method
        private Vector2 pos;
        private Rectangle hitBox;
        private Animator anim;
        private double despawnTimer;

        /// <summary>
        /// Property that returns hitbox of scrap
        /// </summary>
        public Rectangle HitBox { get { return hitBox; } }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="pos">Position of scrap</param>
        /// <param name="anim">Animation</param>
        /// <param name="despawnTimer">Deswpawn timer</param>
        public Scrap(Vector2 pos, Animator anim, double despawnTimer = 5)
        {
            this.pos = pos;
            this.despawnTimer = despawnTimer;
            this.anim = anim;
            this.hitBox = new Rectangle((int)pos.X + 6, (int)pos.Y, 18, 22);
        }

        /// <summary>
        /// Update method for scrap
        /// </summary>
        /// <param name="gt">Gametime</param>
        /// <param name="player">Player isntance</param>
        public void Update(GameTime gt, Player player)
        {
            //Despawn timer
            despawnTimer -= gt.ElapsedGameTime.TotalSeconds;
            if(despawnTimer <= 0) { EnemyManager.Instance.RemoveScrap(this); }

            //Remove scrap if player picks it up
            if (hitBox.Intersects(player.HitBox)) 
            { 
                player.Scraps++;
                EnemyManager.Instance.RemoveScrap(this);
            }
            anim.Update(gt, 0);
        }

        /// <summary>
        /// Draw method for scrap
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public void Draw(SpriteBatch sb) 
        {
            anim.Draw(sb, pos);
        }
    }
}
