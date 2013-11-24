using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GUI;
using JamGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Gamestate
{
    public class HowManyPlayersState : GameState
    {
        private GuiManager gui;

        public HowManyPlayersState()
        {
            gui  = new GuiManager(Game.Instance.Content.Load<SpriteFont>("default"));
            LinkLabel one = new LinkLabel();
            one.Position = new Vector2(Game.Instance.ScreenWidth / 2, 300);
            one.Color = Color.Red;
            one.SelectedColor = Color.White;
            one.Text = "One player";
            one.HasFocus = true;

            one.OnSelected += one_OnSelected;

            LinkLabel two = new LinkLabel();
            two.Position = new Vector2(one.Position.X, one.Position.Y + 100);
            two.Color = Color.Red;
            two.SelectedColor = Color.White;
            two.Text = "Two players - Gamepad";
            two.OnSelected += two_OnSelected;


            gui.AddControl(one);
            gui.AddControl(two);
        }

        void two_OnSelected(object sender, ControlEventArgs e)
        {
            Game.Instance.GameStateManager.ChangeState(new GameplayState(2));
        }

        void one_OnSelected(object sender, ControlEventArgs e)
        {
            Game.Instance.GameStateManager.ChangeState(new GameplayState(1));
        }

        public override void Update(GameTime gameTime)
        {
            gui.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            gui.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Unload()
        {
            gui.Controller.ChangeSetup(new InputControlSetup());
        }
    }
}
