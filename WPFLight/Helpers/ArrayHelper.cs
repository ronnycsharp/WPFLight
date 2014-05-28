using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;

namespace WPFLight.Helpers {
    internal static class ArrayHelper {
        public static void ArrayFill<T> (T[] arrayToFill, T fillValue) {
            // if called with a single value, wrap the value in an array and call the main function
            ArrayFill<T>(
                arrayToFill, new T[] { fillValue });
        }

        public static void ArrayFill<T> (T[] arrayToFill, T[] fillValue) {
            if (fillValue.Length >= arrayToFill.Length)
                throw new ArgumentException("fillValue array length must be smaller than length of arrayToFill");

            // set the initial array value
            Array.Copy(fillValue, arrayToFill, fillValue.Length);
            var arrayToFillHalfLength = arrayToFill.Length / 2;
            for (var i = fillValue.Length; i < arrayToFill.Length; i *= 2) {
                var copyLength = i;
                if (i > arrayToFillHalfLength)
                    copyLength = arrayToFill.Length - i;

                Array.Copy(
                    arrayToFill, 0, arrayToFill, i, copyLength);
            }
        }
    }
}