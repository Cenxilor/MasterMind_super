using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MasterMind_super
{
    public static class Grid_background
    {

        public static string file_ref_name = "sprites/case_ref";
        public static Texture2D file_ref;
        public static int ref_Width, ref_Height, ref_size;
        public static Zone resultat, essai, reponse, pioche;
        public static Button Button_Valide_essai;

        // declaration OnClic
        public static void OnLeftClick_released_Valide_essai(Button pSender, MainGame pMainGame)
        {
            if(pSender.IsVisible == true)
            {
                //Debug.WriteLine("click validé essai");

                SCENE_gameplay.my_IA.Compare_resultat(essai.lst_current_essai, SCENE_gameplay.lst_tempon_iActor); // compare les resultats

                essai.Create_zone_essai(pMainGame, SCENE_gameplay.lst_tempon_iActor); // créer une nouvelle zone d'essai au dessus de la précédente

                // bouge la zone de pioche
                if(essai.nb_essai_valide > 2 && essai.nb_essai_valide <= essai.nbMax_essai_tot)
                {
                    pioche.Moove_pioche(-ref_Height);
                }
            }
        }

        public static void load(List<iActor> pLst_actor_scene, MainGame pMaingame)
        {

            file_ref = pMaingame.Content.Load<Texture2D>(file_ref_name); ; // sprite de référence

            ref_Width = (int)(file_ref.Width * MainGame.SIZE_mutliply);
            ref_Height = (int)(file_ref.Height * MainGame.SIZE_mutliply);
            if(ref_Width == ref_Height)
            {
                ref_size = ref_Width;
            }
            else
            {
                ref_size = -1;// valeur impossible == null
            }

            Create_zone(pLst_actor_scene, pMaingame);

            Sprite pion_explications;


            //pLst_actor_scene.Add(new Sprite(pMaingame, "sprites/pions_reponses/pion_blanc"));
            //pLst_actor_scene.Add(new Sprite(pMaingame, "sprites/pions_reponses/pion_blanc"));

            Button_Valide_essai = new Button(pMaingame, "sprites/button", new Vector2(reponse.pos_zone.X + reponse.Width/2 , ref_size/2), true, false);
            Button_Valide_essai.Moove(0, Button_Valide_essai.file.Height / 2);
            Button_Valide_essai.onLeftClick_released = OnLeftClick_released_Valide_essai;
            pLst_actor_scene.Add(Button_Valide_essai);            
        }

        private static void Create_zone(List<iActor> pLst_actor_scene, MainGame pMaingame)
        {
            float bord, cases;
            bord = ref_size / 4;
            cases = ref_size;

            resultat = new Zone(pLst_actor_scene, pMaingame, Zone.zType.resultat,
                                bord + cases * 4 + bord,
                                bord);
            essai = new Zone(pLst_actor_scene, pMaingame, Zone.zType.essai,
                                bord + cases * 4 + bord,
                                bord + cases + bord);
            reponse = new Zone(pLst_actor_scene, pMaingame, Zone.zType.reponse,
                                bord + cases * 4 + bord + cases * 5 + bord,
                                bord + cases + bord);
            pioche = new Zone(pLst_actor_scene, pMaingame, Zone.zType.pioche,
                                bord,
                                bord + cases + bord + cases * 10);
        }

    }
}
