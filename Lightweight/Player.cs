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
    public class Player : IShoot, IMove, ITakeDamage
    {
        
        int playerHealth;
        int playerDefense;
        int scraps;

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
        /// A get only property for scraps
        /// </summary>
        public int Scraps
        {
            get { return scraps; }
        }

        public float Speed { get { return speed; } set { speed = value; } }

        private Vector2 position = new Vector2(100, 100);
        private float speed;
        private PlayerAnimator anims = new PlayerAnimator();

        public Player()
        {
            scraps = 10;
        }

        public void LoadAnims(ContentManager content)
        {
            anims.AddAnimation(PlayerState.RunRight, new Animation(
                content.Load<Texture2D>("PNG/Mage/Run/run"), 8));
            anims.AddAnimation(PlayerState.RunLeft, new Animation(
                content.Load<Texture2D>("PNG/Mage/Run/run"), 8, SpriteEffects.FlipHorizontally));
            anims.AddAnimation(PlayerState.RollRight, new Animation(
                content.Load<Texture2D>("PNG/Mage/High_Jump/roll"), 10, SpriteEffects.None, false));
            anims.AddAnimation(PlayerState.RollLeft, new Animation(
                content.Load<Texture2D>("PNG/Mage/High_Jump/roll"), 10, SpriteEffects.FlipHorizontally, false));
        }

        public void Update(GameTime gt)
        {
            PlayerController.Update(gt);

            position += PlayerController.Direction * this.Speed
                * (float)gt.ElapsedGameTime.TotalSeconds;

            anims.Update(gt, PlayerController.PlayerState);

            if (PlayerController.SingleKeyPress(Keys.P)) scraps++;
            if (PlayerController.SingleKeyPress(Keys.O)) scraps--;
            if (scraps <= 0) scraps = 10;
            speed = 1f/scraps * 1000f;
        }

        public void Draw(SpriteBatch sb)
        {
            anims.Draw(sb, position);
        }
        
        public void ITakeDamage(int damage, int defense)
        {
            //damage taken is reduced by defense of player,
            //possibly modified by armor or similar attributes
            this.playerHealth = playerHealth - (damage - defense);
        }   

        public void Shoot(GameObject target)
        {
            throw new NotImplementedException();
        }

        public void Move(Direction direction)
        {
            throw new NotImplementedException();
        }

        /*
        public void Shoot(GameObject bullet)
        {

        }


        public void Move(Direction dir)
        {

        }
        */
    }
}
