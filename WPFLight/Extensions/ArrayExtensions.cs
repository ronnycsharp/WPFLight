using WPFLight.Helpers;
namespace WPFLight.Extensions {
    internal static class ArrayExtensions {
        public static void Fill<T> (this T[] array, T value) {
            ArrayHelper.ArrayFill(array, value);
        }

        public static void Fill<T> (this T[] array, T[] values) {
            ArrayHelper.ArrayFill(array, values);
        }
    }
}