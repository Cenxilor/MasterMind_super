using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MasterMind_super
{

    public static class Animation
    {
        // créer une nouvelle animation

        private class Sprite_Animation : Sprite
        {
            public int frame;
            public int frame_max;
            public int frame_min;
            public int chrono;
            public int speed_frame;
            public string file_origine;
            public SoundEffectInstance my_sound_effect;
            public int Timer_start;

            // initialisation des composant du sprite
            public void Init_sprite(string pFile_name, Vector2 pPos, int pFrame_max, int pSpeed_frame, int pTimer_start = 0, float pScale = 1.0f)
            {
                frame_max = pFrame_max;
                speed_frame = pSpeed_frame;

                IsVisible = false;
                Timer_start = pTimer_start;
                chrono = Timer_start;

                frame_min = Int32.Parse(pFile_name.Substring(pFile_name.Length - 1, 1)); // convertion du dernier caractère de la file, la frame de départ, en int32
                frame = frame_min;
                file_origine = pFile_name.Remove(pFile_name.Length - 1); // recupere le chemin sans la frame

                // centre la position du sprite sur la position
                position = new Vector2(pPos.X - (this.file.Width / 2 * MainGame.SIZE_mutliply), pPos.Y - (this.file.Height / 2 * MainGame.SIZE_mutliply)); ;

                scale = pScale;
            }

            // base
            public Sprite_Animation(string pFile_name, Vector2 pPos, int pFrame_max, int pChrono_init, int pTimer_start = 0 , float pScale = 1.0f) : base(maingame, pFile_name)
            {
                Init_sprite(pFile_name, pPos, pFrame_max, pChrono_init, pTimer_start, pScale);
            }

            // base + sound_effect
            public Sprite_Animation(string pFile_name, Vector2 pPos, int pFrame_max, int pChrono_init, SoundEffectInstance pSound_effect, int pTimer_start = 0, float pScale = 1.0f) : base(maingame, pFile_name)
            {
                Init_sprite(pFile_name, pPos, pFrame_max, pChrono_init, pTimer_start, pScale);

                my_sound_effect = pSound_effect;
                my_sound_effect.Play(); // joue le son d'explosion 
            }

            public override void Update(GameTime gameTime)
            {
                
                chrono--;
                if(IsVisible == true)
                {
                    if (chrono <= 0) // tick de temps
                    {
                        chrono = speed_frame; // reset le chrono

                        if (frame <= frame_max)
                        {
                            Change_file(maingame, file_origine + frame);
                        }
                        else
                        {
                            frame = frame_min;
                            chrono = Timer_start;
                            this.toRemove = true;
                        }

                        frame++;
                    }

                    base.Update(gameTime);
                }
                else // l'animation n'a pas commencé
                {
                    // debute l'animation si le chrono atteint pour la première fois la vitesse d'une frame (si timer_start = 1000 il faudra = 1000 -"speed_frame" ! boucle)
                    if (chrono <= speed_frame)
                    {
                        IsVisible = true;
                    }
                }
            }

        }

        // créer une liste de sprite animé
        private class new_Animation
        {
            public List<iActor> lst_iActor_anim = new List<iActor>();
            public Name_animation my_animation_name { get; private set; }
            public bool to_Remove_anim = false;

            // gestion des dimentions de l'annimation
            private int chaos_zone_X, chaos_zone_Y;
            private float Magnitude;
            private Vector2 Origine_pos_anim;

            public new_Animation(Name_animation pAnimation_name,Vector2 pPos_start, int pChaos_zone_X = 20, int pChaos_zone_Y = 20, float pMagnitude = 1.0f)
            {
                my_animation_name = pAnimation_name;
                to_Remove_anim = false;

                // determine l'elargissement de la zone
                Magnitude = pMagnitude;
                // centre de la zone
                Origine_pos_anim = pPos_start;

                // determine la distance en X et Y depuis le centre de la zone (Origine_pos_anim)
                chaos_zone_X = (int)((pChaos_zone_X * MainGame.SIZE_mutliply / 2) * Magnitude); // application du multiplicateur de scale et de la magnitude
                chaos_zone_Y = (int)((pChaos_zone_Y * MainGame.SIZE_mutliply / 2) * Magnitude);

                string file_tx;

                // création des différentes animations
                switch (pAnimation_name)
                {
                    case Name_animation.Artifice:

                        file_tx = "Annimation/Artifice/firework_red0";


                        int pas_inter_artifice = 0; // temps entre deux feux d'artifice
                        int pas_max = 100; // pas maximum de départ (réduit de 3 en 3 jusqu'a 40 a chaque création d'artifice)
                        int pas_min = 10;
                        int timer_start_artifice = 0; // temps de départ du prochain feu d'artifice

                        for (int i = 1; i < 200; i++)
                        {
                            if(pas_max >= pas_min + 3) // reduit le pas maximum (accelere l'apparition des artifices)
                            {
                                pas_max -= 3;
                            }
                            
                            pas_inter_artifice = Utilitaires.Rand(pas_min, pas_max);

                            timer_start_artifice += pas_inter_artifice;

                            // création de l'artifice
                            Sprite_Animation my_new_artifice = new Sprite_Animation(file_tx, Get_Random_pos(), 7, 8, AssetManager.sndEffect_instance_artifice, timer_start_artifice, 5);

                            my_new_artifice.my_color = Utilitaires.Random_Color();

                            lst_iActor_anim.Add(my_new_artifice);
                        }
                        
                        break;
                    case Name_animation.Explosion:

                        break;
                    case Name_animation.none:
                        break;
                    default:
                        break;
                }

                lst_of_lst_animation.Add(this);
            }

            private Vector2 Get_Random_pos()
            {
                int Xrand = Utilitaires.Rand((int)(Origine_pos_anim.X - chaos_zone_X), (int)(Origine_pos_anim.X + chaos_zone_X));
                int Yrand = Utilitaires.Rand((int)(Origine_pos_anim.Y - chaos_zone_Y), (int)(Origine_pos_anim.Y + chaos_zone_Y));

                // positionne le sprite de manière aléatoire a partir du centre de la zone
                return new Vector2(Xrand,Yrand);
            }
        }

        public enum Name_animation
        {
            Artifice,
            Explosion,
            none
        }

        private static MainGame maingame;
        private static List<new_Animation> lst_of_lst_animation = new List<new_Animation>();
        private static Rectangle Screen;
        private static Vector2 center_screen;

        public static void Load(MainGame pMaingame)
        {
            maingame = pMaingame;

            Screen = maingame.Window.ClientBounds;
            center_screen = new Vector2(Screen.Width / 2, Screen.Height / 2);

        }

        public static void UnLoad()
        {
            clean_liste();
        }

        public static void Update(GameTime gameTime)
        {
            clean_liste(); // MAJ des to_remove

            foreach (new_Animation Animation in lst_of_lst_animation)
            {
                foreach (Sprite my_sprite in Animation.lst_iActor_anim)
                {
                    my_sprite.Update(gameTime);
                }
            }
        }

        public static void Draw(GameTime gameTime)
        {
            foreach (new_Animation Animation in lst_of_lst_animation)
            {
                
                foreach (Sprite my_sprite in Animation.lst_iActor_anim)
                {
                    if(my_sprite.IsVisible == true)
                    {
                        my_sprite.Draw(maingame.spriteBatch);
                    }
                }
            }
        }

        private static void clean_liste()
        {
            lst_of_lst_animation.RemoveAll(item => item.to_Remove_anim == true);
            foreach (new_Animation Animation in lst_of_lst_animation)
            {
                Animation.lst_iActor_anim.RemoveAll(item => item.toRemove == true);
            }
        }

        // créer une animation sur tout l'ecran
        public static void New_Create_animation(Name_animation pName_animation) 
        {
            new new_Animation(pName_animation, center_screen, (int)(Screen.Width / MainGame.SIZE_mutliply), (int)(Screen.Height / 2 / MainGame.SIZE_mutliply), 1.0f);
        }

        // créer une animation avec le centre et la zone demandé
        public static void New_Create_animation(Name_animation pName_animation, Vector2 pPos_start, int pChaos_zone_X = 20, int pChaos_zone_Y = 20, float pMagnitude = 1.0f)
        {
            new new_Animation(pName_animation, pPos_start, pChaos_zone_X, pChaos_zone_Y, pMagnitude);
        }

        // créer une animation d'artifice de victoire
        public static void New_Artifice_victory()
        {
            new new_Animation(Name_animation.Artifice, new Vector2(center_screen.X, center_screen.Y / 2), (int)(Screen.Width / MainGame.SIZE_mutliply), (int)(Screen.Height / 2 / MainGame.SIZE_mutliply));
        }
    }
}
