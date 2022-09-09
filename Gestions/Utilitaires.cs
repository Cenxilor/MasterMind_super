using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind_super
{
    static class Utilitaires
    {
        static Random Random_GEN = new Random();

        public static void SetRandomSeed(int pSeed)
        {
            Random_GEN = new Random(pSeed);
        }

        public static int Rand(int pMin, int pMax)
        {
            return Random_GEN.Next(pMin, pMax + 1);
        }

        private static List<Color> lst_color = new List<Color>() {Color.AliceBlue, Color.Blue, Color.Green, Color.Yellow, Color.Gray,
                                                                  Color.LightBlue,Color.LightPink, Color.LightYellow, Color.LightGreen,
                                                                  Color.DarkGray, Color.White};
        public static Color Random_Color()
        {
            int randum_number = Rand(0, lst_color.Count - 1);

            return lst_color[randum_number];
        }

        public static bool Collision_boundingBox(iActor p1, iActor p2)
        {
            return p1.BoundingBox.Intersects(p2.BoundingBox);
        }

        public static void Mix_list(List<int> lst_to_mix)
        {
            int nb_element = lst_to_mix.Count-1;

            for (int i = 0; i < 20; i++) // melange 20 fois
            {
                int Rand_index = Rand(0, nb_element);
                int Rand_new_id = Rand(0, nb_element);

                int element = lst_to_mix[Rand_index];
                lst_to_mix.RemoveAt(Rand_index);
                lst_to_mix.Insert(Rand_new_id, element);
            }
        }

        public static void Mix_list(List<IA.Resultat_comparaison> lst_to_mix)
        {
            int nb_element = lst_to_mix.Count - 1;

            for (int i = 0; i < 20; i++) // melange 20 fois
            {
                int Rand_index = Rand(0, nb_element);
                int Rand_new_id = Rand(0, nb_element);

                IA.Resultat_comparaison element = lst_to_mix[Rand_index];
                lst_to_mix.RemoveAt(Rand_index);
                lst_to_mix.Insert(Rand_new_id, element);
            }
        }
    }
    
}
