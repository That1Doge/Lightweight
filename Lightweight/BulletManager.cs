using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

/// <summary>
/// Class that manages bullets
/// </summary>
public class BulletManager
{
    //Fields used in class
    private List<Bullet> bullets = new List<Bullet>();
    private Texture2D bulletTexture;
    private static BulletManager instance;

    /// <summary>
    /// Instance of bullet manager
    /// </summary>
    public static BulletManager Instance
    {
        get
        {
            if(instance == null) instance = new BulletManager();
            return instance;
        }
    }

    /// <summary>
    /// Get/set property for list of bullets
    /// </summary>
    public List<Bullet> Bullets
    {
        get { return bullets; }
        set { bullets = value; }
    }

    /// <summary>
    /// Get/set propety for bullet texture
    /// </summary>
    public Texture2D BulletTexture
    {
        get { return bulletTexture; }
        set { bulletTexture = value; }
    }

    /// <summary>
    /// Method that adds a bullet to bullet list
    /// </summary>
    /// <param name="bullet">Bullet instance</param>
    public void Add(Bullet bullet)
    {
        bullets.Add(bullet);
    }
    
    /// <summary>
    /// Method that removes a bullet from bullet list
    /// </summary>
    /// <param name="bullet">Bullet instance</param>
    public void Remove(Bullet bullet)
    {
        bullets.Remove(bullet);
    }
}
