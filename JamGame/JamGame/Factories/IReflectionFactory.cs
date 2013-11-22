using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamGame.Factories
{
    public interface IReflectionFactory<T>
    {
        T MakeNew(string typename);
    }
}
