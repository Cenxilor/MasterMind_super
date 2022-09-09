using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MasterMind_super
{
    class IA
    {

        private int nb_pion_resultat_MAX = 0;
        private List<Button> lst_resultat;
        private MainGame maingame;
        private Sprite cache_resultat;

        public IA(MainGame pMaingame, List<iActor> lst_maj)
        {
            maingame = pMaingame;
            nb_pion_resultat_MAX = Grid_background.resultat.nbMAX_pion_by_essai;

            lst_resultat = new List<Button>();

            for (int i = 0; i < nb_pion_resultat_MAX; i++)
            {
                // chiffre aléatoire entre 0 et le nombre de couleur dans le tableau de zone
                int Rand = Utilitaires.Rand(0, Zone.tbl_couleur.Length - 1); // "blanc", "bleu", "jaune", "noir", "orange", "rouge", "vert", "violet" }
                string new_color = Zone.tbl_couleur[Rand];
                Button new_pion = new Button(pMaingame, "sprites/couleurs/" + new_color,
                                            new Vector2(Grid_background.resultat.pos_zone.X + Grid_background.ref_Width * i,
                                                        Grid_background.resultat.pos_zone.Y),
                                            false, true);
                new_pion.Set_color_pion(new_color);
                lst_resultat.Add(new_pion);
                lst_maj.Add(new_pion);
            }

            cache_resultat = new Sprite(pMaingame, "sprites/cache", Sprite.Type.cache);
            cache_resultat.Moove(Grid_background.resultat.pos_zone.X - 1, Grid_background.resultat.pos_zone.Y); // positionne sur le jeux de l'IA
            cache_resultat.IsVisible = true;
            lst_maj.Add(cache_resultat);
        }

        public enum Resultat_comparaison
        {
            juste,
            mauvaise_place,
            faux,
        }
        public bool Compare_resultat(List<Button> lst_to_compare, List<iActor> pLst_to_maj)
        {
            List<Resultat_comparaison> lst_final_comparaison = new List<Resultat_comparaison>(); // lst contenant les resultat des comparaisons 0 = vrai, 1 = mal placé, 2 = faux
            List<int> lst_ID_final_comparaison = new List<int>();
            List<Button> lst_current_reponse = new List<Button>();

            // créer une copie qui permetra de supprimé les couleurs traité au fur et a mesure des essaies (evité les doublons)
            List<Button> copie_lst_resultat = new List<Button>();
            foreach (var item in lst_resultat)
            {
                copie_lst_resultat.Add(item);
            }

            // comparaison entre listes
            // couleur juste 0 = Ok !
            for (int i = 0; i < lst_resultat.Count; i++)
            {
                string color_pion_resultat = lst_resultat[i].color_pion;
                string color_pion_compare = lst_to_compare[i].color_pion;

                copie_lst_resultat[i].ID_on_zone_list = i;

                if (color_pion_resultat == color_pion_compare)// si les deux pions sont egaux 0 = ok
                {
                    Debug.WriteLine("Add juste : " + color_pion_compare);
                    lst_ID_final_comparaison.Add(i);
                    lst_final_comparaison.Add(Resultat_comparaison.juste); // ajoute les id des resultats ok (sera mis a jour en dessou)
                }
            }

            // suppression et MAJ des pions qui sont juste de la liste copie 0 = ok
            for (int i = lst_final_comparaison.Count - 1 ; i >= 0; i--) // parcourt de la liste à revers (pour supprimé les objet de la copie)
            {
                int ID = lst_ID_final_comparaison[i];
                copie_lst_resultat.RemoveAt(ID); // enlève le pion juste (evité de le retrouvé dans els mal placé
            }

            Debug.WriteLine("### apres juste ####");
            foreach (Button item in copie_lst_resultat)
            {
                Debug.WriteLine("couleur restante resultat: " + item.color_pion);
            }

            // couleur mal placée 
            for (int i = 0; i < lst_to_compare.Count; i++)
            {
                Button pion_compare = lst_to_compare[i];

                bool color_present = false; // récupere la présence ou non de la couleur
                int id = -1;
                for (int n = 0; n < copie_lst_resultat.Count; n++)
                {
                    Button pion_resultat = copie_lst_resultat[n];

                    if(lst_ID_final_comparaison.Contains(pion_compare.ID_on_zone_list) == false) // si le pion testé n'est pas déjà "juste"
                    {
                        if(pion_resultat.color_pion == pion_compare.color_pion)
                        {
                            id = n; // récupère l'id de la couleur traité 
                            color_present = true; //
                            break;
                        }
                    }
                }

                if (color_present == true) // couleur présente 1 = mal placé
                {
                    lst_final_comparaison.Add(Resultat_comparaison.mauvaise_place);
                    copie_lst_resultat.RemoveAt(id);
                }
            }

            Debug.WriteLine("### apres mal placé ####");
            foreach (Button item in copie_lst_resultat)
            {
                Debug.WriteLine("couleur restante resultat : " + item.color_pion);
            }

            // couleur fausse (restante)
            foreach (Button item in copie_lst_resultat)
            {
                lst_final_comparaison.Add(Resultat_comparaison.faux);
            }

            // vérification du résultat (si tout à 0 == WIN)
            int count_conforme_resultat = 0;
            for (int i = 0; i < lst_final_comparaison.Count; i++)
            {
                if (lst_final_comparaison[i] == 0)
                {
                    count_conforme_resultat++;
                }
            }

            if(count_conforme_resultat == lst_final_comparaison.Count) // si tout les resultats son positif
            {
                SCENE_gameplay.Player_win = true;

                Animation.New_Artifice_victory();

                Debug.WriteLine("##########################");
                Debug.WriteLine("YOU WIN !!!!!!!!!!!");
                Debug.WriteLine("##########################");
                cache_resultat.IsVisible = false;
            }

            // MELANGE LA LISTE DES RESULTAT
            Utilitaires.Mix_list(lst_final_comparaison);
            lst_final_comparaison.RemoveAll(item => item == Resultat_comparaison.faux); // supprime les faux de la liste

            // création des pions reponses (les pions faux sont invisible)
            for (int i = 0; i < lst_to_compare.Count; i++)
            {
                Button pion_reponse;
                string couleurs = "";
                bool out_lst = false;
                if (i < lst_final_comparaison.Count) // si i ne dépasse pas le nombre d'élément (les 2 = faux ont été supprimé)
                {
                    // recupere la couleur en fonction du resultat 0 = ok = 'blanc' , 1 = mal placé = 'noir'
                    int id_color_tbl = -1;
                    if(lst_final_comparaison[i] == Resultat_comparaison.juste)
                    {
                        id_color_tbl = 0;
                    }
                    else
                    {
                        id_color_tbl = 1;
                    }
                    couleurs = Zone.tbl_couleur_resultat[id_color_tbl];
                    //Debug.WriteLine("place pions reponse " + couleurs);
                }
                else
                {
                    couleurs = Zone.tbl_couleur_resultat[0];
                    out_lst = true;
                }

                pion_reponse = new Button(maingame, "sprites/pions_reponses/pion_" + couleurs);
                if (out_lst == true)
                    pion_reponse.Set_invisible(maingame, false);
                pion_reponse.ID_on_zone_list = i;
                pion_reponse.zone_attachement = Zone.zType.reponse;

                pion_reponse.Set_color_pion(couleurs);

                lst_current_reponse.Add(pion_reponse);
            }

            Grid_background.reponse.Set_position_button_onZone(lst_current_reponse, 1 , Grid_background.essai.nbMax_essai_tot - (Grid_background.essai.nb_essai_valide));

            // maj lst actor
            foreach (Button item in lst_current_reponse)
            {
                pLst_to_maj.Add(item);
            }

            return false;
        }

        public void supprimer_cache_resultat()
        {
            if(cache_resultat.IsVisible == true)
            {
                cache_resultat.IsVisible = false;
            }
            else
            {
                cache_resultat.IsVisible = true;
            }
        }
    }
}
