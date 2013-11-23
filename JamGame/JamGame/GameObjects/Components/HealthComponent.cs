using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamGame.GameObjects.Components
{
    public class HealthComponent : IObjectComponent
    {
        #region Vars
        private int maxHealth;
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

        public void IncreaseMaxHealth(int amount)
        {
            maxHealth += amount;
        }
        public void ReduceMaxHealth(int amount)
        {
            maxHealth -= maxHealth;
        }
        public void Heal(int amount)
        {
            Health += amount;
        }
        public void TakeDamage(int amount)
        {
            Health -= amount;
        }
    }
}
