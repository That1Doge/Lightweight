using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal class LevelManager
    {
        Texture2D tileTexture;
        GraphicsDeviceManager _graphics;
        int windowWidth;
        int windowHeight;
        int yPosTile = 0;
        List<Tile> floorTiles = new List<Tile>();

        public LevelManager(Texture2D tile, Texture2D wall, int height, int width) 
        { 
            tileTexture = tile;
            windowWidth = width;
            windowHeight = height;
        }   
        public void LoadLevel(string filename) 
        {
            StreamReader input = null;
            
        }

        public void BuildLevel() 
        { 
            for (int i = 0; i < (windowHeight / 16); i++) 
            {
                for (int x = 0; x < ((windowWidth) / 15); x++) 
                { 
                    if (x == 0) 
                    {
                        floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 16, 16)));   
                    }
                    else 
                    {
                        floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[x - 1].X + 16, yPosTile, 16, 16)));
                    }
                }
                yPosTile += 16;
            } 
        }

        public void Draw(SpriteBatch sb) 
        {
            foreach (Tile tile in floorTiles)
            {
                tile.Draw(sb);
            }
        }
    }
}
