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
    /// Whether this object is currently active 
    /// </summary>
    private bool isAlive;

    /// <summary>
    /// Instantiate a new bullet at the given position in the given direction
    /// </summary>
    /// <param name="texture">The texture to aapply to this bullet object</param>
    /// <param name="position">The position at which to instantiate the bullet</param>
    /// <param name="direction">The direction for the bullet to travel in</param>
    public Bullet(Texture2D texture, Vector2 position, Vector2 direction) : base(texture, position)
    {
        this.direction = direction;
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
        position += direction;

        // if the bullet has gone off-screen, remove from game
        if (position.X < 0 || position.X > windowWidth ||
            position.Y < 0 || position.Y > windowHeight)
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