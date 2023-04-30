using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Enemy Manager Class 
    /// Creates Enemy Manager
    /// </summary>
    internal class EnemyManager
    {
        //Fields used in method
        private static EnemyManager instance;
        private List<Enemy> enemies;
        private List<Scrap> scraps;
        private bool freeze;
        private Dictionary<object, Animation> enemyAnims;
        private Dictionary<object, Animation> scrapAnims;
        private Texture2D hitBoxTex;

        /// <summary>
        /// Property that returns list of enemies
        /// </summary>
        public List<Enemy> Enemies { get { return enemies; } }
        public List<Scrap> Scraps { get { return scraps; } }
        public bool Freeze { get { return freeze; } set { freeze = value; } }


        /// <summary>
        /// Constructor
        /// </summary>
        private EnemyManager()
        {
            enemies = new List<Enemy>();
            scraps = new List<Scrap>();
            enemyAnims = new Dictionary<object, Animation>();
            scrapAnims = new Dictionary<object, Animation>();
        }

        /// <summary>
        /// Creates instance of the enemy manager
        /// </summary>
        public static EnemyManager Instance
        {
            get 
            {
                if (instance == null) instance = new EnemyManager();
                return instance;
            }
        }

        /// <summary>
        /// Loads sprite sheets of enemy
        /// </summary>
        /// <param name="content">COntent</param>
        public void LoadSpriteSheets(ContentManager content)
        {
            enemyAnims.Add(EnemyState.RunLeft, 
                new Animation(content.Load<Texture2D>("PNG/enemy"), 5, SpriteEffects.FlipHorizontally));
            enemyAnims.Add(EnemyState.RunRight,
                new Animation(content.Load<Texture2D>("PNG/enemy"), 5));
            scrapAnims[0] = new Animation(content.Load<Texture2D>("PNG/scrap"), 8,
                SpriteEffects.None, true, 8);
            hitBoxTex = content.Load<Texture2D>("hitbox");
        }

        /// <summary>
        /// Spawns enemies
        /// </summary>
        /// <param name="numSpawn">Number of enemies to spawn</param>
        /// <param name="pos">Position of where to stawn them</param>
        public void SpawnEnemies(int numSpawn, Vector2 pos)
        {
            // sets enemy speed, firing rate, and health
            double enemyShotRate = 2.0 - LevelManager.Instance.Wave / 10;
            int enemyHealth = LevelManager.Instance.Wave * 10;
            int enemySpeed = LevelManager.Instance.Wave + 1;

            // minum shot timer of 0.5, and max speed of 8
            if (enemyShotRate < 0.5) enemyShotRate = 0.5;
            if(enemySpeed > 8) enemySpeed = 8;

            //Spawns enemies based on parameters (enemies are faster and have more health every wave)
            for(int i = 0; i < numSpawn; i++)
            {
                enemies.Add(new Enemy(enemyHealth, enemySpeed, enemyShotRate, pos, new Animator(enemyAnims)));
            }
        }


        /// <summary>
        /// Method that spawns scrap
        /// </summary>
        /// <param name="value">How much the scrap spawned is worth</param>
        /// <param name="pos">Position at which to spawn them</param>
        public void SpawnScrap(int value, Vector2 pos)
        {
            scraps.Add(new Scrap(value, pos, new Animator(scrapAnims), 10));
        }

        /// <summary>
        /// Update method of enemy manager
        /// </summary>
        /// <param name="gt">Gametime</param>
        /// <param name="player">Player</param>
        public void Update(GameTime gt, Player player)
        {
            if (!Game1.Instance.GodMode) freeze = false;
            if (freeze) return;
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gt, player);
            }
            for(int i = 0; i < scraps.Count; i++)
            {
                scraps[i].Update(gt, player);
            }
        }

        /// <summary>
        /// Method that kills enemies
        /// </summary>
        /// <param name="enemy">Enemy instance</param>
        public void KillEnemy(Enemy enemy) 
        {
            //Drops scrap upon enemy death
            SpawnScrap(LevelManager.Instance.Wave + 2, enemy.Pos);
            enemies.Remove(enemy);
        }

        /// <summary>
        /// Method that removes scrap
        /// </summary>
        /// <param name="scrap">Scrap to remove</param>
        public void RemoveScrap(Scrap scrap)
        {
            scraps.Remove(scrap);
        }

        /// <summary>
        /// Draw method of enemy
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public void Draw(SpriteBatch sb)
        {
            //Draws enemies in list
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(sb);
                if (Game1.Instance.GodMode)
                {
                    sb.Draw(hitBoxTex, enemies[i].HitBox, Color.Red);
                }
            }

            //Draws Scraps
            for(int i = 0; i < scraps.Count; i++)
            {
                scraps[i].Draw(sb);
                if (Game1.Instance.GodMode)
                {
                    sb.Draw(hitBoxTex, scraps[i].HitBox, Color.Red);
                }
            }
        }
    }
}
