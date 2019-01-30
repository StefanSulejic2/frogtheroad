using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    class BackgroundSprite:GameObject
    {
        public BackgroundSprite(Texture2D texture, float X, float Y):base(texture, X, Y)
        {
        }
        public void Update(GameWindow window, int nrBackgroundsY)
        {
        }
    }
}