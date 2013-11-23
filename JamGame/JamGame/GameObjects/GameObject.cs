using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using JamGame.DataTypes;
using JamGame.GameObjects.Components;

namespace JamGame.GameObjects
{
    public abstract class GameObject
    {
        #region Vars
        private bool destroying;
        protected readonly List<IObjectComponent> components;
        #endregion

        #region Events
        public event GameObjectEventHandler OnDestroying;
        public event GameObjectEventHandler OnDestroyed;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get;
            set;
        }

        public Vector2 Velocity
        {
            get;
            set;
        }
        public Size Size
        {
            get;
            private set;
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
            else
            {
                OnDestroy();

                if (OnDestroyed != null)
                {
                    OnDestroyed(this, new GameObjectEventArgs(true, false));
                }

                Game.Instance.RemoveGameObject(this);
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            if (destroying)
            {
                Destory();
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
