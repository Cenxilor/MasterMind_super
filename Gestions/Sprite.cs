using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind_super
{
    public class Sprite : iActor
    {
        //iterface iActor
        public Vector2 position { get; set; }
        public Rectangle BoundingBox { get; private set; }
        public bool toRemove { get; set; }

        //class Sprite
        public enum Type
        {
            boite,
            pion,
            pion_cache,
            cache,
            none
        }

        public Texture2D file { get; private set; }
        public Color my_color { get; set; }

        public Type my_type;
        public float vitesse_x;
        public float vitesse_y;
        protected Rectangle Screen;
        public bool IsMooving = false;
        public bool IsVisible { get; set; }
        public float scale { get; set; }

        // master mind variable pion
        public string color_pion { get; private set; }
        public int ID_on_zone_list = 0;
        public Zone.zType zone_attachement = Zone.zType.none;

        public Sprite(MainGame pMaingame, string pFile_name, Type pType = Type.none, float pScale = 1.0f)
        {
            IsVisible = true;
            toRemove = false;
            Screen = pMaingame.Window.ClientBounds;
            if(pFile_name != "")
            {
                file = pMaingame.Content.Load<Texture2D>(pFile_name);
            }
            else
            {
                Change_file(pMaingame, "sprites/case_ref");
                IsVisible = false;
            }
            
            my_color = Color.White;
            my_type = pType;

            scale = pScale;
        }

        public void Moove(float pvitesse_X, float pvitesse_Y = 0f)
        {
            position = new Vector2(position.X + pvitesse_X, position.Y + pvitesse_Y);
        }

        public void Change_file(Texture2D new_file)
        {
            file = new_file;
        }

        public void Change_file(MainGame pMaingame, string new_file)
        {
            file = pMaingame.Content.Load<Texture2D>(new_file);
        }

        public virtual void Touched_By(iActor pBy)
        { 
        }

        public virtual void Load()
        {
            BoundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(file.Width * MainGame.SIZE_mutliply), (int)(file.Height * MainGame.SIZE_mutliply));
        }

        public virtual void Update(GameTime pGameTime)
        {
            if(IsMooving)
            {
                Moove(vitesse_x, vitesse_y);
            }

            BoundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(file.Width * MainGame.SIZE_mutliply), (int)(file.Height * MainGame.SIZE_mutliply));
        }

        public virtual void Draw(SpriteBatch pSpriteBatach)
        {
            pSpriteBatach.Draw(file, position, null, my_color, 0, Vector2.Zero, scale * MainGame.SIZE_mutliply, SpriteEffects.None, 0);
        }

        public void Set_color_pion(string new_color)
        {
            color_pion = new_color;
        }

    }
}
