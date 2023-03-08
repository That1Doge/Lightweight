using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lightweight
{
    public enum PlayerState
    {
        FaceLeft,
        FaceRight,
        WalkLeft,
        WalkRight,
        RollLeft,
        RollRight
    }

    public class Player : GameObject, IShoot, IMove, ITakeDamage
    {
        // player state enum
        private PlayerState playerState;

        // Sprite sheet data
        private int numSprites;
        private int spriteWidth;
        private int spriteHeight;

        // Animation data
        private int currentFrame;
        private double fps;
        private double frameTime;
        private double timeCounter;

        // roll data
        private bool isRolling;

        // moving data
        private Vector2 direction;
        private int speed;

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

            // setting up sprite data
            spriteWidth = 100;
            spriteHeight = 100;
            numSprites = 10;

            // setting up animation data
            fps = 8;
            frameTime = 1 / fps;
            timeCounter = 0;
            currentFrame = 1;

            // speed data
            speed = 10;
        }

        public override void Update()
        {
            // keyboard state information
            KeyboardState kb = Keyboard.GetState();
            direction = Vector2.Zero;

            if(!isRolling )
            {
                if (kb.IsKeyDown(Keys.W))
                {
                    direction.Y -= 1f;
                }
                if (kb.IsKeyDown(Keys.S))
                {
                    direction.Y += 1f;
                }
                if (kb.IsKeyDown(Keys.D))
                {
                    direction.X += 1f;
                }
                if (kb.IsKeyDown(Keys.A))
                {
                    direction.X -= 1f;
                }

                if (direction != Vector2.Zero)
                {
                    direction = Vector2.Normalize(direction);
                }

                position += direction * speed;
            }

            switch (playerState)
            {
                case PlayerState.FaceLeft:
                    if(kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    else if (kb.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    else if (kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.LeftShift))
                    {
                        playerState = PlayerState.RollLeft;
                    }
                    break;
                case PlayerState.FaceRight:
                    if (kb.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    else if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.WalkRight;
                    }
                    else if (kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.LeftShift))
                    {
                        playerState = PlayerState.RollRight;
                    }
                    break;
                case PlayerState.WalkLeft:
                    if (kb.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    else if (!kb.IsKeyDown(Keys.A) && !kb.IsKeyDown(Keys.W) && !kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    else if (kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.LeftShift))
                    {
                        playerState = PlayerState.RollLeft;
                        isRolling = true;
                    }
                    break;
                case PlayerState.WalkRight:
                    if (kb.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    else if (!kb.IsKeyDown(Keys.D) && !kb.IsKeyDown(Keys.W) && !kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    else if (kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.Space))
                    {
                        playerState = PlayerState.RollRight;
                        isRolling = true;
                    }
                    break;
                case PlayerState.RollLeft:
                    if (!isRolling)
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    position.X -= speed;
                    break;
                case PlayerState.RollRight:
                    if (!isRolling)
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    position.X += speed;
                    break;
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            // Elapsed time - duration of the last game frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Checks if enough time has passed to move to next frame
            if (timeCounter >= frameTime)
            {
                // increases current frame
                currentFrame++;

                // moves to 1st frame if on last sprite
                if (currentFrame >= numSprites)
                {
                    if (isRolling) { isRolling = false; }
                    currentFrame = 1;
                }
            }

            // resets time counter
            timeCounter -= frameTime;
        }

        private void DrawIdle(SpriteBatch sb, SpriteEffects flip)
        {
            sb.Draw(
                texture,                                // sprite sheet
                position,                               // position
                new Rectangle(
                    currentFrame * spriteWidth,         // - from left edge
                    0,                                  // - top of sprite sheet
                    spriteWidth,                        // - width
                    spriteHeight),                      // - height
                Color.White,                            // no change in color
                0.0f,                                   // no rotation
                Vector2.Zero,                           // start at origin of sprite sheet
                Vector2.One,                            // no scaling
                flip,                                   // flip
                0.0f);                                  // layer depth
        }

        private void DrawWalking(SpriteBatch sb, SpriteEffects flip)
        {
            sb.Draw(
                texture,                                // sprite sheet
                position,                               // position
                new Rectangle(
                    currentFrame * spriteWidth,         // - from left edge
                    spriteHeight,                       // - 2nd row of sprite sheet
                    spriteWidth,                        // - width
                    spriteHeight),                      // - height
                Color.White,                            // no change in color
                0.0f,                                   // no rotation
                Vector2.Zero,                           // start at origin of sprite sheet
                Vector2.One,                            // no scaling
                flip,                                   // flip
                0.0f);                                  // layer depth
        }

        private void DrawRoll(SpriteBatch sb, SpriteEffects flip)
        {
            sb.Draw(
                texture,                                // sprite sheet
                position,                               // position
                new Rectangle(
                    currentFrame * spriteWidth,         // - from left edge
                    spriteHeight * 2,                       // - 3rd row of sprite sheet
                    spriteWidth,                        // - width
                    spriteHeight),                      // - height
                Color.White,                            // no change in color
                0.0f,                                   // no rotation
                Vector2.Zero,                           // start at origin of sprite sheet
                Vector2.One,                            // no scaling
                flip,                                   // flip
                0.0f);                                  // layer depth
        }
    }
}
