﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    // Code for enemy that heads in Left direction
    class carL2 : Enemy
    {
        public carL2(Texture2D texture, float X, float Y) : base(texture, X, 190, 3f, 0f)
        {
        }
        public override void Update(GameWindow window)
        {
            vector.X += speed.X;
            if (vector.X > window.ClientBounds.Width - texture.Width || vector.X < 0)
                speed.X *= -1;
        }
    }
}
