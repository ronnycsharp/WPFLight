using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace System.Windows.Media {

    // the used fonts must be registered in the LoadContent-Method of the Game-class

	public static class FontContainer {
        static FontContainer () {
            registeredFonts = new Dictionary<string, SpriteFont>();
        }

        public static void Register (string fontName, SpriteFont font) {
            if (String.IsNullOrEmpty(fontName) || font == null )
                throw new ArgumentException();

            registeredFonts[fontName] = font;
        }

        public static SpriteFont Resolve (string fontName) {
            if (String.IsNullOrEmpty(fontName))
                throw new ArgumentException();

            return registeredFonts[fontName];
        }
			
        private static Dictionary<string, SpriteFont> registeredFonts;
	}
}
