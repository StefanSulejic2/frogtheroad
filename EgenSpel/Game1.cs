using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Enemy enemy;
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
            enemy = new Enemy(Content.Load<Texture2D>("frogg"), 0, 0);
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
            enemy.Update(Window);
            
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
            enemy.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
