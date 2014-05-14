using System.Linq;
using Microsoft.Xna.Framework;
using System;
using System.Windows;

namespace System.Windows.Controls {
	public class StackPanel : Panel {
		public StackPanel () : base () {
			Orientation = Orientation.Vertical;
		}

		#region Properties

		public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register ( 
				"Orientation", 
				typeof ( Orientation ), 
				typeof ( StackPanel ),
				new FrameworkPropertyMetadata (
					Orientation.Vertical,
					FrameworkPropertyMetadataOptions.AffectsMeasure ) );

		public Orientation Orientation { 
			get { return (Orientation)GetValue (OrientationProperty); }
			set { SetValue (OrientationProperty, value); } 
		}

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
			foreach (var c in this.Children.OfType<UIElement> ( )) {
				var itemWidth = c.ActualWidth + c.Margin.Right + c.Margin.Left;

				if (this.Orientation == Orientation.Horizontal)
					width += itemWidth;
				else
					maxWidth = Math.Max (itemWidth, maxWidth);
			}

			return (this.Orientation == Orientation.Horizontal) 
				? width : maxWidth;
		}

		internal override float GetAbsoluteLeft (UIElement child) {
			var left = child.Parent.GetAbsoluteLeft ();
			foreach (var c in this.Children.OfType<UIElement>()) {
				if (c == child)
					return left + c.Margin.Left;

				if (this.Orientation == Orientation.Horizontal)
					left += c.ActualWidth + c.Margin.Right + c.Margin.Left;
			}
			return base.GetAbsoluteLeft (child);
		}

		internal override float GetAbsoluteTop (UIElement child) {
			var top = child.Parent.GetAbsoluteTop ();
			foreach (var c in this.Children.OfType<UIElement>()) {
				if (c == child)
					return top + c.Margin.Top;

				if (this.Orientation == Orientation.Vertical)
					top += c.ActualHeight + c.Margin.Bottom + c.Margin.Top;
			}
			return base.GetAbsoluteTop (child);
		}
	}

	public enum Orientation {
		Vertical,
		Horizontal,
	}
}
