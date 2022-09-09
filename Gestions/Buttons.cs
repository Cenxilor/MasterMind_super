using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MasterMind_super
{
    public delegate void OnClick(Button pSender, MainGame pMainGame);
    public delegate void OnLeave(Button pSender, MainGame pMainGame);

    public class Button : Sprite
    {
        public bool isHover { get; private set; }
        private MouseState MS_OLD_state;
        private MouseState MS_NEW_state;
        private MainGame mainGame;
        public bool pause = false;

        public OnClick onLeftClick_pressed { get; set; }
        public OnClick onRightClick_pressed { get; set; }
        public OnClick onLeftClick_released { get; set; }
        public OnClick onRightClick_released { get; set; }

        public OnLeave onLeave { get; set; }

        public Button(MainGame pMaingame, string pFile_name) : base(pMaingame, pFile_name)
        {
            MS_OLD_state = Mouse.GetState();
            mainGame = pMaingame;
            pause = false;
        }

        public Button(MainGame pMaingame, string pFile_name, Vector2 pPos, bool pOrigine_center = true, bool pIsVisible = true) : base(pMaingame, pFile_name)
        {
            if (pOrigine_center)
            {
                this.position = new Vector2(pPos.X - ((this.file.Width*MainGame.SIZE_mutliply) / 2), pPos.Y - ((this.file.Height*MainGame.SIZE_mutliply) / 2));
            }
            else this.position = pPos;

            MS_OLD_state = Mouse.GetState();
            mainGame = pMaingame;

            IsVisible = pIsVisible;
            pause = false;
        }

        public override void Update(GameTime pGameTime)
        {
            MS_NEW_state = Mouse.GetState();

            Point MS_pos = MS_NEW_state.Position;

            if(this.BoundingBox.Contains(MS_pos))
            {
                if(isHover == false) // boutton survolé !
                {
                    isHover = true;

                    SCENE.NB_button_hover++; // ajoute 1 au nombre de boutton survolé sur la scene 

                    if(!User_gestion.Test_MS_state(User_gestion.MS_state.pion_follower) && this.IsVisible == true && this.pause == false)
                    {
                        User_gestion.Set_MS(User_gestion.MS_state.clicable, true); // ma souris devient clicable (hand)
                    }

                    //Debug.WriteLine("boutton survolé");
                }
            }
            else
            {
                if (isHover == true) // boutton n est plus survolé
                {
                    SCENE.NB_button_hover--;

                    if(SCENE.NB_button_hover == 0 && !User_gestion.Test_MS_state(User_gestion.MS_state.pion_follower)) // si il n'y a plus aucun boutton de survolé dans la scene en cours
                    {
                        User_gestion.Set_MS(User_gestion.MS_state.normal, true); // remetre la souris en mode normal
                    }


                    if (onLeave != null && MS_NEW_state.LeftButton == ButtonState.Pressed)
                    {
                        onLeave(this, mainGame);
                    }
                    //Debug.WriteLine("boutton n est plus survolé");
                }
                isHover = false;
            }

            if(isHover && this.pause == false)
            {
                // ******* CLIC SUR LE BOUTTON

                if (MS_NEW_state.LeftButton == ButtonState.Pressed && MS_OLD_state.LeftButton == ButtonState.Released)
                {
                    //Debug.WriteLine("Boutton Gauche CLIC !");
                    if(onLeftClick_pressed != null)
                    {
                        onLeftClick_pressed(this, mainGame);
                    }
                }

                if (MS_NEW_state.RightButton == ButtonState.Pressed && MS_OLD_state.RightButton == ButtonState.Released)
                {
                    //Debug.WriteLine("Boutton Droit CLIC !");
                    if (onRightClick_pressed != null)
                    {
                        onRightClick_pressed(this, mainGame);
                    }
                }

                // ******* RELACHE LE BOUTTON
                if (MS_NEW_state.LeftButton == ButtonState.Released && MS_OLD_state.LeftButton == ButtonState.Pressed)
                {
                    //Debug.WriteLine("Boutton Gauche RELACHE !");
                    if (onLeftClick_released != null)  // execute la fonction liée au boutton lors d'un RELACHEMENT de celui ci
                    {
                        onLeftClick_released(this, mainGame);
                    }
                }

                if (MS_NEW_state.RightButton == ButtonState.Released && MS_OLD_state.RightButton == ButtonState.Pressed)
                {
                    //Debug.WriteLine("Boutton Droit RELACHE !");

                    if (onRightClick_released != null)  // execute la fonction liée au boutton lors d'un RELACHEMENT de celui ci
                    {
                        onRightClick_released(this, mainGame);
                    }
                }
            }

            MS_OLD_state = MS_NEW_state;
            base.Update(pGameTime);
        }

        // gestion de button de type pion

        public void Add_Pion_to_essai(Sprite pSender, string new_color = "")
        {
            if (new_color == "")
            {
                new_color = pSender.color_pion;
            }

            if (Grid_background.essai.essai_is_toMax == false && this.my_type == Sprite.Type.pion_cache)
            {
                Grid_background.essai.nb_pion_on_current_essai++; // ajoute un nouveau pion a l'essai
                //Debug.WriteLine("Add un essai : " + Grid_background.essai.nb_pion_on_current_essai);
            }

            Debug.WriteLine("add pion essai N° " + Grid_background.essai.nb_pion_on_current_essai + ": " + new_color);

            this.Change_file(pSender.file);
            this.pause = false;
            this.my_type = Sprite.Type.pion;
            this.IsVisible = true;
            this.Set_color_pion(new_color);
            this.my_color = Color.White;

            if (Grid_background.essai.nb_pion_on_current_essai >= Grid_background.essai.nbMAX_pion_by_essai)
            {
                // essai au maximum
                Grid_background.essai.Set_Essai_is_max(true);
            }
        }

        public void Set_invisible(MainGame pMaingame, bool suppr_pion_essai = false)
        {
            IsVisible = false;
            pause = true;

            if(suppr_pion_essai == true)
            {
                Grid_background.essai.nb_pion_on_current_essai--; // supprime un nouveau pion a l'essai
                Debug.WriteLine("suppr un essai : " + Grid_background.essai.nb_pion_on_current_essai);
                Grid_background.essai.Set_Essai_is_max(false);
            }
        }
    }
}
