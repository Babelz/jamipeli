using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using JamGame.Components;
using JamGame.GameObjects;
using JamGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JamGame.Maps;
using JamGame.GameObjects.Components;
using JamGame.DataTypes;
using JamGame.Weapons;
using JamGame.GameObjects.Monsters;

namespace JamGame.Entities
{
    public class Player : DrawableGameObject
    {
        #region Vars
        private InputControlSetup defaultSetup;
        private InputController controller;

        private DirectionalArrow directionalArrow;
        private WeaponComponent weaponComponent;
        private TargetingComponent<Monster> targetingComponent;

        private const float speed = 15f;
        #endregion

        public Player(World world)
        {
            Size = new Size(100, 100);
            Velocity = new Vector2(speed, 0);

            // Inputin alustus.
            defaultSetup = new InputControlSetup();
            controller = new InputController(Game.Instance.InputManager);
            controller.ChangeSetup(defaultSetup);

            // Colliderin alustus.
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(100), 1.0f);
            body.Friction = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0f;
            body.LinearDamping = 5f; 
            body.UserData = this;
            Position = Vector2.Zero;

            // Komponentti alustus.
            components.Add(targetingComponent = new TargetingComponent<Monster>(this));
            components.Add(directionalArrow = new DirectionalArrow());
            components.Add(weaponComponent = new WeaponComponent(targetingComponent, new BaseballBat()));
            components.Add(new HealthComponent(100));

            Game.Instance.MapManager.OnMapChanged += new MapManagerEventHandler(MapManager_OnMapChanged);

            Initialize();
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

        private void InitKeyMaps()
        {
            KeyInputBindProvider keymapper = defaultSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();

            keymapper.Map(new KeyTrigger("move left", Keys.A), (triggered, args) =>
                {
                    body.ApplyForce(Velocity = new Vector2(-speed, 0));
                });

            keymapper.Map(new KeyTrigger("move right", Keys.D), (triggered, args) =>
                {
                    body.ApplyForce(Velocity = new Vector2(speed, 0));
                });

            keymapper.Map(new KeyTrigger("move up", Keys.W), (triggered, args) =>
                {
                    body.ApplyForce(new Vector2(0, -speed));
                });

            keymapper.Map(new KeyTrigger("move down", Keys.S), (triggered, args) =>
                {
                    body.ApplyForce(new Vector2(0, speed));
                });

            keymapper.Map(new KeyTrigger("attck", Keys.Space), (triggered, args) =>
                {
                    if (args.State == InputState.Down)
                    {
                        weaponComponent.Attack();
                    }
                });
        }
        private void InitPadMaps()
        {
            var padmapper = defaultSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("move left", Buttons.LeftThumbstickLeft), (triggered, args) =>
            {
                body.ApplyForce(new Vector2(-speed, 0));
            });
            padmapper.Map(new ButtonTrigger("move right", Buttons.LeftThumbstickRight), (triggered, args) => body.ApplyForce(new Vector2(speed, 0)));
            padmapper.Map(new ButtonTrigger("move up", Buttons.LeftThumbstickUp), (triggered, args) => body.ApplyForce(new Vector2(0, -speed)));
            padmapper.Map(new ButtonTrigger("move down", Buttons.LeftThumbstickDown), (triggered, args) => body.ApplyForce(new Vector2(0, speed)));
        }
        private Monster FindNearestMonster()
        {
            GameObject nearest = Game.Instance.GameObjects
                .Where(o => o as Monster != null)
                    .FirstOrDefault(o => Vector2.Distance(o.Position, Position) == Game.Instance.GameObjects
                        .Min(a => Vector2.Distance(a.Position, Position)));

            return nearest as Monster;
        }

        public void Initialize()
        {
            InitKeyMaps();
            InitPadMaps();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Haetaan lähin monsteri.
            Monster nearest = FindNearestMonster();

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
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Game.Instance.Temp, new Rectangle((int)Position.X, (int)Position.Y, 100, 100), null, Color.Black, 0f, new Vector2(0.5f, 0.5f),SpriteEffects.None,0f );
        }
    }
}
