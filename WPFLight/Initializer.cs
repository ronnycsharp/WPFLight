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
            var fileName = "WPFLight.Themes.Default.xaml";

            #if WIN8
                        var asm = typeof ( Initializer ).GetTypeInfo ( ).Assembly;
                        fileName = "WPFLight.Win8.Themes.Default.xaml";
            #else
                        var asm = Assembly.GetCallingAssembly ( );
            #endif


			using (Stream stream = asm.GetManifestResourceStream (fileName)){
				XamlReader.Load (stream);
			}
		}

		public static ResourceDictionary LoadResourceDictionary ( Stream stream ) {
			return ( ResourceDictionary ) XamlReader.Load (stream);
		}

		public static ResourceDictionary LoadResourceDictionary ( string resourceName ) {
			if (String.IsNullOrEmpty (resourceName) )
				throw new ArgumentException ();

#if WIN8
            var asm = typeof ( Initializer ).GetTypeInfo ( ).Assembly;
#else
            var asm = Assembly.GetCallingAssembly ( );
#endif
			using (Stream stream = asm.GetManifestResourceStream (resourceName)){
				return ( ResourceDictionary ) XamlReader.Load (stream);
			}
		}

        public static ResourceDictionary LoadResourceDictionary ( Assembly assembly, string resourceName ) {
            if ( String.IsNullOrEmpty ( resourceName ) )
                throw new ArgumentException ( "resourceName" );

            if ( assembly == null )
                throw new ArgumentNullException ( "assembly" );

            using ( Stream stream = assembly.GetManifestResourceStream ( resourceName ) ) {
                return ( ResourceDictionary ) XamlReader.Load ( stream );
            }   
        }
	}
}

