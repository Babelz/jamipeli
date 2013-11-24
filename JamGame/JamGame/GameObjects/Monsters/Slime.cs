using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrashMonkeySpriter;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using JamGame.Gamestate;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using Microsoft.Xna.Framework.Graphics;
using JamGame.GameObjects.Components;
using JamGame.Entities;
using JamGame.DataTypes;
using FarseerPhysics.Dynamics.Contacts;

namespace JamGame.GameObjects.Monsters
{
    public class Slime : Monster
    {
        #region Vars
        private Spit spit;
        #endregion

        public Slime()
        {
            Animation = Game.Instance.Content.Load<CharacterModel>("monsters\\blob").CreateAnimator("blob");

            Animation.Scale = 0.60f;

            body = BodyFactory.CreateRectangle(Game.Instance.World,
                ConvertUnits.ToSimUnits(256 * Animation.Scale),
                ConvertUnits.ToSimUnits(256 * Animation.Scale), 1f, this);

            body.Friction = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0f;
            body.LinearDamping = 5f;
            body.Mass = 1f;
            Position = Vector2.Zero;

            GameplayState gameplayState = Game.Instance.GameStateManager.States
                 .FirstOrDefault(s => s is GameplayState)
                 as GameplayState;

            body.IgnoreCollisionWith(gameplayState.RightWall.Body);
            body.IgnoreCollisionWith(gameplayState.LeftWall.Body);
            body.UserData = this;

            components.Add(Health = new HealthComponent(random.Next(250, 500)));

            brain.PushState(MoveToArea);
        }

        #region Event hanlders
        private void spit_OnDestroyed(object sender, GameObjectEventArgs e)
        {
            spit = null;
        }
        private void Animation_Bite_AnimationEnded()
        {
            targetComponent.TargetHealthComponent.TakeDamage(random.Next(5, 8));

            Animation.ChangeAnimation("move");

            Animation.AnimationEnded -= Animation_Bite_AnimationEnded;
        }
        private void Animation_Spit_AnimationEnded()
        {
            spit = new Spit(targetComponent.Target, Position);
            spit.OnDestroyed += new GameObjectEventHandler(spit_OnDestroyed);

            Animation.ChangeAnimation("move");

            Animation.AnimationEnded -= Animation_Spit_AnimationEnded;
        }
        #endregion

        #region Brain states
        private void Charge()
        {
            if (timerWrapper["charge"] < 950)
            {
                MoveToTarget();
            }
            else
            {
                timerWrapper.RemoveTimer("charge");

                brain.PopState();

                if (!brain.HasStates)
                {
                    brain.PushState(GetTarget);
                }
            }
        }
        protected override void GetTarget()
        {
            base.GetTarget();

            if (targetComponent.HasTarget)
            {
                Console.WriteLine(Vector2.Distance(Position, targetComponent.Target.Position));

                float dist = Vector2.Distance(Position, targetComponent.Target.Position);

                brain.PushState(Charge);
                if (dist > 120)
                {
                    brain.PushState(SpitAttack);
                }
                else
                {
                    brain.PushState(BiteAttack);
                }
            }
        }
        private void BiteAttack()
        {
            Console.WriteLine("Bite paska");
            Animation.ChangeAnimation("attack");
            Animation.AnimationEnded += new CharaterAnimator.AnimationEndedHandler(Animation_Bite_AnimationEnded);

            // TODO: hajoaa tässä käsiin koska aika loppuu.
            while (brain.HasStates)
            {
                brain.PopState();
            }

            brain.PushState(Charge);
        }
        private void SpitAttack()
        {
            Console.WriteLine("Spit paska");
            Animation.ChangeAnimation("attack");
            Animation.AnimationEnded += new CharaterAnimator.AnimationEndedHandler(Animation_Spit_AnimationEnded);

            // TODO: hajoaa tässä käsiin koska aika loppuu.
            while (brain.HasStates)
            {
                brain.PopState();
            }

            brain.PushState(Charge);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (spit != null)
            {
                spit.Update(gameTime);
            }

            Animation.Location = new Vector2(Position.X, Position.Y + (256 * Animation.Scale / 2));
            Animation.Update(gameTime);

            if (!Health.Alive)
            {
                Destory();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (spit != null)
            {
                spit.Draw(spriteBatch);
            }

            if (targetComponent.HasTarget)
            {
                Animation.FlipX = targetComponent.VelocityToTarget.X > 0;
            }

            Animation.Draw(spriteBatch);
        }

        #region Spit private class
        private class Spit : DrawableGameObject
        {
            #region Vars
            private bool hitted;
            private readonly Random random;
            private readonly Texture2D texture;
            private Vector2 direction;

            private int elapsed;
            private TargetingComponent<Player> targetinComponent;
            #endregion

            public Spit(Player target, Vector2 position)
            {
                Size = new Size(64, 32);
                random = new Random();
                texture = Game.Instance.Content.Load<Texture2D>("spit");
                body = BodyFactory.CreateRectangle(Game.Instance.World,
                     ConvertUnits.ToSimUnits(texture.Height),
                     ConvertUnits.ToSimUnits(texture.Width), 1f, this);

                body.Friction = 0f;
                body.BodyType = BodyType.Dynamic;
                body.Restitution = 0f;
                body.LinearDamping = 5f;
                body.Mass = 1f;
                Position = new Vector2(position.X, position.Y + Size.Height);


                body.OnCollision += new OnCollisionEventHandler(body_OnCollision);

                targetinComponent = new TargetingComponent<Player>(this);
                targetinComponent.ChangeTarget(target);

                direction = new Vector2(targetinComponent.VelocityToTarget.X * 25, 0);
            }

            #region Event handlers
            private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
            {
                if (hitted)
                {
                    return false;
                }

                Player player = fixtureB.Body.UserData as Player;

                if (player != null)
                {
                    targetinComponent.TargetHealthComponent.TakeDamage(random.Next(15, 25));
                    hitted = true;
                }

                return false;
            }
            #endregion

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > 1250 || hitted)
                {
                    Destory();
                }
                else
                {
                    body.ApplyForce(direction);
                }
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                base.Draw(spriteBatch);
                spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height), Color.White);
            }
        }
        #endregion
    }
}
