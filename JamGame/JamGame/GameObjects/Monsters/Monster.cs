using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Components;
using JamGame.Maps;
using BrashMonkeySpriter;

namespace JamGame.GameObjects.Monsters
{
    public abstract class Monster : DrawableGameObject
    {
        #region Vars
        protected readonly TimerWrapper timerWrapper;
        protected readonly FiniteStateMachine brain;
        protected CharaterAnimator animation;
        #endregion

        #region Properties
        public HealthComponent Health
        {
            get;
            protected set;
        }
        #endregion

        public Monster()
        {
            components.Add(timerWrapper = new TimerWrapper());
            components.Add(brain = new FiniteStateMachine());
        }
    }
}
