using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>
/// 
namespace Lightweight
{
    /// <summary>
    /// Animation class
    /// </summary>
    public class Animation
    {
        // texture data
        private Texture2D spriteSheet;

        // source rectangles for each frame
        private List<Rectangle> sourceRects = new List<Rectangle>();

        // no. of frames & current frame
        private int numFrames;
        private int frame;

        // how much time has elapsed + time per frame
        private float frameTime;
        private float totalTime;

        // turns on/off animation
        private bool active;

        // signals if looping
        private bool loop;

        // signals when animation ended
        public bool Ended => !active;

        // fps
        private float fps;

        // changes fps to reflect speed
        public float Fps { get { return fps; } set { fps = value; } }

        // flip
        private SpriteEffects flip;

        /// <summary>
        /// Parameterized constructor for animation class
        /// </summary>
        /// <param name="spriteSheet">Spritesheet</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="flip">Flip</param>
        /// <param name="loop">Loop</param>
        /// <param name="fps">Frames per second</param>
        public Animation(Texture2D spriteSheet, int numFrames,
            SpriteEffects flip = SpriteEffects.None, bool loop = true, float fps = 24)
        {
            this.spriteSheet = spriteSheet;
            this.numFrames = numFrames;
            this.flip = flip;
            this.loop = loop;
            this.fps = fps;
            frameTime = 1 / this.fps;

            int frameWidth = spriteSheet.Width / numFrames;
            int frameHeight = spriteSheet.Height;

            for(int i = 0; i < numFrames; i++)
            {
                sourceRects.Add(new Rectangle(
                    i * frameWidth, 0, frameWidth, frameHeight));
            }
        }

        /// <summary>
        /// Start method
        /// </summary>
        public void Start() { active = true; }

        /// <summary>
        /// Stop method
        /// </summary>
        public void Stop() { active = false; }

        /// <summary>
        /// Reset Method
        /// </summary>
        public void Reset() { frame = 0; totalTime = 0; }

        /// <summary>
        /// Update method for animation
        /// </summary>
        /// <param name="gt">Gametime</param>
        public void Update(GameTime gt)
        {
            //Animation
            if(!active) return;
            frameTime = 1 / fps;
            totalTime += (float)gt.ElapsedGameTime.TotalSeconds;
            if(totalTime >= frameTime)
            {
                frame++;
                if(frame == numFrames)
                {
                    if (!loop) active = false;
                    frame = 0;
                }
                totalTime = 0;
            }
        }

        /// <summary>
        /// Draw method for animation class
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        /// <param name="pos">Position</param>
        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            sb.Draw(
                spriteSheet, pos, sourceRects[frame], Color.White,
                0.0f, Vector2.Zero, Vector2.One, flip,
                0.0f);
        }
    }
}
