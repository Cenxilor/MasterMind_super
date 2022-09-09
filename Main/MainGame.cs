using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MasterMind_super
{
    public class MainGame : Game
    {
        static GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GameSTATE gameSTATE;
        public static float SIZE_mutliply = 2.0f;
        private static int Screen_Width_origine, Screen_Heigth_origine;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameSTATE = new GameSTATE(this);
        }

        protected override void Initialize()
        {
            AssetManager.Init(Content);

            // Init ...
            Screen_Width_origine = 960;
            Screen_Heigth_origine = 880;

            graphics.PreferredBackBufferWidth = (int)(Screen_Width_origine * SIZE_mutliply);
            graphics.PreferredBackBufferHeight = (int)(Screen_Heigth_origine * SIZE_mutliply);
            graphics.ApplyChanges();

            Utilitaires.SetRandomSeed((int)DateTime.Now.Ticks);

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Crée un nouveau SpriteBatch, qui sera utilisé pour afficher des images (textures)
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // LOAD...
            gameSTATE.Change_scene(GameSTATE.SCENE_type.gameplay);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            /*
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Delete))
                Exit();*/

            // UPDATE ...

            if(gameSTATE.scene_current != null) // verification que la scene actuel n est pas null
            {
                gameSTATE.scene_current.Update(gameTime); // UPDATE de ma scene en cours
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black) ;

            // AFFICHAGE ...
            spriteBatch.Begin();

            if (gameSTATE.scene_current != null)
            {
                gameSTATE.scene_current.Draw(gameTime); // DRAW de ma scene en cours
            }

            spriteBatch.End();
            // FIN AFFICHAGE ...

            base.Draw(gameTime);
        }

        public static void Change_screen_size(float Multiply_screen)
        {
            SIZE_mutliply = Multiply_screen;

            if (SIZE_mutliply != 1.0f)
            {
                graphics.PreferredBackBufferWidth = (int)(Screen_Width_origine * SIZE_mutliply);
                graphics.PreferredBackBufferHeight = (int)(Screen_Heigth_origine * SIZE_mutliply);
            }
            else
            {
                graphics.PreferredBackBufferWidth = Screen_Width_origine;
                graphics.PreferredBackBufferHeight = Screen_Heigth_origine;
            }

            graphics.ApplyChanges();
        }
    }
}
