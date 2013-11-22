using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Maps
{
    public class MapStateManager
    {
        #region Vars
        private readonly List<MapState> states;
        private MapState current;
        #endregion

        #region Propeties
        public bool Finished
        {
            get
            {
                return states.Count == 0;
            }
        }
        #endregion

        public MapStateManager(List<MapState> states)
        {
            this.states = states;
        }

        public void Start()
        {
            if (!Finished)
            {
                current = states.FirstOrDefault();
                current.Start();
            }
        }
        public void Update(GameTime gameTime)
        {
            if (current != null)
            {
                current.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (current != null)
            {
                current.Draw(spriteBatch);
            }
        }
    }

    public delegate void MapStateManagerEventHandle(object sender, MapStateManagerEventArgs e);

    public class MapStateManagerEventArgs : EventArgs
    {
        #region Properties
        public MapState Current
        {
            get;
            private set;
        }
        public MapState Next
        {
            get;
            private set;
        }
        #endregion
    }
}
