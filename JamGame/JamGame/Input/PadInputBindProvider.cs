using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JamGame.Input
{
    public class PadInputBindProvider : IInputBindProvider
    {
        #region Vars
        private readonly Dictionary<string, Buttons[]> buttonBinds;
        #endregion

        #region Properties
        public Dictionary<string, GamepadBinding> Bindings { get; private set; }
        #endregion

        #region Ctor
        public PadInputBindProvider()
        {
            Bindings = new Dictionary<string, GamepadBinding>();
            buttonBinds = new Dictionary<string, Buttons[]>();
        }
        #endregion

        #region Methods

        #region Interface
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providers"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public IEnumerable<IInputCallbacker> Update(Dictionary<Type, IInputStateProvider> providers, GameTime gameTime)
        {
           // Console.WriteLine(providers.Count);
            GamepadStateProvider provider = providers[typeof (GamepadStateProvider)] as GamepadStateProvider;

            IEnumerable<Buttons> toCheck =
                provider.GetOldPressedButtons(PlayerIndex.One).Union(provider.GetCurrentPressedButtons(PlayerIndex.One));
        
            List<String> calls = new List<string>();

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                if (!provider.CurrentState(index).IsConnected) 
                    continue;
                foreach (Buttons button in toCheck)
                {
                    foreach (var bind in buttonBinds.Where(bind => bind.Value.Contains(button)))
                    {
                        // että ei ajeta kahta kertaa samaa keyta
                        if (calls.Contains(bind.Key)) continue;

                        InputState inputState;

                        GamepadBinding binding = Bindings[bind.Key];
                        if (provider.IsButtonDown(button, index))
                        {
                            inputState = InputState.Down;
                            binding.HoldTime += gameTime.ElapsedGameTime.TotalMilliseconds;


                        }
                        else if (provider.IsButtonPressed(button, index))
                        {
                            inputState = InputState.Pressed;
                            binding.HoldTime = 0;

                        }
                        else
                        {
                            inputState = InputState.Released;
                        }

                        GamepadInputEventArgs args = new GamepadInputEventArgs(binding.HoldTime, inputState, gameTime, index, provider.CurrentState(index).ThumbSticks);
                        GamepadInputCallbacker cb = new GamepadInputCallbacker(button, args, binding.Callbacks);
                        calls.Add(bind.Key);
                        yield return cb;
                    }
                }
            }   
        }

        #endregion

        public void Map(ButtonTrigger trigger, GamepadInputCallback callback)
        {
            if (!Bindings.ContainsKey(trigger.Name))
            {
                GamepadBinding binding = new GamepadBinding(trigger.Name);
                Bindings.Add(trigger.Name, binding);
                Bindings[trigger.Name].AddAction(callback);
            }

            Buttons[] buttons = new Buttons[1 + trigger.AlternateButtons.Length];
            buttons[0] = trigger.Button;
            for (int i = 0, j = 1; i < trigger.AlternateButtons.Length; i++, j++)
            {
                buttons[j] = trigger.AlternateButtons[i];
            }
            buttonBinds.Add(trigger.Name, buttons);
        }

        #endregion
    }
}
