using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrashMonkeySpriter;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using JamGame.Gamestate;
using JamGame.GameObjects.Components;
using JamGame.Entities;
using FarseerPhysics.Dynamics.Contacts;
using JamGame.Extensions;

namespace JamGame.GameObjects.Monsters
{
    public class Shoemon : Monster
    {
        public Shoemon()
            : base()
        {
            Animation = Game.Instance.Content.Load<CharacterModel>("monsters\\shoemon").CreateAnimator("Shoemon");
           // Animation.ChangeAnimation("attack");
            Animation.Scale = 0.27f;

            body = BodyFactory.CreateRectangle(Game.Instance.World, 
                ConvertUnits.ToSimUnits(256*Animation.Scale), 
                ConvertUnits.ToSimUnits(256*Animation.Scale), 1f, this);

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
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.UserData = this;

            components.Add(Health = new HealthComponent(50, 75));

            brain.PushState(MoveToArea);
        }

        #region Event handlers
        private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            bool results = false;

            if (fixtureB.Body.UserData as Player != null)
            {
                brain.PushState(RunAway);
                brain.PushState(Attack);
                results = true;
            }
            else
            {
                results = (fixtureB.Body.UserData as Monster != null);

                if (!results)
                {
                    results = (fixtureB.Body.UserData as Wall != null);
                }
            }

            return results;
        }
        private void Animation_AnimationEnded()
        {
            Animation.AnimationEnded -= Animation_AnimationEnded;
            Animation.ChangeAnimation("Move");
        }
        #endregion

        #region Brain states
        protected override void GetTarget()
        {
            base.GetTarget();

            brain.PushState(MoveToTarget);
        }
        private void RunAway()
        {
            if (timerWrapper["runawaytimer"] < 1500)
            {
                Vector2 velocity = -(targetComponent.VelocityToTarget * 5); 
                body.ApplyForce(velocity);
                Animation.FlipX = velocity.X > 0;
            }
            else
            {
                timerWrapper.RemoveTimer("runawaytimer");
                brain.PopState();
            }
        }
        private void Attack()
        {
            brain.PopState();

            Animation.ChangeAnimation("attack");
            Animation.AnimationEnded += new CharaterAnimator.AnimationEndedHandler(Animation_AnimationEnded);
            
            if (targetComponent.HasTarget)
            {
                targetComponent.TargetHealthComponent.TakeDamage(random.Next(2, 4));
            }
            else
            {
                brain.PushState(GetTarget);
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

            if (targetComponent.HasTarget)
            {
                Animation.FlipX = targetComponent.VelocityToTarget.X > 0;
            }

            Animation.Draw(spriteBatch);
        }
    }
}
