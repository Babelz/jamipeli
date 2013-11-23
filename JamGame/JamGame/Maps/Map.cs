using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Maps
{
    /// <summary>
    /// Stateja käyttävä kartta.
    /// </summary>
    public class Map
    {
        #region Vars
        // Kartan kaikki statet.
        private MapStateManager mapStateManager;
        #endregion

        #region Properties
        public string Name
        {
            get;
            private set;
        }
        public MapStateManager StateManager
        {
            get
            {
                return mapStateManager;
            }
        }
        #endregion

        public Map(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Alustaa kartan kaikki statet.
        /// </summary>
        public void Load()
        {
            MapProcessor mapProcessor = new MapProcessor(@"Maps\MapFiles\" + Name + ".xml");
            mapStateManager = new MapStateManager(mapProcessor.LoadMapStates());
        }
        public void Update(GameTime gameTime)
        {
            mapStateManager.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            mapStateManager.Draw(spriteBatch);
        }
    }
}
