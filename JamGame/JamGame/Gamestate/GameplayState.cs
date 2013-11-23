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
        private Wall topWall;
        private Wall bottomWall;
        private Wall leftWall;
        private Wall rightWall;

        private List<Player> players; 
        #endregion

        #region Properties
        public Player Player
        {
            get
            {
                return player;
            }
        }

        public Player PlayerTwo
        {
            get;
            private set;
        }

        public Player PlayerThree
        {
            get;
            private set;
        }
        public Wall LeftWall
        {
            get
            {
                return leftWall;
            }
        }
        public Wall RightWall
        {
            get
            {
                return rightWall;
            }
        }
        #endregion

        public GameplayState()
        {
            World world = Game.Instance.World;

            player = new KeyboardPlayer(world);
            PlayerTwo = new GamepadPlayer(world, PlayerIndex.One);
            PlayerTwo.Position = new Vector2(500, 700);
            player.Position = new Vector2(500,500);

            players = new List<Player>();
            players.Add(player);
            players.Add(PlayerTwo);

            topWall = new Wall(world, new Vector2(Game.Instance.ScreenWidth / 2f, Game.Instance.ScreenHeight / 2f - 50), Game.Instance.ScreenWidth * 2, 100);
            bottomWall = new Wall(world, new Vector2(Game.Instance.ScreenWidth / 2f, Game.Instance.ScreenHeight + 50), Game.Instance.ScreenWidth * 2, 100);
            leftWall = new Wall(world, new Vector2(-50, Game.Instance.ScreenHeight / 2f), 100, Game.Instance.ScreenHeight);
            rightWall = new Wall(world, new Vector2(Game.Instance.ScreenWidth + 50, Game.Instance.ScreenHeight / 2f), 100, Game.Instance.ScreenHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Game.Instance.World.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * .001));
            players.ForEach(p => p.Update(gameTime));
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            players.ForEach(p => p.Draw(spriteBatch));
          //  leftWall.Draw(spriteBatch);
         //   rightWall.Draw(spriteBatch);
           // bottomWall.Draw(spriteBatch);
          //  topWall.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
