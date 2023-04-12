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
        Rectangle hitbox;
        GraphicsDeviceManager _graphics;
        int windowWidth;
        int windowHeight;
        int yPosTile;
        List<Tile> floorTiles = new List<Tile>();
        Texture2D trapTexture;
        Random rng = new Random();
        int trapChance;
        int attempt = 0;

        public List<Tile> FloorTiles { get { return floorTiles; } set { floorTiles = value; } } 

        public LevelManager(Texture2D tile, Texture2D trap, Texture2D wall, int height, int width) 
        { 
            tileTexture = tile;
            trapTexture = trap;
            windowWidth = width;
            windowHeight = height;
        }   
        public void LoadLevel(string filename) 
        {
            StreamReader input = new StreamReader(filename);
            string line = input.ReadLine();
            yPosTile = 0;
            while (line != null)
            {
                string[] split = line.Split(',');

                if (attempt == 0)
                {
                    string[] symbols = new string[split.Length];
                    for (int i = 0; i < symbols.Length; i++) 
                    {
                        symbols[i] = split[i];
                    }

                    floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
                    for (int i = 1; i < 25; i++) 
                    {
                        floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
                    }
                    yPosTile += 32;
                    attempt++;
                }
                else if (attempt < 14)
                {
                    string tilesToRead = line;

                    
                    for (int i = 0; i < 22; i++) 
                    {
                        //Add random enemy spawns
                        switch (tilesToRead[i]) 
                        {
                            case 'X':
                                if (i == 0) 
                                {
                                    floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
                                }
                                else 
                                {
                                    floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
                                } 
                                break;
                            case 'O':
                                floorTiles.Add(new Tile(trapTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), true));
                                break;
                        }
                    }

                   
                    yPosTile += 32;
                    attempt++;
                }
                line = input.ReadLine();
            }
            input.Close();
        }

        public void BuildLevel() 
        {
            if (floorTiles.Count != 0)
            {
                floorTiles.Clear();
                BuildLevel();
            }
            else 
            {
                yPosTile = 0;
                for (int i = 0; i < 15; i++)
                {
                    for (int x = 0; x < 25; x++)
                    {
                        trapChance = rng.Next(1, 21);
                        if (x == 0)
                        {
                            floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
                        }
                        else
                        {
                            if (trapChance == 1 && x != 24 && i != 14 && i != 0)
                            {
                                floorTiles.Add(new Tile(trapTexture, new Rectangle(floorTiles[x - 1].X + 32, yPosTile, 32, 32), true));
                            }
                            else
                            {
                                floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[x - 1].X + 32, yPosTile, 32, 32), false));
                            }
                        }
                    }
                    yPosTile += 32;
                }
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
