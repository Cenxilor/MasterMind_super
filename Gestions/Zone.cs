using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MasterMind_super
{
    public class Zone
    {
        public enum zType
        {
            resultat,
            essai,
            reponse,
            pioche,
            none
        }

        public zType type_zone;
        public int Width, Height;
        public Vector2 pos_zone;
        public Sprite background_zone;
        public List<Button> lst_button = new List<Button>();

        // pioche
        public static string[] tbl_couleur = new string[8] { "blanc", "bleu", "jaune", "noir", "orange", "rouge", "vert", "violet" };

        // essai
        public int nbMAX_pion_by_essai = 5;
        public int nb_pion_on_current_essai = 0;
        public int nbMax_essai_tot = 12;
        public bool essai_is_toMax { get; private set;}
        public int nb_essai_valide = 0;
        public List<Button> lst_current_essai = new List<Button>();

        // reponse
        public static string[] tbl_couleur_resultat = new string[2] {"blanc", "noir"};

        public Zone(List<iActor> pLst_actor_scene, MainGame pMaingame,  zType pType, float pos_X, float pos_Y)
        {
            type_zone = pType;

            pos_zone = new Vector2(pos_X, pos_Y); // position reccuillit en argument

            // selection du multiplicateur de taille de la zone
            switch (pType)
            {
                // la taille ici (width et height) correspond au nombre de case - taille en px maj en fin de switch
                case zType.resultat:
                    Width = 5;
                    Height = 1;

                    nbMAX_pion_by_essai = Width;
                    break;
                case zType.essai:
                    Width = 5;
                    Height = 12;

                    nbMAX_pion_by_essai = Width;
                    nbMax_essai_tot = Height;
                    nb_essai_valide = 0;
                    Create_zone_essai(pMaingame, pLst_actor_scene);

                    break;

                case zType.reponse:
                    Width = 5;
                    Height = 12;
                    break;
                case zType.pioche:
                    Width = 4;
                    Height = 2;

                    // ajout de la boite a la liste des acteurs
                    background_zone = new Sprite(pMaingame, "sprites/boite", Sprite.Type.boite);
                    background_zone.Moove(pos_zone.X, pos_zone.Y);
                    pLst_actor_scene.Add(background_zone);

                    // création des pions (button) de pioche
                    for (int i = 0; i < tbl_couleur.Length; i++)
                    {
                        Button my_pioche;

                        my_pioche = new Button(pMaingame, "sprites/couleurs/" + tbl_couleur[i], Vector2.Zero, false);
                        my_pioche.zone_attachement = Zone.zType.pioche;
                        my_pioche.Set_color_pion(tbl_couleur[i]); // donne la couleur du pion au button
                        my_pioche.onLeftClick_pressed = SCENE_gameplay.onLeftClic_pion_pressed; // dans la scene gameplay
                        my_pioche.onLeftClick_released = SCENE_gameplay.onLeftClic_released_pioche; // dans la scene gameplay

                        this.lst_button.Add(my_pioche);
                    }

                    Set_position_button_onZone(lst_button, 2);

                    // maj lst actor
                    foreach (Button item in lst_button)
                    {
                        pLst_actor_scene.Add(item);
                    }

                    break;
                case zType.none:
                    Width = -1;
                    Height = -1;
                    break;
                default:
                    Width = -1;
                    Height = -1;
                    break;
            }

            // application des multiplicateur
            Width = Width * Grid_background.ref_Width;
            Height = Height * Grid_background.ref_Height;
        }


        public void Moove_pioche(float pos_Y) // surcharge
        {
            this.pos_zone = new Vector2(this.pos_zone.X, this.pos_zone.Y + pos_Y);
            background_zone.Moove(0, pos_Y);
            foreach (Button item in this.lst_button)
            {
                item.Moove(0, pos_Y);
            }
        }

        public void Create_zone_essai(MainGame pMaingame, List<iActor> pLst_actor_scene)
        {
            // set/reset des paramettres de base
            nb_pion_on_current_essai = 0;
            essai_is_toMax = false;
            nb_essai_valide++;
            Set_Essai_is_max(false);

            if (SCENE_gameplay.Player_win == false && this.nb_essai_valide <= this.nbMax_essai_tot) // ne créé une nouvelle zone que si le joueur n'a pas encore gagné
            {
                // transfert de la liste des pions courant dans la liste des boutton globals
                foreach (Button item in lst_current_essai)
                {
                    lst_button.Add(item);
                }
                lst_current_essai.Clear();

                // création des buttons vide de la zone d'essai de départ
                for (int i = 0; i < nbMAX_pion_by_essai; i++)
                {
                    Button my_essai;

                    my_essai = new Button(pMaingame, "sprites/couleurs/" + tbl_couleur[0]);
                    my_essai.ID_on_zone_list = i;
                    my_essai.my_type = Sprite.Type.pion_cache;
                    my_essai.zone_attachement = Zone.zType.essai;
                    my_essai.my_color = Color.Transparent;

                    my_essai.Set_color_pion(tbl_couleur[0]);
                    my_essai.onLeftClick_released = SCENE_gameplay.onLeftClick_released_Pose_pion_follower; // dans la scene gameplay
                    my_essai.onRightClick_pressed = SCENE_gameplay.onRightClic_Pressed_pion_essai;
                    my_essai.onLeftClick_pressed = SCENE_gameplay.onLeftClic_pion_pressed;

                    this.lst_current_essai.Add(my_essai);
                }

                Set_position_button_onZone(lst_current_essai, 1, nbMax_essai_tot - (nb_essai_valide));

                // maj lst actor
                foreach (Button item in lst_current_essai)
                {
                    pLst_actor_scene.Add(item);
                }
            }
        }

        private float facteur_multpiply_transparence = -0.001f;
        public const float transparence_MAX = 0.3f; // transparence entre 0f et 1f
        public float transparence = transparence_MAX; // transparence entre 0f et 1f
        
        public void Update_current_essai_color() // permet le bounding transparent de la zone d'essai active
        {
            foreach (Button item in lst_current_essai)
            {
                if(item.my_type == Sprite.Type.pion_cache) // si le pion est cache
                {
                    if (transparence < 0.0f || transparence >= transparence_MAX)
                    {
                        //Debug.WriteLine("swap");
                        facteur_multpiply_transparence = -facteur_multpiply_transparence;
                    }

                    transparence = transparence - facteur_multpiply_transparence;
                    //Debug.WriteLine("transparence : " + item.transparence);

                    item.my_color = Color.Multiply(Color.White, transparence);
                }
            }
        }

        public void Set_position_button_onZone(List<Button> pLst_button, int NB_line, int STARTER_LINE = -100)
        {
            if (STARTER_LINE == -100) // si la ligne de départ n'est pas renseigné alors on la remplace par le nb de ligne - 1
            {
                STARTER_LINE = NB_line - 1;
            }

            int col = 0, lin = STARTER_LINE;
            for (int i = 0; i < pLst_button.Count; i++)
            {
                Vector2 pos_pioche;

                pos_pioche.X = pos_zone.X + (col * Grid_background.ref_Width); // position de la pioche + n * taille case standard donc décalage
                pos_pioche.Y = pos_zone.Y + (lin * Grid_background.ref_Height);

                if (col < (tbl_couleur.Length / NB_line) - 1)
                    col++;
                else
                {
                    col = 0; // reset la colonne a la motier du nombre de couleur
                    lin--; // passage a la seconde ligne
                }

                pLst_button[i].Moove(pos_pioche.X, pos_pioche.Y);
            }
        }

        public void Set_Essai_is_max(bool new_state)
        {
            //Debug.WriteLine("essai au maximum");
            essai_is_toMax = new_state;

            if(Grid_background.Button_Valide_essai != null) // vérification que l'objet existe
            {
                if (new_state == true) // si le nombre de pion essai est au max
                {
                    Grid_background.Button_Valide_essai.IsVisible = true;
                }
                else
                {
                    Grid_background.Button_Valide_essai.IsVisible = false;
                }
            }
        }
    }
}
