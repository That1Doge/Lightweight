using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal class Tile : GameObject
    {
        /// <summary>
        /// X position property
        /// </summary>
        public int X { get { return this.rectangle.X; } set { this.rectangle.X = value; } }

        /// <summary>
        /// Y position property
        /// </summary>
        public int Y { get { return this.rectangle.Y; } set { this.rectangle.Y = value; } }

        /// <summary>
        /// Tile parameterized constructor
        /// </summary>
        /// <param name="texture">Desired tile texture</param>
        /// <param name="rectangle">Desired rectangle</param>
        public Tile(Texture2D texture, Rectangle rectangle)
            : base(texture, rectangle)
        {
            this.texture = texture;
            this.rectangle = rectangle;
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
    }
}
