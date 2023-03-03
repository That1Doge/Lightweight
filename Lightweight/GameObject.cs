using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lightweight
{
    public abstract class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;

        public GameObject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch sb);
    }
}
