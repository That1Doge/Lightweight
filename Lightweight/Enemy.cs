using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    public enum EnemyState { FaceLeft, FaceRight, RunLeft, RunRight }
    /// <summary>
    /// Enemy Class 
    /// Creates a single Instance of a enemy
    /// </summary>
    public class Enemy : ITakeDamage, ICollidable, IShoot
    {
        private int enemyHealth;
        private Vector2 pos;
        private float speed;
        private Rectangle hitBox;

        public Rectangle HitBox { get { return hitBox; } }

        public Enemy(Vector2 pos)
        {
            this.pos = pos;
            speed = 50;
        }

        public void ITakeDamage(int damage, int defense)
        {
            //defense is half as effective on enemies
            this.enemyHealth = enemyHealth - (damage - defense / 2);
        }

        public void Update(GameTime gt, Player player)
        {
            Vector2 direction = player.Position - pos;
            if(direction != Vector2.Zero)
            {
                pos += Vector2.Normalize(direction) * speed * 
                    (float)gt.ElapsedGameTime.TotalSeconds * 1000f;
            }
        }

        public void Die()
        {
            EnemyManager.Instance.Remove(this);
        }

        public bool Intersect(Rectangle hitbox)
        {
            throw new NotImplementedException();
        }

        public void Shoot(Texture2D texture, Vector2 origin, Vector2 target)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}
