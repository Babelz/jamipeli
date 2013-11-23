using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using JamGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Gamestate
{
    class GameplayState : GameState
    {
        private Player player;

        private World world;
        private Wall wall;
        public GameplayState()
        {
            world = new World(new Vector2(0f, 0f));
            player = new Player(world);
            wall = new Wall(world, new Vector2(0, 300), 1000, 100);

        }

        public override void Update(GameTime gameTime)
        {
            world.Step((float) (gameTime.ElapsedGameTime.TotalMilliseconds * .001));
           // player.Update(gameTime);   
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(Color.Red);
            player.Draw(spriteBatch);
            wall.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
