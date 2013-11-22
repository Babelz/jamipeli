using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using JamGame.GameObjects.Components;

namespace JamGame.GameObjects
{
    public abstract class DrawableGameObject : GameObject
    {
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (IObjectComponent component in components)
            {
                IDrawableObjectComponent drawableComponent = component as IDrawableObjectComponent;
                if (drawableComponent != null)
                {
                    drawableComponent.Draw(spriteBatch);
                }
            }
        }
    }
}
