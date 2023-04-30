using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Animator class
    /// </summary>
    public class Animator
    {

        //Fields used in class
        protected Dictionary<object, Animation> animations;
        protected object lastKey;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Animator() 
        { 
            animations = new Dictionary<object, Animation>();
            lastKey = null;
        }


        /// <summary>
        /// Animator constructor that takes in a dictionary
        /// </summary>
        /// <param name="animations">Animation dictionary</param>
        public Animator(Dictionary<object, Animation> animations)
        {
            this.animations = animations;
            lastKey = null;
        }

        /// <summary>
        /// Method that adds an animation to animation dictionary
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <param name="animation">Animation</param>
        public void AddAnimation(object key, Animation animation)
        {
            animations.Add(key, animation);
            if (lastKey == null) { lastKey = key; }
        }

        /// <summary>
        /// Update method for animator
        /// </summary>
        /// <param name="gt">Gametime</param>
        /// <param name="key">Dictionary key</param>
        public void Update(GameTime gt, object key)
        {
            //If dictionary contains key
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

        /// <summary>
        /// Draw method for animator
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        /// <param name="pos">Position</param>
        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            if(lastKey == null) { return; }
            animations[lastKey].Draw(sb, pos);
        }
    }
}
