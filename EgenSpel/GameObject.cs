﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    // Code for all objects in the game
    class GameObject
    {
        // Variable for textures
        protected Texture2D texture;
        protected Vector2 vector;

        // Position and spriteBatch for the objects.
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

    // Movement identity for the objects
    class MovingObject : GameObject
    {
         protected Vector2 speed;
         public MovingObject(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y)
         {
             this.speed.X = speedX;
             this.speed.Y = speedY;
         }
    }
    // Hitbox for objects thatare meant to be physical
    abstract class PhysicalObject : MovingObject
    {
        protected bool isAlive = true;
        public PhysicalObject (Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, speedX, speedY)
        {
        }

        public bool CheckCollision(PhysicalObject other)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Width), Convert.ToInt32(Height));
            Rectangle otherRect = new Rectangle(Convert.ToInt32(other.X), Convert.ToInt32(other.Y), Convert.ToInt32(other.Width), Convert.ToInt32(other.Height));
            return myRect.Intersects(otherRect);
        }
        public bool IsAlive {  get { return isAlive; } set { isAlive = value; } }
    }
}