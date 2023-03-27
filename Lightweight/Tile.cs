using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightweight
{
    internal class Tile : GameObject
    {
        public int X { get { return this.rectangle.X; } set { this.rectangle.X = value; } }
        public int Y { get { return this.rectangle.Y; } set { this.rectangle.Y = value; } }
        public Tile(Texture2D texture, Rectangle rectangle, int windowWidth, int windowHeight)
            : base(texture, rectangle, windowHeight, windowWidth)
        {
            this.texture = texture;
            this.rectangle = rectangle;
        }

        public override void Update() 
        { 
            
        }

        //total possible amount of tiles in default window size: 50x30
        public override void Draw(SpriteBatch sb) 
        {
            int ogXCoord = this.X;

            for (int i = 0; i < ((windowWidth - 180) / 15); i++)
            {
                if (i != 0) 
                {
                    this.Y += 16;
                }
                for (int x = 0; x < ((windowHeight - 200) / 16); x++)
                {
                    if (x == 0)
                    {
                        this.X = ogXCoord;
                    }
                    sb.Draw(this.texture, this.rectangle, Color.White);
                    this.X += 16;
                }
            }
            
        }
    }
}
