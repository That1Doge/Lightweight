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
    public class Player : IShoot, IMove, ITakeDamage, ICollidable
    {
        //Fields used in class
        int playerHealth;
        int playerDefense;
        int scraps;
        private Rectangle hitBox;
        private Texture2D hitBoxTex;

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
        /// Returns hitbox of player (for collision)
        /// </summary>
        public Rectangle HitBox { get { return hitBox; } }
        public int X { get { return (int)position.X; } set { position.X = value; } }
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }

        public int Scraps => scraps;

        public float Speed { get { return speed; } set { speed = value; } }

        private Vector2 position = new Vector2(100, 100);
        private float speed;
        private PlayerAnimator anims = new PlayerAnimator();

        public Player()
        {
            scraps = 10;
            hitBox = new Rectangle((int)position.X + 5, (int)position.Y + 10, 31, 44);
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
                * (float)gt.ElapsedGameTime.TotalSeconds;

            hitBox.X = (int)position.X + 5;
            hitBox.Y = (int)position.Y + 10;

            if (PlayerController.SingleKeyPress(Keys.P)) scraps++;
            if (PlayerController.SingleKeyPress(Keys.O)) scraps--;
            if (scraps <= 0) scraps = 1;
            speed = 1f/scraps * 1000f;
            anims.Update(gt, PlayerController.PlayerState, (1f / scraps) * 128); ;
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

        public void Shoot(Texture2D texture, Vector2 position)
        {
            // get current mouse position
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            // calculate direction to mouse position
            Vector2 direction = Vector2.Normalize(mousePosition - position);

            // instantiate bullet at the player's position with the calculated direction
            Bullet bullet = new Bullet(texture, position, direction);

            // TODO: implement bullets list and add bullet to list
            // bullets.Add(bullet);
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
