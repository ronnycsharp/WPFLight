using System;

namespace System.Windows
{
	public class RowDefinition
	{
		static RowDefinition ()
		{
			Auto = new RowDefinition { Height = GridLength.Auto };
			Star = new RowDefinition { Height = GridLength.Star };
		}

		public RowDefinition ()
		{
			Height = GridLength.Star;
		}

		public RowDefinition (GridLength height, float? minHeight, float? maxHeight)
		{
			this.Height = height;
			this.MinHeight = minHeight;
			this.MaxHeight = maxHeight;
		}

		public RowDefinition (GridLength height) : this ( height, null, null )
		{
		}

		public float? MinHeight { get; set; }

		public float? MaxHeight { get; set; }

		public GridLength Height { get; set; }

		public static RowDefinition Auto { get; private set; }

		public static RowDefinition Star { get; private set; }
	}
}

