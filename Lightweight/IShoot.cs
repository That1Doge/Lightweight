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
        /// <param name="target">The object that the bullet is directed towards</param>
        public void Shoot(GameObject target);

        // 1. finish functional IShoot interface
        // 2. create bullet class instantiated by IShoot
        // 3. implement into Player (and Enemy) classes
    }
}
