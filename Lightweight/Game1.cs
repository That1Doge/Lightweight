using Microsoft.Xna.Framework;
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
        private SpriteFont titleFont;
        private Texture2D crossHair;
        private int playerStartX;
        private int playerStartY;
        private static Game1 instance;

        public static Game1 Instance
        {
            get
            {
                if (instance == null) instance = new Game1();
                return instance;
            }
        }

        /// <summary>
        /// Property that gets the window width
        /// </summary>
        public static int WindowWidth { get { return windowWidth; } }

        /// <summary>
        /// Property that gets the window height
        /// </summary>
        public static int WindowHeight { get { return windowHeight; } }

        /// <summary>
        /// Property that returns the menu state
        /// </summary>
        public MenuStates MenuState { get { return menuState; } }

        public bool GodMode { get { return godMode.IsOn; } }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            instance = this;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            //Changes window resolution
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            //Variables for window width/height
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;

            //Misc initializations
            player = new Player();
            menuState = MenuStates.MainMenu;
            base.Initialize();
            playerStartX = (windowWidth / 2) - player.HitBox.Width;
            playerStartY = windowHeight / 2;
        }

        protected override void LoadContent()
        {
            //Loads each asset
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            buttonText = Content.Load<SpriteFont>("arial-14");
            buttonTexture = Content.Load<Texture2D>("PNG/ButtonImages/gameButton");
            toggleOn = Content.Load<Texture2D>("PNG/ButtonImages/toggleOptionOn");
            toggleOff = Content.Load<Texture2D>("PNG/ButtonImages/toggleOptionOff");
            backButton = Content.Load<Texture2D>("PNG/ButtonImages/backArrow");
            titleFont = Content.Load<SpriteFont>("impact-50");
            crossHair = Content.Load<Texture2D>("crosshair173");

            //Loads instance content
            LevelManager.Instance.LoadContent(Content);
            BulletManager.Instance.LoadTextures(Content);
            //BulletManager.Instance.BulletTexture = Content.Load<Texture2D>("rsz_plain-circle1");
            EnemyManager.Instance.LoadSpriteSheets(Content);

            // Loads all of the Menu buttons
            playButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
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
                windowHeight / 2 + (buttonTexture.Height * 3), buttonTexture.Width * 4, buttonTexture.Height * 2));
            backToMenu = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width * 2,
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

            //LevelManager.Instance.Instance.Instance = new LevelManager(floorTile, trapTexture, topWall, bottomWall, leftWall, rightWall, windowHeight, windowWidth);
            player.LoadAnims(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            //    Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();


            // Switch statement that changes the Menu States depending on what action is done
            switch (menuState)
            {
                //Main Menu
                case MenuStates.MainMenu:

                    //If play button is clicked, build level based upon if level is loaded or not
                    if (playButton.ButtonClicked())
                    {
                        //Load level from file if option is selected
                        if (LevelManager.Instance.IsLoaded)
                        {
                            LevelManager.Instance.LoadLevel();
                        }
                        //Otherwise change level
                        else
                        {
                            Reset();
                        }
                        menuState = MenuStates.Gameplay;
                    }

                    //Brings up options menu
                    else if (optionsButton.ButtonClicked())
                    {
                        menuState = MenuStates.OptionsMenu;
                    }

                    //Brings up instruction menu if clicked
                    else if (instructionsButton.ButtonClicked())
                    {
                        menuState = MenuStates.InstructionMenu;
                    }

                    //Exit game if button is clicked
                    else if (quitButton.ButtonClicked())
                    {
                        Exit();
                    }
                    break;

                //Instruction menu
                case MenuStates.InstructionMenu:

                    //Goes back to main menu if button is clicked
                    if (instructionsBack.ButtonClicked())
                    {
                        menuState = MenuStates.MainMenu;
                    }
                    break;

                //Options menu
                case MenuStates.OptionsMenu:

                    //If back button is clicked, return to main menu
                    if (optionsBack.ButtonClicked())
                    {
                        menuState = MenuStates.MainMenu;
                    }

                    //If read button file is clickec, change indication to true
                    if (readFile.ButtonClicked())
                    {
                        LevelManager.Instance.IsLoaded = true;
                    }

                    //God mode configuration
                    if (godMode.isClicked())
                    {
                        if (godMode.IsOn)
                        {
                            // Maybe put something that sets a godMode setting to true and sets off
                            // what needs to be done
                            player.PlayerHealth = 999999;
                        }
                        else
                        {
                            // Same thing applies here
                        }
                    }
                    break;

                //Gameplay
                case MenuStates.Gameplay:
                    player.Update(gameTime);
                    EnemyManager.Instance.Update(gameTime, player);

                    //Wall collision
                    foreach (Wall walls in LevelManager.Instance.Walls)
                    {
                        //Left wall
                        if (walls.Intersect(player.HitBox) && player.X < 5)
                        {
                            player.X = 5;
                        }

                        //Bottom wall
                        else if (walls.Intersect(player.HitBox) && player.Y > 1015)
                        {
                            player.Y = 1015;
                        }

                        //Right wall
                        else if (walls.Intersect(player.HitBox) && player.X > 1877)
                        {
                            player.X = 1877;
                        }

                        //Top wall
                        else if (walls.Intersect(player.HitBox) && player.Y < 5)
                        {
                            player.Y = 5;
                        }
                    }

                    // remove bullets when no longer active
                    BulletManager.Instance.Update(gameTime);

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
                        menuState = MenuStates.GameOver;
                    }

                    // Accesses pause menu
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) &&
                        prevState.IsKeyUp(Keys.Escape))
                    {
                        menuState = MenuStates.Pause;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        menuState = MenuStates.GameOver;
                    }

                    //When all enemies are defeated, advance to next screen
                    if (EnemyManager.Instance.Enemies.Count == 0)
                    {
                        menuState = MenuStates.WaveComplete;
                    }
                    break;

                //Wave complete screen
                case MenuStates.WaveComplete:

                    //When continue button clicked, continue game
                    if (waveComplete.ButtonClicked())
                    {
                        Continue();
                        menuState = MenuStates.Gameplay;
                    }
                    break;

                //Pause menu
                case MenuStates.Pause:

                    //Returns to gameplay 
                    if ((Keyboard.GetState().IsKeyDown(Keys.Escape) && prevState.IsKeyUp(Keys.Escape))
                        || pauseBack.ButtonClicked())
                    {
                        menuState = MenuStates.Gameplay;
                    }

                    //Returns to menu if clicked
                    if (backToMenu.ButtonClicked())
                    {
                        Reset();
                        menuState = MenuStates.MainMenu;
                    }
                    break;

                //Game over screen
                case MenuStates.GameOver:

                    //Return to menu
                    if (menuButton.ButtonClicked())
                    {
                        menuState = MenuStates.MainMenu;
                    }

                    //Retry game
                    else if (retryButton.ButtonClicked())
                    {
                        Reset();
                        menuState = MenuStates.Gameplay;
                    }
                    break;
            }
            base.Update(gameTime);

            prevState = Keyboard.GetState();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            _spriteBatch.Begin();


            // Switch statement for what gets drawn to the screen
            switch (menuState)
            {
                //Main menu
                case MenuStates.MainMenu:
                    // Draws buttons to the play screen
                    playButton.Render(_spriteBatch, "PLAY", playButton.Rectangle);
                    optionsButton.Render(_spriteBatch, "OPTIONS", optionsButton.Rectangle);
                    instructionsButton.Render(_spriteBatch, "INSTRUCTIONS", instructionsButton.Rectangle);
                    quitButton.Render(_spriteBatch, "QUIT", quitButton.Rectangle);

                    // Draws the title of the game
                    _spriteBatch.DrawString(titleFont, "LIGHTWEIGHT", new Vector2(windowWidth / 2 - (titleFont.MeasureString("LIGHTWEIGHT").X / 2), 30), Color.Black);
                    break;

                //Instruction menu
                case MenuStates.InstructionMenu:

                    // Draws the back button onto the instructions screen and writes how to play the game
                    instructionsBack.Render(_spriteBatch, "", instructionsBack.Rectangle);
                    _spriteBatch.DrawString(buttonText, "- Use W/ A/ S/ D to move" +
                                                                          "\n- Press Space or Left Shift to Dodge Roll and " +
                                                                          "\n  avoid damage" +
                                                                          "\n- Left Click to shoot at your cursor" +
                                                                          "\n- Shoot at enemies and pick up the scrap they drop " +
                                                                          "\n  to stay slow and take less damage" +
                                                                          "\n- Avoid the traps!" +
                                                                          "\n- Try to survive as long as possible",
                    new Vector2(windowWidth / 2 - 260, 350), Color.Black);
                    _spriteBatch.DrawString(titleFont,
                        "INSTRUCTIONS",
                        new Vector2(windowWidth / 2 - (titleFont.MeasureString("INSTRUCTIONS").X / 2), 30),
                        Color.Black);
                    break;

                //Options menu
                case MenuStates.OptionsMenu:

                    // Draws all of the Options buttons to the options screen
                    optionsBack.Render(_spriteBatch, "", optionsBack.Rectangle);
                    godMode.Draw(_spriteBatch, buttonText, "GOD MODE");
                    readFile.Render(_spriteBatch, "READ FROM FILE", readFile.Rectangle);

                    //Draws indication of file reading if on/off
                    if (LevelManager.Instance.IsLoaded == true)
                    {
                        _spriteBatch.DrawString(buttonText,
                            "On!", new Vector2(readFile.Rectangle.X + (readFile.Rectangle.X / 4) - 5,
                            readFile.Rectangle.Y + 75),
                            Color.Black);
                    }
                    if (LevelManager.Instance.IsLoaded == false)
                    {
                        _spriteBatch.DrawString(buttonText,
                                "Off!", new Vector2(readFile.Rectangle.X + (readFile.Rectangle.X / 4) - 5,
                                readFile.Rectangle.Y + 75),
                                Color.Black);
                    }

                    //Draws options text
                    _spriteBatch.DrawString(titleFont,
                        "OPTIONS",
                        new Vector2(windowWidth / 2 -
                        (titleFont.MeasureString("OPTIONS").X / 2),
                        30), Color.Black);
                    break;

                //Gameplay
                case MenuStates.Gameplay:
                    GraphicsDevice.Clear(Color.Black);

                    //This draws the tiles/walls, and player across the screen
                    LevelManager.Instance.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    EnemyManager.Instance.Draw(_spriteBatch);
                    BulletManager.Instance.Draw(_spriteBatch);

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

                    //Draws wave number to screen
                    _spriteBatch.DrawString(buttonText,
                        $"Wave: {LevelManager.Instance.Wave}",
                        new Vector2(15, 90),
                        Color.Black);

                    break;

                //Wave complete screen
                case MenuStates.WaveComplete:

                    //Draws wave completion to screen
                    _spriteBatch.DrawString(buttonText,
                        String.Format("Completed Wave {0}!",
                        LevelManager.Instance.Wave),
                        new Vector2(waveComplete.Rectangle.X + 50, waveComplete.Rectangle.Y - 250),
                        Color.Black);
                    waveComplete.Render(_spriteBatch, "CONTINUE", waveComplete.Rectangle);
                    break;

                //Pause menu
                case MenuStates.Pause:

                    // Draws the buttons for the pause menu
                    pauseBack.Render(_spriteBatch, "BACK", pauseBack.Rectangle);
                    backToMenu.Render(_spriteBatch, "BACK TO MENU", backToMenu.Rectangle);
                    _spriteBatch.DrawString(titleFont, "PAUSED", new Vector2(windowWidth / 2 - (titleFont.MeasureString("PAUSED").X / 2), 30), Color.Black);
                    break;

                //Game Over Screen
                case MenuStates.GameOver:
                    GraphicsDevice.Clear(Color.DarkRed);

                    // Draws the needed items for the game over screen 
                    _spriteBatch.DrawString(titleFont, "GAME OVER", new Vector2(windowWidth / 2 - (titleFont.MeasureString("GAME OVER").X / 2), 30), Color.Black);


                    if (LevelManager.Instance.Wave == 0)
                    {
                        _spriteBatch.DrawString(buttonText, $"Waves Survived: {LevelManager.Instance.Wave}",
                       new Vector2(windowWidth / 2 - (buttonText.MeasureString($"Waves Survived: {LevelManager.Instance.Wave - 1}").X / 2), 250), Color.Black);
                    }
                    else
                    {
                        _spriteBatch.DrawString(buttonText, $"Waves Survived: {LevelManager.Instance.Wave - 1}",
                        new Vector2(windowWidth / 2 - (buttonText.MeasureString($"Waves Survived: {LevelManager.Instance.Wave - 1}").X / 2), 250), Color.Black);
                    }

                    menuButton.Render(_spriteBatch, "MENU", menuButton.Rectangle);
                    retryButton.Render(_spriteBatch, "RETRY", retryButton.Rectangle);
                    break;
            }

            _spriteBatch.Draw
                (crossHair,
                new Vector2(Mouse.GetState().Position.X - crossHair.Width / 2,
                Mouse.GetState().Position.Y - crossHair.Height / 2),
                Color.White
                );

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Resets level and player after a retry/start of game
        /// </summary>
        public void Reset()
        {
            //Resets player position
            player.X = playerStartX;
            player.Y = playerStartY;

            //Clears enemy and bullet list
            EnemyManager.Instance.Enemies.Clear();
            BulletManager.Instance.Bullets.Clear();

            // Changes health based on the GodMode setting
            if (!godMode.IsOn)
            {
                player.PlayerHealth = 100;
                player.Scraps = 10;
            }

            //Resets wave and builds level
            LevelManager.Instance.Wave = 0;
            LevelManager.Instance.BuildLevel();
        }

        /// <summary>
        /// Method that advances to the next wave
        /// </summary>
        public void Continue()
        {
            //Resets player position
            player.X = playerStartX;
            player.Y = playerStartY;

            // Rebuilds level
            if (!(player.PlayerHealth <= 0))
            {
                LevelManager.Instance.BuildLevel();
            }

            // Changes health based on the GodMode setting
            if (godMode.IsOn)
            {
                player.Scraps = 10;
            }

        }
    }
}