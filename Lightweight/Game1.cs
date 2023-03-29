﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int windowWidth;
        int windowHeight;
        private Texture2D floorTile;
        private Texture2D wall;
        private MenuStates menuState;
        private MenuButton button;
        private SpriteFont buttonText;
        private Player player;
        private Texture2D horizontalWall;
        private Rectangle buttonRectangle;
        private Texture2D buttonTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;
            player = new Player();
            menuState = MenuStates.MainMenu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            floorTile = Content.Load<Texture2D>("floor_tile");
            wall = Content.Load<Texture2D>("starter_wall");
            buttonText = Content.Load<SpriteFont>("arial-12");
            horizontalWall = Content.Load<Texture2D>("starter_wall_horizontal");
            buttonTexture = Content.Load<Texture2D>("buttonPlaceholder");

            // TODO: use this.Content to load your game content here
            button = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth/2 - buttonTexture.Width/2 , 
                windowHeight / 2, buttonTexture.Width, buttonTexture.Height));

            player.LoadAnims(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Switch statement that changes the Menu States depending on what action is done
            switch(menuState)
            {
                case MenuStates.MainMenu:
                    // Tests the menu state 
                    
                    if(Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        menuState = MenuStates.Gameplay;
                    }
                    


                    break;
                case MenuStates.InstructionMenu:
                    
                    break;
                case MenuStates.OptionsMenu:
                    
                    break;
                case MenuStates.Gameplay:
                    
                    break;
                case MenuStates.Pause:

                    break;
                case MenuStates.GameOver:

                    break;
            }
            // TODO: Add your update logic here
            player.Update(gameTime);
            
            
            base.Update(gameTime);
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
                    button.Draw(_spriteBatch, "PLAY");

                    if(button.ButtonClicked())
                    {
                        menuState = MenuStates.Gameplay;
                    }
                    break;
                case MenuStates.InstructionMenu:

                    break;
                case MenuStates.OptionsMenu:

                    break;
                case MenuStates.Gameplay:
                    /*Creates floor tile object
                        Tile floorTileObject = new Tile(floorTile, new Rectangle(0, 0, 16, 16), 
                        windowHeight, windowWidth);
                        Wall walls = new Wall(wall, new Rectangle(0, 0, 12, 50), 
                        windowHeight, windowWidth);
                        Creates floor tile/wall objects
                    */
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    Tile floorTileObject = new Tile(floorTile, new Rectangle(0, 0, 16, 16), windowHeight, windowWidth);
                    Wall walls = new Wall(wall, new Rectangle(0, 0, 12, 50), windowHeight, windowWidth);
            

                    //This draws the tiles/walls across the screen
                    floorTileObject.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    walls.Draw(_spriteBatch);


                    //This draws the tiles across the screen
                    //floorTileObject.Draw(_spriteBatch);
                    _spriteBatch.DrawString(
                        buttonText,
                        $"Scraps: {player.Scraps}",
                        new Vector2(15, 10),
                        Color.Black);

                    break;
                case MenuStates.Pause:

                    break;
                case MenuStates.GameOver:

                    break;
            }


            _spriteBatch.End();  
            base.Draw(gameTime);
        }
    }
}