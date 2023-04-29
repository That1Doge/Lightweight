﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lightweight
{
    /// <summary>
    /// Enumerator for the menu states
    /// </summary>
    public enum MenuStates
    {
        MainMenu,
        InstructionMenu,
        OptionsMenu,
        Gameplay,
        GameOver,
        Pause
    }

    public class Game1 : Game
    {
        //Fields used within class
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private static int windowWidth;
        private static int windowHeight;
        private MenuStates menuState;
        private MenuButton playButton;
        private MenuButton optionsButton;
        private MenuButton quitButton;
        private MenuButton menuButton;
        private MenuButton retryButton;
        private MenuButton optionsBack;
        private MenuButton instructionsButton;
        private MenuButton instructionsBack;
        private MenuButton pauseBack;
        private MenuButton readFile;
        private MenuButton backToMenu;
        private ToggleButton godMode;
        private SpriteFont buttonText;
        private Rectangle buttonRectangle;
        private Texture2D buttonTexture;
        private Texture2D toggleOn;
        private Texture2D toggleOff;
        private Texture2D backButton;
        private KeyboardState prevState;
        private Player player;
        private SpriteFont titleFont;
        private Stopwatch timer;
        private int timeSurvived;

        /// <summary>
        /// Property that gets the window width
        /// </summary>
        public static int WindowWidth { get { return windowWidth; } }

        /// <summary>
        /// Property that gets the window height
        /// </summary>
        public static int WindowHeight { get { return windowHeight; } }

        public MenuStates MenuState { get { return menuState; } }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;
            player = new Player();
            menuState = MenuStates.MainMenu;
            base.Initialize();
            timer = new Stopwatch();
        }

        protected override void LoadContent()
        {
            //Loads each asset
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //floorTile = Content.Load<Texture2D>("floor_tile");
            buttonText = Content.Load<SpriteFont>("arial-14");
            buttonTexture = Content.Load<Texture2D>("PNG/ButtonImages/gameButton");
            toggleOn = Content.Load<Texture2D>("PNG/ButtonImages/toggleOptionOn");
            toggleOff = Content.Load<Texture2D>("PNG/ButtonImages/toggleOptionOff");
            //topWall = Content.Load<Texture2D>("topwall");
            //bottomWall = Content.Load<Texture2D>("bwall");
            //rightWall = Content.Load<Texture2D>("rwall");
            //leftWall = Content.Load<Texture2D>("lwall");
            //trapTexture = Content.Load<Texture2D>("trap");
            backButton = Content.Load<Texture2D>("PNG/ButtonImages/backArrow");
            titleFont = Content.Load<SpriteFont>("impact-50");

            LevelManager.Instance.LoadContent(Content);

            BulletManager.Instance.BulletTexture = Content.Load<Texture2D>("rsz_plain-circle1");

            EnemyManager.Instance.LoadSpriteSheets(Content);

            // TODO: use this.Content to load your game content here
            // Loads all of the Menu buttons
            playButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth/2 - buttonTexture.Width, 
                windowHeight / 2, buttonTexture.Width * 2, buttonTexture.Height));
            optionsButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width,
                windowHeight / 2 + (buttonTexture.Height * 2), buttonTexture.Width * 2, buttonTexture.Height));
            instructionsButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width,
                windowHeight / 2 + (buttonTexture.Height * 4), buttonTexture.Width * 2, buttonTexture.Height));
            quitButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width,
                windowHeight / 2 + (buttonTexture.Height * 6), buttonTexture.Width * 2, buttonTexture.Height));

            // Loads all of the Options buttons
            optionsBack = new MenuButton(backButton, buttonText, buttonRectangle = new Rectangle(20, 20, 
                backButton.Width, backButton.Height));
            godMode = new ToggleButton(new Rectangle(windowWidth / 2 - toggleOff.Width, windowHeight / 2, toggleOff.Width, toggleOff.Height), toggleOn, toggleOff);

            // Loads all of the Instructions menu content
            instructionsBack = new MenuButton(backButton, buttonText, buttonRectangle = new Rectangle(20, 20,
                backButton.Width, backButton.Height));

            // Loads all of the Pause buttons
            pauseBack = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width,
                windowHeight / 2 + (buttonTexture.Height * 2), buttonTexture.Width * 2, buttonTexture.Height));
            backToMenu = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width,
                windowHeight / 2, buttonTexture.Width * 2, buttonTexture.Height));

            // Game over buttons loaded here
            menuButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width / 2,
                windowHeight / 2, buttonTexture.Width, buttonTexture.Height));
            retryButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width / 2,
                windowHeight / 2 + (buttonTexture.Height * 2), buttonTexture.Width, buttonTexture.Height));

            // Button that reads from file
            readFile = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - 115,
                windowHeight / 2 + (buttonTexture.Height * 4), buttonTexture.Width * 2, buttonTexture.Height));

            //LevelManager.Instance.Instance.Instance = new LevelManager(floorTile, trapTexture, topWall, bottomWall, leftWall, rightWall, windowHeight, windowWidth);
            player.LoadAnims(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            //    Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();


            // Switch statement that changes the Menu States depending on what action is done
            switch(menuState)
            { 
                case MenuStates.MainMenu:
                    // Tests the menu state 
                    //if(Keyboard.GetState().IsKeyDown(Keys.W))
                    //{
                    //    menuState = MenuStates.Gameplay;
                    //}
                    
                    if (playButton.ButtonClicked())
                    {
                        menuState = MenuStates.Gameplay;
                        Reset();
                    }
                    else if (optionsButton.ButtonClicked())
                    {
                        menuState = MenuStates.OptionsMenu;
                    }
                    else if(instructionsButton.ButtonClicked())
                    {
                        menuState = MenuStates.InstructionMenu;
                    }
                    else if (quitButton.ButtonClicked())
                    {
                        Exit();
                    }
                    break;
                case MenuStates.InstructionMenu:
                    if(instructionsBack.ButtonClicked())
                    {
                        menuState = MenuStates.MainMenu;
                    }
                    break;
                case MenuStates.OptionsMenu:
                    if(optionsBack.ButtonClicked())
                    {
                        menuState = MenuStates.MainMenu;
                    }
                    if (readFile.ButtonClicked()) 
                    {
                        LevelManager.Instance.LoadLevel("..\\..\\..\\testBoard.txt");
                        LevelManager.Instance.IsLoaded = true;
                    }

                    //God mode configuration
                    if (godMode.isClicked())
                    {
                        if (godMode.IsOn)
                        {
                            // Maybe put something that sets a godMode setting to true and sets off
                            //      what needs to be done
                            player.PlayerHealth = 999999;
                        }
                        else
                        {
                            // Same thing applies here
                        }
                    }

                    break;
                case MenuStates.Gameplay:
                    player.Update(gameTime);
                    EnemyManager.Instance.Update(gameTime, player);

                    //Wall collision
                    foreach (Wall walls in LevelManager.Instance.Walls) 
                    { 
                        if (walls.Intersect(player.HitBox) && player.X < 5) 
                        {
                            player.X = 5;
                        }
                        else if (walls.Intersect(player.HitBox) && player.Y > 413) 
                        {
                            player.Y = 413;
                        }
                        else if (walls.Intersect(player.HitBox) && player.X > 751) 
                        { 
                            player.X = 751;
                        }
                        else if (walls.Intersect(player.HitBox) && player.Y < 5) 
                        { 
                            player.Y += 5;
                        }
                    }

                    // remove bullets when no longer active
                    for (int i = BulletManager.Instance.Bullets.Count - 1; i >= 0; i--)
                    {
                        BulletManager.Instance.Bullets[i].Update();

                        if (!BulletManager.Instance.Bullets[i].IsAlive)
                        {
                            BulletManager.Instance.Bullets.RemoveAt(i);
                        }
                    }

                    // update bullets
                    /*foreach (Bullet bullet in BulletManager.Instance.Bullets)
                    {
                        bullet.Update();
                    }*/

                    //If player hits trap
                    foreach (Tile tile in LevelManager.Instance.FloorTiles) 
                    {
                        if (tile.Intersect(player.HitBox) && tile.IsTrap && !player.Controller.IsRolling) 
                        {
                            player.PlayerHealth -= 1;
                        }
                    }

                    //Puts total time on gameover screen
                    if (player.PlayerHealth <= 0) 
                    {
                        timeSurvived = (int)(timer.ElapsedMilliseconds / 1000);
                        timer.Stop();

                        menuState = MenuStates.GameOver;
                    }

                    // Accesses pause menu
                    if(Keyboard.GetState().IsKeyDown(Keys.Escape) && 
                        prevState.IsKeyUp(Keys.Escape))
                    {
                        timer.Stop();
                        menuState = MenuStates.Pause;
                        
                    }
                    if(Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        menuState = MenuStates.GameOver;
                        timeSurvived = (int)(timer.ElapsedMilliseconds / 1000);
                        timer.Stop();
                    }

                    if(!timer.IsRunning)
                    {
                        timer.Start();
                    }

                    if(EnemyManager.Instance.Enemies.Count == 0)
                    {
                        Reset();
                    }
                    break;
                case MenuStates.Pause:
                    if((Keyboard.GetState().IsKeyDown(Keys.Escape) && prevState.IsKeyUp(Keys.Escape))
                        || pauseBack.ButtonClicked())
                    {
                        timer.Start();
                        menuState = MenuStates.Gameplay;
                        
                    }

                    if(backToMenu.ButtonClicked())
                    {
                        menuState = MenuStates.MainMenu;
                    }
                    break;
                case MenuStates.GameOver:
                    if (menuButton.ButtonClicked())
                    {
                        menuState = MenuStates.MainMenu;
                    }
                    else if (retryButton.ButtonClicked())
                    { 
                        menuState = MenuStates.Gameplay;
                        Reset();
                    }
                    break;
            }
            // TODO: Add your update logic here
            base.Update(gameTime);

            prevState = Keyboard.GetState();
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            _spriteBatch.Begin();

            // TODO: Add your drawing code here
            // Switch statement for what gets drawn to the screen
            switch (menuState)
            {

                case MenuStates.MainMenu:
                    // Draws buttons to the play screen
                    playButton.Render(_spriteBatch, "PLAY", playButton.Rectangle);
                    optionsButton.Render(_spriteBatch, "OPTIONS", optionsButton.Rectangle);
                    instructionsButton.Render(_spriteBatch, "INSTRUCTIONS", instructionsButton.Rectangle);
                    quitButton.Render(_spriteBatch, "QUIT", quitButton.Rectangle);
                    
                    // Draws the title of the game
                    _spriteBatch.DrawString(titleFont, "LIGHTWEIGHT", new Vector2(windowWidth/2 - (titleFont.MeasureString("LIGHTWEIGHT").X / 2), 30), Color.Black);
                    
                    break;
                case MenuStates.InstructionMenu:
                    // Draws the back button onto the instructions screen and writes how to play the game
                    instructionsBack.Render(_spriteBatch, "", instructionsBack.Rectangle);
                    _spriteBatch.DrawString(buttonText, "- Use W/ A/ S/ D to move" +
                                                                          "\n- Press Space or Left Shift to Dodge Roll and " +
                                                                          "\n  avoid damage" +
                                                                          "\n- Left Click to shoot at your cursor" +
                                                                          "\n- Shoot at enemies and pick up the scrap they drop " +
                                                                          "\n  to stay slow" +
                                                                          "\n- Avoid the traps!" +
                                                                          "\n- Try to survive as long as possible",
                    new Vector2(windowWidth/2 - 150, 190), Color.Black);
                    _spriteBatch.DrawString(titleFont, "INSTRUCTIONS", new Vector2(windowWidth / 2 - (titleFont.MeasureString("INSTRUCTIONS").X / 2), 30), Color.Black);
                    // Player tries to survive as long as possible

                    break;
                case MenuStates.OptionsMenu:
                    // Draws all of the Options buttons to the options screen
                    optionsBack.Render(_spriteBatch, "", optionsBack.Rectangle);
                    godMode.Draw(_spriteBatch, buttonText, "GOD MODE");
                    readFile.Render(_spriteBatch, "READ FROM FILE", readFile.Rectangle);
                    _spriteBatch.DrawString(titleFont, "OPTIONS", new Vector2(windowWidth / 2 - (titleFont.MeasureString("OPTIONS").X / 2), 30), Color.Black);

                    break;
                case MenuStates.Gameplay:
                    GraphicsDevice.Clear(Color.Black);

                    //This draws the tiles/walls, and player across the screen
                    LevelManager.Instance.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    EnemyManager.Instance.Draw(_spriteBatch);
                    
                    //Draws bullet to screen
                    foreach (Bullet bullet in BulletManager.Instance.Bullets)
                    {
                        bullet.Draw(_spriteBatch);
                    }

                    //Draws scrap balance
                    _spriteBatch.DrawString(
                        buttonText,
                        $"Scraps: {player.Scraps}",
                        new Vector2(15, 10),
                        Color.Black);

                    //Draws total health
                    _spriteBatch.DrawString(buttonText, 
                        $"Health: {player.PlayerHealth}",
                        new Vector2(15, 35),
                        Color.Black);

                    //Draws timer
                    _spriteBatch.DrawString(buttonText,
                        $"Time: {timer.ElapsedMilliseconds/1000}",
                        new Vector2(15, 60),
                        Color.Black);

                    _spriteBatch.DrawString(buttonText, $"Wave: {LevelManager.Instance.Wave}", new Vector2(15, 95), Color.Black);

                    break;
                case MenuStates.Pause:
                    // Draws the buttons for the pause menu
                    pauseBack.Render(_spriteBatch, "BACK", pauseBack.Rectangle);
                    backToMenu.Render(_spriteBatch, "BACK TO MENU", backToMenu.Rectangle);
                    _spriteBatch.DrawString(titleFont, "PAUSED", new Vector2(windowWidth / 2 - (titleFont.MeasureString("PAUSED").X / 2), 30), Color.Black);
                    break;
                case MenuStates.GameOver:
                    GraphicsDevice.Clear(Color.DarkRed);
                    // Draws the needed items for the game over screen 
                    _spriteBatch.DrawString(titleFont, "GAME OVER", new Vector2(windowWidth / 2 - (titleFont.MeasureString("GAME OVER").X / 2), 30), Color.Black);
                    _spriteBatch.DrawString(buttonText, $"Time Survived: {timeSurvived} seconds", 
                        new Vector2(windowWidth/2 - (buttonText.MeasureString($"Time Survived: {timeSurvived} seconds").X / 2), 150), Color.Black);
                    menuButton.Render(_spriteBatch, "MENU", menuButton.Rectangle);
                    retryButton.Render(_spriteBatch, "RETRY", retryButton.Rectangle);
                    break;
            }


            _spriteBatch.End();  
            base.Draw(gameTime);
        }

        /// <summary>
        /// Resets level and player after a retry/start of game
        /// </summary>
        public void Reset()
        {
            player.X = 400;
            player.Y = 240;

            if(!(player.PlayerHealth <= 0))
            {
                LevelManager.Instance.BuildLevel();
            }
            if (LevelManager.Instance.IsLoaded) 
            {
                if (LevelManager.Instance.Wave > 1)
                LevelManager.Instance.BuildLevel();
            }
            // Changes health based on the GodMode setting
            if (!godMode.IsOn)
            {
                player.PlayerHealth = 100;
            }
            player.Scraps = 10;
            timer.Reset();
        }


    }
}