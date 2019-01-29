using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    // Class with all elements of the game
    static class GameElements
    {
        static Texture2D menuSprite;
        static Vector2 menuPos;
        static Player player;
        static List<Enemy> enemies;
        static List<Fly> fly;
        static Texture2D flySprite;
        static PrintText printText;

        public enum State { Menu, Run, Quit};
        public static State currentState;
        
        public static void Initialize()
        {
            fly = new List<Fly>();
        }
        public static void LoadContent(ContentManager content, GameWindow window)
        {
            // Menu and its position
            menuSprite = content.Load<Texture2D>("frogg");
            menuPos.X = window.ClientBounds.Width / 2 - menuSprite.Width / 2;
            menuPos.Y = window.ClientBounds.Height / 2 - menuSprite.Height / 2;

            // Player
            player = new Player(content.Load<Texture2D>("frogg"), 380, 400, 2.5f, 4.5f);

            // Collectable point and printed score
            flySprite = content.Load<Texture2D>("ladybugg");
            printText = new PrintText(content.Load<SpriteFont>("myFont"));

            // Enemies
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("carL");
            for (int i = 0; i < 2; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carL temp = new carL(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carR");
            for (int i = 0; i < 2; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carR temp = new carR(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
        }
        public static State MenuUpdate()
        {
            // Keybindings
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.S))
                return State.Run;
            if (keyboardState.IsKeyDown(Keys.A))
                return State.Quit;

            return State.Menu;
        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuSprite, menuPos, Color.White);
        }
        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            //Player checker
            player.Update(window, gameTime);

            // Enemy checker
            foreach (Enemy e in enemies.ToList())
            {
                if (e.IsAlive)
                {
                    if (e.CheckCollision(player))
                        player.IsAlive = false;
                    e.Update(window);
                }
                else
                    enemies.Remove(e);
            }

            // Point spawner
            Random random = new Random();
            int newfly = random.Next(1, 200);
            if (newfly == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - flySprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - flySprite.Height);
                fly.Add(new Fly(flySprite, rndX, rndY, gameTime));
            }

            // Point checker.
            foreach (Fly f in fly.ToList())
            {
                if (f.IsAlive)
                {
                    f.Update(gameTime);

                    if (f.CheckCollision(player))
                    {
                        fly.Remove(f);
                        player.Points++;
                    }
                }
                else
                    fly.Remove(f);
            }

            // If player is killed, return to menu
            if (!player.IsAlive)
            {
                Reset(window, content);
                return State.Menu;
            }
            // Stay in the game as default.
            return State.Run;
        }
        public static void RunDraw(SpriteBatch spriteBatch)
        {
            // Drawing the sprites
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach (Fly f in fly)
                f.Draw(spriteBatch);
        }
        private static void Reset(GameWindow window, ContentManager content)
        {
            player.Reset(380, 400, 2.5f, 4.5f);

            enemies.Clear();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("carL");
            for (int i = 0; i < 2; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carL temp = new carL(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carR");
            for (int i = 0; i < 2; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carR temp = new carR(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
        }
    }
}