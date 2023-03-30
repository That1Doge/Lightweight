using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;

namespace Lightweight
{
    internal interface ICollidable
    {
       public Rectangle HitBox { get; }
       public bool Intersect(Rectangle hitbox);
    }
}
