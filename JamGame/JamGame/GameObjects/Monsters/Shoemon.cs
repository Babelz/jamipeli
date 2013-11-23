using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrashMonkeySpriter;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.GameObjects.Monsters
{
    public class Shoemon : Monster
    {
        #region Vars
        private Body body;
        #endregion

        public Shoemon()
        {
            animation = Game.Instance.Content.Load<CharacterModel>("monsters\\shoemon").CreateAnimator("Shoemon");
            animation.Scale = 0.5f;

            brain.PushState(MoveToArea);
        }

        #region Brain states
        private void MoveToArea()
        {
            Position = new Vector2(Position.X - 0.75f, Position.Y);
        }
        private void GetTarget()
        {
        }
        private void Attack()
        {
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animation.Location = Position;
            animation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            animation.Draw(spriteBatch);
        }
    }
}
