using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace System.Windows.Media {

    // the used fonts must be registered in the LoadContent-Method of the Game-class

	public static class FontContainer {
        static FontContainer () {
            registeredFonts = new Dictionary<string, SpriteFont>();
        }

        public static void Register (FontFamily fontFamily, SpriteFont font) {
            if (fontFamily == null || font == null)
                throw new ArgumentException();

            registeredFonts[fontFamily.Source.ToLower ( )] = font;
        }

        public static void Register (string fontName, SpriteFont font) {
            if (String.IsNullOrEmpty(fontName) || font == null )
                throw new ArgumentException();

            registeredFonts[fontName.ToLower ( )] = font;
        }

        public static SpriteFont Resolve (string fontName) {
            if (String.IsNullOrEmpty(fontName))
                throw new ArgumentException();

            return registeredFonts[fontName.ToLower ( )];
        }

        public static SpriteFont Resolve (FontFamily fontFamily) {
            if (fontFamily == null)
                throw new ArgumentNullException();

            return registeredFonts[fontFamily.Source.ToLower()];
        }
	
        private static Dictionary<string, SpriteFont> registeredFonts;
	}
}
