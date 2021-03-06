﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JamGame.GameObjects.Components
{
    public class FiniteStateMachine : IUpdatableObjectComponent
    {
        #region Vars
        private readonly Stack<Action> states;
        #endregion

        #region Properties
        public bool HasStates
        {
            get
            {
                return states.Count > 0;
            }
        }
        public Action CurrentState
        {
            get
            {
                if (HasStates)
                {
                    return states.Peek();
                }

                return null;
            }
        }
        #endregion

        public FiniteStateMachine()
        {
            states = new Stack<Action>();
        }

        public Action PopState()
        {
            if (HasStates)
            {
                return states.Pop();
            }

            return null;
        }
        public void PushState(Action action)
        {
            states.Push(action);
        }
        public void Update(GameTime gameTime)
        {
            if (HasStates)
            {
                CurrentState();
            }
        }
    }
}
