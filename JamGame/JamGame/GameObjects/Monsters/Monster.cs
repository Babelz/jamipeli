using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Components;
using JamGame.Maps;
using BrashMonkeySpriter;
using JamGame.Entities;

namespace JamGame.GameObjects.Monsters
{
    public abstract class Monster : DrawableGameObject
    {
        #region Vars
        protected readonly TimerWrapper timerWrapper;
        protected readonly FiniteStateMachine brain;
        
        protected TargetingComponent<Player> targetComponent;
        #endregion

        #region Properties
        public CharaterAnimator Animation
        {
            get;
            protected set;
        }
        public HealthComponent Health
        {
            get;
            protected set;
        }
        #endregion

        public Monster()
        {
            components.Add(timerWrapper = new TimerWrapper());
            timerWrapper.AutoCreateTimers = true;

            components.Add(brain = new FiniteStateMachine());
            components.Add(targetComponent = new TargetingComponent<Player>(this));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Game.Instance.World.RemoveBody(body);
        }
    }
}
