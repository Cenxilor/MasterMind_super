using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind_super
{
    public interface iActor
    {
        Vector2 position { get; }
        Rectangle BoundingBox { get; }
        bool toRemove { get; set; }
        bool IsVisible { get; set; }

        void Update(GameTime pGameTime);
        void Draw(SpriteBatch pSpriteBatach);
        void Touched_By(iActor pBy);
    }
}
