using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JamGame.GameObjects.Components
{
    public class DirectionalArrow : IDrawableObjectComponent
    {
        #region Vars
        private readonly Texture2D arrowTexture;
        private int elapsed;
        private bool enabled;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get;
            private set;
        }
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                elapsed = 0;
                enabled = value;
            }
        }
        #endregion

        public DirectionalArrow()
        {
            arrowTexture = Game.Instance.Content.Load<Texture2D>("arrow");
        }

        public void Update(GameTime gameTime)
        {
            if (enabled)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                Position = new Vector2(Game.Instance.ScreenWidth - arrowTexture.Width - 25, 25);

                if (elapsed > 500)
                {
                    elapsed = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (enabled)
            {
                if (elapsed < 250)
                {
                    spriteBatch.Draw(arrowTexture, Position, Color.White);
                }
            }
        }
    }
}
