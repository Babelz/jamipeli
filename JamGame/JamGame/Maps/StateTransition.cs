using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Maps
{
    public class StateTransition
    {
        #region Vars
        private readonly MapState next;
        private readonly MapState current;
        private readonly Point endPoint;

        private Action velofunc;
        private bool playing;

        private int velocity;
        private const int maxVelocity = 15;
        #endregion

        #region Events
        public event StateTransitionEventHandler OnFinished;
        #endregion

        #region Properties
        public bool Finished
        {
            get
            {
                return !(next.Position.X > endPoint.X);
            }
        }
        #endregion

        public StateTransition(MapState next, MapState current)
        {
            this.next = next;
            this.current = current;

            endPoint = current.Position;
        }

        private void Brake()
        {
            if (velocity > 1)
            {
                velocity--;
            }
        }
        private void Accelrate()
        {
            if (velocity < maxVelocity)
            {
                velocity++;
            }
        }
        
        public void Start()
        {
            if (!playing)
            {
                velocity = 0;
                playing = true;
            }
        }
        public void Update(GameTime gameTime)
        {
            if (playing && !Finished)
            {
                if (next.Position.X <= 120)
                {
                    velofunc = Brake;
                }
                else
                {
                    velofunc = Accelrate;
                }

                velofunc();

                next.Position = new Point(next.Position.X - velocity, next.Position.Y);
                current.Position = new Point(current.Position.X - velocity, current.Position.Y);
            }
            else
            {
                if (OnFinished != null)
                {
                    OnFinished(this, new StateTransitionEventArgs());
                }

                playing = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            next.Draw(spriteBatch);
        }
    }

    public delegate void StateTransitionEventHandler(object sender, StateTransitionEventArgs e);

    public class StateTransitionEventArgs
	{
	}
}
