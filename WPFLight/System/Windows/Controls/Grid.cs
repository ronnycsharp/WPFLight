using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;

namespace System.Windows.Controls {
	public class Grid : Panel {
		public Grid () : base () {
			ColumnDefinitions = new List<ColumnDefinition> ();
			RowDefinitions = new List<RowDefinition> ();

			topValues = new Dictionary<UIElement, float> ();
			leftValues = new Dictionary<UIElement, float> ();
		}

		#region Eigenschaften

		public static DependencyProperty RowProperty =
			DependencyProperty.RegisterAttached ( 
				"Row", 
				typeof ( int ), 
				typeof ( Grid ), 
				new FrameworkPropertyMetadata ( 
					0, FrameworkPropertyMetadataOptions.AffectsArrange ) );

		public static DependencyProperty RowSpanProperty =
			DependencyProperty.RegisterAttached ( 
				"RowSpan", 
				typeof ( int ), 
				typeof ( Grid ), 
				new FrameworkPropertyMetadata ( 
					1, FrameworkPropertyMetadataOptions.AffectsArrange ) );

		public static DependencyProperty ColumnProperty =
			DependencyProperty.RegisterAttached ( 
				"Column", 
				typeof ( int ), 
				typeof ( Grid ), 
				new FrameworkPropertyMetadata ( 
					0, FrameworkPropertyMetadataOptions.AffectsArrange ) );

		public static DependencyProperty ColumnSpanProperty =
			DependencyProperty.RegisterAttached ( 
				"ColumnSpan", 
				typeof ( int ), 
				typeof ( Grid ), 
				new FrameworkPropertyMetadata ( 
					1, FrameworkPropertyMetadataOptions.AffectsArrange ) );

		public List<ColumnDefinition> ColumnDefinitions { get; private set; }
		public List<RowDefinition> RowDefinitions { get; private set; }

		#endregion

		public static void SetRow (UIElement item, int row) {
			item.SetValue (RowProperty, row);
		}

		public static void SetRowSpan (UIElement item, int rowSpan) {
			item.SetValue (RowSpanProperty, rowSpan);
		}

		public static void SetColumn (UIElement item, int column) {
			item.SetValue (ColumnProperty, column);
		}

		public static void SetColumnSpan (UIElement item, int columnSpan) {
			item.SetValue (ColumnSpanProperty, columnSpan);
		}
			
		public static int GetRow (UIElement item) {
			return ( int ) item.GetValue (RowProperty);
		}

		public static int GetRowSpan (UIElement item) {
			return ( int ) item.GetValue (RowSpanProperty);
		}

		public static int GetColumn (UIElement item) {
			return ( int ) item.GetValue (ColumnProperty);
		}

		public static int GetColumnSpan (UIElement item) {
			return ( int ) item.GetValue (ColumnSpanProperty);
		}

		internal float GetStarColumnsWidth () {

			// TODO, Issue infinte loop

			if ( this.HorizontalAlignment != HorizontalAlignment.Stretch && this.Width == null )
				return 0;

			return
				this.ActualWidth
					- this.GetAutoColumnsWidth ()
					- this.GetAbsoluteColumnsWidth ();
		}

		internal float GetStarRowsHeight () {
			return
				this.ActualHeight
			- this.GetAutoRowsHeight ()
			- this.GetAbsoluteRowsHeight ();
		}

		internal float GetAutoColumnsWidth () {
			var result = 0f;
			var index = 0;
			foreach (var columnDef in this.ColumnDefinitions) {
				if (columnDef.Width.IsAuto)
					result += GetColumnWidth (index);

				index++;
			}
			return result;
		}

		internal float GetAutoRowsHeight () {
			var result = 0f;
			var index = 0;
			foreach (var rowDef in this.RowDefinitions) {
				if (rowDef.Height.IsAuto)
					result += GetRowHeight (index);

				index++;
			}
			return result;
		}

		internal float GetAbsoluteColumnsWidth () {
			var result = 0f;
			var index = 0;
			foreach (var columnDef in this.ColumnDefinitions) {
				if (columnDef.Width.IsAbsolute)
					result += GetColumnWidth (index);

				index++;
			}
			return result;
		}

		internal float GetAbsoluteRowsHeight () {
			var result = 0f;
			var index = 0;
			foreach (var rowDef in this.RowDefinitions) {
				if (rowDef.Height.IsAbsolute)
					result += GetRowHeight (index);

				index++;
			}
			return result;
		}

		internal float GetColumnWidth (int column) {
			var result = 0f;
			if (this.ColumnDefinitions.Count > 0 && column < this.ColumnDefinitions.Count) {
				var def = this.ColumnDefinitions [column];
				if (def.Width.IsAbsolute)
					result = def.Width.Value;
				else if (def.Width.IsAuto) {
					if (this.Children.Count > 0) {
						var widths = this.Children.OfType<Control> ().Where (c => GetColumn (c) == column && c.Width != null);
						if (widths.Count () > 0)
							result = widths.Max (c => c.ActualWidth);
					}
				} else if (def.Width.IsStar) {
					if (def.Width.Value > 0) {
						var sum = this.ColumnDefinitions
							.Where (c => c.Width.IsStar)
								.Sum (c => c.Width.Value);

						result = (def.Width.Value / sum)
							* GetStarColumnsWidth ();
					} else
						result = 0;
				}
			} else
				result = this.ActualWidth;

			return result;
		}

		internal float GetRowHeight (int row) {
			var result = 0f;
			if (this.RowDefinitions.Count > 0 && row < this.RowDefinitions.Count) {
				var def = this.RowDefinitions [row];
				if (def.Height.IsAbsolute)
					result = def.Height.Value;
				else if (def.Height.IsAuto) {
					if (this.Children.Count > 0) {
						var heights = this.Children.OfType<Control> ().Where (c => GetRow (c) == row && c.Height != null);
						if (heights.Count () > 0)
							result = heights.Max (c => c.ActualHeight);
					}
				} else if (def.Height.IsStar) {
					if (def.Height.Value > 0) {
						var sum = this.RowDefinitions
							.Where (c => c.Height.IsStar)
								.Sum (c => c.Height.Value);

						result = (def.Height.Value / sum)
						* GetStarRowsHeight ();
					} else
						result = 0;
				}
			} else
				result = this.ActualHeight;

			return result;
		}

		internal override float MeasureWidth (UIElement child) {
			if (child.HorizontalAlignment == HorizontalAlignment.Stretch) {
				var column = GetColumn (child);
				var columnSpan = GetColumnSpan (child);
				if (this.ColumnDefinitions.Count > 0) {
					var result = 0f;
					for (var index = 0; index < columnSpan; index++) {
						var columnIndex = column + index;
						if (columnIndex < this.ColumnDefinitions.Count) {
							result += GetColumnWidth (columnIndex);
						} else
							break;
					}
					return result;
				} else {
					return this.ActualWidth;
				}
			}
			return base.MeasureWidth (child);
		}

		internal override float MeasureHeight (UIElement child) {
			if (child.VerticalAlignment == VerticalAlignment.Stretch) {
				var row = GetRow (child);
				var rowSpan = GetRowSpan (child);

				if (this.RowDefinitions.Count > 0) {
					var result = 0f;
					for (var index = 0; index < rowSpan; index++) {
						var rowIndex = row + index;
						if (rowIndex < this.RowDefinitions.Count)
							result += GetRowHeight (rowIndex);
						else
							break;
					}
					return result;
				} else {
					return this.ActualHeight;
				}
			}
			return base.MeasureHeight (child);
		}

		internal override float GetAbsoluteLeft (UIElement child) {
			if (leftValues.ContainsKey (child))
				return leftValues [child];

			var result = 0f;
			var columnIndex = GetColumn (child);
			var columnSpan = GetColumnSpan (child);
			var columnLeft = GetAbsoluteLeft ();
			for (var index = 0; index < columnIndex; index++)
				columnLeft += GetColumnWidth (index);

			var columnWidth = 0f;
			for (var index = columnIndex; index < (columnIndex + columnSpan); index++)
				columnWidth += GetColumnWidth (index);

			if (child.HorizontalAlignment == HorizontalAlignment.Left
			    || child.HorizontalAlignment == HorizontalAlignment.Stretch) {
				result = columnLeft + child.Margin.Left;
			} else if (child.HorizontalAlignment == HorizontalAlignment.Right) {
				result = columnLeft + columnWidth
				- child.Margin.Right
				- child.ActualWidth;
			} else if (child.HorizontalAlignment == HorizontalAlignment.Center)
				result = columnLeft + ((columnWidth) / 2f) - (child.ActualWidth / 2f) + child.Margin.Left - child.Margin.Right;

			leftValues [child] = result;
			return result;
		}

		internal override float GetAbsoluteTop (UIElement child) {
			if (topValues.ContainsKey (child))
				return topValues [child];

			var result = 0f;
			var rowIndex = GetRow (child);
			var rowSpan = GetRowSpan (child);
			var rowTop = GetAbsoluteTop ();
			for (var index = 0; index < rowIndex; index++)
				rowTop += GetRowHeight (index);

			var rowHeight = 0f;
			for (var index = rowIndex; index < (rowIndex + rowSpan); index++)
				rowHeight += GetRowHeight (index);

			if (child.VerticalAlignment == VerticalAlignment.Top
			    || child.VerticalAlignment == VerticalAlignment.Stretch) {
				result = rowTop + child.Margin.Top;
			} else if (child.VerticalAlignment == VerticalAlignment.Bottom) {
				result = rowTop + rowHeight
				- child.Margin.Bottom
				- child.ActualHeight;
			} else if (child.VerticalAlignment == VerticalAlignment.Center)
				result = rowTop + ((rowHeight) / 2f) - (child.ActualHeight / 2f) + child.Margin.Top - child.Margin.Bottom;

			topValues [child] = result;
			return result;
		}

		public override void Invalidate () {

			base.Invalidate ();

			if (this.IsInitialized) {
				topValues.Clear ();
				leftValues.Clear ();

				foreach (var child in this.Children.OfType<Control> ( ))
					child.Invalidate ();
			}
		}

		protected void InvalidateLeft () {
			leftValues.Clear ();
			foreach (var child in this.Children.OfType<Grid> ( ))
				child.InvalidateLeft ();
		}

		protected void InvalidateTop () {
			topValues.Clear ();
			foreach (var child in this.Children.OfType<Grid> ( ))
				child.InvalidateTop ();
		}

		protected override void OnMarginChanged (Thickness newValue, Thickness oldValue) {
			if (this.Width != null) {
				if (oldValue.Left != newValue.Left)
					InvalidateLeft ();
			}
			if (this.Height != null) {
				if (oldValue.Top != newValue.Top)
					InvalidateTop ();
			}
		}
			
		internal override float MeasureWidth (float availableWidth) {
			if (this.Width == null
			             && this.HorizontalAlignment != HorizontalAlignment.Stretch) {
				var count = this.ColumnDefinitions.Count;
				var width = 0f;
				for (var i = 0; i < count; i++)
					width += GetColumnWidth (i);

				return width;
			}
			return base.MeasureWidth (availableWidth);
		}

		internal override float MeasureHeight (float availableHeight) {
			if (this.Height == null
			             && this.VerticalAlignment != VerticalAlignment.Stretch) {
				var count = this.RowDefinitions.Count;
				var height = 0f;
				for (var i = 0; i < count; i++)
					height += GetRowHeight (i);

				return height;
			}
			return base.MeasureWidth (availableHeight);
		}

		public override bool HitTest (Vector2 v) {
			return base.HitTest (v);
		}

		private Dictionary<UIElement, int> rows;
		private Dictionary<UIElement, int> rowSpans;
		private Dictionary<UIElement, int> columns;
		private Dictionary<UIElement, int> columnSpans;
		private Dictionary<UIElement, float> topValues;
		private Dictionary<UIElement, float> leftValues;
	}
}

