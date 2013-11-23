using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Gamestate
{
    public class GameStateManager : DrawableGameComponent
    {
        #region Vars
        private readonly SpriteBatch spriteBatch;
        private readonly List<GameState> gameStates;
        #endregion

        #region Properties
        public List<GameState> States
        {
            get
            {
                return gameStates;
            }
        }
        #endregion

        public GameStateManager(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            gameStates = new List<GameState>();

            DrawOrder = 1;
        }

        public void PopState()
        {
            if (gameStates.Count > 0)
                gameStates.RemoveAt(gameStates.Count - 1);

        }

        public void PushState(GameState gameState)
        {
            gameStates.Add(gameState);
        }

        public void ChangeState(GameState gameState)
        {
            if (gameStates.Count == 0)
            {
                gameStates.Add(gameState);
            }
            else
            {
                gameStates[gameStates.Count - 1] = gameState;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (gameStates.Count == 0)
            {
                Game.Exit();
                return;
            }

            gameStates[gameStates.Count - 1].Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (gameStates.Count == 0)
                return;

            gameStates[gameStates.Count - 1].Draw(spriteBatch);
        }
    }
}
