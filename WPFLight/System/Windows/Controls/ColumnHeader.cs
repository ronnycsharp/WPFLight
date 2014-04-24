using System;
using Microsoft.Xna.Framework;

namespace System.Windows.Controls
{
	public class ColumnHeader : ColumnDefinition {
		public ColumnHeader (string text, GridLength width ) : base ( width )
		{
			if (width.IsAuto)
				throw new NotImplementedException ("Auto");

			this.TextAlignment 	= TextAlignment.Left;
			this.Text 			= text;
			this.TextColor 		= Color.White;
		}

		public Color 			TextColor		{ get; set; }
		public TextAlignment 	TextAlignment 	{ get; set; }
		public string 			Text 			{ get; private set; }
	}
}

