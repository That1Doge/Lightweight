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
    public class Player : IMove, ITakeDamage, ICollidable, IShoot
    {        
        //Fields used in class
        int playerHealth;
        int playerDefense;
        int scraps;
        private Rectangle hitBox;
        private Texture2D hitBoxTex;
        private Texture2D bulletTex;
        private double immuneCounter;
        private PlayerController controller;

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
        private Vector2 position = new Vector2(400, 240);
        private float speed;
        private PlayerAnimator anims;

        /// <summary>
        /// Parameterised constructor of Player
        /// </summary>
        public Player()
        {
            scraps = 10;
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
            controller.Update(gt);

            position += controller.Direction * this.Speed
                * (float)gt.ElapsedGameTime.TotalSeconds * 1000f;

            hitBox.X = (int)position.X + 5;
            hitBox.Y = (int)position.Y + 10;

            //Single key presses that add to game
            if (controller.SingleKeyPress(Keys.P)) scraps++;
            if (controller.SingleKeyPress(Keys.O) && scraps > 0) scraps--;
            if (controller.SingleKeyPress(Keys.Enter)) EnemyManager.Instance.SpawnEnemies(1, Vector2.Zero);
            speed = 1f/(scraps+2);
            anims.Update(gt, controller.PlayerState, (1f / (scraps+2)) * 128);

            if(immuneCounter > 0)
            {
                immuneCounter -= gt.ElapsedGameTime.TotalSeconds;
            }

            for(int i = 0; i < BulletManager.Instance.Bullets.Count; i++)
            {
                if (hitBox.Intersects(BulletManager.Instance.Bullets[i].HitBox) && immuneCounter <= 0 && !controller.IsRolling)
                {
                    ITakeDamage(BulletManager.Instance.Bullets[i].Damage, 0);
                    BulletManager.Instance.Remove(BulletManager.Instance.Bullets[i]);
                }
            }
        }

        /// <summary>
        /// Draw method of player
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public void Draw(SpriteBatch sb)
        {
            anims.Draw(sb, position);
            sb.Draw(hitBoxTex, hitBox, Color.Pink);
        }

        /// <summary>
        /// Intersect method of player class
        /// </summary>
        /// <param name="rect">Rectangle to check</param>
        /// <returns>If hitbox has been intersected with</returns>
        public bool Intersect(Rectangle rect)
        {
            if (hitBox.Intersects(rect))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method that deals with taking damage
        /// </summary>
        /// <param name="damage">Damage to take</param>
        /// <param name="defense">Defense of player</param>
        public void ITakeDamage(int damage, int defense)
        {
            //Take no damage if rolling
            if (controller.IsRolling) { return; }

            //damage taken is reduced by defense of player,
            //possibly modified by armor or similar attributes
            this.playerHealth = playerHealth - (damage - defense);
            if(scraps > 0) scraps--;
            immuneCounter = 0.5;
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
            //If out of scraps, do not shoot
            if(scraps == 0) { return; }

            immuneCounter = 0.5;

            scraps--;

            // calculate direction to mouse pos
            Vector2 direction = Vector2.Normalize(target - origin);

            // instantiate bullet at the player's pos with the calculated direction
            Bullet bullet = new Bullet(this, origin, direction, speed, damage);

            // implement bullets list and add bullet to list
            BulletManager.Instance.Add(bullet);
        }

        /// <summary>
        /// Move method that is not implemented
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <exception cref="NotImplementedException">Error message that method is not implemendted</exception>
        public void Move(Direction direction)
        {
            throw new NotImplementedException();
        }
    }
}
