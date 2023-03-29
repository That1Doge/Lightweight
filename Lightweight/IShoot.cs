using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    /// <summary>
    /// This object can shoot bullets
    /// </summary>
    internal interface IShoot
    {
        /// <summary>
        /// Instantiate a new bullet object at this object's position
        /// moving in the direction of the target object
        /// </summary>
        /// <param name="texture">The texture to apply to the bullet</param>
        /// <param name="position">The position at which to spawnn a bullet</param>
        public void Shoot(Texture2D texture, Vector2 position);
    }
}
