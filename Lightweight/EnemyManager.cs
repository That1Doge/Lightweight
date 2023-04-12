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
        private List<Enemy> enemies;
        private Dictionary<string, Texture2D> spriteSheets;

        private EnemyManager()
        {
            spriteSheets = new Dictionary<string, Texture2D>();
            enemies = new List<Enemy>();
        }

        public static EnemyManager Instance
        {
            get 
            {
                if (instance == null) instance = new EnemyManager();
                return instance;
            }
        }

        public void LoadSpriteSheets(ContentManager content)
        {
            spriteSheets.Add("run", content.Load<Texture2D>("PNG/Mage/Run/run"));
        }

        public void SpawnEnemies(int numSpawn, Vector2 pos)
        {
            for(int i = 0; i < numSpawn; i++)
            {
                enemies.Add(new Enemy(pos, new Animator()));
                enemies[enemies.Count - 1].Anims.AddAnimation(EnemyState.RunRight,
                    new Animation(spriteSheets["run"], 6));
                enemies[enemies.Count - 1].Anims.AddAnimation(EnemyState.RunLeft,
                    new Animation(spriteSheets["run"], 6, SpriteEffects.FlipHorizontally));
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
