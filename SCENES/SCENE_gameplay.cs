using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MasterMind_super;
using Microsoft.Xna.Framework.Media;

namespace MasterMind_super
{
    class SCENE_gameplay : SCENE
    {
        public string this_scene_name;
        public static List<iActor> lst_tempon_iActor = new List<iActor>();
        public const int chronoMAX = 30;
        public int chrono = 0;
        public static IA my_IA;

        public static bool Player_win = false;
        private bool suppr_follower_next_step = false;

        // ##################  declaration des onClic #################

        // boutton pioche
        public static void onLeftClic_pion_pressed(Button pSender, MainGame pMainGame)
        {
            if (!User_gestion.Test_MS_state(User_gestion.MS_state.pion_follower)) // si la souris n'est pas en mode suivi
            {
                if(pSender.pause == false)
                {
                    pSender.onLeave = onLeave_pion; // ajoute la nouvelle fonction onLeave pour le boutton survolé et cliqué;
                }
            }
            else // le pion est deja créer et suis la souris alors changer son sprite
            {
                //Debug.WriteLine("change couleur");
                Sprite follower = User_gestion.Get_MS_follower();
                follower.Change_file(pMainGame, "sprites/couleurs/" + pSender.color_pion);
            }
        }

        public static void onLeftClic_released_pioche(Button pSender, MainGame pMainGame) 
        {
            if(!User_gestion.Test_MS_state(User_gestion.MS_state.pion_follower)) // si la souris n'est pas en mode suivi
            {
                // retrouve le boutton vierge et change son sprite par celui de la pioche cliqué

                foreach (Button my_pion_essai in Grid_background.essai.lst_current_essai)
                {
                    if(my_pion_essai.my_type == Sprite.Type.pion_cache)
                    {
                        my_pion_essai.Add_Pion_to_essai(pSender);
                        break;
                    }
                }
            }
        }

        public static void onLeave_pion(Button pSender, MainGame pMainGame) // activé si le boutton est quitté 
        {
            if (!User_gestion.Test_MS_state(User_gestion.MS_state.pion_follower)) // vérifie si la souris n'est pas en mode suivi
            {
                User_gestion.Create_follower_MS(pMainGame, pSender, lst_tempon_iActor);
            }

            pSender.onLeave = null; // supprime le lien a la fonction car boutton quitté
        }

        //boutton essai
        public static void onRightClic_Pressed_pion_essai(Button pSender, MainGame pMainGame) // pour un clic droit sur un button de la essai
        {
            //Debug.WriteLine("clic pion essaie");
            if(pSender.my_type == Sprite.Type.pion) // si on clic bien sur un pion actif alors le supprimer
            {
                pSender.my_type = Sprite.Type.pion_cache;
                pSender.Change_file(pMainGame, "sprites/couleurs/" + Zone.tbl_couleur[0]);
                pSender.Set_color_pion(Zone.tbl_couleur[0]);
                pSender.my_color = Color.Transparent;

                Grid_background.essai.nb_pion_on_current_essai--; // ajoute un nouveau pion a l'essai
                Grid_background.essai.Set_Essai_is_max(false);
            }
        }

        public static void onLeftClick_released_Pose_pion_follower(Button pSender, MainGame pMainGame)
        {
            //Debug.WriteLine("released on essai");
            
            if(User_gestion.Test_MS_state(User_gestion.MS_state.pion_follower)) // si on a un pion a poser (souris en mode suivi)
            {
                Debug.WriteLine("pose pion essai");
                Sprite follower = User_gestion.Get_MS_follower();

                if (follower.zone_attachement == Zone.zType.essai) // si le pion follower vient de la zone d'essai alors remmetre l'ancien
                {
                    Button last_pion = Grid_background.essai.lst_current_essai[follower.ID_on_zone_list];
                    last_pion.Add_Pion_to_essai(pSender);
                }

                pSender.Add_Pion_to_essai(follower);

                User_gestion.Suppr_follower();
            }
        }

        // ########### CONSTRUCTEUR ###########
        public SCENE_gameplay(MainGame pGame) : base(pGame)
        {
            this_scene_name = "GAMEPLAY";

            music_ambiance_name = "";

            Debug.WriteLine("New --- SCENE : " + this_scene_name + "...");
        }

        // ###########  LOAD ###########
        public override void Load()
        {
            User_gestion.Load(mainGame);

            Player_win = false;

            Background = mainGame.Content.Load<Texture2D>("sprites/plateau");

            Grid_background.load(lst_Actors, mainGame);
            chrono = 0;

            my_IA = new IA(mainGame, lst_Actors);

            base.Load();
            Debug.WriteLine(this_scene_name + "...");
        }

        // ###########  UNLOAD ###########
        public override void UnLoad()
        {
            base.UnLoad();
            Debug.WriteLine(this_scene_name + "...");
        }

        // ########### UPDATE ###########
        public override void Update(GameTime gameTime)
        {
            // ---------------- AJOUTE LES ETATS DANS LES NEW STATES -- clavier + gamepad
            User_gestion.set_NEW_Key_GP();

            // permet de faire briller les pion à poser
            Grid_background.essai.Update_current_essai_color();


            // Timer à 1seconde
            chrono++;
            if(chrono >= chronoMAX)
            {
                chrono = 0;

                // code a executé toutes les secondes.
                /* ******************* */
                
                //Debug.WriteLine("state souris : " + User_gestion.Get_MS_state());

                /* ******************* */
            }

            // ---------------- TEST APPUIE SUR UNE TOUCHE ET EFFET

            if (User_gestion.Key_GP_IsDown(Keys.Up, Buttons.DPadUp,false))
            {
                //Appuie avec Update
            }

            if (User_gestion.Key_GP_IsDown(Keys.Space))
            {
                // Appuie une fois
            }

            if(suppr_follower_next_step == true)
            {
                User_gestion.Suppr_follower();
                suppr_follower_next_step = false;
            }
            if (User_gestion.Get_MS_Button_down(User_gestion.MS_button_down.RightButton_pressed) || User_gestion.Get_MS_Button_down(User_gestion.MS_button_down.LeftButton_Released))
            {
                suppr_follower_next_step = true;
            }

            if (User_gestion.Key_GP_IsDown(Keys.Back, Buttons.A)) // supprime le dernier pion
            {
                Button Last_button = Grid_background.essai.lst_current_essai[0];
                foreach (Button item in Grid_background.essai.lst_current_essai)
                {
                    if(item.IsVisible == true)
                    {
                        Last_button = item;
                    }
                }
                Last_button.Set_invisible(mainGame, true);
            }

            // quitte la partie - retour au menu
            if (User_gestion.Key_GP_IsDown(Keys.Escape, Buttons.A))
            {
                Debug.WriteLine("go to Menu...");

                AssetManager.sndEffect_instance_artifice.Stop();
                mainGame.gameSTATE.Change_scene(GameSTATE.SCENE_type.menu); // transfer sur la scene de gameplay
            }

            if (User_gestion.Key_GP_IsDown(Keys.Up, Buttons.A))
            {
                my_IA.supprimer_cache_resultat();
            }

            if (User_gestion.Key_GP_IsDown(Keys.Enter, Buttons.A))
            {
                Grid_background.OnLeftClick_released_Valide_essai(Grid_background.Button_Valide_essai, mainGame);
            }

            // ---------------- TRANSFERE LES NEW STATES DANS LES OLD STATES -- clavier + gamepad
            User_gestion.set_OLD_Key_GP();

            //Maj liste iactor
            clean_liste();
            foreach (iActor item in lst_tempon_iActor)
            {
                lst_Actors.Add(item);
            }
            lst_tempon_iActor.Clear();

            base.Update(gameTime);
        }

        // ########### DRAW ###########
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            mainGame.spriteBatch.DrawString(AssetManager.main_police, this_scene_name + "...", trace_pos, Color.White);

            if(Player_win == true)
            {
                mainGame.spriteBatch.DrawString(AssetManager.main_police, "VICTORY", new Vector2(Screen.Width/4, Screen.Height/2 - 100*MainGame.SIZE_mutliply), Color.Green, 0, Vector2.Zero, 6.66667f * MainGame.SIZE_mutliply, SpriteEffects.None, 1.0f);
            }
        }
    }
}
