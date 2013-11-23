using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.GameObjects.Components
{
    public class AttackComponent
    {
                #region Vars
        private Weapon currentWeapon;
        private readonly TargetingComponent<Monster> targetingComponent;
        private readonly List<EffectDrawer> effectDrawers;
        private readonly TimerWrapper effectTimers;
        private readonly SpriteFont font;
        #endregion

        #region Properties
        public bool HasWeapon
        {
            get
            {
                return currentWeapon != null;
            }
        }
        #endregion

        public WeaponComponent(TargetingComponent<Monster> targetingComponent, Weapon startWeapon)
        {
            this.targetingComponent = targetingComponent;
 
            currentWeapon = startWeapon;

            effectDrawers = new List<EffectDrawer>();
            effectTimers = new TimerWrapper();

            font = Game.Instance.Content.Load<SpriteFont>("default");
        }

        private void AddStringDrawer(Vector2 startPosition, string stringToDraw)
        {
            string timerkey = "timer" + effectDrawers.Count;
            effectTimers.AddTimer(timerkey, 0);

            EffectDrawer effectDrawer = new EffectDrawer();
            Vector2 position = startPosition;

            effectDrawer.Update = (gameTime) =>
                {
                    if (effectTimers[timerkey] > 1500)
                    {
                        effectDrawer.Dispose();
                    } 
                    else 
                    {
                        position = new Vector2(position.X, position.Y -= 0.25f);
                    }
                };
            effectDrawer.Draw = (spriteBatch) =>
                {
                    spriteBatch.DrawString(font, stringToDraw, position, Color.Red);
                };
            effectDrawer.Dispose = () =>
                {
                    effectDrawers.Remove(effectDrawer);
                    effectTimers.RemoveTimer(timerkey);
                };
        }

        public void Attack()
        {
            if (targetingComponent.HasTarget && HasWeapon)
            {
                if (currentWeapon.CanMakeDamage())
                {
                    int damage = currentWeapon.CalculateDamage();
                    targetingComponent.TargetHealthComponent.TakeDamage(damage);
                    AddStringDrawer(new Vector2(targetingComponent.Target.Position.X / 2, targetingComponent.Target.Position.Y), damage.ToString());
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            if (HasWeapon)
            {
                currentWeapon.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (HasWeapon && currentWeapon.IsDrawing)
            {
                currentWeapon.DrawEffects(spriteBatch, targetingComponent.Target.Position);
            }
        }
    }
}
