using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JamGame.GameObjects.Components
{
    public class HealthComponent : IObjectComponent
    {
        #region Vars
        private int maxHealth;
        #endregion

        #region Events
        public event HealthComponentEventHandler OnDamaged;
        public event HealthComponentEventHandler OnHealed;
        public event HealthComponentEventHandler MaxChanged;
        #endregion

        #region Properties
        public int Health
        {
            get;
            private set;
        }
        public bool Alive
        {
            get
            {
                return Health > 0;
            }
        }
        #endregion

        public HealthComponent(int startHealth)
        {
            Health = startHealth;

            this.maxHealth = startHealth;
        }
        public HealthComponent(int startHealth, int maxHealth)
        {
            Health = startHealth;

            this.maxHealth = maxHealth;
        }

        #region Event callers
        private void LaunchOnHealed(int amount)
        {
            if (OnHealed != null)
            {
                OnHealed(this, new HealthComponentEventArgs(amount, wasHealed: true));
            }
        }
        private void LaunchOnDamaged(int amount)
        {
            if (OnDamaged != null)
            {
                OnDamaged(this, new HealthComponentEventArgs(amount, tookDamage: true));
            }
        }
        private void LaunchOnMaxChanged(int amount)
        {
            if (MaxChanged != null)
            {
                MaxChanged(this, new HealthComponentEventArgs(amount, maxChanged: true));
            }
        }
        #endregion

        public void IncreaseMaxHealth(int amount)
        {
            maxHealth += amount;

            LaunchOnMaxChanged(amount);
        }
        public void ReduceMaxHealth(int amount)
        {
            maxHealth -= maxHealth;

            LaunchOnMaxChanged(amount);
        }
        public void Heal(int amount)
        {
            Health = (int)MathHelper.Clamp(Health + amount, 0, maxHealth);

            LaunchOnHealed(amount);
        }
        public void TakeDamage(int amount)
        {
            Health -= amount;

            LaunchOnDamaged(amount);
        }
    }

    public delegate void HealthComponentEventHandler(object sender, HealthComponentEventArgs e);

    public class HealthComponentEventArgs : EventArgs
    {
        #region Properties
        public bool WasHealed
        {
            get;
            private set;
        }
        public bool MaxChanged
        {
            get;
            private set;
        }
        public bool TookDamage
        {
            get;
            private set;
        }
        public int Amount
        {
            get;
            private set;
        }
        #endregion

        public HealthComponentEventArgs(int amount, bool wasHealed = false, bool maxChanged = false, bool tookDamage = false)
        {
            WasHealed = wasHealed;
            MaxChanged = maxChanged;
            TookDamage = tookDamage;
            Amount = amount;
        }
    }
}
