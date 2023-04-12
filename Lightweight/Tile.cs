using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Tile Class 
    /// Creates a single instance of a tile
    /// </summary>
    internal class Tile : GameObject, ICollidable
    {
        bool isTrap;
        Rectangle hitbox;

        /// <summary>
        /// X position property
        /// </summary>
        public int X { get { return this.rectangle.X; } set { this.rectangle.X = value; } }

        /// <summary>
        /// Y position property
        /// </summary>
        public int Y { get { return this.rectangle.Y; } set { this.rectangle.Y = value; } }
        public Rectangle HitBox { get { return hitbox; } }
        public bool IsTrap { get { return isTrap; } }

        /// <summary>
        /// Tile parameterized constructor
        /// </summary>
        /// <param name="texture">Desired tile texture</param>
        /// <param name="rectangle">Desired rectangle</param>
        public Tile(Texture2D texture, Rectangle rectangle, bool isTrap)
            : base(texture, rectangle)
        {
            this.texture = texture;
            hitbox = rectangle;
            this.isTrap = isTrap;
        }

        /// <summary>
        /// Update override for tile class
        /// </summary>
        public override void Update() 
        { 
            
        }

        /// <summary>
        /// Draws tile to window
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb) 
        {
            sb.Draw(this.texture, this.rectangle, Color.White);
        }

        public bool Intersect(Rectangle hitbox)
        {
            if (this.HitBox.Intersects(hitbox) && isTrap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
