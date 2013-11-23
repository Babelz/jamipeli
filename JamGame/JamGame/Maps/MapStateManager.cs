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
        private StateTransition transition;
        private MapState currentMapState;
        private MapState nextMapState;
        #endregion

        #region Events
        public event MapStateManagerEventHandle OnMapFinished;
        public event MapStateManagerEventHandle OnStateFinished;
        #endregion

        #region Propeties
        public MapState CurrentMapState
        {
            get
            {
                return currentMapState;
            }
        }
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

        /// <summary>
        /// Palauttaa seuraavan staten ja poistaa sen state listasta.
        /// </summary>
        /// <returns></returns>
        private MapState GetNextState()
        {
            MapState nextMapState = states.FirstOrDefault();
            states.Remove(nextMapState);

            return nextMapState;
        }
        private void ChangeState()
        {
            if (!Finished)
            {
                MapState lastMapState = currentMapState;

                // Jos on jo state, otetaan seuraava vain talteen.
                if (currentMapState != null)
                {
                    nextMapState = GetNextState();
                }
                else
                {
                    // Jos meillä ei ole state, tulee toisto aloittaa.
                    currentMapState = GetNextState();
                    currentMapState.Start();
                }

                // Jos edellinen state ei ole null ja se on jo toistettu loppuun,
                // laukaistaan eventti.
                if (lastMapState != null && lastMapState.Finished)
                {
                    if (OnStateFinished != null)
                    {
                        OnStateFinished(this, new MapStateManagerEventArgs(lastMapState, nextMapState));
                    }
                }
            }
        }

        /// <summary>
        /// Aloittaa statejen toiston.
        /// </summary>
        public void Start()
        {
            // Jos current on null, ei olla aloitettu statejen toistamista.
            if (currentMapState == null)
            {
                ChangeState();
            }
        }
        /// <summary>
        /// Jos quessa on seuraava state, asettaa sen aktiiviseksi.
        /// </summary>
        public void ChangeNextState()
        {
            // TODO: playaa tässä transition efekti.
            
            currentMapState = nextMapState;
            nextMapState = null;
        }
        public void Update(GameTime gameTime)
        {
            // Jos omataan map state, sallitaan päivitykset.
            if (currentMapState != null)
            {
                // Päivittää nykysen staten.
                currentMapState.Update(gameTime);

                // Jos omataan vielä stateja, tarkistaa onko nykyinen state jo toistettu,
                // jos näin on, yrittää vaihtaa staten.
                if (!Finished)
                {
                    if (currentMapState.Finished)
                    {
                        ChangeState();
                    }
                }
                else
                {
                    // Jos stateja ei ole, alkaa toistaa eventtiä.
                    if (OnMapFinished != null)
                    {
                        OnMapFinished(this, new MapStateManagerEventArgs(currentMapState, null));
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentMapState != null)
            {
                currentMapState.Draw(spriteBatch);
            }
        }
    }

    public delegate void MapStateManagerEventHandle(object sender, MapStateManagerEventArgs e);

    public class MapStateManagerEventArgs : EventArgs
    {
        #region Properties
        public MapState Last
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

        public MapStateManagerEventArgs(MapState last, MapState next)
        {
            Last = last;
            Next = next;
        }
    }
}
