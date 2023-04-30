using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Player Class 
    /// Creates player
    /// </summary>
    public class Player : ITakeDamage, ICollidable, IShoot
    {        
        //Fields used in class
        int playerHealth;
        int playerDefense;
        int scraps;
        int maxScraps;
        private Rectangle hitBox;
        private Texture2D hitBoxTex;
        private Texture2D bulletTex;
        private double immuneCounter;
        private PlayerController controller;
        private bool freezeDamage;

        /// <summary>
        /// Property that returns player controller
        /// </summary>
        public PlayerController Controller { get { return controller; } }

        /// <summary>
        /// Property that returns bullet text
        /// </summary>
        public Texture2D BulletTex
        { get { return bulletTex; } }

        /// <summary>
        /// A get and set property for the player health
        /// </summary>
        public int PlayerHealth
        {
            get { return playerHealth; }
            set { playerHealth = value; }
        }

        public int MaxScraps { get { return maxScraps; } }

        /// <summary>
        /// A get and set property for the player defense
        /// </summary>
        public int PlayerDefense
        {
            get { return playerDefense; }
            set { playerDefense = value; }
        }
        
        /// <summary>
        /// Property that returns player position
        /// </summary>
        public Vector2 Position { get { return position; } }

        /// <summary>
        /// Returns hitbox of player (for collision)
        /// </summary>
        public Rectangle HitBox { get { return hitBox; } }

        /// <summary>
        /// Property that returns X value of position of player
        /// </summary>
        public int X { get { return (int)position.X; } set { position.X = value; } }

        /// <summary>
        /// Property that returns Y value of position of player
        /// </summary>
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }

        /// <summary>
        /// Get/set property of scrap
        /// </summary>
        public int Scraps { get { return scraps; } set { scraps = value; } }

        /// <summary>
        /// Get/set property of player speed
        /// </summary>
        public float Speed { get { return speed; } set { speed = value; } }

        //Fields used in class
        private Vector2 position = new Vector2(929, 496);
        private float speed;
        private PlayerAnimator anims;

        /// <summary>
        /// Parameterised constructor of Player
        /// </summary>
        public Player()
        {
            scraps = 10;
            maxScraps = 30;
            playerHealth = 100;
            hitBox = new Rectangle((int)position.X + 5, (int)position.Y + 10, 31, 44);
            controller = new PlayerController(this);
            anims = new PlayerAnimator(this);
        }

        /// <summary>
        /// Method that loads all player animations
        /// </summary>
        /// <param name="content">Content</param>
        public void LoadAnims(ContentManager content)
        {
            anims.AddAnimation(PlayerState.RunRight, new Animation(
                content.Load<Texture2D>("PNG/Mage/Run/run"), 6));
            anims.AddAnimation(PlayerState.RunLeft, new Animation(
                content.Load<Texture2D>("PNG/Mage/Run/run"), 6, SpriteEffects.FlipHorizontally));
            anims.AddAnimation(PlayerState.RollRight, new Animation(
                content.Load<Texture2D>("PNG/Mage/High_Jump/roll"), 10, SpriteEffects.None, false));
            anims.AddAnimation(PlayerState.RollLeft, new Animation(
                content.Load<Texture2D>("PNG/Mage/High_Jump/roll"), 10, SpriteEffects.FlipHorizontally, false));
            hitBoxTex = content.Load<Texture2D>("hitbox");
        }

        /// <summary>
        /// Update method of player
        /// </summary>
        /// <param name="gt">Gametime</param>
        public void Update(GameTime gt)
        {
            // updates movement
            controller.Update(gt);
            position += controller.Direction * speed
                * (float)gt.ElapsedGameTime.TotalSeconds * 1000f;
            hitBox.X = (int)position.X + 5;
            hitBox.Y = (int)position.Y + 10;

            // press E to drop scraps
            if (controller.SingleKeyPress(Keys.E) && scraps > 0) scraps--;

            // adjust speed with scraps
            speed = 1.25f / maxScraps * ((float)(maxScraps - scraps) + 1);
            anims.Update(gt, controller.PlayerState, (1.25f / (scraps + 2)) * 128);

            // reduces immune timer if is immune to damage
            if (immuneCounter > 0)
            {
                immuneCounter -= gt.ElapsedGameTime.TotalSeconds;
            }

            // checks for collision with each bullet
            for (int i = 0; i < BulletManager.Instance.Bullets.Count; i++)
            {
                // if is in contact with bullet, not dodging, not immune, and didn't shoot it
                // takes damage, and removes bullet
                Bullet bullet = BulletManager.Instance.Bullets[i];
                if (hitBox.Intersects(bullet.HitBox)
                    && immuneCounter <= 0 && !controller.IsRolling &&
                   bullet.Source != this)
                {
                    ITakeDamage(BulletManager.Instance.Bullets[i].Damage);
                    BulletManager.Instance.Remove(bullet);
                }
            }

            // god mode controls 
            if (Game1.Instance.GodMode)
            {
                // P freezes enemies and bullets
                if (controller.SingleKeyPress(Keys.P))
                {
                    EnemyManager.Instance.Freeze = !EnemyManager.Instance.Freeze;
                    BulletManager.Instance.Freeze = !BulletManager.Instance.Freeze;
                }

                // O turns on/off damage
                if (controller.SingleKeyPress(Keys.O))
                {
                    freezeDamage = !freezeDamage;
                }

                // [Enter] spawns enemies at mouse cursor
                if (controller.SingleKeyPress(Keys.Enter))
                {
                    EnemyManager.Instance.SpawnEnemies(1, Mouse.GetState().Position.ToVector2());
                }

                // Q increases scraps
                if (controller.SingleKeyPress(Keys.Q))
                {
                    scraps++;
                }

                // I sets health to max
                if (controller.SingleKeyPress(Keys.I))
                {
                    playerHealth = 999999;
                }

                // stops player from dying:
                if(playerHealth <= 1)
                {
                    playerHealth = 1;
                }
            }
            else
            {
                freezeDamage = false;
            }

        }

        /// <summary>
        /// Draw method of player
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public void Draw(SpriteBatch sb)
        {
            anims.Draw(sb, position);
            if (Game1.Instance.GodMode)
            {
                sb.Draw(hitBoxTex, hitBox, Color.Pink);
            }
        }

        public void ITakeDamage(int damage)
        {
            // does nothing if player is rolling or has frozen damage
            if (controller.IsRolling || freezeDamage) { return; }

            // sets defens to scraps
            int defense = scraps;

            // if defense <= damage, reduces damage by defense and health
            // by leftover damage, and sets temporary immunity to damage for 0.5 seconds
            if (defense <= damage) 
            { 
                playerHealth -= (damage - defense);
                immuneCounter = 0.5;
            }

            //reduces scraps
            if (scraps > 0) scraps--;
        }

        /// <summary>
        /// Player shoots a bullet in the direction specified
        /// </summary>
        /// <param name="origin">Coordinates of the origin</param>
        /// <param name="target">Coordinates of the target</param>
        /// <param name="speed">Speed value</param>
        /// <param name="damage">Damage value</param>
        public void Shoot(Vector2 origin, Vector2 target, int speed, int damage)
        {
            if(scraps == 0) { return; }

            immuneCounter = 0.5;

            scraps--;

            // calculate direction to mouse pos
            Vector2 direction = Vector2.Normalize(target - origin);

            // instantiate bullet at the player's pos with the calculated direction
            BulletManager.Instance.Add(this, origin, direction, speed, damage);
        }
    }
}
