﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Player Anims Class
    /// Animates the player sprite
    /// </summary>
    internal class PlayerAnimator : Animator
    {
        private Player player;

        public PlayerAnimator(Player player) 
            : base()
        {
            this.player = player;
        }

        public void Update(GameTime gt, object key, float fps)
        {
            if(animations.ContainsKey(key))
            {
                animations[key].Start();
                animations[key].Fps = fps;
                animations[key].Update(gt);
                if (key is (object)PlayerState.RollLeft 
                    or (object)PlayerState.RollRight)
                {
                    if (animations[key].Ended) { player.Controller.IsRolling = false; }
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
