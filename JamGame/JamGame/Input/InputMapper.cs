using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace JamGame.Input
{
    public class InputMapper
    {
        #region Vars
        private readonly List<InputControlSetup> setups;
        #endregion

        #region Properties
        public Dictionary<Type, IInputStateProvider> StateProviders
        {
            get;
            set;
        }
        public Dictionary<Type, IInputBindProvider> InputProviders
        {
            get;
            set;
        }
        #endregion

        public InputMapper()
        {
            setups = new List<InputControlSetup>();
            InputProviders = new Dictionary<Type, IInputBindProvider>();
        }

        public IEnumerable<IInputCallbacker> Update(GameTime gameTime)
        {

            //UpdateKeys(gameTime.ElapsedGameTime.TotalMilliseconds).ForEach(MappedKeys.Add);
            foreach (var inputBindProvider in InputProviders)
            {
                foreach (var bind in inputBindProvider.Value.Update(StateProviders, gameTime))
                {
                    yield return bind;
                }

            }
            foreach (var invoke in setups.SelectMany(inputControlSetup => inputControlSetup.Mapper.Update(gameTime)))
            {
                yield return invoke;
            }
            //setups.ForEach(s => s.Mapper.MappedKeys.ForEach(MappedKeys.Add));
            
        }

        public T GetInputStateProvider<T>() where T : IInputStateProvider
        {
            return (T)StateProviders[typeof (T)];
        }

        public T GetInputBindProvider<T>() where T : IInputBindProvider
        {
            return (T) InputProviders[typeof (T)];
        }

        public void AddInputBindProvider(Type t, IInputBindProvider provider)
        {
            if (!InputProviders.ContainsKey(t))
            {
                InputProviders.Add(t, provider);
            }
        }

        public void RemoveSetup(InputControlSetup setup)
        {
            setups.Remove(setup);
        }

        public void AddSetup(InputControlSetup setup)
        {
            if (setups.Contains(setup)) return;

            setups.Add(setup);
            setup.Mapper.StateProviders = StateProviders;
        }
    }
}
