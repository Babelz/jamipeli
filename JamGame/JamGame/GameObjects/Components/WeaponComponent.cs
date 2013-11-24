using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Monsters;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using JamGame.Weapons;
using JamGame.Components;

namespace JamGame.GameObjects.Components
{
    public class WeaponComponent : IDrawableObjectComponent
    {
        #region Vars
        private readonly TargetingComponent<Monster> targetingComponent;
        private readonly List<EffectDrawer> effectDrawers;
        private readonly SpriteFont spriteFont;

        private Weapon currentWeapon;
        private SoundEffect weaponSound;
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
            spriteFont = Game.Instance.Content.Load<SpriteFont>("default");
            weaponSound = Game.Instance.Content.Load<SoundEffect>("music\\baseballbat");
        }

        private void AddStringDrawer(Vector2 startPosition, string stringToDraw)
        {
            int elapsed = 0;
            bool isScrit = false;
            
            Random random = null;
            Vector2 position = startPosition;
            EffectDrawer effectDrawer = new EffectDrawer();

            if (currentWeapon.IsCrit(int.Parse(stringToDraw)))
            {
                random = new Random();
                isScrit = true;
            }

            effectDrawer.Update = (gameTime) =>
                {
                    if (elapsed > 850)
                    {
                        effectDrawer.Dispose();
                    }
                    else
                    {
                        elapsed += gameTime.ElapsedGameTime.Milliseconds;
                        position = new Vector2(position.X, position.Y - 1.25f);

                        if (isScrit)
                        {
                            position = new Vector2(Math.Abs(position.X + (float)random.NextDouble() * 5), position.Y - Math.Abs((float)random.NextDouble() * 5));
                        }
                    }
                };
            effectDrawer.Draw = (spriteBatch) =>
                {
                    if (isScrit)
                    {
                        bool negate = random.NextDouble() >= 0.5;
                        float value = MathHelper.Clamp((float)random.NextDouble(), 0.0f, 0.15f);
                        float rotation = (negate ? -value : value);

                        spriteBatch.DrawString(spriteFont, stringToDraw, position, Color.Red, rotation, spriteFont.MeasureString(stringToDraw) / 2, 2.0f, SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        spriteBatch.DrawString(spriteFont, stringToDraw, position, Color.Red);
                    }
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
                    weaponSound.Play(0.5f, 0.5f, 0.5f);
                    int damage = currentWeapon.CalculateDamage();
                    targetingComponent.TargetHealthComponent.TakeDamage(damage);
                    AddStringDrawer(new Vector2(targetingComponent.Target.Position.X, targetingComponent.Target.Position.Y - targetingComponent.Target.Animation.Scale * 256), damage.ToString());
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
                // TODO: pitäs piirtää weapon effectit.
            }

            effectDrawers.ForEach(e => e.Draw(spriteBatch));
        }
    }
}
