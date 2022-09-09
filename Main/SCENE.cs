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
    abstract public class SCENE
    {
        protected MainGame mainGame;
        protected Rectangle Screen;
        protected Song musicGameplay;
        protected Vector2 trace_pos = new Vector2(1, 1);
        protected Texture2D Background { get; set; }
        protected Vector2 Background_pos = new Vector2(0, 0);
        protected string music_ambiance_name = ""; // A instencier dans le constructeur de la scene - sert a choisir la musique

        public static int NB_button_hover = 0; // compte le nombre de boutton survolé sur la scene

        public List<iActor> lst_Actors { get; set; } // recupere une liste d'acteur de type iActors (interface)

        public SCENE(MainGame pGame)
        {
            mainGame = pGame;
            Screen = mainGame.Window.ClientBounds;

            lst_Actors = new List<iActor>();
            Background = null;
            NB_button_hover = 0;
        }

        public void clean_liste()
        {
            lst_Actors.RemoveAll(item => item.toRemove == true);
            Grid_background.essai.lst_current_essai.RemoveAll(item => item.toRemove == true); //  maj liste des essais
            Grid_background.essai.lst_button.RemoveAll(item => item.toRemove == true); //  maj liste des essais
        }

        protected void Musique_PLAY()
        {
            if (music_ambiance_name != "")
            {
                MediaPlayer.Play(musicGameplay);
                MediaPlayer.IsRepeating = true;
            }
        }

        protected void Musique_STOP()
        {
            if (music_ambiance_name != "")
            {
                MediaPlayer.Stop();
            }
        }

        public virtual void Load()
        {
            // ---------------- GESTION DES OLD STATE -- clavier + gamepad + souris
            User_gestion.Load(mainGame);

            if(music_ambiance_name != "")
            {
                musicGameplay = mainGame.Content.Load<Song>(music_ambiance_name);
            }
            
            Musique_PLAY();
            Animation.Load(mainGame);

            Debug.Write("LOAD SCENE : ");
        }


        public virtual void UnLoad()
        {
            Musique_STOP();
            Animation.UnLoad();
            Debug.Write("UNLOAD SCENE : ");
        }

        public virtual void Update(GameTime gameTime)
        {

            foreach (iActor actor in lst_Actors)
            {
                actor.Update(gameTime);
            }

            // ---------------- APPUIE SUR UNE TOUCHE ET EFFET

            if (User_gestion.Key_GP_IsDown(Keys.Add))
            {
                Debug.WriteLine("agrandi ecran");
                MainGame.Change_screen_size(MainGame.SIZE_mutliply + 0.5f);
            }
            if (User_gestion.Key_GP_IsDown(Keys.Subtract))
            {
                Debug.WriteLine("reduit ecran");
                MainGame.Change_screen_size(MainGame.SIZE_mutliply - 0.5f);
            }

            Animation.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            // affichage du fond en premier
            if (Background != null)
            {
                mainGame.spriteBatch.Draw(Background, Background_pos, null, Color.White, 0f, Vector2.Zero, MainGame.SIZE_mutliply, SpriteEffects.None, 1.0f);
            }

            foreach (iActor actor in lst_Actors)
            {
                if(actor.IsVisible == true)
                    actor.Draw(mainGame.spriteBatch);
            }

            Animation.Draw(gameTime);
        }
    }
}