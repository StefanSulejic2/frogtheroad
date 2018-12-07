﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    /// <summary>
    /// Här skrivs kod för att kunna skapa objekter och sprites (Ej fysiska)
    /// </summary>
    class GameObject
    {
        /// <summary>
        /// Skapar variabler för teturer.
        /// </summary>
        protected Texture2D texture;
        protected Vector2 vector;

        /// <summary>
        /// Spritebatch för att kunna rita ut dem och deras position.
        /// </summary>
        public GameObject(Texture2D texture, float X, float Y)
        {
            this.texture = texture;
            this.vector.X = X;
            this.vector.Y = Y;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, vector, Color.White);
        }
        public float X { get { return vector.X; } }
        public float Y { get { return vector.Y; } }
        public float Width { get { return texture.Width; } }
        public float Height { get { return texture.Height; } }
    }

    /// <summary>
    /// Rörelse för sprites.
    /// </summary>
    class MovingObject : GameObject
    {
         protected Vector2 speed;
         public MovingObject(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y)
         {
             this.speed.X = speedX;
             this.speed.Y = speedY;
         }
    }
}