using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    /// <summary>
    /// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
    /// Enemy Manager Class 
    /// Creates Enemy Manager
    /// </summary>
    
    internal class EnemyManager
    {
        //Fields used in method
        private static EnemyManager instance;
        private List<Enemy> enemies;
        private List<Scrap> scraps;
        private Dictionary<object, Animation> enemyAnims;
        private Dictionary<object, Animation> scrapAnims;
        private Texture2D hitBoxTex;

        /// <summary>
        /// Property that returns list of enemies
        /// </summary>
        public List<Enemy> Enemies { get { return enemies; } }


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
                SpriteEffects.None, true, 3);
            hitBoxTex = content.Load<Texture2D>("hitbox");
        }

        /// <summary>
        /// Spawns enemies
        /// </summary>
        /// <param name="numSpawn">Number of enemies to spawn</param>
        /// <param name="pos">Position of where to stawn them</param>
        public void SpawnEnemies(int numSpawn, Vector2 pos)
        {
            //Spawns enemies based on parameters
            for(int i = 0; i < numSpawn; i++)
            {
                enemies.Add(new Enemy(pos, new Animator(enemyAnims), hitBoxTex));
            }
        }


        /// <summary>
        /// Method that spawns scrap
        /// </summary>
        /// <param name="numSpawn">Number of scrap to spawn</param>
        /// <param name="pos">Position at which to spawn them</param>
        public void SpawnScrap(int numSpawn, Vector2 pos)
        {
            for(int i = 0; i <= numSpawn; i++)
            {
                scraps.Add(new Scrap(pos, new Animator(scrapAnims)));
            }
        }

        /// <summary>
        /// Update method of enemy manager
        /// </summary>
        /// <param name="gt">Gametime</param>
        /// <param name="player">Player</param>
        public void Update(GameTime gt, Player player)
        {
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
            }

            //Draws Scraps
            for(int i = 0; i < scraps.Count; i++)
            {
                scraps[i].Draw(sb);
                sb.Draw(hitBoxTex, scraps[i].HitBox, Color.Red);
            }
        }
    }
}
