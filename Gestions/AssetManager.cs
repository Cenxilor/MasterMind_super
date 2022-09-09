using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind_super
{
    class AssetManager
    {
        public static SpriteFont main_police { get; private set; } // propriété static accessible sans instance de assetmanager (mais accessible uniquement par des méthodes static

        private static SoundEffect sndEffect_explode;
        public static SoundEffectInstance sndEffect_instance_explode { get; private set; }

        private static SoundEffect sndEffect_artifice;
        public static SoundEffectInstance sndEffect_instance_artifice { get; private set; }

        public static void Init(ContentManager pContent) // recois le content manager de maingame
        {
            main_police = pContent.Load<SpriteFont>("mainFont"); //créer la nouvelle police

            sndEffect_artifice = pContent.Load<SoundEffect>("sound/feu_artifice");
            sndEffect_instance_artifice = sndEffect_artifice.CreateInstance();
        }
    }
}
