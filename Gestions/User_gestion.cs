using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MasterMind_super
{
    static class User_gestion
    {
        public enum MS_state
        {
            normal,
            clicable,
            pion_follower
        }
        public enum MS_button_down
        {
            LeftButton_pressed,
            RightButton_pressed,
            LeftButton_Released,
            RightButton_Released
        }

        // clavier
        static KeyboardState KB_OLD_state;
        static KeyboardState KB_NEW_state;

        // game pad
        static GamePadCapabilities GP_capabilities;
        static GamePadState GP_OLD_state;
        static GamePadState GP_NEW_state;

        // souris
        private static MouseState MS_OLD_state;
        private static MouseState MS_NEW_state;
        private static MS_state MS_current_state;
        private static Texture2D MS_file;
        private static Sprite MS_follower;
        private static bool MS_isVisible = true;

        //other
        private static MainGame mainGame;

        public static void Load(MainGame pMaingame)
        {
            mainGame = pMaingame;

            KB_OLD_state = Keyboard.GetState();

            GP_capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (GP_capabilities.IsConnected) // MANETTE --- verifie si le une manette est connecté
            {
                Debug.WriteLine("... GAMEPAD CONNECTED ...");
                GP_OLD_state = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.IndependentAxes); // MANETTE --- si oui old_state recois l'etat de la manette
            }

            MS_OLD_state = Mouse.GetState();
            MS_follower = null;
            Set_MS(MS_state.normal, true);
        }

        public static void set_NEW_Key_GP() // ce passe a chaque update
        {
            KB_NEW_state = Keyboard.GetState();

            if (GP_capabilities.IsConnected) // MANETTE --- verifie si le une manette est connecté
            {
                GP_NEW_state = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.IndependentAxes);
            }

            MS_NEW_state = Mouse.GetState();

            if(MS_follower != null) // si on a un follower de souris le met a jour à chaque début d'update
            {
                //Debug.WriteLine("ms follower moove");

                Vector2 new_pos_follower = MS_NEW_state.Position.ToVector2(); // recupère la position de la souris

                // centre l'objet sur la souris
                new_pos_follower.X -= (MS_follower.file.Width * MainGame.SIZE_mutliply) / 2;
                new_pos_follower.Y -= (MS_follower.file.Height * MainGame.SIZE_mutliply) / 2;

                MS_follower.position = new_pos_follower;
            }
        }

        public static void set_OLD_Key_GP()
        {
            KB_OLD_state = KB_NEW_state;

            if (GP_capabilities.IsConnected) // MANETTE --- verifie si le une manette est connecté
            {
                GP_OLD_state = GP_NEW_state;
            }

            MS_OLD_state = MS_NEW_state;
        }

        // GESTION  SOURIS !! --------------------------------------------------------------------------------------------

        public static void Set_MS(MS_state pMS_state, bool pMS_isVisible = true, int OrigineX = 0, int OrigineY = 0, Sprite pNew_MS_Follower = null, bool Maj_follower = false)
        {

            //Debug.WriteLine("set ms : " + MS_current_state.ToString());

            MS_current_state = pMS_state;
            MS_isVisible = pMS_isVisible;

            string files_pointeur = "sprites/pointeur_souri/";
            switch (pMS_state)
            {
                case MS_state.normal:
                    Mouse.SetCursor(MouseCursor.Arrow);
                    MS_isVisible = true;
                    break;

                case MS_state.clicable:
                    Mouse.SetCursor(MouseCursor.Hand);
                    break;

                case MS_state.pion_follower:
                    Maj_follower = true; // permet que quoi qu'il arrive si on demande une souris "pion" alors maj follower
                    Mouse.SetCursor(MouseCursor.Hand);
                    break;

                default:
                    // files_pointeur = files_pointeur + "";
                    // Mouse.SetCursor(MouseCursor.FromTexture2D(MS_file, OrigineX, OrigineY)); // remplace le pointeur par un sprite
                    break;
            }

            if(Maj_follower == true)
            {
                MS_follower = pNew_MS_Follower; // met a jour le follower de souris Si aucun alors = null donc aucun effet
                // ATTENTION NE PAS OUBLIER DE METTRE A JOUR LA LISTE DES ACTEUR AVEC LE SPRITE DE MS_FOLLOWER
            }

        }

        public static void Create_follower_MS(MainGame pMainGame, Button pSender, List<iActor> list_maj)
        {
            Sprite pion_follower = new Sprite(pMainGame, "sprites/couleurs/" + pSender.color_pion);
            pion_follower.Set_color_pion(pSender.color_pion);
            pion_follower.ID_on_zone_list = pSender.ID_on_zone_list; // copie l'ID
            pion_follower.zone_attachement = pSender.zone_attachement;

            list_maj.Add(pion_follower); // ajoute a la liste des acteurs pour suivre la souris
            Set_MS(User_gestion.MS_state.pion_follower, true, 0, 0, pion_follower);
        }

        public static void Suppr_follower()
        {
            if(MS_follower != null)
            {
                MS_follower.toRemove = true;
                Set_MS(MS_state.normal, true, 0, 0, null, true);
            }
        }

        public static bool Test_MS_state(MS_state pState_test) // retourne vrai si le type de la souris actuelle est bien celui demandé
        {
            if (MS_current_state == pState_test)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool Get_MS_Button_down(MS_button_down button_down)
        {

            switch (button_down)
            {
                case MS_button_down.LeftButton_pressed:
                    if (MS_NEW_state.LeftButton == ButtonState.Pressed && MS_OLD_state.LeftButton == ButtonState.Released)
                    {
                        //Debug.WriteLine("Boutton Gauche CLIC !");
                        return true;
                    }
                    break;
                case MS_button_down.RightButton_pressed:
                    if (MS_NEW_state.RightButton == ButtonState.Pressed && MS_OLD_state.RightButton == ButtonState.Released)
                    {
                        //Debug.WriteLine("Boutton Droit CLIC !");
                        return true;
                    }
                    break;
                case MS_button_down.LeftButton_Released:
                    if (MS_NEW_state.LeftButton == ButtonState.Released && MS_OLD_state.LeftButton == ButtonState.Pressed)
                    {
                        //Debug.WriteLine("Boutton Gauche Relache !");
                        return true;
                    }
                    break;
                case MS_button_down.RightButton_Released:
                    if (MS_NEW_state.RightButton == ButtonState.Released && MS_OLD_state.RightButton == ButtonState.Pressed)
                    {
                        //Debug.WriteLine("Boutton Droit Relache !");
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        public static MS_state Get_MS_state()
        {
            return MS_current_state;
        }

        public static bool Get_MS_isVisible()
        {
            return MS_isVisible;
        }

        public static Sprite Get_MS_follower()
        {
            return MS_follower;
        }

        // GESITON GLOBAL CLAVIER

        public static bool Key_GP_IsDown(Keys pKey, Buttons pButton_GP, bool test_unique = true) // pKey et pButton_GP = nom de la touche enfoncé, test_unique => true pour tester une fois (sans le OLD_state)
        {

            if (test_unique)
            {
                if (KB_NEW_state.IsKeyDown(pKey) && !KB_OLD_state.IsKeyDown(pKey)) // ############## TEST DE LA TOUCHE CLAVIER
                {
                    //Debug.WriteLine(pKey);
                    return true;
                }
                if (GP_capabilities.IsConnected) // test de connexion GamePad
                {
                    if (GP_NEW_state.IsButtonDown(pButton_GP) && GP_OLD_state.IsButtonDown(pButton_GP)) // ############## TEST DE LA TOUCHE DU GAMEPAD
                    {
                        //Debug.WriteLine(pKey);
                        return true;
                    }
                }
            }
            if(!test_unique)
            {
                if (KB_NEW_state.IsKeyDown(pKey)) // ############## TEST DE LA TOUCHE CLAVIER
                {
                    //Debug.WriteLine(pKey);
                    return true;
                }
                if (GP_capabilities.IsConnected) // test de connexion GamePad
                {
                    if (GP_NEW_state.IsButtonDown(pButton_GP)) // ############## TEST DE LA TOUCHE DU GAMEPAD
                    {
                        //Debug.WriteLine(pKey);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool Key_GP_IsDown(Keys pKey, bool test_unique = true) // pKey = nom de la touche enfoncé, test_unique => true pour tester une fois (sans le OLD_state)
        {

            if (test_unique)
            {
                if (KB_NEW_state.IsKeyDown(pKey) && !KB_OLD_state.IsKeyDown(pKey)) // ############## TEST DE LA TOUCHE CLAVIER
                {
                    //Debug.WriteLine(pKey);
                    return true;
                }
            }
            if (!test_unique)
            {
                if (KB_NEW_state.IsKeyDown(pKey)) // ############## TEST DE LA TOUCHE CLAVIER
                {
                    //Debug.WriteLine(pKey);
                    return true;
                }
            }
            return false;
        }

        public static void onClic_play(Button pSender, MainGame pMainGame)
        {
            pMainGame.gameSTATE.Change_scene(GameSTATE.SCENE_type.gameplay);
        }

    }
}
/*
KB_NEW_state.IsKeyDown(Keys.Enter) &&    // NEW = true
                   !KB_OLD_state.IsKeyDown(Keys.Enter) ) || //  OLD = false */