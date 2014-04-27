using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace System.Windows.Media {

    // the used fonts must be registered in the LoadContent-Method of the Game-class

	public static class FontContainer {
        static FontContainer () {
			registeredFonts = new Dictionary<FontFamily, SpriteFont>();
        }

        public static void Register (FontFamily fontFamily, SpriteFont font) {
            if (fontFamily == null || font == null)
                throw new ArgumentException();

            registeredFonts[fontFamily] = font;
        }

        public static void Register (string fontName, SpriteFont font) {
            if (String.IsNullOrEmpty(fontName) || font == null )
                throw new ArgumentException();

			var fontFamily = new FontFamily (fontName, font.Spacing, font.LineSpacing);
			registeredFonts[fontFamily] = font;
        }

        public static SpriteFont Resolve (string fontName) {
            if (String.IsNullOrEmpty(fontName))
                throw new ArgumentException();

			foreach ( var ff in registeredFonts ) {
				if (ff.Key.Source.ToLower () == fontName.ToLower ())
					return ff.Value;
			}
			throw new KeyNotFoundException ();
        }

        public static SpriteFont Resolve (FontFamily fontFamily) {
            if (fontFamily == null)
                throw new ArgumentNullException();

            return registeredFonts[fontFamily];
        }
	
		private static Dictionary<FontFamily, SpriteFont> registeredFonts;
	}
}
