﻿using Microsoft.Xna.Framework.Graphics;
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
            for (int x = 0; x < 10; x++)
            {
                sb.Draw(this.texture, this.rectangle, Color.White);
                this.Y += this.rectangle.Height - 10;
            }
            this.Y += 15;
            this.X += 5;
            for (int y = 0; y < 21; y++) 
            {
                sb.Draw(this.texture, this.rectangle, null, Color.White, 4.71f, Vector2.Zero, SpriteEffects.None, 0.0f);
                this.X += this.rectangle.Width + 25;
            }
            this.X += 18;
            this.Y -= 5;
            for (int x = 0; x < 10; x++)
            {
                sb.Draw(this.texture, this.rectangle, null, Color.White, 3.15f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                this.Y -= this.rectangle.Height - 10;
            }
        }
    }
}
