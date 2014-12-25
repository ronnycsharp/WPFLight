using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Xna.Framework.Graphics;

namespace WPFLight {
	public static class Initializer {

		#region Properties

		public static GraphicsDevice CurrentDevice { get; private set; }

		#endregion

		public static void Setup ( GraphicsDevice device ) {
			CurrentDevice = device;
		}

		/// <summary>
		/// load resource-dictionary and set default styles
		/// </summary>
		public static void Load () {
            #if WIN8
                        var asm = typeof ( Initializer ).GetTypeInfo ( ).Assembly;
            #else
                        var asm = Assembly.GetCallingAssembly ( );
            #endif

			using (Stream stream = asm.GetManifestResourceStream ("WPFLight.Themes.Default.xaml")){
				XamlReader.Load (stream);
			}
		}

		public static ResourceDictionary LoadResourceDictionary ( Stream stream ) {
			return ( ResourceDictionary ) XamlReader.Load (stream);
		}

		public static ResourceDictionary LoadResourceDictionary ( string resourceId ) {
			if (String.IsNullOrEmpty (resourceId))
				throw new ArgumentException ();

#if WIN8
            var asm = resourceId.GetType ( ).GetTypeInfo ( ).Assembly;
#else
            var asm = Assembly.GetCallingAssembly ( );
#endif

			using (Stream stream = asm.GetManifestResourceStream (resourceId)){
				return ( ResourceDictionary ) XamlReader.Load (stream);
			}
		}
	}
}

