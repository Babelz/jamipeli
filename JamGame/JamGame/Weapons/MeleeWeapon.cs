using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Weapons
{
    public abstract class MeleeWeapon : Weapon
    {
        #region Vars
        protected readonly Random random;
        protected readonly int swingTime;
        protected readonly int drawTime;
        private bool isInCoolDown;
        private int elapsed;
        private int elapsedDrawTime;
        #endregion

        public MeleeWeapon(string name, int minDamage, int maxDamage, int critChance, int swingTime, int drawTime)
            : base(name, minDamage, maxDamage, critChance)
        {
            this.swingTime = swingTime;
            this.drawTime = drawTime;

            random = new Random();
        }

        protected abstract void OnDrawEffects(SpriteBatch spriteBatch, Vector2 position, Vector2 area, int elapsedDrawTime);

        public override bool CanMakeDamage()
        {
            return elapsed == 0 && !isInCoolDown;
        }
        public override void Update(GameTime gameTime)
        {
            if (isInCoolDown)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsed > swingTime)
                {
                    isInCoolDown = false;
                    elapsed = 0;
                }
            }

            if (IsDrawing)
            {
                elapsedDrawTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedDrawTime > drawTime)
                {
                    IsDrawing = false;
                    elapsedDrawTime = 0;
                }
            }
        }
        public override void DrawEffects(SpriteBatch spriteBatch, Vector2 position, Vector2 area, int elapsedDrawTime)
        {
            if (IsDrawing)
            {
                OnDrawEffects(spriteBatch, position, area, elapsedDrawTime);
            }
        }
    }
}
