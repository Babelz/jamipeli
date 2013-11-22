using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamGame.DataTypes
{
    public struct Size
    {
        #region Vars
        private readonly int width;
        private readonly int height;
        #endregion

        #region Properties
        public int Width
        {
            get
            {
                return width;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
        }
        #endregion

        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
