using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JamGame.GameObjects.Components
{
    public interface IUpdatableObjectComponent : IObjectComponent
    {
        void Update(GameTime gameTime);
    }
}
