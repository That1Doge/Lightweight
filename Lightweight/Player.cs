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

        public int Scraps => scraps;

        public float Speed { get { return speed; } set { speed = value; } }

        private Vector2 position = new Vector2(400, 240);
        private float speed;
        private PlayerAnimator anims = new PlayerAnimator();

        public Player()
        {
            scraps = 10;
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
            if (PlayerController.SingleKeyPress(Keys.O)) scraps--;
            if (PlayerController.SingleKeyPress(Keys.Enter)) EnemyManager.Instance.SpawnEnemies(1, Vector2.Zero);
            if (scraps <= 0) scraps = 1;
            speed = 1f/scraps;
            anims.Update(gt, PlayerController.PlayerState, (1f / scraps) * 128);
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
            //damage taken is reduced by defense of player,
            //possibly modified by armor or similar attributes
            this.playerHealth = playerHealth - (damage - defense);
        }

        /// <summary>
        /// Player shoots a bullet in the direction specified
        /// </summary>
        /// <param name="origin">Coordinates of the origin</param>
        /// <param name="target">Coordinates of the target</param>

        public void Shoot(Vector2 origin, Vector2 target, int speed, int damage)
        {
            // calculate direction to mouse pos
            Vector2 direction = Vector2.Normalize(target - origin);

            // instantiate bullet at the player's position with the calculated direction
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
