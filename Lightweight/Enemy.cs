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
    public class Enemy : ITakeDamage, ICollidable, IShoot
    {

        //Fields used in class
        private int enemyHealth;
        private Vector2 pos;
        private float speed;
        private Rectangle hitBox;
        private Animator anims;
        private EnemyState animState;
        private double shootTimer;
        private double shotImmune;
        private double damageTimer;
        private int damage;
        private bool playerContact;
        private double startTimer;

        /// <summary>
        /// Property that returns enemy hitbox
        /// </summary>
        public Rectangle HitBox { get { return hitBox; } }

        /// <summary>
        /// Property that returns animations
        /// </summary>
        public Animator Anims { get { return anims; } }

        /// <summary>
        /// Property that returns enemy position
        /// </summary>
        public Vector2 Pos { get { return pos; } }

        /// <summary>
        /// Parameterized constructor of enemy
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="anims">Animations</param>
        /// <param name="hitBoxTex">Hitbox texture</param>
        public Enemy(int enemyHealth, int speed, Vector2 pos, Animator anims)
        {
            this.pos = pos;
            this.anims = anims;
            this.enemyHealth = enemyHealth;
            this.speed = speed;

            hitBox = new Rectangle((int)pos.X + 4, (int)pos.Y + 4, 25, 25);
            shootTimer = 2;
            damage = (LevelManager.Instance.Wave + 1) * 3;
            if(damage > 30) { damage = 30; }
        }

        /// <summary>
        /// Method that deals damage to enemies
        /// </summary>
        /// <param name="damage">Amount of damage being taken</param>
        /// <param name="defense">Amount of defense</param>
        public void ITakeDamage(int damage)
        {
            enemyHealth -= damage;
            shotImmune = 0.5;

            //Kills enemy
            if (enemyHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gt">Gametime</param>
        /// <param name="player">Player instance</param>
        public void Update(GameTime gt, Player player)
        {
            // waits [startTimer] time before moving
            startTimer -= gt.ElapsedGameTime.TotalSeconds;
            Vector2 direction = new Vector2(player.HitBox.X, player.HitBox.Y) - pos;
            direction.Normalize();
            if(startTimer > 0) { return; }

            //Updates animation state
            if (direction.X > 0) { animState = EnemyState.RunRight; }
            else { animState = EnemyState.RunLeft; }
            anims.Update(gt, animState);

            // moves towards player
            pos += direction * speed * (float)gt.ElapsedGameTime.TotalSeconds * 10f;
            hitBox.X = (int)pos.X + 4;
            hitBox.Y = (int)pos.Y + 4;

            //Damages player if enemy intersects
            if (HitBox.Intersects(player.HitBox))
            {
                // hits player for 10dmg on first contact
                if (!playerContact)
                {
                    player.ITakeDamage(10);
                    damageTimer = 0.5;
                    playerContact = true;
                }

                // starts counting down before next damage
                damageTimer -= gt.ElapsedGameTime.TotalSeconds;

                // 'hits' the player and starts damage timer for next 'hit'
                if (damageTimer <= 0)
                {
                    player.ITakeDamage(5);
                    damageTimer = 0.5;
                }
            }
            else
            {
                // when not in contact with player, sets contact to false
                playerContact = false;
            }

            // if not in contact with player, starts to shoot at player
            if (!playerContact)
            {
                // waits [shoot timer] seconds before shooting at player
                if (shootTimer <= 0)
                {
                    Shoot(hitBox.Center.ToVector2(), player.Position, 10, damage);
                    shootTimer = 1.5;
                }
                else
                {
                    shootTimer -= gt.ElapsedGameTime.TotalSeconds;
                }
            }

            // if is immortal (takes no damage after getting hit for [immune timer] seconds)
            if (shotImmune > 0)
            {
                // counts down how long enemy is immune to damage
                shotImmune -= gt.ElapsedGameTime.TotalSeconds;
            }

            // checks collisions with bullets
            for (int i = 0; i < BulletManager.Instance.Bullets.Count; i++)
            {
                // if not immune to damage and if didn't shoot the bullet
                // removes bullet from bullet manager
                Bullet bullet = BulletManager.Instance.Bullets[i];
                if (hitBox.Intersects(bullet.HitBox)
                    && bullet.Source != this
                    && shotImmune <= 0)
                {
                    BulletManager.Instance.Remove(bullet);
                    ITakeDamage(bullet.Damage);
                }
            }
        }

        /// <summary>
        /// Method that kills enemy
        /// </summary>
        public void Die()
        {
            EnemyManager.Instance.KillEnemy(this);
        }

        /// <summary>
        /// Method that deals with enemy shooting
        /// </summary>
        /// <param name="origin">Origin of bullet</param>
        /// <param name="target">Position of target</param>
        /// <param name="speed">Speed of bullet</param>
        /// <param name="damage">Damage of bullet</param>
        public void Shoot(Vector2 origin, Vector2 target, int speed, int damage)
        {
            // calculate direction to mouse pos
            Vector2 direction = Vector2.Normalize(target - origin);

            // instantiate bullet at the player's pos with the calculated direction
            BulletManager.Instance.Add(this, origin, direction, speed, damage);
        }

        /// <summary>
        /// Draws enemy to screen
        /// </summary>
        /// <param name="sb">Spritebatcj</param>
        public void Draw(SpriteBatch sb)
        {
            anims.Draw(sb, pos);
        }
    }
}
