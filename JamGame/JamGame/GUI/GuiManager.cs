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
    public class GuiManager
    {
        private int selected = 0;
        private List<Control> controls;

        public InputController Controller
        {
            get;
            private set;
        }

        public InputControlSetup InputSetup
        {
            get;
            private set;
        }

        public SpriteFont Font
        {
            get; 
            private set;
        }

        public GuiManager(SpriteFont font)
        {
            controls = new List<Control>();
            Font = font;
            Controller = new InputController(Game.Instance.InputManager);
            InputSetup = new InputControlSetup();
            Controller.ChangeSetup(InputSetup);
            BindKeys();
        }

        private void BindKeys()
        {
            var keyinput = InputSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keyinput.Map(new KeyTrigger("alas", Keys.Down), (triggered, args) => NextControl(), InputState.Released);
            keyinput.Map(new KeyTrigger("ylos", Keys.Up), (triggered, args) => PreviousControl(), InputState.Released);

            var padinput = InputSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padinput.Map(new ButtonTrigger("alas", Buttons.DPadUp, Buttons.LeftThumbstickUp), (triggered, args) =>
            {
                if (args.State != InputState.Released) return;
                NextControl();
            });
            padinput.Map(new ButtonTrigger("ylos", Buttons.DPadDown, Buttons.LeftThumbstickDown), (triggered, args) =>
            {
                if (args.State != InputState.Released) return;
                PreviousControl();
            });
        }

        public void AddControl(Control control)
        {
            control.Manager = this;
            control.Font = Font;
            control.Init();
            controls.Add(control);
        }

        private void PreviousControl()
        {
            if (controls.Count == 0)
                return;

            int current = selected;
            controls[selected].HasFocus = false;
            do
            {
                selected--;
                if (selected < 0)
                    selected = controls.Count - 1;
                if (controls[selected].TabStop && controls[selected].Enabled)
                    break;
            } while (current != selected);
            controls[selected].HasFocus = true;
        }

        private void NextControl()
        {
            if (controls.Count == 0)
                return;

            int current = selected;
            controls[selected].HasFocus = false;
            do
            {
                selected++;
                if (selected == controls.Count)
                    selected = 0;
                if (controls[selected].TabStop && controls[selected].Enabled)
                    break;
            } while (current != selected);
            controls[selected].HasFocus = true;
        }

        #region Methods

        public void Update(GameTime gameTime)
        {
            foreach (var control in controls)
            {
                if (control.Enabled) control.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var control in controls)
            {
                if (control.Visible) control.Draw(sb);
            }
        }

        #endregion
    }
}
