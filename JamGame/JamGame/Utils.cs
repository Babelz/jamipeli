using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamGame
{
    public static class Utils
    {
        public static bool InRange(int min, int max, int value)
        {
            return value >= min && value <= max;
        }
    }
}
