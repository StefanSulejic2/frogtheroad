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
    abstract class Enemy : PhysicalObject
    {
        /// <summary>
        /// Basvariabler för enemy
        /// </summary>
        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, speedX, speedY)
        {
        }
        
        public abstract void Update(GameWindow window);
    }
}