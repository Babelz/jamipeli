using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JamGame.GameObjects.Components;

namespace JamGame.GameObjects.Monsters
{
    public class Chutulu : Monster
    {
        #region Vars
        private readonly Random random;
        private float velo_x;
        private float velo_y;
        #endregion

        public Chutulu()
            : base()
        {
            random = new Random();
            velo_y = random.Next(0, 2);
            velo_x = random.Next(0, 2);

            components.Add(Health = new HealthComponent(90));
            brain.PushState(() =>
                {
                    Position = new Vector2(Position.X + velo_x, Position.Y + velo_y);
                });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Game.Instance.Temp, new Rectangle((int)Position.X, (int)Position.Y, 64, 64), Color.Teal);
        }
    }
}
