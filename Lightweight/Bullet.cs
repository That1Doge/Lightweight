﻿using Lightweight;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Bullet Class 
/// Creates projectile bullet
/// Lightweight
/// </summary>
public class Bullet : GameObject
{
    /// <summary>
    /// Direction vector for the path of the bullet
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// Hitbox texture
    /// </summary>
    private Texture2D hitBoxTex;

    /// <summary>
    /// Source of bullet
    /// </summary>
    private IShoot source;

    /// <summary>
    /// Property that returns source of bullet
    /// </summary>
    public IShoot Source { get { return source; } }

    /// <summary>
    /// Speed of the bullet per frame
    /// </summary>
    private int speed;

    /// <summary>
    /// Damage
    /// </summary>
    private int damage;

    /// <summary>
    /// Hotbox of bullet
    /// </summary>
    private Rectangle hitBox;

    /// <summary>
    /// Whether this object is currently active 
    /// </summary>
    private bool isAlive;

    /// <summary>
    /// Property that returns if bullet is alive
    /// </summary>
    public bool IsAlive{ get { return isAlive; } }

    /// <summary>
    /// Property that returns hitbox of bullet
    /// </summary>
    public Rectangle HitBox { get { return hitBox; } }

    /// <summary>
    /// Property that returns damage
    /// </summary>
    public int Damage { get { return damage; } }

    /// <summary>
    /// Instantiate a new bullet at the given pos in the given direction
    /// </summary>
    /// <param name="position">The pos at which to instantiate the bullet</param>
    /// <param name="direction">The direction for the bullet to travel in</param>
    public Bullet(IShoot source, Vector2 position, Vector2 direction, int speed,int damage, Texture2D hitBoxTex) : base(BulletManager.Instance.BulletTexture, position)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.source = source;
        isAlive = true;
        this.hitBoxTex = hitBoxTex;
        hitBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
    }

    /// <summary>
    /// Collision method for bullet
    /// </summary>
    /// <param name="target">Desired target</param>
    /// <returns>Returns if intersected with or not</returns>
    public bool Intersect(Rectangle target)
    {
        // Check if pos is inside rectangle bounds
        if (position.X >= target.X &&
            position.X <= target.X + target.Width &&
            position.Y >= target.Y &&
            position.Y <= target.Y + target.Height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Update bullet object
    /// </summary>
    public override void Update()
    {
        // remove if bullet isn't active
        if (!isAlive) BulletManager.Instance.Remove(this);

        // update the bullet's pos in the given direction
        position += direction * speed;
        hitBox.X = (int)position.X;
        hitBox.Y = (int)position.Y;

        // if the bullet has gone off-screen, remove from game
        if (position.X < 0 || position.X > Game1.Instance.WindowWidth ||
            position.Y < 0 || position.Y > Game1.Instance.WindowHeight)
        {
            isAlive = false;
        }
    }

    /// <summary>
    /// Draws bullet to window
    /// </summary>
    /// <param name="spriteBatch"></param>
    public override void Draw(SpriteBatch spriteBatch)
    {
        // don't draw if bullet isn't active
        if (!isAlive) return;

        // draw the bullet 
        spriteBatch.Draw(
            texture, 
            position, 
            Color.White);
    }
}