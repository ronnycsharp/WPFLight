using System;

namespace System.Windows {
	public struct Rect {
		public Rect (float left, float top, float width, float height) {
			this.x = left;
			this.y = top;
			this.width = width;
			this.height = height;
		}

		public Rect (Point p1, Point p2) {
			var left = Math.Min (p1.X, p2.X);
			var top = Math.Min (p1.Y, p2.Y);
			var right = Math.Max (p1.X, p2.X);
			var bottom = Math.Max (p1.Y, p2.Y);

			this.x = left;
			this.y = top;
			this.width = right - left;
			this.height = bottom - top;
		}

		public Rect ( Point location, Size size ) {
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		}

		public Rect ( Size size ) {
			if (size.IsEmpty)
				this = empty;
			else {
				x = y = 0;
				width = size.Width;
				height = size.Height;
			}
		}

		#region Operators

		public static bool operator == (Rect rect1, Rect rect2) {
			return rect1.X == rect2.X &&
				rect1.Y == rect2.Y &&
				rect1.Width == rect2.Width &&
				rect1.Height == rect2.Height;
		}

		public static bool operator != (Rect rect1, Rect rect2) {
			return !(rect1 == rect2);
		}

		#endregion

		#region Properties

		public float X { get { return x; } set { x = value; } }

		public float Y { get { return y; } set { y = value; } }

		public float Left { get { return x; } }

		public float Top { get { return y; } }

		public float Width { get { return width; } set { width = value; } }

		public float Height { get { return height; } set { height = value; } }

		public float Right {
			get { return x + width; }
		}

		public float Bottom {
			get { return y + height; }
		}

		public Size Size {
			get {
				if (IsEmpty)
					return Size.Empty;

				return new Size (width, height);
			}
			set {
				if (value.IsEmpty)
					this = empty;
				else {
					if (IsEmpty)
						throw new System.InvalidOperationException ("Rect is not initialized");

					width = value.Width;
					height = value.Height;
				}
			}
		}

		public Point Location {
			get {
				return new Point (x, y);
			}
			set {
				if (IsEmpty)
					throw new InvalidOperationException ("Rect is not initialized");

				x = value.X;
				y = value.Y;
			}
		}

		public bool IsEmpty {
			get {
				return width < 0;
			}
		}

		#endregion

		static Rect CreateEmptyRect () {
			Rect rect = new Rect ();
			rect.x = Single.PositiveInfinity;
			rect.y = Single.PositiveInfinity;
			rect.width = Single.NegativeInfinity;
			rect.height = Single.NegativeInfinity;
			return rect;
		}

		public static bool Equals (Rect rect1, Rect rect2) {
			if (rect1.IsEmpty) {
				return rect2.IsEmpty;
			} else {
				return rect1.X.Equals (rect2.X) &&
					rect1.Y.Equals (rect2.Y) &&
					rect1.Width.Equals (rect2.Width) &&
					rect1.Height.Equals (rect2.Height);
			}
		}

		public override bool Equals (object o) {
			if ((null == o) || !(o is Rect))
				return false;

			var value = (Rect)o;
			return Rect.Equals (this, value);
		}

		public override int GetHashCode () {
			if (IsEmpty)
				return 0;
			else {
				// Perform field-by-field XOR of HashCodes
				return X.GetHashCode () ^
					Y.GetHashCode () ^
					Width.GetHashCode () ^
					Height.GetHashCode ();
			}
		}

		private float x;
		private float y;
		private float width;
		private float height;

		static readonly Rect empty = CreateEmptyRect ();
	}
}