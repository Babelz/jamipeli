using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Maps
{
    public class Map : DrawableGameObject
    {
        #region Vars
        private MapStateManager mapStateManager;
        #endregion

        public void Load(string mapName)
        {
            MapProcessor mapProcessor = new MapProcessor(mapName);
            mapStateManager = new MapStateManager(mapProcessor.LoadMapStates());

            mapStateManager.Start();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            mapStateManager.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            mapStateManager.Draw(spriteBatch);
        }
    }
}
