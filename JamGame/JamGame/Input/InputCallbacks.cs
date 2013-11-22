using Microsoft.Xna.Framework.Input;

namespace JamGame.Input
{
    public delegate void KeyboardInputCallback(Keys triggered, InputEventArgs args);
    public delegate void GamepadInputCallback(Buttons triggered, GamepadInputEventArgs args);
}
