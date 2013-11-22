using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JamGame.Input
{
    /// <summary>
    /// Bindaus
    /// </summary>
    public abstract class Binding
    {
        private readonly string name;

        /// <summary>
        /// Bindauksen nimi
        /// </summary>
        /// <param name="name">Nimi</param>
        protected Binding(String name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Kauan bindausta on pidetty pohjassa
        /// </summary>
        public double HoldTime { get; set; }
    }

    /// <summary>
    /// Gamepad bindaus
    /// </summary>
    public class GamepadBinding : Binding
    {
        private readonly List<GamepadInputCallback> callbacks;
        public GamepadBinding(string name) : base(name)
        {
            callbacks = new List<GamepadInputCallback>();
        }

        /// <summary>
        /// Lisää callbackin
        /// </summary>
        /// <param name="callback">Callback</param>
        public void AddAction(GamepadInputCallback callback)
        {
            if (!callbacks.Contains(callback))
            {
                callbacks.Add(callback);
            }
            else
            {
                Debug.WriteLine("Yritettiin lisätä olemassa olevaa callbackkia bindaukselle {0}", Name);
            }
        }
        public List<GamepadInputCallback> Callbacks
        {
            get { return callbacks; }
        }
    }

    public class KeyboardBinding : Binding
    {
        private readonly List<KeyboardInputCallback> callbacks;

        public KeyboardBinding(string name) : base(name)
        {
            callbacks = new List<KeyboardInputCallback>();
        }

        public void AddAction(KeyboardInputCallback callback)
        {
            if (!callbacks.Contains(callback))
            {
                callbacks.Add(callback);
            }
            else
            {
                Debug.WriteLine("Yritettiin lisätä olemassa olevaa callbackkia bindaukselle {0}", Name);
            }
        }

        public List<KeyboardInputCallback> Callbacks
        {
            get { return callbacks;  }
        }

        public InputState Condition { get; set; }
    }
}
