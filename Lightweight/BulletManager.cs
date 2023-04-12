using System;
using System.Collections.Generic;

public class BulletManager
{
    /// <summary>
    /// List of active bullets
    /// </summary>
    private static List<Bullet> bullets;
    public static List<Bullet> Bullets
    {
        get { return bullets; }
        set { bullets = value; }
    }

    public BulletManager()
	{

	}

    public static void Add(Bullet bullet)
    {
        bullets.Add(bullet);
    }

}
