using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Class that creates a base game object
    /// </summary>
    public abstract class GameObject
    {
        //Fields within GameObject
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle rectangle;
        protected int windowWidth;
        protected int windowHeight;
        
        /// <summary>
        /// Parameterised constructor that uses Vector2
        /// </summary>
        /// <param name="texture">Texture</param>
        /// <param name="position">Position</param>
        public GameObject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        /// <summary>
        /// Constructor for GameObject that uses a rectangle instead of a vector2
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="rectangle"></param>
        public GameObject(Texture2D texture, Rectangle rectangle) 
        {
            this.texture = texture;
            this.rectangle = rectangle;
        }

        /// <summary>
        /// Update Method to be overridden
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Draw method to be overridden
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public abstract void Draw(SpriteBatch sb);
    }
}
