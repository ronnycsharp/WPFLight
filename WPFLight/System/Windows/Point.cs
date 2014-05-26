using System;
using System.ComponentModel;
namespace System.Windows {
	[TypeConverter(typeof(PointConverter))]
	public struct Point {
		public Point (float x, float y) {
			X = x;
			Y = y;
		}

		#region Operators

		public static bool operator ==(Point p1, Point p2) {
			return p1.Equals(p2);
		}

		public static bool operator != (Point p1, Point p2) {
			return !p1.Equals(p2);
		}

		public static Point operator +(Point p1, Point p2) {
			return new Point (p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point operator -(Point p1, Point p2) {
			return new Point (p1.X - p2.X, p1.Y - p2.Y);
		}

		#endregion

		#region Properties

		public float X;
		public float Y;

		#endregion

		public override bool Equals (object obj) {
			if (obj is Point) {
				var p = (Point)obj;
				return p.X == this.X && p.Y == this.Y;
			}
			return false;
		}

		public override int GetHashCode () {
			return base.GetHashCode();
		}
	}
}