using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MasterMind_super
{
    class SCENE_menu : SCENE
    {
        protected string this_scene_name;


        // ########### CONSTRUCTEUR ###########
        public SCENE_menu(MainGame pGame) : base(pGame)
        {
            this_scene_name = "";
            Debug.WriteLine("New --- SCENE : " + this_scene_name + "...");
        }


        // ########### LOAD ###########
        public override void Load()
        {
            //CREATION DES BUTTONS
            Button play_Button = new Button(mainGame, 
                                            "sprites/button",
                                            new Vector2(Screen.Width / 2, Screen.Height / 2),
                                            true);
            play_Button.onLeftClick_pressed = User_gestion.onClic_play;
            lst_Actors.Add(play_Button);


            Background = mainGame.Content.Load<Texture2D>("sprites/plateau");

            // LOAD : 

            base.Load();
            Debug.WriteLine(this_scene_name + "...");
        }

        // ########### UNLOAD ###########
        public override void UnLoad()
        {
            base.UnLoad();
            Debug.WriteLine(this_scene_name + "...");
        }

        // ########### UPDATE ###########
        public override void Update(GameTime gameTime)
        {
            // ---------------- AJOUTE LES ETATS DANS LES NEW STATES -- clavier + gamepad + souris
            User_gestion.set_NEW_Key_GP();

            // ---------------- TEST APPUIE SUR UNE TOUCHE ET EFFET

            if (User_gestion.Key_GP_IsDown(Keys.Enter,Buttons.A))
            {
                Debug.WriteLine("go to gameplay...");
                mainGame.gameSTATE.Change_scene(GameSTATE.SCENE_type.gameplay); // transfer sur la scene de gameplay
            }

            // ---------------- TRANSFERE LES NEW STATES DANS LES OLD STATES -- clavier + gamepad + souris
            User_gestion.set_OLD_Key_GP();

            base.Update(gameTime);
        }

        // ########### DRAW ###########
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            mainGame.spriteBatch.DrawString(AssetManager.main_police, "MENU...", trace_pos, Color.White);
        }
    }
}
