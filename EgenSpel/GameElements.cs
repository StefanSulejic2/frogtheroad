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
        static Player player;
        static List<Enemy> enemies;
        static List<Fly> fly;
        static Texture2D flySprite;
        static Menu menu;
        static PrintText printText;
        static Background background;
        static HighScore highScore;
        static SpriteFont font;

        public enum State { Menu, Run, HighScore, AddHS, Quit};
        public static State currentState;
        
        public static void Initialize()
        {
            fly = new List<Fly>();
            highScore = new HighScore(10, font);
        }
        public static void LoadContent(ContentManager content, GameWindow window)
        {
            menu = new Menu((int)State.Menu);
            menu.AddItem(content.Load<Texture2D>("start"), (int)State.Run);
            menu.AddItem(content.Load<Texture2D>("highscore"), (int)State.HighScore);
            menu.AddItem(content.Load<Texture2D>("quit"), (int)State.Quit);
            // Background
            background = new Background(content.Load<Texture2D>("background"), window);

            // Player
            player = new Player(content.Load<Texture2D>("frogg"), 380, 420, 3f, 4.5f);

            // Collectable point and printed score
            flySprite = content.Load<Texture2D>("ladybugg");
            printText = new PrintText(content.Load<SpriteFont>("myFont"));

            // Enemies
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("carL");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carL temp = new carL(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carL");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carL2 temp = new carL2(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carR");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carR temp = new carR(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carR");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carR2 temp = new carR2(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            SpriteFont tmpFont = content.Load<SpriteFont>("myFont");
            printText = new PrintText(tmpFont);
            highScore = new HighScore(5, tmpFont);
            highScore.LoadFromFile("hs.txt"); 

        }
        public static State MenuUpdate(GameTime gameTime)
        {
            // Keybindings
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.S))
                return State.Run;
            if (keyboardState.IsKeyDown(Keys.H))
                return State.HighScore;
            if (keyboardState.IsKeyDown(Keys.A))
                return State.Quit;

            return (State)menu.Update(gameTime);
        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
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

            //To spawn the background
            background.Update(window);

            // If player is killed, switch to Highscore screen
            if (!player.IsAlive)
            {
                Reset(window, content); //TL-190203 Logiskt fel! Vad händer om ni nollställer poängen innan ni sparar den?
                return State.AddHS;
            }
            
                return State.Run;
        }

        public static void RunDraw(SpriteBatch spriteBatch)
        {
            // Drawing the sprites
            background.Draw(spriteBatch);
            printText.Print("Points: " + player.Points, spriteBatch, 360, 0);
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach (Fly f in fly)
                f.Draw(spriteBatch);        
        }

        public static State AddHSUpdate(GameTime gameTime, GameWindow window, ContentManager content)
        {
            if (highScore.EnterUpdate(gameTime, player.Points))
            {
                highScore.SaveToFile("hs.txt");
                Reset(window, content);
                return State.HighScore;
            }
            else
                return State.AddHS;
        }
        public static void AddHSDraw(SpriteBatch spriteBatch)
        {
            highScore.EnterDraw(spriteBatch);
        }
        public static State HighScoreUpdate(GameWindow window)
        {
            background.Update(window);
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.B))
                return State.Menu;
            return State.HighScore;
        }
        public static void HighScoreDraw(SpriteBatch spriteBatch)
        {

            highScore.PrintDraw(spriteBatch);
        }
        
        private static void Reset(GameWindow window, ContentManager content)
        {
            player.Reset(380, 420, 3f, 4.5f);

            enemies.Clear();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("carL");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carL temp = new carL(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carL");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carL2 temp = new carL2(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carR");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carR temp = new carR(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = content.Load<Texture2D>("carR");
            for (int i = 0; i < 1; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                carR2 temp = new carR2(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            fly.Clear();
            flySprite = content.Load<Texture2D>("ladybugg");
            printText = new PrintText(content.Load<SpriteFont>("myFont"));
            
        }
    }
}