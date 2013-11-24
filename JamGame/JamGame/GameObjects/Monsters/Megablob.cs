using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Factories;
using JamGame.Gamestate;
using JamGame.GameObjects.Components;
using FarseerPhysics.Dynamics;
using BrashMonkeySpriter;
using JamGame.DataTypes;

namespace JamGame.GameObjects.Monsters
{
    public class Megablob : Monster
    {
        #region Vars
        private Vector2 oldPositon;
        private Vector2 idlePosition;
        private List<BossSpit> spits;

        private static SoundEffect idleSound;
        private static SoundEffectInstance idleSoundInstance;

        private static SoundEffect randomHampaat;
        private static SoundEffectInstance randomHampaatInstance;
        #endregion

        #region Properties

        #endregion 

        public Megablob()
            : base()
        {
            Animation = Game.Instance.Content.Load<CharacterModel>("monsters\\bossi").CreateAnimator("boss");

            Animation.Scale = 0.75f;

            body = BodyFactory.CreateRectangle(Game.Instance.World,
                ConvertUnits.ToSimUnits(256 * Animation.Scale / 2),
                ConvertUnits.ToSimUnits(256 * Animation.Scale / 2), 1f, this);

            body.Friction = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0f;
            body.LinearDamping = 5f;
            body.Mass = 2.5f;
            Position = Vector2.Zero;

            GameplayState gameplayState = Game.Instance.GameStateManager.States
                 .FirstOrDefault(s => s is GameplayState)
                 as GameplayState;

            body.IgnoreCollisionWith(gameplayState.RightWall.Body);
            body.IgnoreCollisionWith(gameplayState.LeftWall.Body);
            body.UserData = this;

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);

            components.Add(Health = new HealthComponent(3500));

            brain.PushState(MoveToArea);

            timerWrapper.AddTimer("player", 0);

            spits = new List<BossSpit>();

            if (idleSound == null && idleSoundInstance == null)
            {
                idleSound = Game.Instance.Content.Load<SoundEffect>("music\\weird_monster");
                idleSoundInstance = idleSound.CreateInstance();
            }

            if (randomHampaat == null && randomHampaatInstance == null)
            {
                randomHampaat = Game.Instance.Content.Load<SoundEffect>("music\\randomhampaat");
                randomHampaatInstance = randomHampaat.CreateInstance();
            }
        }

        #region Event handlers
        private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Player player = fixtureB.Body.UserData as Player;

            if (player != null)
            {
                if (timerWrapper["player"] > 500)
                {
                    Console.WriteLine("Collisionista damaa...");
                    HealthComponent healthComponent = player.Components
                        .FirstOrDefault(c => c is HealthComponent)
                        as HealthComponent;

                    healthComponent.TakeDamage(random.Next(5, 15));

                    timerWrapper.ResetTimer("player");

                    if (brain.CurrentState == Charge)
                    {
                        brain.PopState();

                        brain.PushState(GoBack);
                        brain.PushState(GetTarget);
                    }
                }
            }

            return true;
        }
        private void spit_OnDestroyed(object sender, GameObjectEventArgs e)
        {
            spits.Remove(sender as BossSpit);
        }
        #endregion

        #region Brain states
        private void Idle()
        {
            if (timerWrapper["idle"] > 2500)
            {
                brain.PopState();

                brain.PushState(Charge);
                brain.PushState(GetTarget);

                oldPositon = Position;

                timerWrapper.RemoveTimer("idle");
            }
        }
        private void Charge()
        {
            if (Position.X > 100)
            {
                body.ApplyForce(new Vector2(-35, targetComponent.VelocityToTarget.Y * 8));
            }
            else
            {
                brain.PopState();

                brain.PushState(GoBack);
                brain.PushState(GetTarget);
            }
        }
        private void GoBack()
        {
            if(Position.X < idlePosition.X)
            {
                body.ApplyForce(new Vector2(35, 0));
            } 
            else 
            {
                brain.PopState();
                brain.PopState();

                brain.PushState(MoveAndShoot);
            }
        }
        private void MoveAndShoot()
        {
            if (timerWrapper["moveandshoot"] < 12500)
            {
                if (timerWrapper["spitspawn"] > 500)
                {
                    body.ApplyForce(new Vector2(0, targetComponent.VelocityToTarget.Y * 20));


                    BossSpit spit = new BossSpit(targetComponent.Target, Position);
                    spit.OnDestroyed += new GameObjectEventHandler(spit_OnDestroyed);
                    spits.Add(spit);

                    timerWrapper.ResetTimer("spitspawn");

                    brain.PushState(GetTarget);
                } 
            }
            else
            {
                timerWrapper.ResetTimer("moveandshoot");

                brain.PopState();
                idleSoundInstance.Play();
                brain.PushState(Idle);
                
            }
            // TODO: liiku ja ammu
            Console.WriteLine("liiku ja ammu");

            body.ApplyForce(new Vector2(0, targetComponent.VelocityToTarget.Y * 12));
        }

        protected override void MoveToArea()
        {
            if (Position.X > 1210 - (256 * Animation.Scale) / 2)
            {
                body.ApplyForce(new Vector2(-25f, 0.0f));
            }
            else
            {
                idlePosition = Position;

                brain.PopState();
                brain.PushState(Idle);
                brain.PushState(GetTarget);
            }
        }
        private void GoHome()
        {
            body.ApplyForce(new Vector2(12, -12));
            Animation.FlipX = true;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            while (brain.CurrentState == MoveToTarget)
            {
                brain.PopState();
            }

            Animation.Location = new Vector2(Position.X , Position.Y + (Animation.Scale * 256) / 2);
            Animation.Update(gameTime);

            spits.ForEach(p => p.Update(gameTime));

            GameplayState gameplay = Game.Instance.GameStateManager.States
                .FirstOrDefault(s => s is GameplayState)
                as GameplayState;

            if (gameplay.Players.Length == 0 && brain.CurrentState != GoHome)
            {
                while (brain.HasStates)
                {
                    brain.PopState();
                }

                brain.PushState(GoHome);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spits.ForEach(p => p.Draw(spriteBatch));

            if (targetComponent.HasTarget)
            {
                Animation.FlipX = targetComponent.VelocityToTarget.X > 0;
            }

            Animation.Draw(spriteBatch);
        }
    }
    public class BossSpit : JamGame.GameObjects.Monsters.Slime.Spit
    {
        public BossSpit(Player target, Vector2 position)
            : base(target, position)
        {
            aliveTime = 8500;
        }

        #region Event handlers
        protected override bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (hitted)
            {
                return false;
            }

            Player player = fixtureB.Body.UserData as Player;

            if (player != null)
            {
                HealthComponent healthComponent = player.Components
                    .FirstOrDefault(c => c is HealthComponent)
                    as HealthComponent;

                healthComponent.TakeDamage(random.Next(15, 25));

                hitted = true;
            }

            return false;
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height), Color.Green);
        }
    }
}
