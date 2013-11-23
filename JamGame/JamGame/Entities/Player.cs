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

namespace JamGame.Entities
{
    public class Player : DrawableGameObject
    {
        #region Vars
        private MotionEngine motionEngine;
        private InputControlSetup defaultSetup;
        private InputController controller;
        private DirectionalArrow directionalArrow;

        private float speed = 15f;
        private Body body;
        #endregion

        public Player(World world)
        {
            motionEngine = new MotionEngine(this);
            defaultSetup = new InputControlSetup();
            controller = new InputController(Game.Instance.InputManager);
            controller.ChangeSetup(defaultSetup);
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(100), 1.0f);
            body.Mass = 0.1f;
            body.Friction = 1f;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0f;
            body.LinearDamping = 5f;
            Position = Vector2.Zero;
            
            
            
            Initialize();

            Game.Instance.MapManager.OnMapChanged += new MapManagerEventHandler(MapManager_OnMapChanged);
        }

        #region Event handlers
        private void MapManager_OnMapChanged(object sender, MapManagerEventArgs e)
        {
            e.Next.StateManager.OnStateFinished += new MapStateManagerEventHandle(StateManager_OnStateFinished);
            //Console.WriteLine("No ainaki kartta vaihtuu?");
        }
        public void StateManager_OnStateFinished(object sender, MapStateManagerEventArgs e)
        {
            //Console.WriteLine("HAISTA VITTU HAISTA VITTU");
            
        }
        #endregion

        public override Vector2 Position
        {
            get
            {
                return new Vector2(
                    ConvertUnits.ToDisplayUnits(body.Position.X),
                    ConvertUnits.ToDisplayUnits(body.Position.Y)
                    );
            }
            set
            {
                body.Position = new Vector2(
                    ConvertUnits.ToSimUnits(value.X),
                    ConvertUnits.ToSimUnits(value.Y)
                    );
            }
        }

        public void Initialize()
        {
            InitKeyMaps();
            InitPadMaps();
        }

        private void InitKeyMaps()
        {
            var keymapper = defaultSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keymapper.Map(new KeyTrigger("move left", Keys.A),
                (triggered, args) =>
                    body.ApplyForce(new Vector2(-speed, 0)));
            keymapper.Map(new KeyTrigger("move right", Keys.D),
                (triggered, args) => body.ApplyForce(new Vector2(speed, 0)));
            keymapper.Map(new KeyTrigger("move up", Keys.W),
                (triggered, args) => body.ApplyForce(new Vector2(0, -speed)));
            keymapper.Map(new KeyTrigger("move down", Keys.S),
                (triggered, args) => body.ApplyForce(new Vector2(0, speed)));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            motionEngine.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game.Instance.Temp, new Rectangle((int) Position.X,
                (int) Position.Y, 100, 100), Color.Black);
        }

        private void InitPadMaps()
        {
            var padmapper = defaultSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("move left", Buttons.LeftThumbstickLeft), (triggered, args) => body.ApplyForce(new Vector2(-speed, 0)));
            padmapper.Map(new ButtonTrigger("move right", Buttons.LeftThumbstickRight), (triggered, args) => body.ApplyForce(new Vector2(speed, 0)));
            padmapper.Map(new ButtonTrigger("move up", Buttons.LeftThumbstickUp), (triggered, args) => body.ApplyForce(new Vector2(0, -speed)));
            padmapper.Map(new ButtonTrigger("move down", Buttons.LeftThumbstickDown), (triggered, args) => body.ApplyForce(new Vector2(0, speed)));

        }
    }
}
