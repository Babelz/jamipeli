using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JamGame.Input
{
    /// <summary>
    /// Argsit jotka lähetään kun tapahtuu globaali input event
    /// </summary>
    public class InputEventArgs
    {
        #region Properties
        public GameTime Delta { get; private set; }
        public double HeldTime { get; private set; }
        public InputState State { get; private set; }
        #endregion

        public InputEventArgs(double heldTime, InputState state, GameTime delta)
        {
            Delta = delta;
            HeldTime = heldTime;
            State = state;
        }
    }

    public class GamepadInputEventArgs : InputEventArgs
    {
        #region Properties
        private readonly GameTime delta;
        public PlayerIndex PlayerIndex { get; set; }
        public GamePadThumbSticks ThumbSticks { get; set; }
        #endregion

        public GamepadInputEventArgs(double heldTime, InputState state, GameTime delta, PlayerIndex index, GamePadThumbSticks thumbSticks ) : base(heldTime, state, delta)
        {
            this.delta = delta;
            PlayerIndex = index;
            ThumbSticks = thumbSticks;
        }
    }
}
