using Lightweight;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Trap : ICollidable
{
	public Rectangle hitbox;

	public Rectangle HitBox { get {  return hitbox; } }

	public Trap()
	{
		
	}

	public bool Intersect(Rectangle rectangle) 
	{
        if (hitbox.Intersects(rectangle))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
