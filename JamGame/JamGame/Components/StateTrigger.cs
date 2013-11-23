using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using JamGame.Maps;
using JamGame.Entities;
using JamGame.Gamestate;

namespace JamGame.Components
{
    public class StateTrigger : GameComponent
    {
        #region Vars
        private Rectangle collisionRectangle;
        #endregion

        public StateTrigger(Game game)
            : base(game)
        {
            Enabled = false;

            JamGame.Game.Instance.MapManager.OnMapChanged += new MapManagerEventHandler(MapManager_OnMapChanged);
        }

        #region Event hanlders
        private void StateManager_OnStateFinished(object sender, MapStateManagerEventArgs e)
        {
            Enabled = true;

            int width = JamGame.Game.Instance.ScreenWidth / 10;
            int height = JamGame.Game.Instance.ScreenHeight;

            collisionRectangle = new Rectangle(JamGame.Game.Instance.ScreenWidth - width, 0, width, height);
        }
        private void MapManager_OnMapChanged(object sender, MapManagerEventArgs e)
        {
            JamGame.Game.Instance.MapManager.Active.StateManager.OnStateFinished += new MapStateManagerEventHandle(StateManager_OnStateFinished);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Update(gameTime);

                Player player = (JamGame.Game.Instance.GameStateManager.States
                    .First(c => c is GameplayState) as GameplayState).Player;

                Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Size.Width, player.Size.Height);

                if (collisionRectangle.Intersects(playerRect))
                {
                    Console.WriteLine("gg");
                    JamGame.Game.Instance.MapManager.Active.StateManager.StartTransition();
                    Enabled = false;
                }
            }
        }
    }
}
