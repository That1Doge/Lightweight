using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    /// <summary>
    /// Class that handles what the Menu Buttons look like, what text they have,
    ///     and what position they are at
    /// </summary>
    internal class MenuButton
    {
        // Fields for the menu buttons
        private MouseState prevMState;
        private Rectangle rect;
        private SpriteFont text;
        private Texture2D texture;


        public Rectangle Rectangle
        {
            get { return rect; }
        }

        /// <summary>
        /// Parameterized constructor for the button, giving it the looks of the button
        /// </summary>
        /// <param name="texture">The image for the button</param>
        /// <param name="rect">the size of the button</param>
        /// <param name="position">the position of the button</param>
        public MenuButton(Texture2D texture, SpriteFont text, Rectangle rect)
        {
            this.texture = texture;
            this.rect = rect;
            this.text = text;
        }

        /// <summary>
        /// Detects whether or not the button was clicked by the player
        /// </summary>
        /// <returns>boolean value of whether or not the button was clicked</returns>
        public bool ButtonClicked()
        {
            // Initializes the mouse state 
            MouseState mState = Mouse.GetState();

            // If the mouse left button, the previous state is released, and is on the button,
            //     then return true
            if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed &&
                rect.Contains(mState.Position))
            {
                // Gets the current mouse state and sets it to previous state
                prevMState = mState;
                return true;
            }
            else
            {
                // Gets the current mouse state and sets it to previous state
                prevMState = mState;
                return false;
            }
        }

        /// <summary>
        /// Method that draws the button
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            // Draws the button with its texture, at the positon, with a hitbox of a rectangle, and the text above it
            sb.Draw(
                texture,
                rect,
                Color.White);
            sb.DrawString(
                text,
                "test",
                new Vector2(rect.X, rect.Y),
                Color.Black);
        }

        /// <summary>
        /// Update for the button by frame
        /// </summary>
        public void Update()
        {
           // Checks if the button is clicked every frame
            this.ButtonClicked();
        }
        
    }
}
