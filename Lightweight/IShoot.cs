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
    public interface IShoot
    {
        /// <summary>
        /// Instantiate a new bullet object at this object's pos
        /// moving in the direction of the target object
        /// </summary>
        /// <param name="origin">The pos for the bullet to move towards</param>
        /// <param name="target">The pos for the bullet to move towards</param>
        /// <param name="speed">how fast the bullet moves</param>
        /// <param name="damage">How much damage the bullet will do</param>
        public void Shoot(Vector2 origin, Vector2 target, int speed, int damage);
    }
}
