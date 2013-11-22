using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JamGame.Input
{
    public class GamepadStateProvider : IInputStateProvider
    {
        #region Properties

        /// <summary>
        /// Palauttaa kaikki nykyisen framen gamepadien statet
        /// </summary>
        public GamePadState[] CurrentStates
        {
            get;
            protected set;
        }

        /// <summary>
        /// Palauttaa kaikki viime framen gamepadien statet
        /// </summary>
        public GamePadState[] OldStates
        {
            get;
            protected set;
        }


        private ButtonProvider[] Cachers
        {
            get;
            set;
        }

        #endregion

        #region ctor

        public GamepadStateProvider()
        {
            CurrentStates = new GamePadState[4];
            Cachers = new ButtonProvider[CurrentStates.Length];
            for (int i = 0; i < Cachers.Length; i++)
                Cachers[i] = new ButtonProvider(this);
            GetNewState();
            OldStates = CurrentStates;
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Palauttaa nykyisen framen gamepadin statuksen pelaajaindeksillä
        /// </summary>
        /// <param name="index">Indeksi.</param>
        /// <returns>nykyisen framen gamepadin statuksen</returns>
        public GamePadState CurrentState(PlayerIndex index)
        {
            return CurrentStates[(int) index];
        }

        /// <summary>
        /// Palauttaa viime framen gamepadin statuksen pelaajaindeksillä
        /// </summary>
        /// <param name="index">Indeksi.</param>
        /// <returns>viime framen gamepadin statuksen</returns>
        public GamePadState OldState(PlayerIndex index)
        {
            return OldStates[(int)index];
        }

        /// <summary>
        /// Päivittää stateja
        /// </summary>
        public void Update()
        {
            foreach (ButtonProvider cacher in Cachers)
            {
                cacher.Update();
            }
            OldStates = (GamePadState[])CurrentStates.Clone();
            GetNewState();

        }

        /// <summary>
        /// Hakee kaikkien ohjaimien statet
        /// </summary>
        private void GetNewState()
        {
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                CurrentStates[(int)index] = GamePad.GetState(index);
            }
        }

        /// <summary>
        /// Tarkistaa onko näppäintä painettu
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <param name="index">Ohjaimen indeksi</param>
        /// <returns>True jos on</returns>
        public bool IsButtonPressed(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsButtonDown(button) &&
                OldStates[(int)index].IsButtonUp(button);
        }

        /// <summary>
        /// Tarkistaa onko näppäin ylhäällä
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <param name="index">Ohjaimen indeksi</param>
        /// <returns>True jos on</returns>
        public bool IsButtonReleased(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsButtonUp(button) &&
                OldStates[(int)index].IsButtonDown(button);
        }

        /// <summary>
        /// Tarkistaa painetaanko näppäintä
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <param name="index">Ohjaimen indeksi</param>
        /// <returns>True jos painetaan</returns>
        public bool IsButtonDown(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsButtonDown(button) 
                && OldStates[(int)index].IsButtonDown(button);
        }

        /// <summary>
        /// Tarkistaa onko näppäin ylhäällä
        /// </summary>
        /// <param name="button">Näppäin.</param>
        /// <param name="index">Ohjaimen indeksi</param>
        /// <returns>True jos näppäin on ollu nykyisen ja viime framen ylhäällä/returns>
        public bool IsButtonUp(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsButtonUp(button)
                && OldStates[(int)index].IsButtonUp(button);
        }

        public IEnumerable<Buttons> GetCurrentPressedButtons(PlayerIndex index)
        {
            return Cachers[(int) index].GetCurrentPressedButtons(index);
        }

        public IEnumerable<Buttons> GetOldPressedButtons(PlayerIndex index)
        {
            return Cachers[(int)index].GetOldPressedButtons(index);
        }




        #endregion

        private class ButtonProvider
        {
            #region Vars
            private static readonly IEnumerable<Buttons> AllButtons;
            private bool newCached;
            private bool oldCached;
            #endregion

            #region Properties
            private GamepadStateProvider Provider { get; set; }
            private GamePadState Old { get; set; }
            private GamePadState New { get; set; }
            #endregion

            static ButtonProvider()
            {
                AllButtons = Enum.GetValues(typeof(Buttons)).OfType<Buttons>();
            }

            public ButtonProvider(GamepadStateProvider provider)
            {
                Provider = provider;
                NewCache = new List<Buttons>();
                OldCache = new List<Buttons>();
            }

            public IEnumerable<Buttons> NewCache
            {
                get;
                private set;
            }

            public IEnumerable<Buttons> OldCache
            {
                get;
                private set;
            }

            private IEnumerable<Buttons> GetButtons(IEnumerable<Buttons> collection, Predicate<Buttons> predicate)
            {
                return collection.Where(b => predicate(b));
            }

            public void Update()
            {
                oldCached = false;
                newCached = false;
            }

            public IEnumerable<Buttons> GetCurrentPressedButtons(PlayerIndex index)
            {
                // ei tarvi generoida uudelleen :))))
                if (newCached)
                {
                    return NewCache;
                }

                New = Provider.CurrentStates[(int) index];
                NewCache = GetButtons(AllButtons, b => Provider.CurrentStates[(int)index].IsButtonDown(b));
                newCached = true;
                return NewCache;
            }

            public IEnumerable<Buttons> GetOldPressedButtons(PlayerIndex index)
            {
                // ei tarvi generoida uudelleen :))))
                if (oldCached) return OldCache;

                Old = Provider.CurrentStates[(int) index];
                OldCache = GetButtons(AllButtons, b => Provider.OldStates[(int)index].IsButtonDown(b));
                oldCached = true;
                return OldCache;
            }
        }
    }
}
