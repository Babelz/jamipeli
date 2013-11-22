using System;

namespace JamGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Game.Instance.Run();
            Game.Instance.Dispose();
        }
    }
#endif
}

