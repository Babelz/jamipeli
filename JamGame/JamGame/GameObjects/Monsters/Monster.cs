using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Components;
using JamGame.Maps;
using BrashMonkeySpriter;
using JamGame.Entities;
using Microsoft.Xna.Framework;
using JamGame.Gamestate;
using JamGame.Extensions;

namespace JamGame.GameObjects.Monsters
{
    public abstract class Monster : DrawableGameObject
    {
        #region Vars
        protected readonly Random random;
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
            random = new Random();

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

        #region Brain states
        protected void MoveToArea()
        {
            if (Position.X < 0)
            {
                body.ApplyForce(new Vector2(15f, 0.0f));
            }
            else if (Position.X > 1000)
            {
                body.ApplyForce(new Vector2(-15f, 0.0f));
            }
            else
            {
                brain.PopState();
                brain.PushState(MoveToTarget);
                brain.PushState(GetTarget);
            }
        }
        protected virtual void GetTarget()
        {
            GameplayState gameplayState = Game.Instance.GameStateManager.States
                .FirstOrDefault(s => s is GameplayState)
                as GameplayState;

            Player nearest = gameplayState.Players.FindNearest<Player>(Position) as Player;

            if (nearest != null)
            {
                targetComponent.ChangeTarget(nearest);
            }

            brain.PopState();
        }
        protected virtual void MoveToTarget()
        {
            if (targetComponent.HasTarget)
            {
                body.ApplyForce(targetComponent.VelocityToTarget * 5);
            }
            else
            {
                brain.PopState();
                brain.PushState(GetTarget);
            }
        }
        #endregion
    }
}
