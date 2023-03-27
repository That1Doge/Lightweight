using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal class PlayerAnimator
    {
        private Dictionary<object, Animation> animations = new();
        private object lastKey;

        public void AddAnimation(object key, Animation animation)
        {
            animations.Add(key, animation);
            if (lastKey == null) { lastKey = key; }
        }

        public void Update(GameTime gt, object key)
        {
            if(animations.ContainsKey(key))
            {
                animations[key].Start();
                animations[key].Update(gt);
                if (key is (object)PlayerState.RollLeft 
                    or (object)PlayerState.RollRight)
                {
                    if (animations[key].Ended) { PlayerController.IsRolling = false; }
                }
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
            animations[lastKey].Draw(sb, pos);
        }
    }
}
