using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using JamGame.DataTypes;
using JamGame.GameObjects.Components;
using FarseerPhysics.Dynamics;
using FarseerPhysics;

namespace JamGame.GameObjects
{
    public abstract class GameObject
    {
        #region Vars
        protected readonly List<IObjectComponent> components;
        protected Body body;

        private bool destroying;
        #endregion

        #region Events
        public event GameObjectEventHandler OnDestroying;
        public event GameObjectEventHandler OnDestroyed;
        #endregion

        #region Properties
        public Body Body
        {
            get
            {
                return body;
            }
        }
        public virtual Vector2 Position
        {
            get
            {
                return new Vector2(
                    ConvertUnits.ToDisplayUnits(body.Position.X),
                    ConvertUnits.ToDisplayUnits(body.Position.Y));
            }
            set
            {
                body.Position = new Vector2(
                    ConvertUnits.ToSimUnits(value.X),
                    ConvertUnits.ToSimUnits(value.Y));
            }
        }
        public Vector2 Velocity
        {
            get;
            set;
        }
        public Size Size
        {
            get;
            protected set;
        }
        public List<IObjectComponent> Components
        {
            get
            {
                return components;
            }
        }
        #endregion

        public GameObject()
        {
            components = new List<IObjectComponent>();
        }

        protected virtual void OnDestroy()
        {
        }

        public void Destory()
        {
            if (!destroying)
            {
                destroying = true;

                if (OnDestroying != null)
                {
                    OnDestroying(this, new GameObjectEventArgs(false, true));
                }
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            if (destroying)
            {
                OnDestroy();

                if (OnDestroyed != null)
                {
                    OnDestroyed(this, new GameObjectEventArgs(true, false));
                }

                Game.Instance.RemoveGameObject(this);

                return;
            }

            foreach (IObjectComponent component in components)
            {
                IUpdatableObjectComponent updatableComponent = component as IUpdatableObjectComponent;
                if (updatableComponent != null)
                {
                    updatableComponent.Update(gameTime);
                }
            }
        }
    }

    public delegate void GameObjectEventHandler(object sender, GameObjectEventArgs e);

    public class GameObjectEventArgs : EventArgs
    {
        #region Properties
        public bool Destroyed
        {
            get;
            private set;
        }
        public bool Destroying
        {
            get;
            private set;
        }
        #endregion

        public GameObjectEventArgs(bool destroyed, bool destroying)
        {
            Destroyed = destroyed;
            Destroying = destroying;
        }
    }
}
