using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamGame.GameObjects.Components
{
    public class HealthComponent : IObjectComponent
    {
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
                return Health != 0;
            }
        }
        #endregion

        public HealthComponent(int startHealth)
        {
            Health = startHealth;
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
