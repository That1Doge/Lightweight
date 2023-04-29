﻿using Microsoft.Xna.Framework;
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
    public class PlayerController
    {
        private Vector2 direction;
        private PlayerState playerState;
        private bool isRolling;
        private KeyboardState kb;
        private KeyboardState prevKb;
        private MouseState prevMouse;
        private MouseState mouse;

        private Player player;
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public  Vector2 Direction { get { return direction; } }
        public PlayerState PlayerState { get { return playerState; } }

        public bool IsRolling 
        { 
            get { return isRolling; } 
            set { isRolling = value; } 
        }

        public PlayerController(Player player)
        {
            this.player = player;
            playerState = PlayerState.FaceRight;
        }

        public bool SingleKeyPress(Keys key)
        {
            return prevKb.IsKeyUp(key) && kb.IsKeyDown(key);
        }

        public void Update(GameTime gt)
        {
            prevKb = kb;
            kb = Keyboard.GetState();

            // update mouse states
            prevMouse = mouse;
            mouse = Mouse.GetState();

            // if LMB clicked, shoot bullet at mouse pos
            if (mouse.LeftButton == ButtonState.Pressed &&
                prevMouse.LeftButton == ButtonState.Released &
                mouse.X >= 0 && mouse.X <= Game1.WindowWidth &&
                mouse.Y >= 0 && mouse.Y <= Game1.WindowHeight)
                // TODO: make it so you don't shoot by clicking outside the game window (see above commment)
            {
                // get current mouse pos
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);

                // shoot from player pos
                Player.Shoot(new Vector2(Player.HitBox.X, Player.HitBox.Y), mousePosition, 10, 10);
            }
            


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
                    //else if ((kb.IsKeyDown(Keys.Space)
                    //    || kb.IsKeyDown(Keys.LeftShift)) && player.Scraps > 0)
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollLeft;
                        isRolling = true;
                        player.Scraps--;
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
                    //else if ((kb.IsKeyDown(Keys.Space)
                    //    || kb.IsKeyDown(Keys.LeftShift)) && player.Scraps > 0)
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollRight;
                        isRolling = true;
                        player.Scraps--;
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
                    //else if ((kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.LeftShift))
                    //    && player.Scraps > 0)
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollLeft;
                        isRolling = true;
                        player.Scraps--;
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
                    //else if ((kb.IsKeyDown(Keys.Space)
                    //    || kb.IsKeyDown(Keys.LeftShift)) && player.Scraps > 0)
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollRight;
                        isRolling = true;
                        player.Scraps--;
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
