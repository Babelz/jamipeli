using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JamGame.GameObjects.Components
{
    public class TargetingComponent<T> : IObjectComponent where T : GameObject
    {
        #region Vars
        private readonly GameObject owner;
        #endregion

        #region Events
        public event TargetingComponentEventHandler<T> OnTargetChanged;
        #endregion

        #region Properties
        public T Target
        {
            get;
            private set;
        }
        public bool HasTarget
        {
            get
            {
                return Target != null;
            }
        }
        public Vector2 DistanceToTarget
        {
            get
            {
                if (HasTarget)
                {
                    return new Vector2(Target.Position.X - owner.Position.X, Target.Position.Y - owner.Position.Y);
                }

                return new Vector2(0.0f);
            }
        }
        public Vector2 VelocityToTarget
        {
            get
            {
                if (HasTarget)
                {
                    return new Vector2(MathHelper.Clamp(DistanceToTarget.X, -1, 1), MathHelper.Clamp(DistanceToTarget.Y, -1, 1));
                }

                return Vector2.Zero;
            }
        }
        #endregion

        public TargetingComponent(GameObject owner)
        {
            this.owner = owner;
        }

        private void LaunchOnTargetChanged(T nextTarget = null)
        {
            if (OnTargetChanged != null)
            {
                OnTargetChanged(this, new TargetingComponentEventArgs<T>(Target, nextTarget));
            }
        }

        public void ChangeTarget(T target)
        {
            LaunchOnTargetChanged(target);

            this.Target = target;
        }
        public void GetNearestTarget()
        {
            // TODO: hae lähin target.
        }
    }

    public delegate void TargetingComponentEventHandler<T>(object sender, TargetingComponentEventArgs<T> e) where T : GameObject;

    public class TargetingComponentEventArgs<T> where T : GameObject
    {
        #region Properties
        public bool ChangedTarget
        {
            get;
            private set;
        }
        public T CurrentTarget
        {
            get;
            private set;
        }
        public T NextTarget
        {
            get;
            private set;
        }
        #endregion

        public TargetingComponentEventArgs(T currentTarget, T nextTarget = null)
        {
            CurrentTarget = currentTarget;
            NextTarget = nextTarget;

            ChangedTarget = nextTarget != null;
        }
    }
}
