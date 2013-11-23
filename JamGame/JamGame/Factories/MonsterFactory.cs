using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Monsters;

namespace JamGame.Factories
{
    public class MonsterFactory : IReflectionFactory<Monster>
    {
        #region Vars
        private readonly string rootNamespace;
        #endregion

        public MonsterFactory(string rootNamespace)
        {
            this.rootNamespace = rootNamespace;
        }

        public Monster MakeNew(string typename)
        {
            return Activator.CreateInstance(Type.GetType(rootNamespace + "." + typename)) as Monster; 
        }
        public List<Monster> MakeNew(string typename, int count)
        {
            List<Monster> monsters = new List<Monster>();

            Type type = Type.GetType(rootNamespace + "." + typename);
            for (int i = 0; i < count; i++)
            {
                monsters.Add(Activator.CreateInstance(type) as Monster);
            }

            return monsters;
        }
    }
}
