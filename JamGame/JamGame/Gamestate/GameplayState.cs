using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Gamestate
{
    class GameplayState : GameState
    {
        private Player player;

        public GameplayState()
        {
            player = new Player();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);   
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(Color.Red);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
