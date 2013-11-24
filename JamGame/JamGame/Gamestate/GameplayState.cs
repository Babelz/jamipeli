using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using JamGame.Components;
using JamGame.Entities;
using JamGame.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JamGame.GameObjects.Components;

namespace JamGame.Gamestate
{
    class GameplayState : GameState
    {
        #region Vars
        private Wall topWall;
        private Wall bottomWall;
        private Wall leftWall;
        private Wall rightWall;

        private Player[] players;
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

        public GameplayState(int playerCount)
        {

        }

        public override void Init()
        {
            Game.Instance.MapManager = new MapManager(Game.Instance, Game.Instance.SpriteBatch);

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
            
            Game.Instance.Components.Add(Game.Instance.MapManager);
            
            StateTrigger trigger = new StateTrigger(Game.Instance);
            Game.Instance.Components.Add(trigger);

            Game.Instance.MapManager.ChangeMap("testmap");
            



        }

        public override void Update(GameTime gameTime)
        {
            Game.Instance.World.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * .001));
            foreach (var gobject in Game.Instance.GameObjects.ToList())
            {
                gobject.Update(gameTime);
            }
            

            List<Player> alive = new List<Player>();
            foreach (Player player in players)
            {
                player.Update(gameTime);

                if ((player.Components.FirstOrDefault(c => c is HealthComponent) as HealthComponent).Alive)
                {
                    alive.Add(player);
                }
                else
                {
                    player.Destory();
                    player.Update(gameTime);
                }
            }

            players = alive.ToArray();
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Array.ForEach(players,
                p => p.Draw(spriteBatch));

            players.OrderBy(p => p.Position.Y)
                .ToList()
                .ForEach(p =>
                {
                    p.Draw(spriteBatch);

                    HealthComponent hp = p.Components
                        .FirstOrDefault(c => c is HealthComponent)
                        as HealthComponent;

                    Vector2 pos = (p is GamepadPlayer ? new Vector2(0, 100) : new Vector2(0, 0));

                    spriteBatch.DrawString(Game.Instance.Content.Load<SpriteFont>("default"), "Player" + (pos == Vector2.Zero ? "1: " : "2: ") + hp.Health, pos, Color.Red);
                });
            foreach (var gobject in Game.Instance.DrawableGameObjects.OrderBy(g => g.Position.Y).ToList())
            {
                gobject.Draw(spriteBatch);
            }
            


            spriteBatch.End();
        }
    }
}
