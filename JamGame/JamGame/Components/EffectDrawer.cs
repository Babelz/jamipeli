using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Components
{
    /// <summary>
    /// Luokka jonka on tarkoitus piirtää ja päivittää efektejä delekaattien avulla.
    /// </summary>
    public sealed class EffectDrawer
    {
        #region Properties
        public Action<GameTime> Update
        {
            get;
            set;
        }
        public Action<SpriteBatch> Draw
        {
            get;
            set;
        }
        /// <summary>
        /// Suoritetaan disposaus logiikka jos semmoista tarvitaan.
        /// </summary>
        public Action Dispose
        {
            get;
            set;
        }
        #endregion
    }
}
