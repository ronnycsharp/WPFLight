using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Windows {
	[TypeConverter(typeof(ThicknessConverter))]
	public struct Thickness : IEquatable<Thickness> {
		public Thickness (float thickness) {
			_left = thickness;
			_top = thickness;
			_right = thickness;
			_bottom = thickness;
		}

		public Thickness (Thickness thickness) {
			_left = thickness.Left;
			_top = thickness.Top;
			_right = thickness.Right;
			_bottom = thickness.Bottom;
		}

		public Thickness (float left, float top, float right, float bottom) {
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		public float Left { 
			get { return _left; } 
		}

		public float Top {
			get { return _top; }
		}

		public float Right {
			get { return _right; }
		}

		public float Bottom {
			get { return _bottom; }
		}

		public bool IsEmpty {
			get {
				return this.Left == 0
				&& this.Top == 0
				&& this.Right == 0
				&& this.Bottom == 0;
			}
		}

		public bool Equals (Thickness t) {
			return Left == t.Left
			&& Top == t.Top
			&& Right == t.Right
			&& Bottom == t.Bottom;
		}

		private float _left;
		private float _top;
		private float _right;
		private float _bottom;
	}
}
