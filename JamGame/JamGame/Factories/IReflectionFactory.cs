using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamGame.Factories
{
    /// <summary>
    /// Rajapinta reflectionia hyödyntäville tehtaille.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReflectionFactory<T>
    {
        /// <summary>
        /// Luo uuden olion tyyppi nimen perusteella.
        /// </summary>
        T MakeNew(string typename);
    }
}
