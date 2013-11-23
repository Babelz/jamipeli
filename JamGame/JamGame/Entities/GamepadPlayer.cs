using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using JamGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JamGame.Entities
{
    class GamepadPlayer : Player
    {
        public PlayerIndex PlayerIndex { get; private set; }
        public GamepadPlayer(World world, PlayerIndex playerIndex) : base(world)
        {
            PlayerIndex = playerIndex;
            InitPadMaps();
        }

        private void InitPadMaps()
        {
            var padmapper = defaultSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("move left", Buttons.LeftThumbstickLeft), (triggered, args) =>
            {
                if (args.PlayerIndex != args.PlayerIndex) return;
                body.ApplyForce(new Vector2(-speed, 0));
            });
            padmapper.Map(new ButtonTrigger("move right", Buttons.LeftThumbstickRight), (triggered, args) =>
            {
                if (args.PlayerIndex != args.PlayerIndex) return;
                body.ApplyForce(new Vector2(speed, 0));
            });
            padmapper.Map(new ButtonTrigger("move up", Buttons.LeftThumbstickUp), (triggered, args) =>
            {
                if (args.PlayerIndex != args.PlayerIndex) return;
                body.ApplyForce(new Vector2(0, -speed));
            });
            padmapper.Map(new ButtonTrigger("move down", Buttons.LeftThumbstickDown), (triggered, args) =>
            {
                if (args.PlayerIndex != args.PlayerIndex) return;
                body.ApplyForce(new Vector2(0, speed));
            });

        }
    }
}
