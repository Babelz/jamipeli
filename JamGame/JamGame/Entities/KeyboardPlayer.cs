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
    internal class KeyboardPlayer : Player
    {
        public KeyboardPlayer(World world) : base(world)
        {
            InitKeyMaps();
        }

        private void InitKeyMaps()
        {
            var keymapper = defaultSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keymapper.Map(new KeyTrigger("move left", Keys.A),
                (triggered, args) =>
                {
                    body.ApplyForce(Velocity = new Vector2(-Speed, 0));
                });
            keymapper.Map(new KeyTrigger("move right", Keys.D),
                (triggered, args) =>
                {
                    body.ApplyForce(Velocity = new Vector2(Speed, 0));
                });
            keymapper.Map(new KeyTrigger("move up", Keys.W),
                (triggered, args) => body.ApplyForce(new Vector2(0, -Speed)));
            keymapper.Map(new KeyTrigger("move down", Keys.S),
                (triggered, args) => body.ApplyForce(new Vector2(0, Speed)));
            keymapper.Map(new KeyTrigger("attck", Keys.Space),
                (triggered, args) =>
                {
                    if (args.State == InputState.Down)
                    {
                        weaponComponent.Attack();
                        
                    }
                });
        }
    }
}
