using System;
using Microsoft.Xna.Framework;
namespace System.Windows {
    public struct Size {
		public Size (float width, float height) {
            Width = width;
            Height = height;
        }

        public Size (Vector2 vec) : this ( vec.X, vec.Y ) { }

        public Size (Point point) : this(point.X, point.Y) { }

        #region Eigenschaften

		public float Width;
		public float Height;

		public static Size Empty {
			get { return new Size ( ); }
        }

        public bool IsEmpty {
            get { 
                return Width == 0 
                    && Height == 0; }
        }

        #endregion
    }
}