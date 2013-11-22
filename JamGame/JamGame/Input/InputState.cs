using System;

namespace JamGame.Input
{
    [Flags]
    public enum InputState
    {
        /// <summary>
        /// default
        /// </summary>
        None = 0x0,
        /// <summary>
        /// kun nappi on juuri painettu
        /// </summary>
        Pressed = 0x1,
        /// <summary>
        /// Kun nappi on ollut pohjassa ainakin 2 framea
        /// </summary>
        Down = 0x2,
        /// <summary>
        /// Kun nappi oli viime framella pohjassa mutta nykyisellä ei
        /// </summary>
        Released = 0x4,
        /// <summary>
        /// Kun nappi on ollut vähintään 2 framea ylhäällä
        /// </summary>
        Up = 0x8
        
    }
}
