using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    public class Animator
    {
        protected Dictionary<object, Animation> animations;
        protected object lastKey;

        public Animator() 
        { 
            animations = new Dictionary<object, Animation>();
            lastKey = null;
        }

        public Animator(Dictionary<object, Animation> animations)
        {
            this.animations = animations;
            lastKey = null;
        }

        public void AddAnimation(object key, Animation animation)
        {
            animations.Add(key, animation);
            if (lastKey == null) { lastKey = key; }
        }

        public void Update(GameTime gt, object key)
        {
            if (animations.ContainsKey(key))
            {
                animations[key].Start();
                animations[key].Update(gt);
                lastKey = key;
            }
            else
            {
                animations[lastKey].Stop();
                animations[lastKey].Reset();
            }
        }

        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            if(lastKey == null) { return; }
            animations[lastKey].Draw(sb, pos);
        }
    }
}
