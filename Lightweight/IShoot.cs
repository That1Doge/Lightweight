using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal interface IShoot
    {
        /// <summary>
        /// Instantiate a new bullet object at this object's position
        /// moving in the direction of the target object
        /// </summary>
        /// <param name="target">The object that the bullet is directed towards</param>
        public void Shoot(GameObject target);
    }
}
