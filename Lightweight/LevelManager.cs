using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Class that manages the level and its aspects
    /// </summary>
    internal class LevelManager
    {
        //Fields used in class
        Texture2D tileTexture;
        Texture2D trapTexture;
        Texture2D topWallTexture;
        Texture2D bottomWallTexture;
        Texture2D rightWallTexture;
        Texture2D leftWallTexture;
        bool isLoaded;
        Rectangle hitbox;
        int yPosTile;
        List<Tile> floorTiles = new List<Tile>();
        List<Wall> walls = new List<Wall>();
        Random rng = new Random();
        int trapChance;
        int wave;
        bool loadedAlready;

        /// <summary>
        /// Property that returns if board is loaded
        /// </summary>
        public bool IsLoaded { get { return isLoaded; } set { isLoaded = value; } } 

        public int Wave { get { return wave; } set { wave = value; } }

        /// <summary>
        /// Property that returns list of floor tiles
        /// </summary>
        public List<Tile> FloorTiles { get { return floorTiles; } set { floorTiles = value; } } 

        /// <summary>
        /// Property that returns list of walls
        /// </summary>
        public List<Wall> Walls { get { return walls; } set { walls = value; } }

        //Parameterized constructor that initialises all things it handles
        public LevelManager(Texture2D tile, Texture2D trap, Texture2D topWall, 
            Texture2D bottomWall, Texture2D leftWall, Texture2D rightWall, 
            int height, int width) 
        { 
            tileTexture = tile;
            trapTexture = trap;
            topWallTexture = topWall;
            bottomWallTexture = bottomWall;
            leftWallTexture = leftWall;
            rightWallTexture = rightWall;
            isLoaded = false;
            loadedAlready = false;
            wave = 0;
        }   

        /// <summary>
        /// Method that loads the board from a file
        /// Files are 58*32
        /// </summary>
        /// <param name="filename">filename of desired load from file</param>
        public void LoadLevel() 
        {
            //Fields used in method
            StreamReader input = new StreamReader("../../../Content/map/loadBoard.txt");
            string line = "";
            yPosTile = 0;

            //Builds border set of floor tiles
            floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
            for (int i = 1; i < 59; i++)
            {
                floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
            }
            floorTiles.Add(new Tile(tileTexture, new Rectangle(1888, yPosTile, 32, 32), false));
            yPosTile += 32;

            //Reads until there is no more text
            while ((line = input.ReadLine()) != null)
            { 
                //Reads from subsequent lines
                string tilesToRead = line;

                floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));

                //Builds board based on symbol present within file
                for (int i = 1; i < 59; i++)
                {
                    //Determines what to place based on symbol
                    switch (tilesToRead[i - 1])
                    {
                        case 'X':
                            floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
                            break;
                        case 'O':
                            floorTiles.Add(new Tile(trapTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), true));
                            break;
                        case 'E':
                            EnemyManager.Instance.SpawnEnemies(1, new Vector2(floorTiles[i - 1].X + 32, yPosTile));
                            floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
                            break;
                    }
                }
                //Places final tile and goes to next line
                floorTiles.Add(new Tile(tileTexture, new Rectangle(1888, yPosTile, 32, 32), false));
                yPosTile += 32;
            }

            //Adds last line of border tiles
            floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
            for (int i = 1; i < 59; i++)
            {
                floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[i - 1].X + 32, yPosTile, 32, 32), false));
            }
            floorTiles.Add(new Tile(tileTexture, new Rectangle(1888, yPosTile, 32, 32), false));

            //Builds walls and closes the input
            wave++;
            BuildWalls();
            input.Close();
        }

        /// <summary>
        /// Method that builds the level if it has not been loaded
        /// </summary>
        public void BuildLevel() 
        {
            //If board has been loaded, do not build a new board
            if (isLoaded == true)
            {
                for (int i = 0; i < wave + 2; i++)
                {
                    EnemyManager.Instance.SpawnEnemies(1, new Vector2(rng.Next(13, 1910), rng.Next(12, 1065)));
                }
                wave++;
                return;
            }
            //If board is not loaded and isn't clear, clear it
            else if (floorTiles.Count != 0)
            {
                floorTiles.Clear();
            }

            yPosTile = 0;

            //For loops that build all floor tiles
            for (int i = 0; i < 34; i++)
            {
                for (int x = 0; x < 60; x++)
                {
                    //Determines chance if tile will spawn trap/enemy 
                    trapChance = rng.Next(1, 35);

                    //Builds border tile
                    if (x == 0)
                    {
                        floorTiles.Add(new Tile(tileTexture, new Rectangle(0, yPosTile, 32, 32), false));
                    }
                    else
                    {
                        //If hit the trap chance, build a trap
                        if (trapChance == 1 && x != 59 && i != 14 && i != 0 && x != 10 && x != 11 && x != 12 && i != 33)
                        {
                            floorTiles.Add(new Tile(trapTexture, new Rectangle(floorTiles[x - 1].X + 32, yPosTile, 32, 32), true));
                        }
                        //Builds normal tile if chance is not hit
                        else
                        {
                            floorTiles.Add(new Tile(tileTexture, new Rectangle(floorTiles[x - 1].X + 32, yPosTile, 32, 32), false));
                        }
                    }
                }
                //changes Y value to build the next line
                yPosTile += 32;
            }
            
            for(int i = 0; i < wave + 2; i++) 
            {
                EnemyManager.Instance.SpawnEnemies(1, new Vector2(rng.Next(13, 1910), rng.Next(12, 1065)));
            }
                
            //Buids walls of level
            BuildWalls();
            wave++;
        }

        /// <summary>
        /// Builds walls of level
        /// </summary>
        public void BuildWalls() 
        {
            walls.Add(new Wall(bottomWallTexture, new Rectangle(0, 1068, 1916, 12)));
            walls.Add(new Wall(rightWallTexture, new Rectangle(1908, 0, 12, 1076)));
            walls.Add(new Wall(leftWallTexture, new Rectangle(0, 0, 12, 1076)));
            walls.Add(new Wall(topWallTexture, new Rectangle(4, 0, 1916, 12)));
        }

        /// <summary>
        /// Draws the level to the screen
        /// </summary>
        /// <param name="sb">Spritebatch file</param>
        public void Draw(SpriteBatch sb) 
        {
            //Draws every tile and wall
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
