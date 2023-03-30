using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

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
        private Texture2D leftWall;
        private Texture2D rightWall;
        private Texture2D topWall;
        private Texture2D bottomWall;
        private MenuStates menuState;
        private MenuButton playButton;
        private MenuButton optionsButton;
        private MenuButton quitButton;
        private SpriteFont buttonText;
        private Player player;
        private Rectangle buttonRectangle;
        private Texture2D buttonTexture;
        private List<Tile> floorTiles;
        private List<Wall> walls;
        int yPosTile;


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
            floorTiles = new List<Tile>();
            walls = new List<Wall>();
            yPosTile = 0;
            menuState = MenuStates.MainMenu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            floorTile = Content.Load<Texture2D>("floor_tile");
            leftWall = Content.Load<Texture2D>("starter_wall");
            buttonText = Content.Load<SpriteFont>("arial-12");
            buttonTexture = Content.Load<Texture2D>("buttonPlaceholder");
            bottomWall = Content.Load<Texture2D>("bottom_wall");
            rightWall = Content.Load<Texture2D>("right_wall");
            topWall = Content.Load<Texture2D>("top_wall");

            // TODO: use this.Content to load your game content here
            // Loads all of the Main Menu buttons
            playButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth/2 - buttonTexture.Width/2 , 
                windowHeight / 2, buttonTexture.Width, buttonTexture.Height));
            optionsButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width / 2,
                windowHeight / 2 + (buttonTexture.Height * 2), buttonTexture.Width, buttonTexture.Height));
            quitButton = new MenuButton(buttonTexture, buttonText, buttonRectangle = new Rectangle(windowWidth / 2 - buttonTexture.Width / 2,
                windowHeight / 2 + (buttonTexture.Height * 4), buttonTexture.Width, buttonTexture.Height));

            //Creates floor tiles and adds them to a list
            for (int i = 0; i < (windowHeight / 16); i++) 
            {
                for (int x = 0; x < ((windowWidth) / 15); x++) 
                { 
                    if (x == 0) 
                    {
                        floorTiles.Add(new Tile(floorTile, new Rectangle(0, yPosTile, 16, 16)));   
                    }
                    else 
                    {
                        floorTiles.Add(new Tile(floorTile, new Rectangle(floorTiles[x - 1].X + 16, yPosTile, 16, 16)));
                    }
                }
                yPosTile += 16;
            }

            //Creates walls and adds them to a list
            for (int i = 0; i < 4; i++) 
            {
                for (int x = 0; x < 11; x++) 
                { 
                    switch (i) 
                    { 
                        case 0:
                            if (x == 0)
                            {
                                walls.Add(new Wall(leftWall, new Rectangle(0, 0, 12, 50)));
                            } 
                            else 
                            {
                                walls.Add(new Wall(leftWall, new Rectangle(0, walls[walls.Count - 1].Y + 43, 12, 50)));
                            }
                            break;
                        case 1:
                            if (x == 0)
                            {
                                walls.Add(new Wall(rightWall, new Rectangle(windowWidth - 12, windowHeight - 50, 12, 50)));
                            }
                            else
                            {
                                walls.Add(new Wall(rightWall, new Rectangle(windowWidth - 12, walls[walls.Count - 1].Y - 43, 12, 50)));
                            }
                            break;
                    }
                }
                for (int x = 0; x < 21; x++)
                {
                    switch (i)
                    {
                        case 0:
                            if (x == 0)
                            {
                                walls.Add(new Wall(topWall, new Rectangle(0, 0, 50, 12)));
                            }
                            else
                            {
                                walls.Add(new Wall(topWall, new Rectangle(walls[walls.Count - 1].X + 43, 0, 50, 12)));
                            }
                            break;
                        case 1:
                            if (x == 0)
                            {
                                walls.Add(new Wall(bottomWall, new Rectangle(4, windowHeight - 12, 50, 12)));
                            }
                            else
                            {
                                walls.Add(new Wall(bottomWall, new Rectangle(walls[walls.Count - 1].X + 43, windowHeight - 12, 50, 12)));
                            }
                            break;
                    }
                }
            }

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

                    //Collision mechanic
                    foreach (Wall walls in walls) 
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
                    
                    // Draws buttons to the Main Menu, and detects if the buttons are clicked and what to do if they are
                    playButton.Draw(_spriteBatch, "PLAY");
                    optionsButton.Draw(_spriteBatch, "OPTIONS");
                    quitButton.Draw(_spriteBatch, "QUIT");
                    if(playButton.ButtonClicked())
                    {
                        menuState = MenuStates.Gameplay;
                    }
                    else if(optionsButton.ButtonClicked())
                    {

                    }
                    else if(quitButton.ButtonClicked())
                    {
                        Exit();
                    }

                    break;
                case MenuStates.InstructionMenu:

                    break;
                case MenuStates.OptionsMenu:

                    break;
                case MenuStates.Gameplay:
                    GraphicsDevice.Clear(Color.Black);

                    
                    
                    

                    //This draws the tiles/walls across the screen
                    foreach (Tile tile in floorTiles) 
                    {
                        tile.Draw(_spriteBatch);
                    }
                    player.Draw(_spriteBatch);
                    foreach (Wall wall in walls)
                    {
                        wall.Draw(_spriteBatch);
                    }

                    _spriteBatch.DrawString(
                        buttonText,
                        $"Scraps: {player.Scraps}",
                        new Vector2(15, 10),
                        Color.Black);

                    _spriteBatch.DrawString(buttonText, player.Scraps.ToString(), Vector2.Zero, Color.White);
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