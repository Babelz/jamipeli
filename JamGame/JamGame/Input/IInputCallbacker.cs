using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace JamGame.Input
{
    public interface IInputCallbacker
    {
        void Fire();
    }

    public class GamepadInputCallbacker : IInputCallbacker
    {
        #region Vars
        private readonly Buttons button;
        private readonly GamepadInputEventArgs args;
        private readonly List<GamepadInputCallback> callbacks;
        #endregion

        public GamepadInputCallbacker(Buttons button, GamepadInputEventArgs args, List<GamepadInputCallback> callbacks) 
        {
            this.button = button;
            this.args = args;
            this.callbacks = callbacks;
        }
        public void Fire()
        {
            callbacks.ForEach(c => c(button, args));
        }
    }

    public class KeyboardInputCallbacker : IInputCallbacker
    {
        #region Vars
        private readonly Keys key;
        private readonly InputEventArgs args;
        private readonly List<KeyboardInputCallback> callbacks;
        #endregion

        public KeyboardInputCallbacker(Keys key, InputEventArgs args,
            List<KeyboardInputCallback> callbacks)
        {
            this.key = key;
            this.args = args;
            this.callbacks = callbacks;
        }

        public void Fire()
        {
            callbacks.ForEach(c => c(key, args));
        }
    }
}
