using System.ComponentModel;
namespace System.Windows.Media {
	[TypeConverter (typeof(FontFamilyConverter))]
	public class FontFamily {
		public FontFamily ( string fontName ) {
			if (String.IsNullOrEmpty (fontName))
				throw new ArgumentException ();

			this.Source = fontName;
		}

		public FontFamily ( string fontName, float spacing, int lineSpacing ) : this ( fontName ) {
			this.Spacing = spacing;
			this.LineSpacing = lineSpacing;
		}
			
		#region Properties

		public string Source { get; private set; }

		public float Spacing { get; private set; }

		public int LineSpacing { get; private set; }

		#endregion

		public override bool Equals (object obj) {
			var ff = obj as FontFamily;
			return ff != null 
				&& ff.Source.ToLower () == this.Source.ToLower ();
		}

		public override int GetHashCode () {
			return this.Source.ToLower ().GetHashCode ();
		}
	}
}
