using Lightweight;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

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
    private Texture2D hitBoxTex;
    private static BulletManager instance;
    private bool freeze;

    public bool Freeze { get { return freeze; }set { freeze = value; } }

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

    public void LoadTextures(ContentManager content)
    {
        bulletTexture = content.Load<Texture2D>("rsz_plain-circle1");
        hitBoxTex = content.Load<Texture2D>("hitbox");
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

    public void Add(IShoot source, Vector2 origin, Vector2 direction, int speed, int damage)
    {
        bullets.Add(new Bullet(source, origin, direction, speed, damage, hitBoxTex));
    }
    
    /// <summary>
    /// Method that removes a bullet from bullet list
    /// </summary>
    /// <param name="bullet">Bullet instance</param>
    public void Remove(Bullet bullet)
    {
        bullets.Remove(bullet);
    }

    public void Update(GameTime gt)
    {
        if(!Game1.Instance.GodMode) freeze = false;
        if (freeze) { return; }
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].Update();
        }
    }

    public void Draw(SpriteBatch sb)
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            bullets[i].Draw(sb);
            if (Game1.Instance.GodMode)
            {
                sb.Draw(hitBoxTex, bullets[i].HitBox, Color.Red);
            }
        }
    }
}
