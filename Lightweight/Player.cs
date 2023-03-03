using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    public class Player : GameObject, IShoot, IMove, ITakeDamage
    {

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)        
        {   
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
