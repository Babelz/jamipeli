using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.GameObjects.Components
{
    public interface IDrawableObjectComponent : IUpdatableObjectComponent
    {
        void Draw(SpriteBatch spriteBatch);
    }
}
