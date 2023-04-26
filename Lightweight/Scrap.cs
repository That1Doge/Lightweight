using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    public class Scrap
    {
        private Vector2 pos;
        private Rectangle hitBox;
        private Animator anim;
        private double despawnTimer;

        public Rectangle HitBox { get { return hitBox; } }

        public Scrap(Vector2 pos, Animator anim, double despawnTimer = 5)
        {
            this.pos = pos;
            this.despawnTimer = despawnTimer;
            this.anim = anim;
            this.hitBox = new Rectangle((int)pos.X + 6, (int)pos.Y, 18, 22);
        }

        public void Update(GameTime gt, Player player)
        {
            despawnTimer -= gt.ElapsedGameTime.TotalSeconds;
            if(despawnTimer <= 0) { EnemyManager.Instance.RemoveScrap(this); }
            if (hitBox.Intersects(player.HitBox)) 
            { 
                player.Scraps++;
                EnemyManager.Instance.RemoveScrap(this);
            }
            anim.Update(gt, 0);
        }

        public void Draw(SpriteBatch sb) 
        {
            anim.Draw(sb, pos);
        }
    }
}
