using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Monsters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using JamGame.Weapons;
using JamGame.Components;

namespace JamGame.GameObjects.Components
{
    public class WeaponComponent : IDrawableObjectComponent
    {
        #region Vars
        private Weapon currentWeapon;
        private readonly TargetingComponent<Monster> targetingComponent;
        private readonly List<EffectDrawer> effectDrawers;
        private readonly SpriteFont font;
        #endregion

        #region Properties
        public Weapon CurrentWeapon
        {
            get
            {
                return currentWeapon;
            }
        }
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

            font = Game.Instance.Content.Load<SpriteFont>("default");
        }

        private void AddStringDrawer(Vector2 startPosition, string stringToDraw)
        {
            int elapsed = 0;
            Vector2 position = startPosition;

            EffectDrawer effectDrawer = new EffectDrawer();

            effectDrawer.Update = (gameTime) =>
                {
                    if (elapsed > 500)
                    {
                        effectDrawer.Dispose();
                    } 
                    else 
                    {
                        elapsed += gameTime.ElapsedGameTime.Milliseconds;
                        position = new Vector2(position.X, position.Y - 1.25f);
                    }
                };
            effectDrawer.Draw = (spriteBatch) =>
                {
                    spriteBatch.DrawString(font, stringToDraw, position, Color.Red);
                };
            effectDrawer.Dispose = () =>
                {
                    effectDrawers.Remove(effectDrawer);
                };

            effectDrawers.Add(effectDrawer);
        }

        public void Attack()
        {
            if (targetingComponent.HasTarget && HasWeapon)
            {
                if (currentWeapon.CanMakeDamage())
                {
                    int damage = currentWeapon.CalculateDamage();
                    targetingComponent.TargetHealthComponent.TakeDamage(damage);
                    AddStringDrawer(new Vector2(targetingComponent.Target.Position.X, targetingComponent.Target.Position.Y), damage.ToString());
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            if (HasWeapon)
            {
                currentWeapon.Update(gameTime);
            }

            effectDrawers.ForEach(e => e.Update(gameTime));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (HasWeapon && currentWeapon.IsDrawing)
            {
                //currentWeapon.DrawEffects(SpriteBatch spriteBatch, Vector2 position, Vector2 area, int elapsedDrawTime);
            }

            effectDrawers.ForEach(e => e.Draw(spriteBatch));
        }
    }
}
