using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using JamGame.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Entities
{
    class Wall : DrawableGameObject
    {
        private readonly int width;
        private readonly int height;
        private Body body;
        public Wall(World world, Vector2 position, int width, int height)
        {
            this.width = width;
            this.height = height;
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(width)*2, ConvertUnits.ToSimUnits(height),0f);
            body.Restitution = 0f;
            body.BodyType = BodyType.Static;
            body.Position = new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y));
            body.UserData = this;
        }


       

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Game.Instance.Temp,
                new Rectangle(
                    (int) ConvertUnits.ToDisplayUnits(body.Position.X),
                    (int) ConvertUnits.ToDisplayUnits(body.Position.Y),
                    width,
                    height
                    ), Color.Brown 
                );
        }
    }
}
