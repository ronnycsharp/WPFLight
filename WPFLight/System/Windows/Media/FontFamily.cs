using System.ComponentModel;
namespace System.Windows.Media {
	[TypeConverter (typeof(FontFamilyConverter))]
	public class FontFamily {
		public FontFamily ( string fontName ) {
			if (!String.IsNullOrEmpty (fontName))
				throw new ArgumentNullException ();

			this.Source = fontName;
		}
			
		#region Properties

		public string Source { get; private set; }

		#endregion
	}
}
