using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        List<Enemy> enemies;
        Texture2D frog_texture;

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
        }

        /// <summary>
        /// LoadContent kommer att kallas en gång per spel och här laddas
        /// all innehåll.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player (Content.Load<Texture2D>("frogg"), 380, 400, 2.5f, 4.5f);
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = Content.Load<Texture2D>("frogg");
            for (int i = 0; i < 10; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);
                Enemy temp = new Enemy(tmpSprite, rndX, rndY);
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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
