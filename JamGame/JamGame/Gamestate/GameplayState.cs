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
        #region Vars
        private Player player;
        private World world;
        private Wall topWall;
        private Wall bottomWall;
        private Wall leftWall;
        private Wall rightWall;
        #endregion

        #region Properties
        public Player Player
        {
            get
            {
                return player;
            }
        }
        #endregion

        public GameplayState()
        {
            world = new World(new Vector2(0f, 0f));
            player = new Player(world);
            topWall = new Wall(world, new Vector2(0, -100), 1280, 100);
            bottomWall = new Wall(world, new Vector2(0, 300), 1280, 100);
            leftWall = new Wall(world, new Vector2(-150, 0), 100, 720);
            rightWall = new Wall(world, new Vector2(1280, 0), 100, 720);
        }

        public override void Update(GameTime gameTime)
        {
            world.Step((float) (gameTime.ElapsedGameTime.TotalMilliseconds * .001));
            player.Update(gameTime);   
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            player.Draw(spriteBatch);
            leftWall.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
