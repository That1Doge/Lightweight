using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    /// Shoot interface 
    /// This object can shoot
    /// </summary>
    internal interface IShoot
    {
        /// <summary>
        /// Instantiate a new bullet object at this object's position
        /// moving in the direction of the target object
        /// </summary>
        /// <param name="texture">The texture to apply to the bullet</param>
        /// <param name="origin">The position for the bullet to move towards</param>
        /// <param name="target">The position for the bullet to move towards</param>
        public void Shoot(Texture2D texture, Vector2 origin, Vector2 target);
    }
}
