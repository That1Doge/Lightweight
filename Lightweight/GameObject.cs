using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lightweight
{
    public abstract class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle rectangle;
        protected int windowWidth;
        protected int windowHeight;

        public GameObject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        /// <summary>
        /// Constructor for GameObject that uses a rectangle instead of a vector2
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="rectangle"></param>
        public GameObject(Texture2D texture, Rectangle rectangle) 
        {
            this.texture = texture;
            this.rectangle = rectangle;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch sb);
    }
}
