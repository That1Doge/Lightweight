using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class BulletManager
{
    /// <summary>
    /// List of active bullets
    /// </summary>
    private static List<Bullet> bullets = new();
    private static Texture2D bulletTexture;

    public static List<Bullet> Bullets
    {
        get { return bullets; }
        set { bullets = value; }
    }

    public static Texture2D BulletTexture
    {
        get { return bulletTexture; }
        set { bulletTexture = value; }
    }

    public BulletManager()
	{

	}

    public static void Add(Bullet bullet)
    {
        bullets.Add(bullet);
    }

}
