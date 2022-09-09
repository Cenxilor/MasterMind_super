using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MasterMind_super
{
    // Scene de fin de partie
    class SCENE_gameover : SCENE
    {
        protected string this_scene_name; //

        // ########### CONSTRUCTEUR ###########
        public SCENE_gameover(MainGame pGame) : base(pGame) 
        {
            this_scene_name = "GAMEOVER";
            Debug.WriteLine("New --- SCENE : " + this_scene_name + "...");
        }

        // ########### LOAD ###########
        public override void Load()
        {
            base.Load();
            Debug.WriteLine(this_scene_name + "...");
        }

        // ########### UNLOAD ###########
        public override void UnLoad()
        {
            // fin load
            base.UnLoad();
            Debug.WriteLine(this_scene_name + "...");
        }

        // ########### UPDATE ###########
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // ########### DRAW ###########
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            mainGame.spriteBatch.DrawString(AssetManager.main_police, this_scene_name + "...", trace_pos, Color.White);
        }
    }
}
