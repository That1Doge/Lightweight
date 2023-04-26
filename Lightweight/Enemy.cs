using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
    public class Enemy : ITakeDamage, ICollidable//, IShoot
    {
        private int enemyHealth;
        private Vector2 pos;
        private float speed;
        private Rectangle hitBox;
        private Animator anims;
        private EnemyState animState;
        private Texture2D hitBoxTex;
        private double shootTimer;
        private double shotImmune;
        private double damageTimer;
        private bool playerContact;
        private double startTimer;

        public Rectangle HitBox { get { return hitBox; } }
        public Animator Anims { get { return anims; } }

        public Vector2 Pos { get { return pos; } }

        public Enemy(Vector2 pos, Animator anims, Texture2D hitBoxTex)
        {
            this.pos = pos;
            this.anims = anims;
            speed = 5;
            hitBox = new Rectangle((int)pos.X + 4, (int)pos.Y + 4, 25, 25);
            this.hitBoxTex = hitBoxTex;
            shootTimer = 2;
        }

        public void ITakeDamage(int damage, int defense)
        {
            //defense is half as effective on enemies
            this.enemyHealth = enemyHealth - (damage - defense / 2);
        }

        public void Update(GameTime gt, Player player)
        {
            startTimer -= gt.ElapsedGameTime.TotalSeconds;
            Vector2 direction = new Vector2(player.HitBox.X, player.HitBox.Y) - pos;
            direction.Normalize();
            if (direction.X > 0) { animState = EnemyState.RunRight; }
            else { animState = EnemyState.RunLeft; }
            anims.Update(gt, animState);
            if (startTimer <= 0)
            {
                pos += direction * speed * (float)gt.ElapsedGameTime.TotalSeconds * 10f;
                hitBox.X = (int)pos.X + 4;
                hitBox.Y = (int)pos.Y + 4;

                if (HitBox.Intersects(player.HitBox))
                {
                    if (!playerContact)
                    {
                        player.ITakeDamage(10, 0);
                        damageTimer = 0.5;
                        playerContact = true;
                    }

                    damageTimer -= gt.ElapsedGameTime.TotalSeconds;

                    if (damageTimer <= 0)
                    {
                        player.ITakeDamage(5, 0);
                        damageTimer = 0.5;
                    }
                }
                else
                {
                    playerContact = false;
                }

                if (!playerContact)
                {
                    if (shootTimer <= 0)
                    {
                        Shoot(hitBox.Center.ToVector2(), player.Position, 10, 10);
                        shootTimer = 1.5;
                        shotImmune = 0.5;
                    }
                    else
                    {
                        shootTimer -= gt.ElapsedGameTime.TotalSeconds;
                    }
                }

                if (shotImmune > 0)
                {
                    shotImmune -= gt.ElapsedGameTime.TotalSeconds;
                }

                for (int i = 0; i < BulletManager.Bullets.Count; i++)
                {
                    if (hitBox.Intersects(BulletManager.Bullets[i].HitBox) && shotImmune <= 0)
                    {
                        BulletManager.Remove(BulletManager.Bullets[i]);
                        Die();
                    }
                }
            }
        }

        public void Die()
        {
            EnemyManager.Instance.KillEnemy(this);
        }

        public bool Intersect(Rectangle hitbox)
        {
            throw new NotImplementedException();
        }

        public void Shoot(Vector2 origin, Vector2 target, int speed, int damage)
        {
            // calculate direction to mouse pos
            Vector2 direction = Vector2.Normalize(target - origin);

            // instantiate bullet at the player's pos with the calculated direction
            Bullet bullet = new Bullet(origin, direction, speed, damage);

            // implement bullets list and add bullet to list
            BulletManager.Add(bullet);
        }
        public void Draw(SpriteBatch sb)
        {
            anims.Draw(sb, pos);
            sb.Draw(hitBoxTex, HitBox, Color.Red);
        }
    }
}
