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

namespace Lightweight
{
    public class Player : IMove, ITakeDamage, ICollidable
    {        //Fields used in class
        int playerHealth;
        int playerDefense;
        int scraps;
        private Rectangle hitBox;
        private Texture2D hitBoxTex;
        private Texture2D bulletTex;
        private double immuneCounter;

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
        
        public Vector2 Position { get { return position; } }

        /// <summary>
        /// Returns hitbox of player (for collision)
        /// </summary>
        public Rectangle HitBox { get { return hitBox; } }
        public int X { get { return (int)position.X; } set { position.X = value; } }
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }

        public int Scraps { get { return scraps; } set { scraps = value; } }

        public float Speed { get { return speed; } set { speed = value; } }

        private Vector2 position = new Vector2(400, 240);
        private float speed;
        private PlayerAnimator anims = new PlayerAnimator();

        public Player()
        {
            scraps = 10;
            playerHealth = 100;
            hitBox = new Rectangle((int)position.X + 5, (int)position.Y + 10, 31, 44);
            PlayerController.Player = this;
        }

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

        public void Update(GameTime gt)
        {
            PlayerController.Update(gt);

            position += PlayerController.Direction * this.Speed
                * (float)gt.ElapsedGameTime.TotalSeconds * 1000f;

            hitBox.X = (int)position.X + 5;
            hitBox.Y = (int)position.Y + 10;

            if (PlayerController.SingleKeyPress(Keys.P)) scraps++;
            if (PlayerController.SingleKeyPress(Keys.O) && scraps > 0) scraps--;
            if (PlayerController.SingleKeyPress(Keys.Enter)) EnemyManager.Instance.SpawnEnemies(1, Vector2.Zero);
            speed = 1f/(scraps+2);
            anims.Update(gt, PlayerController.PlayerState, (1f / (scraps+2)) * 128);

            if(immuneCounter > 0)
            {
                immuneCounter -= gt.ElapsedGameTime.TotalSeconds;
            }

            for(int i = 0; i < BulletManager.Bullets.Count; i++)
            {
                if (hitBox.Intersects(BulletManager.Bullets[i].HitBox) && immuneCounter <= 0 && !PlayerController.IsRolling)
                {
                    ITakeDamage(BulletManager.Bullets[i].Damage, 0);
                    BulletManager.Remove(BulletManager.Bullets[i]);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            anims.Draw(sb, position);
            sb.Draw(hitBoxTex, hitBox, Color.Pink);
        }

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

        public void ITakeDamage(int damage, int defense)
        {
            if (PlayerController.IsRolling) { return; }
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

        public void Shoot(Vector2 origin, Vector2 target, int speed, int damage)
        {
            if(scraps == 0) { return; }

            immuneCounter = 0.5;

            scraps--;

            // calculate direction to mouse pos
            Vector2 direction = Vector2.Normalize(target - origin);

            // instantiate bullet at the player's pos with the calculated direction
            Bullet bullet = new Bullet(origin, direction, speed, damage);

            // implement bullets list and add bullet to list
            BulletManager.Add(bullet);
        }

        public void Move(Direction direction)
        {
            throw new NotImplementedException();
        }

        /*
        public void Move(Direction dir)
        {

        }
        */


    }
}
