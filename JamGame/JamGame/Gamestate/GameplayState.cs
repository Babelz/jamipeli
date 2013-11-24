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
        private readonly Wall topWall;
        private readonly Wall bottomWall;
        private readonly Wall leftWall;
        private readonly Wall rightWall;

        private readonly Player[] players;
        #endregion

        #region Properties
        public Player[] Players
        {
            get
            {
                return players;
            }
        }
        public Player PlayerOne
        {
            get;
            private set;
        }
        public Player PlayerTwo
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

            PlayerOne = new KeyboardPlayer(world);
            PlayerTwo = new GamepadPlayer(world, PlayerIndex.One);

            PlayerTwo.Position = new Vector2(500, 700);
            PlayerOne.Position = new Vector2(500, 500);

            players = new Player[] 
            {
                PlayerOne, PlayerTwo
            };

            topWall = new Wall(world, new Vector2(Game.Instance.ScreenWidth / 2f, Game.Instance.ScreenHeight / 2f - 50), Game.Instance.ScreenWidth * 2, 100);
            bottomWall = new Wall(world, new Vector2(Game.Instance.ScreenWidth / 2f, Game.Instance.ScreenHeight + 50), Game.Instance.ScreenWidth * 2, 100);
            leftWall = new Wall(world, new Vector2(-50, Game.Instance.ScreenHeight / 2f), 100, Game.Instance.ScreenHeight);
            rightWall = new Wall(world, new Vector2(Game.Instance.ScreenWidth + 50, Game.Instance.ScreenHeight / 2f), 100, Game.Instance.ScreenHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Game.Instance.World.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * .001));

            Array.ForEach(players,
                p => p.Update(gameTime));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Array.ForEach(players,
                p => p.Draw(spriteBatch));

            spriteBatch.End();
        }
    }
}
