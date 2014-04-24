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

		public List<ColumnDefinition> ColumnDefinitions { get; private set; }

		public List<RowDefinition> RowDefinitions { get; private set; }

		#endregion

		internal void SetRow (UIElement item, int row) {
			if (rows == null)
				rows = new Dictionary<UIElement, int> ();

			rows [item] = row;
		}

		internal void SetRowSpan (UIElement item, int rowSpan) {
			if (rowSpans == null)
				rowSpans = new Dictionary<UIElement, int> ();

			rowSpans [item] = rowSpan;
		}

		internal void SetColumn (UIElement item, int column) {
			if (columns == null)
				columns = new Dictionary<UIElement, int> ();

			columns [item] = column;
		}

		internal void SetColumnSpan (UIElement item, int columnSpan) {
			if (columnSpans == null)
				columnSpans = new Dictionary<UIElement, int> ();

			columnSpans [item] = columnSpan;
		}

		public static void SetRow (Grid gridParent, UIElement item, int row) {
			gridParent.SetRow (item, row);
		}

		public static void SetRowSpan (Grid gridParent, UIElement item, int rowSpan) {
			gridParent.SetRowSpan (item, rowSpan);
		}

		public static void SetColumn (Grid gridParent, UIElement item, int column) {
			gridParent.SetColumn (item, column);
		}

		public static void SetColumnSpan (Grid gridParent, UIElement item, int columnSpan) {
			gridParent.SetColumnSpan (item, columnSpan);
		}

		internal int GetRow (UIElement item) {
			int row = 0;
			if (rows != null && rows.ContainsKey (item)) {
				row = rows [item];
				if (row >= this.RowDefinitions.Count)
					row = this.RowDefinitions.Count - 1;
				else {
					if (row < 0)
						row = 0;
				}
			}
			return row;
		}

		internal int GetRowSpan (UIElement item) {
			int rowSpan = 1;
			if (rowSpans != null && rowSpans.ContainsKey (item)) {
				rowSpan = rowSpans [item];
				if (rowSpan < 1)
					rowSpan = 1;
			}
			return rowSpan;
		}

		internal int GetColumn (UIElement item) {
			int column = 0;
			if (columns != null && columns.ContainsKey (item)) {
				column = columns [item];
				if (column >= this.ColumnDefinitions.Count)
					column = this.ColumnDefinitions.Count - 1;
				else {
					if (column < 0)
						column = 0;
				}
			}
			return column;
		}

		internal int GetColumnSpan (UIElement item) {
			int columnSpan = 1;
			if (columnSpans != null && columnSpans.ContainsKey (item)) {
				columnSpan = columnSpans [item];
				if (columnSpan < 1)
					columnSpan = 1;
			}
			return columnSpan;
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

