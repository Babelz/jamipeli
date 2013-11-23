using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Monsters;
using Microsoft.Xna.Framework;

namespace JamGame.Maps
{
    public class MonsterWave
    {
        #region Vars
        private readonly List<Monster> monsters;
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

        public MonsterWave(List<Monster> monsters, int releaseTime, Vector2 positionModifier)
        {
            this.monsters = monsters;

            ReleaseTime = releaseTime;
            PositionModifier = positionModifier;
        }

        public List<Monster> ReleaseMonsters()
        {
            List<Monster> monsters = new List<Monster>(this.monsters);
            this.monsters.Clear();

            return monsters;
        }
    }
}
