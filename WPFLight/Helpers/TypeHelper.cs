using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace WPFLight.Helpers {
	internal static class TypeHelper {
        public static System.Type GetType ( string typeName, bool throwOnError ) {
            if ( String.IsNullOrEmpty ( typeName ) )
                throw new ArgumentNullException ( "typeName" );

            var result = default ( Type );
#if WIN8
            // In WinRT the Type.GetType-Method needs the full namespace to get the type.

            // check if the namespace is included
            if ( typeName.Contains ( '.' ) ) {
                result = Type.GetType ( typeName, throwOnError );
            } else {
                var ti = typeof ( TypeHelper ).GetTypeInfo ( );
                var types = ti.Assembly.DefinedTypes.Where ( t => t.Name == typeName );

                if ( throwOnError )
                    result = types.First ( ).AsType ( );
                else {
                    var type = types.FirstOrDefault ( );
                    if ( type != null ) {
                        result = type.AsType ( );
                    }
                }
            }
#else
            result = System.Type.GetType ( typeName, throwOnError );                
#endif
            return result;
        }
    }
}

