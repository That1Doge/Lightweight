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
    /// Take Damage interface 
    /// This object can recieve damage from attacks
    /// </summary>
    internal interface ITakeDamage
    {
        void ITakeDamage(int damage);
    }
}
