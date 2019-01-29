using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    // Code for the points in the game
    class Fly : PhysicalObject
    {
        public Fly(Texture2D texture, float X, float Y, GameTime gameTime) : base(texture, X, Y, 0, 2f)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
