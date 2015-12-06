
using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace System.Windows.Media
{
    public static class Brushes
    {
		private static Dictionary<System.Windows.Media.Color,SolidColorBrush> brushes;

		internal static SolidColorBrush GetBrush ( System.Windows.Media.Color color ) {
            var result = default ( SolidColorBrush );
            if ( brushes == null ) {
				brushes = new Dictionary<Color, SolidColorBrush> ( );
            } else {
				if ( brushes.ContainsKey ( color ) )
					result = brushes[color];
            }
            if ( result == null ) {
				result = new SolidColorBrush ( color );
				brushes[color] = result;
            }
            return result;
        }

        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF0F8FF.
        //
        // Returns:
        //     A frozen System.Windows.Media.SolidColorBrush with a System.Windows.Media.SolidColorBrush.Colors
        //     of #FFF0F8FF.
        public static SolidColorBrush AliceBlue { 
            get {
                return Brushes.GetBrush ( Colors.AliceBlue );
            } 
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFAEBD7.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush AntiqueWhite {
            get {
                return Brushes.GetBrush ( Colors.AntiqueWhite );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00FFFF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Aqua {
            get {
                return Brushes.GetBrush ( Colors.Aqua );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF7FFFD4.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Aquamarine {
            get {
                return Brushes.GetBrush ( Colors.Aquamarine);
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF0FFFF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Azure {
            get {
				return Brushes.GetBrush ( Colors.Azure );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF5F5DC.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Beige {
            get {
                return Brushes.GetBrush ( Colors.Beige );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFE4C4.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Bisque {
            get {
                return Brushes.GetBrush ( Colors.Bisque );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF000000.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Black {
            get {
                return Brushes.GetBrush ( Colors.Black );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFEBCD.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush BlanchedAlmond {
            get {
                return Brushes.GetBrush ( Colors.BlanchedAlmond );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF0000FF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Blue {
            get {
                return Brushes.GetBrush ( Colors.Blue );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF8A2BE2.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush BlueViolet {
            get {
                return Brushes.GetBrush ( Colors.BlueViolet );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFA52A2A.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Brown {
            get {
                return Brushes.GetBrush ( Colors.Brown );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFDEB887.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush BurlyWood {
            get {
                return Brushes.GetBrush ( Colors.BurlyWood );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF5F9EA0.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush CadetBlue {
            get {
                return Brushes.GetBrush ( Colors.CadetBlue );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF7FFF00.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Chartreuse {
            get {
                return Brushes.GetBrush ( Colors.Chartreuse );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFD2691E.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Chocolate {
            get {
                return Brushes.GetBrush ( Colors.Chocolate );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF7F50.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Coral {
            get {
                return Brushes.GetBrush ( Colors.Coral );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF6495ED.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush CornflowerBlue {
            get {
                return Brushes.GetBrush ( Colors.CornflowerBlue );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFF8DC.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Cornsilk {
            get {
                return Brushes.GetBrush ( Colors.Cornsilk );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFDC143C.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Crimson {
            get {
                return Brushes.GetBrush ( Colors.Crimson );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00FFFF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Cyan {
            get {
                return Brushes.GetBrush ( Colors.Cyan );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00008B.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkBlue {
            get {
                return Brushes.GetBrush ( Colors.DarkBlue );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF008B8B.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkCyan {
            get {
                return Brushes.GetBrush ( Colors.DarkCyan );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFB8860B.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkGoldenrod {
            get {
                return Brushes.GetBrush ( Colors.DarkGoldenrod );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFA9A9A9.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkGray {
            get {
                return Brushes.GetBrush ( Colors.DarkGray );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF006400.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkGreen {
            get {
                return Brushes.GetBrush ( Colors.DarkGreen );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFBDB76B.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkKhaki {
            get {
                return Brushes.GetBrush ( Colors.DarkKhaki );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF8B008B.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkMagenta {
            get {
                return Brushes.GetBrush ( Colors.DarkMagenta );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF556B2F.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkOliveGreen {
            get {
                return Brushes.GetBrush ( Colors.DarkOliveGreen );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF8C00.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkOrange {
            get {
                return Brushes.GetBrush ( Colors.DarkOrange );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF9932CC.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkOrchid {
            get {
                return Brushes.GetBrush ( Colors.DarkOrchid );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF8B0000.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkRed {
            get {
                return Brushes.GetBrush ( Colors.DarkRed );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFE9967A.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkSalmon {
            get {
                return Brushes.GetBrush ( Colors.DarkSalmon );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF8FBC8F.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkSeaGreen {
            get {
                return Brushes.GetBrush ( Colors.DarkSeaGreen );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF483D8B.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkSlateBlue {
            get {
                return Brushes.GetBrush ( Colors.DarkSlateBlue );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF2F4F4F.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkSlateGray {
            get {
                return Brushes.GetBrush ( Colors.DarkSlateGray );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00CED1.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkTurquoise {
            get {
                return Brushes.GetBrush ( Colors.DarkTurquoise );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF9400D3.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DarkViolet {
            get {
                return Brushes.GetBrush ( Colors.DarkViolet );
            }
        }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF1493.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DeepPink { get { return Brushes.GetBrush ( Colors.DeepPink ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00BFFF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DeepSkyBlue { get { return Brushes.GetBrush ( Colors.DeepSkyBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF696969.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DimGray { get { return Brushes.GetBrush ( Colors.DimGray ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF1E90FF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush DodgerBlue { get { return Brushes.GetBrush ( Colors.DodgerBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFB22222.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Firebrick { get { return Brushes.GetBrush ( Colors.Firebrick ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFFAF0.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush FloralWhite { get { return Brushes.GetBrush ( Colors.FloralWhite ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF228B22.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush ForestGreen { get { return Brushes.GetBrush ( Colors.ForestGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF00FF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Fuchsia { get { return Brushes.GetBrush ( Colors.Fuchsia ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFDCDCDC.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Gainsboro { get { return Brushes.GetBrush ( Colors.Gainsboro ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF8F8FF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush GhostWhite { get { return Brushes.GetBrush ( Colors.GhostWhite ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFD700.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Gold { get { return Brushes.GetBrush ( Colors.Gold ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFDAA520.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Goldenrod { get { return Brushes.GetBrush ( Colors.Goldenrod ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF808080.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Gray { get { return Brushes.GetBrush ( Colors.Gray ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF008000.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Green { get { return Brushes.GetBrush ( Colors.Green ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFADFF2F.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush GreenYellow { get { return Brushes.GetBrush ( Colors.GreenYellow ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF0FFF0.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Honeydew { get { return Brushes.GetBrush ( Colors.Honeydew ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF69B4.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush HotPink { get { return Brushes.GetBrush ( Colors.HotPink ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFCD5C5C.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush IndianRed { get { return Brushes.GetBrush ( Colors.IndianRed ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF4B0082.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Indigo { get { return Brushes.GetBrush ( Colors.Indigo ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFFFF0.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Ivory { get { return Brushes.GetBrush ( Colors.Ivory ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF0E68C.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Khaki { get { return Brushes.GetBrush ( Colors.Khaki ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFE6E6FA.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Lavender { get { return Brushes.GetBrush ( Colors.Lavender ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFF0F5.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LavenderBlush { get { return Brushes.GetBrush ( Colors.LavenderBlush ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF7CFC00.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LawnGreen { get { return Brushes.GetBrush ( Colors.LawnGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFFACD.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LemonChiffon { get { return Brushes.GetBrush ( Colors.LemonChiffon ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFADD8E6.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightBlue { get { return Brushes.GetBrush ( Colors.LightBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF08080.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightCoral { get { return Brushes.GetBrush ( Colors.LightCoral ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFE0FFFF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightCyan { get { return Brushes.GetBrush ( Colors.LightCyan ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFAFAD2.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightGoldenrodYellow { get { return Brushes.GetBrush ( Colors.LightGoldenrodYellow ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFD3D3D3.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightGray { get { return Brushes.GetBrush ( Colors.LightGray ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF90EE90.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightGreen { get { return Brushes.GetBrush ( Colors.LightGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFB6C1.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightPink { get { return Brushes.GetBrush ( Colors.LightPink ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFA07A.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightSalmon { get { return Brushes.GetBrush ( Colors.LightSalmon ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF20B2AA.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightSeaGreen { get { return Brushes.GetBrush ( Colors.LightSeaGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF87CEFA.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightSkyBlue { get { return Brushes.GetBrush ( Colors.LightSkyBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF778899.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightSlateGray { get { return Brushes.GetBrush ( Colors.LightSlateGray ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFB0C4DE.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightSteelBlue { get { return Brushes.GetBrush ( Colors.LightSteelBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFFFE0.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LightYellow { get { return Brushes.GetBrush ( Colors.LightYellow ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00FF00.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Lime { get { return Brushes.GetBrush ( Colors.Lime ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF32CD32.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush LimeGreen { get { return Brushes.GetBrush ( Colors.LimeGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFAF0E6.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Linen { get { return Brushes.GetBrush ( Colors.Linen  ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF00FF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Magenta { get { return Brushes.GetBrush ( Colors.Magenta ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF800000.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Maroon { get { return Brushes.GetBrush ( Colors.Maroon ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF66CDAA.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumAquamarine { get { return Brushes.GetBrush ( Colors.MediumAquamarine ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF0000CD.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumBlue { get { return Brushes.GetBrush ( Colors.MediumBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFBA55D3.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumOrchid { get { return Brushes.GetBrush ( Colors.MediumOrchid ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF9370DB.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumPurple { get { return Brushes.GetBrush ( Colors.MediumPurple ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF3CB371.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumSeaGreen { get { return Brushes.GetBrush ( Colors.MediumSeaGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF7B68EE.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumSlateBlue { get { return Brushes.GetBrush ( Colors.MediumSlateBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00FA9A.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumSpringGreen { get { return Brushes.GetBrush ( Colors.MediumSpringGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF48D1CC.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumTurquoise { get { return Brushes.GetBrush ( Colors.MediumTurquoise ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFC71585.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MediumVioletRed { get { return Brushes.GetBrush ( Colors.MediumVioletRed ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF191970.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MidnightBlue { get { return Brushes.GetBrush ( Colors.MidnightBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF5FFFA.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MintCream { get { return Brushes.GetBrush ( Colors.MintCream ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFE4E1.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush MistyRose { get { return Brushes.GetBrush ( Colors.MistyRose ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFE4B5.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Moccasin { get { return Brushes.GetBrush ( Colors.Moccasin ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFDEAD.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush NavajoWhite { get { return Brushes.GetBrush ( Colors.NavajoWhite ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF000080.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Navy { get { return Brushes.GetBrush ( Colors.Navy ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFDF5E6.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush OldLace { get { return Brushes.GetBrush ( Colors.OldLace ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF808000.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Olive { get { return Brushes.GetBrush ( Colors.Olive ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF6B8E23.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush OliveDrab { get { return Brushes.GetBrush ( Colors.OliveDrab ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFA500.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Orange { get { return Brushes.GetBrush ( Colors.Orange ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF4500.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush OrangeRed { get { return Brushes.GetBrush ( Colors.OrangeRed ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFDA70D6.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Orchid { get { return Brushes.GetBrush ( Colors.Orchid ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFEEE8AA.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush PaleGoldenrod { get { return Brushes.GetBrush ( Colors.PaleGoldenrod ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF98FB98.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush PaleGreen { get { return Brushes.GetBrush ( Colors.PaleGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFAFEEEE.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush PaleTurquoise { get { return Brushes.GetBrush ( Colors.PaleTurquoise ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFDB7093.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush PaleVioletRed { get { return Brushes.GetBrush ( Colors.PaleVioletRed ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFEFD5.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush PapayaWhip { get { return Brushes.GetBrush ( Colors.PapayaWhip ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFDAB9.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush PeachPuff { get { return Brushes.GetBrush ( Colors.PeachPuff ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFCD853F.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Peru { get { return Brushes.GetBrush ( Colors.Peru ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFC0CB.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Pink { get { return Brushes.GetBrush ( Colors.Pink ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFDDA0DD.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Plum { get { return Brushes.GetBrush ( Colors.Plum ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFB0E0E6.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush PowderBlue { get { return Brushes.GetBrush ( Colors.PowderBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF800080.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Purple { get { return Brushes.GetBrush ( Colors.Purple ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF0000.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Red { get { return Brushes.GetBrush ( Colors.Red ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFBC8F8F.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush RosyBrown { get { return Brushes.GetBrush ( Colors.RosyBrown ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF4169E1.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush RoyalBlue { get { return Brushes.GetBrush ( Colors.RoyalBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF8B4513.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SaddleBrown { get { return Brushes.GetBrush ( Colors.SaddleBrown ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFA8072.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Salmon { get { return Brushes.GetBrush ( Colors.Salmon ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF4A460.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SandyBrown { get { return Brushes.GetBrush ( Colors.SandyBrown ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF2E8B57.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SeaGreen { get { return Brushes.GetBrush ( Colors.SeaGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFF5EE.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SeaShell { get { return Brushes.GetBrush ( Colors.SeaShell ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFA0522D.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Sienna { get { return Brushes.GetBrush ( Colors.Sienna ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFC0C0C0.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Silver { get { return Brushes.GetBrush ( Colors.Silver ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF87CEEB.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SkyBlue { get { return Brushes.GetBrush ( Colors.SkyBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF6A5ACD.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SlateBlue { get { return Brushes.GetBrush ( Colors.SlateBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF708090.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SlateGray { get { return Brushes.GetBrush ( Colors.SlateGray ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFFAFA.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Snow { get { return Brushes.GetBrush ( Colors.Snow ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF00FF7F.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SpringGreen { get { return Brushes.GetBrush ( Colors.SpringGreen ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF4682B4.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush SteelBlue { get { return Brushes.GetBrush ( Colors.SteelBlue ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFD2B48C.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Tan { get { return Brushes.GetBrush ( Colors.Tan ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF008080.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Teal { get { return Brushes.GetBrush ( Colors.Teal ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFD8BFD8.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Thistle { get { return Brushes.GetBrush ( Colors.Thistle ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFF6347.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Tomato { get { return Brushes.GetBrush ( Colors.Tomato ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #00FFFFFF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Transparent { get { return Brushes.GetBrush ( Colors.Transparent ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF40E0D0.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Turquoise { get { return Brushes.GetBrush ( Colors.Turquoise ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFEE82EE.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Violet { get { return Brushes.GetBrush ( Colors.Violet ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF5DEB3.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Wheat { get { return Brushes.GetBrush ( Colors.Wheat ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFFFFF.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush White { get { return Brushes.GetBrush ( Colors.White ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFF5F5F5.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush WhiteSmoke { get { return Brushes.GetBrush ( Colors.WhiteSmoke ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FFFFFF00.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush Yellow { get { return Brushes.GetBrush ( Colors.Yellow ); } }
        //
        // Summary:
        //     Gets the solid fill Colors that has a hexadecimal value of #FF9ACD32.
        //
        // Returns:
        //     A solid fill Colors.
        public static SolidColorBrush YellowGreen { get { return Brushes.GetBrush ( Colors.YellowGreen ); } }
    }
}
