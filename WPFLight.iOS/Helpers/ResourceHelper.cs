using System;
using System.Linq;
using System.Collections.Generic;

namespace WPFLight.Helpers {
	internal static class ResourceHelper {
		static ResourceHelper ( ) {
			Resources = new Dictionary<object, object> ();
		}

		public static object GetResourceKey ( object item ) {
			if ( Resources.ContainsValue ( item ) ) {
				foreach (var r in Resources)
					if (r.Value == item)
						return r.Key;
			}
			return null;
		}

		public static void SetResourceKey ( object key, object item ) {
			if (key == null || item == null )
				throw new ArgumentNullException ();

			// Prüfen ob der Key bereits verwendet wird
			if (Resources.ContainsKey (key) )
				throw new ArgumentException ("The key is already in use.");

			Resources [key] = item;
		}

		public static object GetResource ( object key ) {
			if (key == null)
				throw new ArgumentNullException ();

			if (key is Type )
				return Resources.Where (r => r.Key.Equals (key)).Select (r => r.Value).FirstOrDefault ();
			else
				return Resources.Where (r => r.Key.Equals (key)).Select (r => r.Value).First ();
		}

		static Dictionary<object, object> Resources;
    }
}

