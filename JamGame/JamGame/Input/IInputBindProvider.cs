using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JamGame.Input
{
    public interface IInputBindProvider
    {
        /// <summary>
        /// Mäppää kaikki tämän framen napit ja yieldaa ne invokettaviksi
        /// </summary>
        /// <param name="providers">Providereita joilla saa nykyisen staten</param>
        /// <param name="gameTime">delta time</param>
        /// <returns>Kaikki napit joita on painettu/löysätty</returns>
        IEnumerable<IInputCallbacker> Update(Dictionary<Type, IInputStateProvider> providers, GameTime gameTime);
    }
}
