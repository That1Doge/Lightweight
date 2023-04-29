using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class BulletManager
{
    /// <summary>
    /// List of active bullets
    /// </summary>
    private List<Bullet> bullets = new List<Bullet>();
    private Texture2D bulletTexture;
    private static BulletManager instance;

    public static BulletManager Instance
    {
        get
        {
            if(instance == null) instance = new BulletManager();
            return instance;
        }
    }

    public List<Bullet> Bullets
    {
        get { return bullets; }
        set { bullets = value; }
    }

    public Texture2D BulletTexture
    {
        get { return bulletTexture; }
        set { bulletTexture = value; }
    }

    public void Add(Bullet bullet)
    {
        bullets.Add(bullet);
    }
    
    public void Remove(Bullet bullet)
    {
        bullets.Remove(bullet);
    }
}
