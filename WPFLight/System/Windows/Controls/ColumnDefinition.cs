using System;

namespace System.Windows.Controls
{
	public class ColumnDefinition
	{
		static ColumnDefinition ()
		{
			Auto = new ColumnDefinition { Width = GridLength.Auto };
			Star = new ColumnDefinition { Width = GridLength.Star };
		}

		public ColumnDefinition ()
		{
			Width = GridLength.Star;
		}

		public ColumnDefinition (GridLength width, float? minWidth, float? maxWidth)
		{
			this.Width = width;
			this.MinWidth = minWidth;
			this.MaxWidth = maxWidth;
		}

		public ColumnDefinition (GridLength width) : this ( width, null, null )
		{
		}

		#region Eigenschaften

		public float? MinWidth { get; set; }

		public float? MaxWidth { get; set; }

		public GridLength Width { get; set; }

		public static ColumnDefinition Auto { get; private set; }

		public static ColumnDefinition Star { get; private set; }

		#endregion
	}
}

