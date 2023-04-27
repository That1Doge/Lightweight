﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        WaveComplete,
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
        private Texture2D floorTile;
        private Texture2D topWall;
        private Texture2D bottomWall;
        private Texture2D leftWall;
        private Texture2D rightWall;
        private Texture2D trapTexture;
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
        private MenuButton waveComplete;
        private ToggleButton godMode;
        private SpriteFont buttonText;
        private Rectangle buttonRectangle;
        private Texture2D buttonTexture;
        private Texture2D toggleOn;
        private Texture2D toggleOff;
        private Texture2D backButton;
        private KeyboardState prevState;
        private Player player;
        private List<Wall> walls;
        private LevelManager levelManager;
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
            walls = new List<Wall>();
            menuState = MenuStates.MainMenu;
            base.Initialize();
            timer = new Stopwatch();
        }

        protected override void LoadContent()
        {
            //Loads each asset
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            floorTile = Content.Load<Texture2D>("floor_tile");
            buttonText = Content.Load<SpriteFont>("arial-14");
            buttonTexture = Content.Load<Texture2D>("PNG/ButtonImages/gameButton");
            toggleOn = Content.Load<Texture2D>("PNG/ButtonImages/toggleOptionOn");
            toggleOff = Content.Load<Texture2D>("PNG/ButtonImages/toggleOptionOff");
            topWall = Content.Load<Texture2D>("topwall");
            bottomWall = Content.Load<Texture2D>("bwall");
            rightWall = Content.Load<Texture2D>("rwall");
            leftWall = Content.Load<Texture2D>("lwall");
            trapTexture = Content.Load<Texture2D>("trap");
            backButton = Content.Load<Texture2D>("PNG/ButtonImages/backArrow");
            titleFont = Content.Load<SpriteFont>("impact-50");

            BulletManager.BulletTexture = Content.Load<Texture2D>("rsz_plain-circle1");

            EnemyManager.Instance.LoadSpriteSheets(Content);

            // TODO: use this.Content to load your game content here
            // Loads all of the Menu buttons
            playButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth/2 - buttonTexture.Width * 2, 
                windowHeight / 2, buttonTexture.Width * 4, buttonTexture.Height * 2));
            optionsButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2 + (buttonTexture.Height * 3), buttonTexture.Width * 4, buttonTexture.Height * 2));
            instructionsButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2 + (buttonTexture.Height * 6), buttonTexture.Width * 4, buttonTexture.Height * 2));
            quitButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2 + (buttonTexture.Height * 9), buttonTexture.Width * 4, buttonTexture.Height * 2));

            // Loads all of the Options buttons
            optionsBack = new MenuButton(backButton, buttonText, buttonRectangle = new Rectangle(20, 20, 
                backButton.Width, backButton.Height));
            godMode = new ToggleButton(new Rectangle(windowWidth / 2 - toggleOff.Width * 2, windowHeight / 2 - toggleOff.Height * 2, toggleOff.Width * 2, toggleOff.Height * 2), toggleOn, toggleOff);

            // Loads all of the Instructions menu content
            instructionsBack = new MenuButton(backButton, buttonText, buttonRectangle = new Rectangle(20, 20,
                backButton.Width, backButton.Height));

            // Loads all of the Pause buttons
            pauseBack = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2 + (buttonTexture.Height * 3), buttonTexture.Width * 4, buttonTexture.Height  * 2));
            backToMenu = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width  *2,
                windowHeight / 2, buttonTexture.Width * 4, buttonTexture.Height * 2));

            // Game over buttons loaded here
            menuButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2, buttonTexture.Width * 4, buttonTexture.Height * 2));
            retryButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2 + (buttonTexture.Height * 3), buttonTexture.Width * 4, buttonTexture.Height * 2));

            // Button that reads from file
            readFile = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2 + (buttonTexture.Height * 6), buttonTexture.Width * 4, buttonTexture.Height * 2));
            waveComplete = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
                windowHeight / 2 + (buttonTexture.Height * 6), buttonTexture.Width * 4, buttonTexture.Height * 2));

            levelManager = new LevelManager(floorTile, trapTexture, topWall, bottomWall, leftWall, rightWall, windowHeight, windowWidth);
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
                        Reset();
                        menuState = MenuStates.Gameplay;
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
                        levelManager.LoadLevel("..\\..\\..\\testBoard.txt");
                        levelManager.IsLoaded = true;
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
                    foreach (Wall walls in levelManager.Walls) 
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
                    for (int i = BulletManager.Bullets.Count - 1; i >= 0; i--)
                    {
                        BulletManager.Bullets[i].Update();

                        if (!BulletManager.Bullets[i].IsAlive)
                        {
                            BulletManager.Bullets.RemoveAt(i);
                        }
                    }

                    // update bullets
                    /*foreach (Bullet bullet in BulletManager.Bullets)
                    {
                        bullet.Update();
                    }*/

                    //If player hits trap
                    foreach (Tile tile in levelManager.FloorTiles) 
                    {
                        if (tile.Intersect(player.HitBox) && tile.IsTrap && !PlayerController.IsRolling) 
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
                        menuState = MenuStates.WaveComplete;
                    }
                    break;
                case MenuStates.WaveComplete:
                    if (waveComplete.ButtonClicked()) 
                    {
                        Continue();
                        menuState = MenuStates.Gameplay;
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
                        Reset();
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
                        Reset();
                        menuState = MenuStates.Gameplay;
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
                    new Vector2(windowWidth/2 - 260, 350), Color.Black);
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
                    levelManager.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    EnemyManager.Instance.Draw(_spriteBatch);
                    
                    //Draws bullet to screen
                    foreach (Bullet bullet in BulletManager.Bullets)
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
                        new Vector2(15, 50),
                        Color.Black);

                    //Draws timer
                    _spriteBatch.DrawString(buttonText,
                        $"Time: {timer.ElapsedMilliseconds/1000}",
                        new Vector2(15, 90),
                        Color.Black);

                    _spriteBatch.DrawString(buttonText, 
                        $"Wave: {levelManager.Wave}", 
                        new Vector2(15, 130), 
                        Color.Black);

                    break;
                case MenuStates.WaveComplete:
                    _spriteBatch.DrawString(buttonText,
                        String.Format("Completed Wave {0}!",
                        levelManager.Wave),
                        new Vector2(waveComplete.Rectangle.X + 50, waveComplete.Rectangle.Y - 250),
                        Color.Black);
                    waveComplete.Render(_spriteBatch, "CONTINUE", waveComplete.Rectangle); 
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
                        new Vector2(windowWidth/2 - (buttonText.MeasureString($"Time Survived: {timeSurvived} seconds").X / 2), 250), Color.Black);
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
            EnemyManager.Instance.Enemies.Clear();
            
            // Changes health based on the GodMode setting
            if (!godMode.IsOn)
            {
                player.PlayerHealth = 100;
            }
            levelManager.Wave = 0;
            levelManager.BuildLevel();
            if (levelManager.IsLoaded)
            {
                if (levelManager.Wave > 1)
                    levelManager.BuildLevel();
            }
            player.Scraps = 10;
            timer.Reset();
        }


        public void Continue() 
        {
            player.X = 400;
            player.Y = 240;

            if (!(player.PlayerHealth <= 0))
            {
                levelManager.BuildLevel();
            }
            if (levelManager.IsLoaded)
            {
                if (levelManager.Wave > 1)
                    levelManager.BuildLevel();
            }
            // Changes health based on the GodMode setting
            if (godMode.IsOn)
            {
                player.PlayerHealth = 99999;
            }

            player.Scraps = 10;
            timer.Reset();
        }

    }
}