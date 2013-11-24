using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.GameObjects.Components
{
    /// <summary>
    /// Rajapinta olio komponenteille jotka voivat suorittaa piirtoja.
    /// </summary>
    public interface IDrawableObjectComponent : IUpdatableObjectComponent
    {
        void Draw(SpriteBatch spriteBatch);
    }
}
