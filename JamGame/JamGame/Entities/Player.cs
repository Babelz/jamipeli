using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.Components;
using JamGame.GameObjects;
using JamGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JamGame.Entities
{
    public class Player : DrawableGameObject
    {
        private MotionEngine motionEngine;
        private InputControlSetup defaultSetup;
        private InputController controller;
        private float speed = 5f;

        public Player()
        {
            motionEngine = new MotionEngine(this);
            defaultSetup = new InputControlSetup();
            controller = new InputController(Game.Instance.InputManager);
            controller.ChangeSetup(defaultSetup);
            Initialize();
        }

        public void Initialize()
        {
            InitKeyMaps();
            InitPadMaps();
        }

        private void InitKeyMaps()
        {
            var keymapper = defaultSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keymapper.Map(new KeyTrigger("move left", Keys.A),(triggered, args) =>  motionEngine.GoalVelocityX = -VelocityFunc(args, speed));
            keymapper.Map(new KeyTrigger("move right", Keys.D), (triggered, args) => motionEngine.GoalVelocityX = VelocityFunc(args, speed));
            keymapper.Map(new KeyTrigger("move up", Keys.W), (triggered, args) => motionEngine.GoalVelocityY = -VelocityFunc(args, speed));
            keymapper.Map(new KeyTrigger("move down", Keys.S), (triggered, args) => motionEngine.GoalVelocityY = VelocityFunc(args, speed));
        }

        private float VelocityFunc(InputEventArgs args, float src)
        {
            if (args.State == InputState.Released)
            {
                return 0;
            }
            return src;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            motionEngine.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Game.Instance.Temp, new Rectangle((int) Position.X, (int) Position.Y, 100,100), Color.Black );
        }

        private void InitPadMaps()
        {
            var padmapper = defaultSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("move left", Buttons.LeftThumbstickLeft), (triggered, args) => motionEngine.GoalVelocityX = -VelocityFunc(args, speed));
            padmapper.Map(new ButtonTrigger("move right", Buttons.LeftThumbstickRight), (triggered, args) => motionEngine.GoalVelocityX = VelocityFunc(args, speed));
            padmapper.Map(new ButtonTrigger("move up", Buttons.LeftThumbstickUp), (triggered, args) => motionEngine.GoalVelocityY = -VelocityFunc(args, speed));
            padmapper.Map(new ButtonTrigger("move down", Buttons.LeftThumbstickDown), (triggered, args) => motionEngine.GoalVelocityY = VelocityFunc(args, speed));

        }
    }
}
