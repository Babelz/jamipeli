using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Components;
using FarseerPhysics.Factories;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using JamGame.DataTypes;

namespace JamGame.GameObjects.PowerUpItems
{
    public abstract class PowerUpItem : DrawableGameObject
    {
        #region Properties
        public bool Used
        {
            get;
            protected set;
        }
        #endregion

        public PowerUpItem()
        {
            Used = false;

            body = BodyFactory.CreateRectangle(Game.Instance.World, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 1.0f);
            body.Friction = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0f;
            body.LinearDamping = 5f;
            body.UserData = this;
            Position = Vector2.Zero;
            Size = new Size(64, 64);
        }
    }
    public abstract class ComponentPowerUp<T> : PowerUpItem where T : IObjectComponent
    {
        public virtual void Apply(T target)
        {
            Used = true;
        }
    }
    public abstract class ObjectPowerUp<T> : PowerUpItem where T : GameObject
    {
        public virtual void Apply(T target)
        {
            Used = true;
        }
    }
}
