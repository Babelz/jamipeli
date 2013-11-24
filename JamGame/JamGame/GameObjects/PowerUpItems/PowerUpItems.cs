using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using JamGame.DataTypes;
using JamGame.Entities;
using JamGame.Weapons;
using FarseerPhysics.Dynamics.Contacts;

// TODO: hovering animation
namespace JamGame.GameObjects.PowerUpItems
{
    public class DoubleDamagePowerUp : ObjectPowerUp<Player>
    {
        #region Vars
        protected readonly Random random;
        protected Texture2D texture;
        #endregion

        public DoubleDamagePowerUp()
            : base()
        {
            texture = Game.Instance.Content.Load<Texture2D>("damageup");
            random = new Random();

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        #region Event handlers
        private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Player player = fixtureB.Body.UserData as Player;
            if (player != null)
            {
                this.Apply(player);
            }

            return false;
        }
        #endregion

        public override void Apply(Player target)
        {
            if (!Used)
            {
                WeaponComponent weaponComponent = target.Components
                    .FirstOrDefault(c => c is WeaponComponent)
                    as WeaponComponent;

                weaponComponent.CurrentWeapon.AddPower(random.Next(5, 10));

                target.Animation.Scale += 0.03f;
                base.Apply(target);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Used)
            {
                Destory();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height);
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
    public class DoubleSpeedPowerUp : DoubleDamagePowerUp
    {
        public DoubleSpeedPowerUp()
            : base()
        {
            texture = Game.Instance.Content.Load<Texture2D>("speedup");
        }

        public override void Apply(Player target)
        {
            if (!Used)
            {
                target.Speed += 2.5f;
                Used = true;
            }
        }
    }
    public class IncreasedHpPowerUp : HealingPowerUp
    {
        public IncreasedHpPowerUp()
            : base()
        {
            texture = Game.Instance.Content.Load<Texture2D>("inchp");
        }
        public override void Apply(HealthComponent target)
        {
            if (!Used)
            {
                target.IncreaseMaxHealth(random.Next(25, 50));
                Used = true;
            }
        }
    }
    public class HealingPowerUp : ComponentPowerUp<HealthComponent>
    {
        #region Vars
        protected readonly Random random;

        protected Texture2D texture;
        private int alpha;
        #endregion

        public HealingPowerUp()
            : base()
        {
            texture = Game.Instance.Content.Load<Texture2D>("heal");
            random = new Random();

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            alpha = 255;
        }

        #region Event handlers
        private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Player player = fixtureB.Body.UserData as Player; 
            if (player != null)
            {
                HealthComponent healthComponent = player.Components
                    .FirstOrDefault(c => c is HealthComponent)
                    as HealthComponent;

                this.Apply(healthComponent);
            }
            return false;
        }
        #endregion

        public override void Apply(HealthComponent target)
        {
            if (!Used)
            {
                target.Heal(random.Next(25, 50));
                base.Apply(target);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Used)
            {
                alpha = Used ? alpha -= 5 : alpha;

                if (alpha <= 0)
                {
                    Destory();
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // TODO: efektien piirtäminen.

            Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height);
            spriteBatch.Draw(texture, rectangle, new Color(alpha, 0, 0, alpha));
        }
    }
}
