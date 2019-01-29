using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    class Fly : PhysicalObject
    {
        double timeToDie;

        public Fly(Texture2D texture, float X, float Y, GameTime gameTime) : base(texture, X, Y, 0, 2f)
        {
            timeToDie = gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
