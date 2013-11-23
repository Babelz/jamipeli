using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Monsters;

namespace JamGame.Factories
{
    /// <summary>
    /// Reflection tehdas joka tuottaa monstereita.
    /// </summary>
    public class MonsterFactory : IReflectionFactory<Monster>
    {
        #region Vars
        // Nimiavaruus josta etsitään olioja kun reflectataan.
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
        /// <summary>
        /// Luo halutun määrän uusia hirviöitä.
        /// </summary>
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
