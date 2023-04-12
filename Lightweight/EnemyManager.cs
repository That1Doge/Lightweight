using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal class EnemyManager
    {
        private static EnemyManager instance;
        private static List<Enemy> enemies;
        private int maxEnemies;

        private EnemyManager(int maxEnemies)
        {
            this.maxEnemies = maxEnemies;
        }

        public static EnemyManager Instance
        {
            get 
            {
                if (Instance == null) instance = new EnemyManager(10);
                return Instance;
            }
        }

        public int MaxEnemies 
        { 
            get { return maxEnemies; } 
            set { maxEnemies = value; }
        }

        public void SpawnEnemies()
        {
            // spawn rules
            Vector2 spawnPos = new Vector2(10, 10);

            for(int i = 0; i < maxEnemies; i++)
            {
                enemies.Add(new Enemy(spawnPos));
            }
        }

        public void Update(GameTime gt, Player player)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gt, player);
            }
        }

        public void Remove(Enemy enemy) 
        {
            enemies.Remove(enemy);
        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(sb);
            }
        }
    }
}
