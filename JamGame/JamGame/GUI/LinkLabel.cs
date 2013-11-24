using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JamGame.GUI
{
    public class LinkLabel : Control
    {
        public Color SelectedColor
        {
            get;
            set;
        }

        public LinkLabel()
        {
            TabStop = true;
            HasFocus = false;
            Position = Vector2.Zero;
            SelectedColor = Color.Red;
        }

        public override void Update(GameTime gameTime)
        {


        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (HasFocus)
                spriteBatch.DrawString(Font, Text, Position, SelectedColor);
            else
            {
                spriteBatch.DrawString(Font, Text, Position, Color);
            }
        }

        private static int counter = 0;
        public override void Init()
        {
            var keyinput = Manager.InputSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keyinput.Map(new KeyTrigger("enter"+counter++,Keys.Enter), (triggered, args) =>  OnSelectedImpl(args));

            var padinput = Manager.InputSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padinput.Map(new ButtonTrigger("enter"+counter, Buttons.A), (triggered, args) => OnSelectedImpl(args) );
        }

        private void OnSelectedImpl(InputEventArgs args)
        {
            if (args.State != InputState.Released) return;
            if (!HasFocus) return;

            FireSelectedEvent(null);
        }
    }
}
