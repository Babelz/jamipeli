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
    /// <summary>
    /// Trigger joka hoitaa kartta statejen transitionin aloittamisen.
    /// </summary>
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
            // Aloitetaan collisionin kuuntelu ja alustetaan rectangle staten koon mukaan.

            Enabled = true;

            int width = JamGame.Game.Instance.ScreenWidth / 10;
            int height = JamGame.Game.Instance.ScreenHeight;

            collisionRectangle = new Rectangle(JamGame.Game.Instance.ScreenWidth - width, 0, width, height);
        }
        private void MapManager_OnMapChanged(object sender, MapManagerEventArgs e)
        {
            // Kun kartta vaihtuu, aloitetaan kuuntelemaan milloin state päättyy.
            JamGame.Game.Instance.MapManager.Active.StateManager.OnStateFinished += new MapStateManagerEventHandle(StateManager_OnStateFinished);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                // Haetaan pelaajat ja katsotaan törmääkö joku heistä triggeriin.
                Player[] players = (JamGame.Game.Instance.GameStateManager.States
                    .First(c => c is GameplayState) as GameplayState).Players;

                foreach (Player player in players)
                {
                    Rectangle playerRectangle = new Rectangle((int)player.Position.X, (int)player.Position.Y, 
                        player.Size.Width, player.Size.Height);

                    // Jos joku pelaajista törmää triggeriin, aloitetaan state transition.
                    if (collisionRectangle.Intersects(playerRectangle))
                    {
                        JamGame.Game.Instance.MapManager.Active.StateManager.StartTransition();
                        Enabled = false;

                        break;
                    }
                }

                base.Update(gameTime);
            }
        }
    }
}
