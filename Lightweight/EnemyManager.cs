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

namespace Lightweight
{
    internal class EnemyManager
    {
        private static EnemyManager instance;
        private List<Enemy> enemies;
        private List<Scrap> scraps;
        private bool freeze;
        private Dictionary<object, Animation> enemyAnims;
        private Dictionary<object, Animation> scrapAnims;
        private Texture2D hitBoxTex;

        public List<Enemy> Enemies { get { return enemies; } }
        public List<Scrap> Scraps { get { return scraps; } }
        public bool Freeze { get { return freeze; } set { freeze = value; } }

        private EnemyManager()
        {
            enemies = new List<Enemy>();
            scraps = new List<Scrap>();
            enemyAnims = new Dictionary<object, Animation>();
            scrapAnims = new Dictionary<object, Animation>();
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
            enemyAnims.Add(EnemyState.RunLeft, 
                new Animation(content.Load<Texture2D>("PNG/enemy"), 5, SpriteEffects.FlipHorizontally));
            enemyAnims.Add(EnemyState.RunRight,
                new Animation(content.Load<Texture2D>("PNG/enemy"), 5));
            scrapAnims[0] = new Animation(content.Load<Texture2D>("PNG/scrap"), 8,
                SpriteEffects.None, true, 8);
            hitBoxTex = content.Load<Texture2D>("hitbox");
        }

        public void SpawnEnemies(int numSpawn, Vector2 pos)
        {
            for(int i = 0; i < numSpawn; i++)
            {
                enemies.Add(new Enemy(LevelManager.Instance.Wave * 10,pos, new Animator(enemyAnims)));
            }
        }

        public void SpawnScrap(int value, Vector2 pos)
        {
            scraps.Add(new Scrap(value, pos, new Animator(scrapAnims), 10));
        }

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

        public void KillEnemy(Enemy enemy) 
        {
            SpawnScrap(LevelManager.Instance.Wave + 2, enemy.Pos);
            enemies.Remove(enemy);
        }

        public void RemoveScrap(Scrap scrap)
        {
            scraps.Remove(scrap);
        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(sb);
                if (Game1.Instance.GodMode)
                {
                    sb.Draw(hitBoxTex, enemies[i].HitBox, Color.Red);
                }
            }

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
