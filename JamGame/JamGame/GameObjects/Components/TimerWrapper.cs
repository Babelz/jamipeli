using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JamGame.GameObjects.Components
{
    public class TimerWrapper : IUpdatableObjectComponent
    {
        #region Private keypair class
        private class Keypair<TKey, TValue>
        {
            #region Properties
            public TKey Key
            {
                get;
                private set;
            }
            public TValue Value
            {
                get;
                set;
            }
            #endregion

            public Keypair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
        #endregion

        #region Vars
        private readonly List<Keypair<string, int>> timers;
        #endregion

        #region Properties
        public int this[string key]
        {
            get
            {
                Keypair<string, int> keypair = TryGetKeypair(key);

                return keypair.Value;
            }
        }
        public bool AutoCreateTimers
        {
            get;
            set;
        }
        #endregion

        public TimerWrapper()
        {
            timers = new List<Keypair<string, int>>();
        }

        private Keypair<string, int> MakeNew(string key, int startvalue = 0)
        {
            if (ContainsTimer(key))
            {
                throw new ArgumentException("Timer with key " + key + " already exists!");
            }
            else
            {
                return new Keypair<string, int>(key, startvalue);
            }
        }
        private Keypair<string, int> TryGetKeypair(string key)
        {
            Keypair<string, int> keypair = timers.FirstOrDefault(
                k => k.Key == key);

            if (keypair == null && AutoCreateTimers)
            {
                keypair = MakeNew(key);
                timers.Add(keypair);
            }

            return keypair;
        }

        public bool ContainsTimer(string key)
        {
            return timers.FirstOrDefault(k => k.Key == key) != null;
        }
        public void AddTimer(string key, int startvalue = 0)
        {
            timers.Add(MakeNew(key, startvalue));
        }
        public bool RemoveTimer(string key)
        {
            if (ContainsTimer(key))
            {
                timers.Remove(timers.First(k => k.Key == key));
                return true;
            }

            return false;
        }
        public void ResetTimer(string key)
        {
            if (ContainsTimer(key))
            {
                timers.First(k => k.Key == key).Value = 0;
            }
        }
        public void ResetAllTimers()
        {
            timers.ForEach(k => k.Value = 0);
        }

        public void Update(GameTime gameTime)
        {
            timers.ForEach(k => k.Value += gameTime.ElapsedGameTime.Milliseconds);
        }
    }
}
