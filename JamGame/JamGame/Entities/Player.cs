using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrashMonkeySpriter;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using JamGame.Components;
using JamGame.GameObjects;
using JamGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JamGame.Maps;
using JamGame.GameObjects.Components;
using JamGame.DataTypes;
using JamGame.Weapons;
using JamGame.GameObjects.Monsters;
using JamGame.Extensions;

namespace JamGame.Entities
{
    public abstract class Player : DrawableGameObject
    {
        #region Vars
        protected InputControlSetup defaultSetup;
        protected InputController controller;
        protected WeaponComponent weaponComponent;

        private DirectionalArrow directionalArrow;
        private CharaterAnimator animator;
        private TargetingComponent<Monster> targetingComponent;

        //protected SoundEffect weaponSound;
        #endregion

        #region Properties
        public CharaterAnimator Animation
        {
            get
            {
                return animator;
            }
        }
        public float Speed
        {
            get;
            set;
        }
        #endregion

        public Player(World world)
        {
            Size = new Size(100, 100);
            Velocity = new Vector2(Speed, 0);

            // Inputin alustus.
            defaultSetup = new InputControlSetup();
            controller = new InputController(Game.Instance.InputManager);
            controller.ChangeSetup(defaultSetup);

            Game.Instance.MapManager.OnMapChanged += new MapManagerEventHandler(MapManager_OnMapChanged);
            animator = Game.Instance.Content.Load<CharacterModel>("playeri\\plaery").CreateAnimator("player");
            animator.ChangeAnimation("attack");
            animator.Scale = 0.35f;

            // Colliderin alustus.
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(100), 1.0f);
            body.Friction = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0f;
            body.LinearDamping = 5f; 
            body.UserData = this;
            Position = Vector2.Zero;
            Size = new Size(100, 100);

            HealthComponent health = new HealthComponent(100);
            // Komponentti alustus.
            components.Add(targetingComponent = new TargetingComponent<Monster>(this));
            components.Add(directionalArrow = new DirectionalArrow());
            components.Add(weaponComponent = new WeaponComponent(targetingComponent, new BaseballBat()));
            components.Add(health);

            Game.Instance.MapManager.OnMapChanged += new MapManagerEventHandler(MapManager_OnMapChanged);
           // weaponSound = Game.Instance.Content.Load<SoundEffect>("music\\baseballbat");
            Speed = 15f;


            
        }

        #region Event handlers
        private void MapManager_OnMapChanged(object sender, MapManagerEventArgs e)
        {
            // Jos kartta vaihtuu, sallitaan suuntanuolen piirto.
            Game.Instance.MapManager.Active.StateManager.OnStateFinished += new MapStateManagerEventHandle(StateManager_OnStateFinished);
            
            // Jos transition alkaa, lopetetaan suuntanuolen piirtäminen.
            Game.Instance.MapManager.Active.StateManager.OnTransitionStart += new MapStateManagerEventHandle(StateManager_OnTransitionStart);
        }
        private void StateManager_OnTransitionStart(object sender, MapStateManagerEventArgs e)
        {
            directionalArrow.Enabled = false;
        }
        private void StateManager_OnStateFinished(object sender, MapStateManagerEventArgs e)
        {
            directionalArrow.Enabled = true;
        }
        #endregion

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Game.Instance.World.RemoveBody(body);
        } 

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            animator.Location = Position + new Vector2(0, 256 * animator.Scale / 2);
            animator.Update(gameTime);

            // Haetaan lähin monsteri.
            Monster nearest = Game.Instance.GameObjects.FindNearest<Monster>(Position) as Monster;

            if (nearest != null)
            {
                targetingComponent.ChangeTarget(nearest);
            }

            // Jos monsteri on jo kuollut ja se on vieläkin targettina, clearitaan target.
            if (!Game.Instance.ContainsGameObject(nearest))
            {
                nearest = null;
                targetingComponent.ClearTarget();
            }

            animator.FlipX = Velocity.X < 0;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            animator.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
