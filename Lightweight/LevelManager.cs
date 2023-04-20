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
        Texture2D trapTexture;
        Texture2D topWallTexture;
        Texture2D bottomWallTexture;
        Texture2D rightWallTexture;
        Texture2D leftWallTexture;
        bool isLoaded;
        Rectangle hitbox;
        GraphicsDeviceManager _graphics;
        int windowWidth;
        int windowHeight;
        int yPosTile;
        List<Tile> floorTiles = new List<Tile>();
        List<Wall> walls = new List<Wall>();
        EnemyManager enemyManager;
        
        Random rng = new Random();
        int trapChance;
        int enemyChance;
        int attempt = 0;

        public Texture2D TileTexture { get { return tileTexture; } set { tileTexture = value; } }
        public Texture2D TrapTexture { get { return trapTexture; } set { trapTexture = value; } }
        public bool IsLoaded { get { return isLoaded; } set { isLoaded = value; } } 
        public List<Tile> FloorTiles { get { return floorTiles; } set { floorTiles = value; } } 
        public List<Wall> Walls { get { return walls; } set { walls = value; } }

        public LevelManager(Texture2D tile, Texture2D trap, Texture2D topWall, Texture2D bottomWall, Texture2D leftWall, Texture2D rightWall, int height, int width) 
        { 
            tileTexture = tile;
            trapTexture = trap;
            topWallTexture = topWall;
            bottomWallTexture = bottomWall;
            leftWallTexture = leftWall;
            rightWallTexture = rightWall;
            windowWidth = width;
            windowHeight = height;
            isLoaded = false;
        }   

        public void LoadLevel(string filename) 
        {
            StreamReader input = new StreamReader(filename);
            string line = "";
            yPosTile = 0;
            while ((line = input.ReadLine()) != null)
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
                    for (int i = 1; i < 24; i++) 
                    {
                        floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
                        switch (tilesToRead[i - 1])
                        {
                            case 'X':
                                floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
                                break;
                            case 'O':
                                floorTiles.Add(new Tile(trapTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), true));
                                break;
                            case 'E':
                                enemyManager.SpawnEnemies(1, new Vector2(floorTiles[i - 1].X + 32, yPosTile));
                                floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
                                break;
                        }

                    }
                    floorTiles.Add(new Tile(tileTexture, new Rectangle(768, yPosTile, 32, 32), false));
                    yPosTile += 32;
                    attempt++;
                }
                floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
                for (int i = 1; i < 25; i++)
                {
                    floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
                }
            }
            BuildWalls();
            input.Close();
        }

        /// <summary>
        /// Method that builds the level if it has not been loaded
        /// </summary>
        public void BuildLevel() 
        {
            if (isLoaded == true)
            {
                return;
            }
            else if (floorTiles.Count != 0)
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
                        trapChance = rng.Next(1, 35);
                        enemyChance = rng.Next(1, 50);

                        while (enemyChance == trapChance) 
                        {
                            enemyChance = rng.Next(1, 21);
                        }

                        if (x == 0)
                        {
                            floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
                        }
                        else
                        {
                            if (trapChance == 1 && x != 24 && i != 14 && i != 0 && x != 10 && x != 11 && x != 12)
                            {
                                floorTiles.Add(new Tile(trapTexture, new Rectangle(floorTiles[x - 1].X + 32, yPosTile, 32, 32), true));
                            }
                            else if (enemyChance == 1 && x != 24 && i != 14 && i != 0 && x != 10 && x != 11 && x != 12)
                            {
                                EnemyManager.Instance.SpawnEnemies(1, new Vector2(floorTiles[i - 1].X + 32, yPosTile));
                                floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[x - 1].X + 32, yPosTile, 32, 32), false));
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
            BuildWalls();
        }

        public void BuildWalls() 
        {
            walls.Add(new Wall(bottomWallTexture, new Rectangle(0, 468, 800, 12)));
            walls.Add(new Wall(rightWallTexture, new Rectangle(0, 0, 12, 476)));
            walls.Add(new Wall(leftWallTexture, new Rectangle(788, 0, 12, 476)));
            walls.Add(new Wall(topWallTexture, new Rectangle(4, 0, 792, 12)));
        }

        public void Draw(SpriteBatch sb) 
        {
            foreach (Tile tile in floorTiles)
            {
                tile.Draw(sb);
            }
            foreach (Wall wall in walls) 
            { 
                wall.Draw(sb);
            }
        }

        
    }
}
