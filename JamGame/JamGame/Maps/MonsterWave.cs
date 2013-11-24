using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Monsters;
using Microsoft.Xna.Framework;
using JamGame.Factories;

namespace JamGame.Maps
{
    public class MonsterWave
    {
        #region Vars
        private readonly MonsterFactory factory;
        private readonly Dictionary<string, int> monsters;
        #endregion

        #region Properties
        public int ReleaseTime
        {
            get;
            private set;
        }
        public Vector2 PositionModifier
        {
            get;
            private set;
        }
        #endregion

        public MonsterWave(Dictionary<string, int> monsters, int releaseTime, Vector2 positionModifier)
        {
            this.monsters = monsters;
            ReleaseTime = releaseTime;
            PositionModifier = positionModifier;

            factory = new MonsterFactory("JamGame.GameObjects.Monsters");
        }

        public List<Monster> ReleaseMonsters()
        {
            List<Monster> monsters = new List<Monster>();

            foreach (KeyValuePair<string, int> key in this.monsters)
            {
                monsters.AddRange(factory.MakeNew(key.Key, key.Value));
            }

            return monsters;
        }
    }
}
