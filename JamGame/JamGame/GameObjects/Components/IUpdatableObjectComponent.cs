using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JamGame.GameObjects.Components
{
    /// <summary>
    /// Rajapinta olio komponenteille jotka voivat suorittaa päivityksiä.
    /// </summary>
    public interface IUpdatableObjectComponent : IObjectComponent
    {
        void Update(GameTime gameTime);
    }
}
