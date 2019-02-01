using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EgenSpel
{
    // Game1 is the class that makes the game run!
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Game constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // When game starts, Initialise is called to make the game run.
        protected override void Initialize()
        {
            GameElements.currentState = GameElements.State.Menu;
            GameElements.Initialize();
            base.Initialize();
        }

        // When game starts, LoadContent is called to load all sprites, sound files etc.
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameElements.LoadContent(Content, Window);
        }
        
        // When the game is closed, UnloadContent unloads that aren't necessary anymore.
        protected override void UnloadContent()
        {
        }
        
        // Update loads in all necessary data to make the game function like movement controlls or gameTime
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            switch (GameElements.currentState)
            {
                // Run the game
                case GameElements.State.Run:
                    GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
                    break;
                case GameElements.State.HighScore:
                    GameElements.currentState = GameElements.HighScoreUpdate(Window);
                    break;
                // Quit the game
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                // Menu
                default:
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        // Draw works together with LoadContent and draws the loaded content (Like sprites, sound files, etc.)
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // Same Switch as in Game1.Update except we're drawing the elements.
            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.RunDraw(spriteBatch);
                    break;
                case GameElements.State.HighScore:
                    GameElements.HighScoreDraw(spriteBatch);
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                default:
                    GameElements.MenuDraw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
