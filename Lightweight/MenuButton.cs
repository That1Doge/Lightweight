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
    internal class MenuButton : GameObject
    {
        // Fields for the menu buttons
        private MouseState prevMState;
        private Rectangle rect;

        /// <summary>
        /// Parameterized constructor for the button, giving it the looks of the button
        /// </summary>
        /// <param name="texture">The image for the button</param>
        /// <param name="rect">the size of the button</param>
        /// <param name="position">the position of the button</param>
        public MenuButton(Texture2D texture, Rectangle rect, Vector2 position) : 
            base(texture, position)
        {
            this.texture = texture;
            this.rect = rect;
            this.position = position;
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
        public override void Draw(SpriteBatch sb)
        {
            // Draws the button with its texture, at the positon, with a hitbox of a rectangle
            sb.Draw(
                texture,
                position,
                rect,
                Color.White);
        }

        /// <summary>
        /// Update for the button by frame
        /// </summary>
        public override void Update()
        {
           // Checks if the button is clicked every frame
            this.ButtonClicked();
        }
    }
}
