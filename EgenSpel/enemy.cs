using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    /// <summary>
    /// Kod för att skapa fiender.
    /// </summary>
    class Enemy : PhysicalObject
    {
        /// <summary>
        /// isAlive innebär att den existerar, om inte den ändras till false.
        /// </summary>
        bool isAlive = true;

        /// <summary>
        /// Basvariabler för enemy
        /// </summary>
        public Enemy(Texture2D texture, float X, float Y) : base(texture, X, Y, 6f, 0f)
        {
        }

        /// <summary>
        /// Rörelsevariabler för enemy
        /// </summary>
        public void Update(GameWindow window)
        {
            vector.X += speed.X;
            if (vector.X > window.ClientBounds.Width - texture.Width || vector.X < 0)
                speed.X *= 1;
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
                isAlive = false;

        }
        /// <summary>
        /// Loop egenskaper för isAlive
        /// </summary>
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }

        }
    }
}