using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind_super
{
    

    public class GameSTATE
    {
        public enum SCENE_type
        {
            menu,
            gameplay,
            gameover
        }

        protected MainGame mainGame;
        public SCENE scene_current { get; set; }

        public GameSTATE(MainGame pGame)
        {
            mainGame = pGame;
        }

        public void Change_scene(SCENE_type pSCENE_type)
        {
            if (scene_current != null)
            {
                scene_current.UnLoad();
                scene_current = null;
            }

            switch (pSCENE_type)
            {
                case SCENE_type.menu:
                    scene_current = new SCENE_menu(mainGame);
                    break;
                case SCENE_type.gameplay:
                    scene_current = new SCENE_gameplay(mainGame);
                    break;
                case SCENE_type.gameover:
                    scene_current = new SCENE_gameover(mainGame);
                    break;
                default:
                    scene_current = new SCENE_menu(mainGame);
                    break;
            }

            scene_current.Load();
        }
    }
}
