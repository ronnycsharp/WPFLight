using System.Linq;
using Microsoft.Xna.Framework;
using System;

namespace System.Windows.Controls {
	public class WrapPanel : Canvas {
		public WrapPanel () : base () {
			Orientation = Orientation.Vertical;
		}

		#region Eigenschaften

		public Orientation Orientation { get; set; }

		#endregion

		internal override float MeasureHeight (float availableHeight) {
			var height = 0f;
			var maxHeight = 0f;
			foreach (var c in this.Children.OfType<Control> ( )) {
				var itemHeight = c.ActualHeight + c.Margin.Bottom + c.Margin.Top;
				if (this.Orientation == Orientation.Vertical)
					height += itemHeight;
				else
					maxHeight = Math.Max (itemHeight, maxHeight);
			}
			return (this.Orientation == Orientation.Vertical) 
				? height : maxHeight;

		}

		internal override float MeasureWidth (float availableWidth) {
			var width = 0f;
			var maxWidth = 0f;
			foreach (var c in this.Children.OfType<Control> ( )) {
				var itemWidth = c.ActualWidth + c.Margin.Right + c.Margin.Left;
				if (this.Orientation == Orientation.Horizontal)
					width += itemWidth;
				else
					maxWidth = Math.Max (itemWidth, maxWidth);
			}
			return (this.Orientation == Orientation.Horizontal) 
				? width : maxWidth;
		}

		public override void Invalidate () {
			base.Invalidate ();
			if (this.IsInitialized) {
				this.Refresh ();
			}
		}

		void Refresh ( ){
			var left = 0f;
			var top = 0f;
			foreach (var c in this.Children.OfType<Control> ( )) {
				if (this.Orientation == Orientation.Vertical) {
					if ((top + c.ActualHeight + c.Margin.Bottom) > this.ActualHeight) {
						top = 0f;
						left += c.ActualWidth + c.Margin.Right;
					}

					if (top == 0)
						left += c.Margin.Left;

					top += c.Margin.Top;

					SetLeft (c, (int)left);
					SetTop (c, (int)top);

					top += c.ActualHeight + c.Margin.Bottom;
				} else {
					if ((left + c.ActualWidth + c.Margin.Right) > this.ActualWidth) {
						left = 0f;
						top += c.ActualHeight + c.Margin.Bottom;
					}

					if (left == 0)
						top += c.Margin.Top;

					left += c.Margin.Left;

					SetLeft (c, (int)left);
					SetTop (c, (int)top);

					left += c.ActualWidth + c.Margin.Right;
				}
			}
		}

		public override void Initialize () {
			this.Refresh ();
			base.Initialize ();
		}
	}
}
