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
    /// Enumeration of player state
    /// </summary>
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
        //Fields used in class
        private Vector2 direction;
        private PlayerState playerState;
        private bool isRolling;
        private KeyboardState kb;
        private KeyboardState prevKb;
        private MouseState prevMouse;
        private MouseState mouse;
        private Player player;

        /// <summary>
        /// Property that returns player
        /// </summary>
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        /// <summary>
        /// Property that returns direction
        /// </summary>
        public  Vector2 Direction { get { return direction; } }

        /// <summary>
        /// Property that returns player state
        /// </summary>
        public PlayerState PlayerState { get { return playerState; } }

        /// <summary>
        /// Property that returns whether player is rolling
        /// </summary>
        public bool IsRolling 
        { 
            get { return isRolling; } 
            set { isRolling = value; } 
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="player">Player the class is handling</param>
        public PlayerController(Player player)
        {
            this.player = player;
            playerState = PlayerState.FaceRight;
        }

        /// <summary>
        /// Method that deals with single key pressed
        /// </summary>
        /// <param name="key">key to be pressed a single time</param>
        /// <returns>If key is pressed once or not</returns>
        public bool SingleKeyPress(Keys key)
        {
            return prevKb.IsKeyUp(key) && kb.IsKeyDown(key);
        }

        /// <summary>
        /// Update method for player controller
        /// </summary>
        /// <param name="gt"></param>
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
                mouse.X >= 0 && mouse.X <= Game1.Instance.WindowWidth &&
                mouse.Y >= 0 && mouse.Y <= Game1.Instance.WindowWidth)

            {
                // get current mouse pos
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);

                // shoot from player pos
                Player.Shoot(new Vector2(Player.HitBox.X, Player.HitBox.Y), mousePosition, 10, 10);
            }
            
            //If player moves while rolling, move in direction

            if (!isRolling) direction = Vector2.Zero;
            if (kb.GetPressedKeyCount() > 0 && !isRolling)
            {
                if (kb.IsKeyDown(Keys.W)) direction.Y--;
                if (kb.IsKeyDown(Keys.S)) direction.Y++;
                if (kb.IsKeyDown(Keys.A)) direction.X--;
                if (kb.IsKeyDown(Keys.D)) direction.X++;
            }
            if(direction != Vector2.Zero) { direction = Vector2.Normalize(direction); }

            //FSM that deals with the player movement
            switch (playerState)
            {
                //Facing left
                case PlayerState.FaceLeft:

                    //If player goes up, left, or right, run left
                    if (kb.IsKeyDown(Keys.A)
                        || kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.RunLeft;
                    }

                    //If right, run right
                    else if (kb.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    
                    //Rolling right
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollLeft;
                        isRolling = true;
                        player.Scraps--;
                    }
                    break;

                //Facing right
                case PlayerState.FaceRight:

                    //Player going left
                    if (kb.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.FaceLeft;
                    }

                    //Going up right, down face run right
                    else if (kb.IsKeyDown(Keys.D) ||
                        kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.RunRight;
                    }

                    //Rolling right
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollRight;
                        isRolling = true;
                        player.Scraps--;
                    }
                    break;

                //Running left
                case PlayerState.RunLeft:

                    //Player going right
                    if (kb.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    //Player stops running
                    else if (!kb.IsKeyDown(Keys.A) &&
                        !kb.IsKeyDown(Keys.W) && !kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    
                    //Rolling while running
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollLeft;
                        isRolling = true;
                        player.Scraps--;
                    }
                    break;

                //Running left
                case PlayerState.RunRight:

                    //Facing left
                    if (kb.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.FaceLeft;
                    }

                    //Player stops running
                    else if (!kb.IsKeyDown(Keys.D) &&
                        !kb.IsKeyDown(Keys.W) && !kb.IsKeyDown(Keys.S))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    
                    //Rolling while running
                    else if ((SingleKeyPress(Keys.Space) || SingleKeyPress(Keys.LeftShift)) && player.Scraps > 0)
                    {
                        playerState = PlayerState.RollRight;
                        isRolling = true;
                        player.Scraps--;
                    }
                    break;

                //Rolling left
                case PlayerState.RollLeft:

                    //Stops rolling
                    if (!isRolling)
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    break;

                //Rolling right
                case PlayerState.RollRight:

                    //Stops rolling
                    if (!isRolling)
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    break;
            }
        }
    }
}
