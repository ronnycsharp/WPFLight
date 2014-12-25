using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
namespace WPFLight.Extensions {
    public static class EnumHelpers {
        private static void CheckIsEnum<T> (bool withFlags) {
#if WIN8
            var t = typeof ( T ).GetTypeInfo ( );
#else
            var t = typeof ( T );
#endif
            if (!t.IsEnum)
                throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
            if (withFlags && !t.IsDefined(typeof(FlagsAttribute)))
                throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
        }

        public static bool IsFlagSet<T> (this T value, T flag) where T : struct {
            CheckIsEnum<T>(true);
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flag);
            return (lValue & lFlag) != 0;
        }

        public static IEnumerable<T> GetFlags<T> (this T value) where T : struct {
            CheckIsEnum<T>(true);

#if WINDOWS_PHONE
            foreach (T flag in GetValues<T> ( )) {
#else
            foreach (T flag in Enum.GetValues(typeof(T)).Cast<T>()) {
#endif
                if (value.IsFlagSet(flag))
                    yield return flag;
            }
        }

        public static T SetFlags<T> (this T value, T flags, bool on) where T : struct {
            CheckIsEnum<T>(true);
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flags);
            if (on) {
                lValue |= lFlag;
            } else {
                lValue &= (~lFlag);
            }
            return (T)Enum.ToObject(typeof(T), lValue);
        }

        public static T SetFlags<T> (this T value, T flags) where T : struct {
            return value.SetFlags(flags, true);
        }

        public static T ClearFlags<T> (this T value, T flags) where T : struct {
            return value.SetFlags(flags, false);
        }

        public static T CombineFlags<T> (this IEnumerable<T> flags) where T : struct {
            CheckIsEnum<T>(true);
            long lValue = 0;
            foreach (T flag in flags) {
                long lFlag = Convert.ToInt64(flag);
                lValue |= lFlag;
            }
            return (T)Enum.ToObject(typeof(T), lValue);
        }

#if WINDOWS_PHONE

        /*
         * Enum.GetValues doesn't exist in Windows Phone 7, so I need to add the same functionality
         * */

        public static T[] GetValues<T> () {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

            FieldInfo[] fields = enumType.GetFields();
            int literalCount = 0;
            for (int i = 0; i < fields.Length; i++)
                if (fields[i].IsLiteral == true)
                    literalCount++;

            T[] arr = new T[literalCount];
            int pos = 0;
            for (int i = 0; i < fields.Length; i++)
                if (fields[i].IsLiteral == true) {
                    arr[pos] = (T)fields[i].GetValue(enumType);
                    pos++;
                }

            return arr;
        }
#endif
    }
}