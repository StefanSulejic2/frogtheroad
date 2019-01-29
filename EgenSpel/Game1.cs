using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EgenSpel
{
    /// <summary>
    /// Det här är huvudkoden
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        PrintText printText;
        List<Enemy> enemies;
        List<Fly> fly;
        Texture2D flySprite;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Låter spelet initialisera.
        /// Här kan du söka efter eventuella tjänster och ladda alla icke-grafiska
        /// relaterat innehåll. Initialisering kommer att summeras genom några komponenter
        /// och initiera dem också.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            fly = new List<Fly>();
        }

        /// <summary>
        /// LoadContent kommer att kallas en gång per spel och här laddas
        /// all innehåll.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player (Content.Load<Texture2D>("frogg"), 380, 400, 2.5f, 4.5f);
            printText = new PrintText(Content.Load<SpriteFont>("myFont"));
            flySprite = Content.Load<Texture2D>("ladybugg");
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = Content.Load<Texture2D>("carL");
            for (int i = 0; i < 2; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);
                carL temp = new carL(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
            tmpSprite = Content.Load<Texture2D>("carR");
            for (int i = 0; i < 2; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);
                carR temp = new carR(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }
        }

        /// <summary>
        /// UnloadContent kommer att kallas en gång per spel och är plats för avslutning av
        /// game-specific innehåll.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Låter spelet köra logik som att uppdatera världen,
        /// kontrollerar kollisioner, samlar in och spelar ljud.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.Update(Window);
            foreach (Enemy e in enemies)
                e.Update(Window);
            
            base.Update(gameTime);

            Random random = new Random();
            int newfly = random.Next(1, 200);
            if (newfly == 1)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - flySprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height - flySprite.Height);
                fly.Add(new Fly (flySprite, rndX, rndY, gameTime));
            }
            else
            {
            }

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
            foreach (Enemy e in enemies.ToList())
            {
                if (e.IsAlive)
                {
                    if (e.CheckCollision(player))
                        this.Exit();
                    e.Update(Window);
                }
                else
                    enemies.Remove(e);
            }
        }

        /// <summary>
        ///Detta kallas när spelet ska rita sig.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach (Fly f in fly)
                f.Draw(spriteBatch);
            printText.Print("Points:" + player.Points, spriteBatch, 0, 0);
            spriteBatch.End();
            base.Draw(gameTime);

            
        }
    }
}
