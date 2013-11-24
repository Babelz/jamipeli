using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.GUI
{
    public abstract class Control
    {
        #region Properties

        public string Text
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public SpriteFont Font
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public bool HasFocus
        {
            get;
            set;
        }

        public bool TabStop
        {
            get; 
            set;
        }

        public Color Color
        {
            get;

            set;
        }

        public GuiManager Manager
        { get; set; }

        #endregion

        #region Events
        public delegate void ControlEventHandler(object sender, ControlEventArgs e);

        public event ControlEventHandler OnSelected;

        #endregion

        #region Methods

        public Control()
        {
            Enabled = true;
            Visible = true;
            TabStop = true;
            HasFocus = false;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Init();

        protected virtual void FireSelectedEvent(ControlEventArgs e)
        {
            if (OnSelected != null)
            {
                OnSelected(this, e);
            }
        }
        #endregion
    }
}
