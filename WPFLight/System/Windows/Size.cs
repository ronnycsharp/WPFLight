using System;
using Microsoft.Xna.Framework;

namespace System.Windows {
	public struct Size {
		public Size (float width, float height) {
			this.width = width;
			this.height = height;
		}

		internal Size (Vector2 vec) : this (vec.X, vec.Y) {
		}

		public Size (Point point) : this (point.X, point.Y) {
		}

		#region Operators

		public static bool operator == (Size size1, Size size2) {
			return size1.Width == size2.Width &&
				size1.Height == size2.Height;
		}

		public static bool operator != (Size size1, Size size2) {
			return !(size1 == size2);
		}

		#endregion

		#region Properties

		public float Width 	{ get { return width; } set { width = value; } }

		public float Height { get { return height; } set { height = value; } }

		public static Size Empty {
			get { return EMPTY; }
		}

		public bool IsEmpty {
			get { 
				return width < 0;
			}
		}

		#endregion

		public static bool Equals (Size size1, Size size2) {
			if (size1.IsEmpty)
				return size2.IsEmpty;
			else
				return size1.Width.Equals (size2.Width)
					&& size1.Height.Equals (size2.Height);
		}

		public override bool Equals (object o) {
			if ((null == o) || !(o is Size))
				return false;

			Size value = (Size)o;
			return Size.Equals (this, value);
		}

		public override int GetHashCode () {
			if (IsEmpty)
				return 0;
			else {
				// Perform field-by-field XOR of HashCodes
				return 
					this.Width.GetHashCode () ^
					this.Height.GetHashCode ();
			}
		}

		private float width;
		private float height;

		static readonly Size EMPTY = new Size ();
	}
}