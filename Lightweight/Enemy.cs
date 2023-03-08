using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
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
