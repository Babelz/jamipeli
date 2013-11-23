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
        private Body body;
        public Shoemon()
        {
            animation = Game.Instance.Content.Load<CharacterModel>("monsters\\shoemon").CreateAnimator("Shoemon");
            Position = new Vector2(200,200);
            animation.Scale = 0.5f;
            //body = BodyFactory.CreateRectangle()

        }

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
