using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    /// <summary>
    /// This object can recieve damage from attacks
    /// </summary>
    internal interface ITakeDamage
    {
        void ITakeDamage(int damage, int damageReduction);
    }
}
