using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lightweight
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int windowWidth;
        int windowHeight;
        private Texture2D floorTile;
        private Texture2D wall;
        private Player player;

        private Texture2D horizontalWall;
        

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            floorTile = Content.Load<Texture2D>("floor_tile");
            wall = Content.Load<Texture2D>("starter_wall");
            horizontalWall = Content.Load<Texture2D>("starter_wall_horizontal");
            // TODO: use this.Content to load your game content here

            player.LoadAnims(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            // TODO: Add your drawing code here

            //Creates floor tile/wall objects
            Tile floorTileObject = new Tile(floorTile, new Rectangle(100, 80, 16, 16), windowHeight, windowWidth);
            Wall walls = new Wall(wall, new Rectangle(100, 80, 12, 50), windowHeight, windowWidth);
            

            //This draws the tiles/walls across the screen
            floorTileObject.Draw(_spriteBatch);
            player.Draw(_spriteBatch);

            walls.Draw(_spriteBatch);

            
            _spriteBatch.End();  
            base.Draw(gameTime);
        }
    }
}