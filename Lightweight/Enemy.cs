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
    public enum EnemyState { RunLeft, RunRight }
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
        private Animator anims;
        private EnemyState animState;

        public Rectangle HitBox { get { return hitBox; } }
        public Animator Anims { get { return anims; } }

        public Enemy(Vector2 pos, Animator anims)
        {
            this.pos = pos;
            this.anims = anims;
            speed = 5;
        }

        public void ITakeDamage(int damage, int defense)
        {
            //defense is half as effective on enemies
            this.enemyHealth = enemyHealth - (damage - defense / 2);
        }

        public void Update(GameTime gt, Player player)
        {
            Vector2 direction = player.Position - pos;
            direction.Normalize();
            pos += direction * speed * (float)gt.ElapsedGameTime.TotalSeconds * 10f;
            if(direction.X > 0) { animState = EnemyState.RunRight; }
            else { animState = EnemyState.RunLeft; }
            anims.Update(gt, animState);
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
        }

        public void Draw(SpriteBatch sb)
        {
            anims.Draw(sb, pos);
        }
    }
}
