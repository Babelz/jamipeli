using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.PowerUpItems;

namespace JamGame.Factories
{
    public class PowerUpFactory : IReflectionFactory<PowerUpItem>
    {
        #region Vars
        private readonly string objectNamespace;
        #endregion

        public PowerUpFactory(string objectNamespace)
        {
            this.objectNamespace = objectNamespace;
        }

        public PowerUpItem MakeNew(string typename)
        {
            Type type = Type.GetType(objectNamespace + "." + typename);

            return Activator.CreateInstance(type) as PowerUpItem;
        }
    }
}
