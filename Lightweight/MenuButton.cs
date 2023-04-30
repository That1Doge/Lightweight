using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    /// Enumerator for where the text is being aligned horizontally
    /// </summary>
    public enum TextHorizontal { LeftAligned, CenterAligned, RightAligned }
    
    /// <summary>
    /// Enumerator for where the text is being aligned vertically
    /// </summary>
    public enum TextVertical { TopAligned, CenterAligned, BottomAligned }

    /// <summary>
    /// Class that handles what the Menu Buttons look like, what text they have,
    /// and what pos they are at
    /// </summary>
    internal class MenuButton
    {
        // Fields for the menu buttons
        private MouseState prevMState;
        private Rectangle rect;
        private SpriteFont text;
        private Texture2D texture;

        /// <summary>
        /// Gets the rectangle of the button
        /// </summary>
        public Rectangle Rectangle
        {
            get { return rect; }
        }

        /// <summary>
        /// Parameterized constructor for the button, giving it the looks of the button
        /// </summary>
        /// <param name="texture">The image for the button</param>
        /// <param name="rect">the size of the button</param>
        /// <param name="position">the pos of the button</param>
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
        /// Draws the button with its text center aligned
        /// </summary>
        /// <param name="spriteBatch">game's sprite batch</param>
        /// <param name="value">what is displayed on the button</param>
        /// <param name="renderArea">the rectangle the text is going in</param>
        /// <param name="h">horizontal alignment of text</param>
        /// <param name="v">vertical alignment of text</param>
        public void Render(SpriteBatch spriteBatch, string value, Rectangle renderArea,
        TextHorizontal h = TextHorizontal.CenterAligned, TextVertical v = TextVertical.CenterAligned)
        {

            var location = new Vector2(renderArea.X, renderArea.Y);

            var size = text.MeasureString(value);

            switch (h)
            {
                case TextHorizontal.CenterAligned:
                    location.X += (renderArea.Width - size.X) / 1.9f;
                    break;
                case TextHorizontal.RightAligned:
                    location.X += renderArea.Width - size.X;
                    break;
            }

            switch (v)
            {
                case TextVertical.CenterAligned:
                    location.Y += (renderArea.Height - size.Y) / 1.9f;
                    break;
                case TextVertical.BottomAligned:
                    location.Y += renderArea.Height - size.Y;
                    break;
            }
            spriteBatch.Draw(
                texture,
                rect,
                Color.White);
            spriteBatch.DrawString(text, value, location, Color.Black);
        }

    }
}
