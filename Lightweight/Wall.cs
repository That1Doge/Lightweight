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

    internal class Wall : GameObject, ICollidable
    {
        //Field used in class
        Rectangle hitBox;

        /// <summary>
        /// X position property
        /// </summary>
        public int X { get { return this.rectangle.X; } set { this.rectangle.X = value; } }

        /// <summary>
        /// Y position property
        /// </summary>
        public int Y { get { return this.rectangle.Y; } set { this.rectangle.Y = value; } }

        /// <summary>
        /// Hitbox property
        /// </summary>
        public Rectangle HitBox { get { return hitBox; } }

        /// <summary>
        /// Parameterized constructor that takes texture and rectangle
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="rectangle"></param>
        public Wall(Texture2D texture, Rectangle rectangle)
            : base(texture, rectangle)
        { 
            this.texture = texture;
            hitBox = rectangle;
        }


        /// <summary>
        /// Method that returns a boolean depending if hitboxes collide
        /// </summary>
        /// <param name="thing">any object that intersects with the level object</param>
        /// <returns></returns>
        public bool Intersect(Rectangle hitbox) 
        { 
            if (this.rectangle.Intersects(hitbox)) 
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// Method that overrides update
        /// </summary>
        public override void Update() 
        { 
            
        }

        /// <summary>
        /// Method that draws background
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public override void Draw(SpriteBatch sb) 
        {
            sb.Draw(this.texture, HitBox, Color.White);
        }
    }
}
