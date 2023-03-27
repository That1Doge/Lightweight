using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal class Animation
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

        // flip
        private SpriteEffects flip;

        public Animation(Texture2D spriteSheet, int numFrames,
            SpriteEffects flip = SpriteEffects.None, bool loop = true, float fps = 24)
        {
            this.spriteSheet = spriteSheet;
            this.numFrames = numFrames;
            this.flip = flip;
            this.loop = loop;
            frameTime = 1 / fps;

            int frameWidth = spriteSheet.Width / numFrames;
            int frameHeight = spriteSheet.Height;

            for(int i = 0; i < numFrames; i++)
            {
                sourceRects.Add(new Rectangle(
                    i * frameWidth, 0, frameWidth, frameHeight));
            }
        }

        public void Start() { active = true; }
        public void Stop() { active = false; }
        public void Reset() { frame = 0; totalTime = 0; }

        public void Update(GameTime gt)
        {
            if(!active) return;
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

        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            sb.Draw(
                spriteSheet, pos, sourceRects[frame], Color.White,
                0.0f, Vector2.Zero, Vector2.One, flip,
                0.0f);
        }
    }
}
