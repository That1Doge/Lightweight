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
    /// Enemy Class 
    /// Creates a single instance of a enemy
    /// </summary>
    public class Enemy : ITakeDamage
    {
        private int enemyHealth;
        public void ITakeDamage(int damage, int defense)
        {
            //defense is half as effective on enemies
            this.enemyHealth = enemyHealth - (damage - defense / 2);
        }
    }
}
