using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Maps
{
    public class MapManager : DrawableGameComponent
    {
        #region Vars
        private readonly SpriteBatch spriteBatch;
        private readonly List<Map> maps;
        #endregion

        #region Events
        public event MapManagerEventHandler OnMapChanged;
        #endregion

        #region Properties
        public Map Active
        {
            get;
            private set;
        }
        #endregion

        public MapManager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.spriteBatch = spriteBatch;

            maps = new List<Map>();

            DrawOrder = 0;
        }

        public void ChangeMap(string name)
        {
            Map last = Active;

            Map map = maps.FirstOrDefault(
                m => m.Name == name);

            if (map == null)
            {
                map = new Map(name);
                map.Load();

                Active = map;
                maps.Add(map);
            }
            else
            {
                Active = map;
            }

            if (OnMapChanged != null)
            {
                OnMapChanged(this, new MapManagerEventArgs(last, Active));
            }
        }
        public void RemoveMap(Map map)
        {
            maps.Remove(map);

            if (ReferenceEquals(map, Active))
            {
                Active = null;
            }
        }
        public Map GetMap(Predicate<Map> predicate)
        {
            return maps.FirstOrDefault(m => predicate(m));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Active != null)
            {
                Active.Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Active != null)
            {
                spriteBatch.Begin();

                Active.Draw(spriteBatch);

                spriteBatch.End();
            }
        }
    }

    public delegate void MapManagerEventHandler(object sender, MapManagerEventArgs e);

    public class MapManagerEventArgs : EventArgs
    {
        #region Vars
        public Map Last
        {
            get;
            private set;
        }
        public Map Next
        {
            get;
            private set;
        }
        #endregion

        public MapManagerEventArgs(Map last, Map next)
        {
            Last = last;
            Next = next;
        }
    }
}
