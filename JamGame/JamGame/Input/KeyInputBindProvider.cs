using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JamGame.Input
{
    public class KeyInputBindProvider : IInputBindProvider
    {
        #region vars
        private readonly Dictionary<string, Keys[]> keyBinds;
        #endregion

        #region Properties
        public Dictionary<string, KeyboardBinding> Bindings { get; private set; }
        #endregion

        public KeyInputBindProvider()
        {
            Bindings = new Dictionary<string, KeyboardBinding>();
            keyBinds = new Dictionary<string, Keys[]>();
        }

        public void Map(KeyTrigger trigger, KeyboardInputCallback callback, InputState inputCondition = (InputState.Down | InputState.Pressed | InputState.Released | InputState.Up))
        {

            if (!Bindings.ContainsKey(trigger.Name))
            {
                KeyboardBinding binding = new KeyboardBinding(trigger.Name);
                Bindings.Add(trigger.Name, binding);
                Bindings[trigger.Name].AddAction(callback);
                Bindings[trigger.Name].Condition = inputCondition;
            }

            if (keyBinds.ContainsKey(trigger.Name))
            {
                return;
            }
            Keys[] keys = new Keys[1 + trigger.AlternateKeys.Length];
            keys[0] = trigger.Key;
            for (int i = 0, j = 1; i < trigger.AlternateKeys.Length; i++, j++)
            {
                keys[j] = trigger.AlternateKeys[i];
            }
            keyBinds.Add(trigger.Name, keys);
        }

        public IEnumerable<IInputCallbacker> Update(Dictionary<Type, IInputStateProvider> providers, GameTime gt)
        {
            KeyboardStateProvider keyInput = (KeyboardStateProvider)providers[typeof (KeyboardStateProvider)];
            //Console.WriteLine(keyInput.IsKeyDown(Keys.Space));

            Keys[] newKeys = keyInput.CurrentState.GetPressedKeys();
            Keys[] oldKeys = keyInput.OldState.GetPressedKeys();

            IEnumerable<Keys> toCheck = oldKeys.Union(newKeys);
            List<String> calls = new List<string>();

            foreach (Keys key in toCheck)
            {
                foreach (var bind in keyBinds.Where(bind => bind.Value.Contains(key)))
                {
                    if (calls.Contains(bind.Key)) continue;
                 
                    KeyboardBinding binding = Bindings[bind.Key];

                    InputState inputState = InputState.Up;
                    // koska key on down useimmiten, niin parempi tarkastaa eka
                    if (keyInput.IsKeyDown(key))
                    {
                        inputState = InputState.Down;
                        binding.HoldTime += gt.ElapsedGameTime.TotalMilliseconds;
                    }
                        // just tällä framella
                    else if (keyInput.IsKeyPressed(key))
                    {
                        inputState = InputState.Pressed;
                 
                        binding.HoldTime = 0;
                    }
                        // just löysätty
                    else if (keyInput.IsKeyReleased(key))
                    {
                        inputState = InputState.Released;
                    }

                    
                    if ((inputState & binding.Condition) != inputState) continue;
                    
                    InputEventArgs args = new InputEventArgs(binding.HoldTime, inputState, gt);
                    KeyboardInputCallbacker cb = new KeyboardInputCallbacker(key, args, binding.Callbacks);

                    calls.Add(bind.Key);
                    yield return cb;
                }
                
            }
        }
    }
}
