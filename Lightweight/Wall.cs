using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal class Wall : GameObject, ICollidable
    {
        public int X { get { return this.rectangle.X; } set { this.rectangle.X = value; } }
        public int Y { get { return this.rectangle.Y; } set { this.rectangle.Y = value; } }

        /// <summary>
        /// Parameterized constructor that takes texture and rectangle
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="rectangle"></param>
        public Wall(Texture2D texture, Rectangle rectangle, int windowHeight, int windowWidth)
            : base(texture, rectangle, windowWidth, windowHeight)
        { 
            this.texture = texture;
            this.rectangle = rectangle;
        }


        /// <summary>
        /// Method that returns a boolean depending if hitboxes collide
        /// </summary>
        /// <param name="thing">any object that intersects with the level object</param>
        /// <returns></returns>
        public bool Intersects(GameObject thing) 
        { 
            if (this.Intersects(thing)) 
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
            sb.Draw(this.texture, this.rectangle, Color.White);
        }
    }
}
