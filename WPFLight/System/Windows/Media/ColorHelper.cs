using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using WPFLight.Extensions;
namespace System.Windows.Media {
	internal static class ColorHelper {
		static ColorHelper () {
			var props = typeof(Colors).GetProperties (
				            System.Reflection.BindingFlags.Public
				            | System.Reflection.BindingFlags.Static);

			NamedColors = new Dictionary<string, Color> ();
			foreach (var p in props) {
				if (p.PropertyType == typeof(Color)) {
					NamedColors.Add ( 
						p.Name.ToUpper ( ), 
						( Color ) p.GetValue (null,null));
				}
			}
		}

		public static Color GetNamedColor (string name) {
			if (name == null)
				throw new ArgumentNullException ();

			return NamedColors [name.ToUpper()];
		}

		static int ParseHexChar(char c )
		{
			int intChar = (int) c;
			if ((intChar >= s_zeroChar) && (intChar <= (s_zeroChar+9)))
			{
				return (intChar-s_zeroChar);
			}

			if ((intChar >= s_aLower) && (intChar <= (s_aLower+5)))
			{
				return (intChar-s_aLower + 10);
			}

			if ((intChar >= s_aUpper) && (intChar <= (s_aUpper+5)))
			{
				return (intChar-s_aUpper + 10);
			}
			throw new ArgumentException ();
		}

		static Color ParseHexColor(string trimmedColor)
		{
			int a,r,g,b;
			a = 255;

			if ( trimmedColor.Length > 7 )
			{
				a = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
				r = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
				g = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
				b = ParseHexChar(trimmedColor[7]) * 16 + ParseHexChar(trimmedColor[8]);
			}
			else if ( trimmedColor.Length > 5)
			{
				r = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
				g = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
				b = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
			}
			else if (trimmedColor.Length > 4)
			{
				a = ParseHexChar(trimmedColor[1]);
				a = a + a*16;
				r = ParseHexChar(trimmedColor[2]);
				r = r + r*16;
				g = ParseHexChar(trimmedColor[3]);
				g = g + g*16;
				b = ParseHexChar(trimmedColor[4]);
				b = b + b*16;
			}
			else
			{
				r = ParseHexChar(trimmedColor[1]);
				r = r + r*16;
				g = ParseHexChar(trimmedColor[2]);
				g = g + g*16;
				b = ParseHexChar(trimmedColor[3]);
				b = b + b*16;
			}
				
			return Color.FromArgb ((byte)a, (byte)r, (byte)g, (byte)b);
		}
			
		public static Color ConvertFromHex ( string hexValue ) {
			return ParseHexColor (hexValue);
		}

		internal static Color Parse (string value, ITypeDescriptorContext context) {
			if (value == null)
				throw new ArgumentNullException ();

			if (value.StartsWith ("#")) {
				return ColorHelper.ConvertFromHex (value);
			} else {
				return ColorHelper.GetNamedColor (value);
			}
		}

		public static Microsoft.Xna.Framework.Color ToXnaColor ( System.Windows.Media.Color color ) {
			var c = new Microsoft.Xna.Framework.Color ();
			c.PackedValue = color.PackedValue;
			return c;
		}
			
		static Dictionary<String, Color> NamedColors;

		private const int s_zeroChar = (int) '0';
		private const int s_aLower   = (int) 'a';
		private const int s_aUpper   = (int) 'A';
	}
}
