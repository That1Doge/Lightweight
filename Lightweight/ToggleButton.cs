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
    /// A button class that toggles an on/off state
    /// </summary>
    internal class ToggleButton 
    {
        // The only fields needed for the toggled button
        private Rectangle rect;
        private Texture2D textureOn;
        private Texture2D textureOff;
        private bool isOn;
        private MouseState prevMState;

        /// <summary>
        /// A get only property for the Rectangle
        /// </summary>
        public Rectangle Rectangle
        {
            get { return rect; }
        }

        /// <summary>
        /// A get and set property for if the toggle is on or not
        /// </summary>
        public bool IsOn
        {
            get { return isOn; }
            set { isOn = value; }
        }

        /// <summary>
        /// A Toggle Button constructor for the toggleable options
        /// </summary>
        /// <param name="rect">the boundaries of the button</param>
        /// <param name="texture">the image of the button</param>
        public ToggleButton(Rectangle rect, Texture2D textureOn, Texture2D textureOff)
        {
            this.rect = rect;
            this.textureOn = textureOn;
            this.textureOff = textureOff;
            isOn = false;
        }

        /// <summary>
        /// Will find if the toggled button is on or not
        /// </summary>
        /// <returns>the state of the toggle</returns>
        public bool isClicked()
        {
            // Initializes the mouse state 
            MouseState mState = Mouse.GetState();

            // If the mouse left button, the previous state is released, and is on the button,
            //     then return true
            if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed &&
                rect.Contains(mState.Position))
            {
                if(isOn)
                {
                    // Gets the current mouse state and sets it to previous state
                    prevMState = mState;
                    isOn = false;
                    return isOn;
                }
                else
                {
                    // Gets the current mouse state and sets it to previous state
                    prevMState = mState;
                    isOn = true;
                    return isOn;
                }
            }
            else
            {
                // Gets the current mouse state and sets it to previous state
                prevMState = mState;
                return isOn;
            }
        }

        /// <summary>
        /// Draws a toggle button that turns on or off and has text to the right of it
        /// </summary>
        /// <param name="sb">the sprite holder</param>
        /// <param name="text">text next to button</param>
        public void Draw(SpriteBatch sb, SpriteFont font,string text)
        {
            if(isOn)
            {
                sb.Draw(textureOn, rect, Color.White);
            }
            else
            {
                sb.Draw(textureOff, rect, Color.White);
            }
            sb.DrawString(font, text, new Vector2((rect.X + rect.Width) + 10, rect.Y + rect.Height/3), Color.White);

        }

    }
}
