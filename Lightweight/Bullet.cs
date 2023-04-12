using Lightweight;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Bullet Class 
/// Creates projectile bullet
/// </summary>
public class Bullet : GameObject
{
    /// <summary>
    /// Direction vector for the path of the bullet
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// Speed of the bullet per frame
    /// </summary>
    private int speed;

    private int damage;

    /// <summary>
    /// Whether this object is currently active 
    /// </summary>
    private bool isAlive;

    public bool IsAlive{ get { return isAlive; } }

    /// <summary>
    /// Instantiate a new bullet at the given position in the given direction
    /// </summary>
    /// <param name="position">The position at which to instantiate the bullet</param>
    /// <param name="direction">The direction for the bullet to travel in</param>
    public Bullet(Vector2 position, Vector2 direction, int speed,int damage) : base(BulletManager.BulletTexture, position)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        isAlive = true;
    }

    /// <summary>
    /// Collision method for bullet
    /// </summary>
    /// <param name="target">Desired target</param>
    /// <returns></returns>
    public bool Intersect(Rectangle target)
    {
        // Check if position is inside rectangle bounds
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
        // don't update if bullet isn't active
        if (!isAlive) return;

        // update the bullet's position in the given direction
        position += direction * speed;

        // if the bullet has gone off-screen, remove from game
        if (position.X < 0 || position.X > Game1.WindowWidth ||
            position.Y < 0 || position.Y > Game1.WindowHeight)
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