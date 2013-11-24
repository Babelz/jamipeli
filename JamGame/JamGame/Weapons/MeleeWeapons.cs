using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Weapons
{
    public class BaseballBat : MeleeWeapon
    {
        public BaseballBat()
            : base("Baseball Bat", 5, 12, 100, 100, 1000)
        {
        }

        protected override void OnDrawEffects(SpriteBatch spriteBatch, Vector2 position, Vector2 area, int elapsedDrawTime)
        {
            int min_X = (int)position.X;
            int max_X = min_X + (int)area.X;

            int min_Y = (int)position.Y;
            int max_Y = min_Y + (int)area.Y;

            for (int i = 0; i < 4; i++)
            {
                Rectangle rectangle = new Rectangle(
                random.Next(min_X, max_X),
                random.Next(min_Y, max_Y),
                random.Next(min_X / 2, max_X / 2),
                random.Next(min_Y / 2, max_Y / 2));

                spriteBatch.Draw(Game.Instance.Temp, rectangle, Color.Red);
            }
        }
    }
}
