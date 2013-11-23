using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Components
{
    public class EffectDrawer
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
        public Action Dispose
        {
            get;
            set;
        }
        #endregion
    }
}
