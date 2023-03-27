using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    //public class Player : GameObject, IShoot, IMove, ITakeDamage
    //{
    //    int playerHealth;
    //    int playerDefense;

    //    public int PlayerHealth
    //    {
    //        get { return playerHealth; }
    //        set { playerHealth = value; }
    //    }

    //    public int PlayerDefense
    //    {
    //        get { return playerDefense; }
    //        set { playerDefense = value; }
    //    }

    //    public Player(Texture2D texture, Vector2 position)
    //        : base(texture, position)        
    //    {   
    //    }

    //    public override void Draw(SpriteBatch sb)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Update()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public void ITakeDamage(int damage, int defense)
    //    {
    //        //damage taken is reduced by defense of player,
    //        //possibly modified by armor or similar attributes
    //        this.playerHealth = playerHealth - (damage - defense);
    //    }
    //}
}
