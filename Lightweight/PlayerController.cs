﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    public enum PlayerState
    {
        FaceLeft,
        FaceRight,
        RunLeft,
        RunRight,
        RollLeft,
        RollRight,
    }

    /// <summary>
    /// Player Controller class
    /// Controls player movement and input
    /// </summary>
    public static class PlayerController
    {
        private static Vector2 direction;
        private static PlayerState playerState;
        private static bool isRolling;
        private static KeyboardState kb;
        private static KeyboardState prevKb;
        public static Vector2 Direction { get { return direction; } }
        public static PlayerState PlayerState { get { return playerState; } }

        public static bool IsRolling 
        { 
            get { return isRolling; } 
            set { isRolling = value; } 
        }

        static PlayerController()
        {
            playerState = PlayerState.FaceRight;
        }

        public static bool SingleKeyPress(Keys key)
        {
            return prevKb.IsKeyUp(key) && kb.IsKeyDown(key);
        }

        public static void Update(GameTime gt)
        {
            kb = Keyboard.GetState();

            if (!isRolling) direction = Vector2.Zero;
            if (kb.GetPressedKeyCount() > 0 && !isRolling)
            {
                if (kb.IsKeyDown(Keys.W)) direction.Y--;
                if (kb.IsKeyDown(Keys.S)) direction.Y++;
                if (kb.IsKeyDown(Keys.A)) direction.X--;
                if (kb.IsKeyDown(Keys.D)) direction.X++;
            }

            if(direction != Vector2.Zero) { direction = Vector2.Normalize(direction); }

            switch (playerState)
            {
                case PlayerState.FaceLeft:
                    if (kb.IsKeyDown(Keys.A)
                        || kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.RunLeft;
                    }
                    else if (kb.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    else if (kb.IsKeyDown(Keys.Space)
                        || kb.IsKeyDown(Keys.LeftShift))
                    {
                        playerState = PlayerState.RollLeft;
                        isRolling = true;
                    }
                    break;
                case PlayerState.FaceRight:
                    if (kb.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    else if (kb.IsKeyDown(Keys.D) ||
                        kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.RunRight;
                    }
                    else if (kb.IsKeyDown(Keys.Space) ||
                        kb.IsKeyDown(Keys.LeftShift))
                    {
                        playerState = PlayerState.RollRight;
                        isRolling = true;
                    }
                    break;
                case PlayerState.RunLeft:
                    if (kb.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    else if (!kb.IsKeyDown(Keys.A) &&
                        !kb.IsKeyDown(Keys.W) && !kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    else if (kb.IsKeyDown(Keys.Space) ||
                        kb.IsKeyDown(Keys.LeftShift))
                    {
                        playerState = PlayerState.RollLeft;
                        isRolling = true;
                    }
                    break;
                case PlayerState.RunRight:
                    if (kb.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    else if (!kb.IsKeyDown(Keys.D) &&
                        !kb.IsKeyDown(Keys.W) && !kb.IsKeyDown(Keys.S))
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
                    break;
                case PlayerState.RollRight:
                    if (!isRolling)
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    break;
            }
        }
    }
}
