using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Collections;

using Microsoft.Xna.Framework;

namespace System.Windows {
	public class PointConverter : TypeConverter {
        public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            } else if ( sourceType == typeof ( Vector2 ) ) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
			
        public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(Vector2))
                return true;
            else if (destinationType == typeof(String))
                return true;

            return base.CanConvertTo(context, destinationType);
        }
			
        public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value) {
            string strValue = value as string;
            if (strValue != null) {
                string text = strValue.Trim();
                if (text.Length == 0) {
                    return null;
                } else {
                    char sep = CultureInfo.InvariantCulture.TextInfo.ListSeparator[0];
                    string[] tokens = text.Split(new char[] { sep });
                    float[] values = new float[tokens.Length];

                    for (int i = 0; i < values.Length; i++) {
                        values[i] = ( float ) Convert.ChangeType(
                            tokens[i], typeof(Single), CultureInfo.InvariantCulture);
                    }
                    if (values.Length == 2) {
                        return new Point(values[0], values[1]);
                    } else {
                        throw new ArgumentException();
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }


        public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType == null) {
                throw new ArgumentNullException("destinationType");
            }
            if (value is Point) {
                if (destinationType == typeof(string)) {
                    Point pt = (Point)value;
                    if (culture == null) {
                        culture = CultureInfo.CurrentCulture;
                    }
                    string sep = culture.TextInfo.ListSeparator + " ";
                    TypeConverter intConverter = TypeDescriptor.GetConverter(typeof(int));
                    string[] args = new string[2];
                    int nArg = 0;

                    // Note: ConvertToString will raise exception if value cannot be converted.
                    args[nArg++] = intConverter.ConvertToString(pt.X);
                    args[nArg++] = intConverter.ConvertToString(pt.Y);

                    return string.Join(sep, args);
                }
                /*
                if (destinationType == typeof(InstanceDescriptor)) {
                    Point pt = (Point)value;

                    ConstructorInfo ctor = typeof(Point).GetConstructor(new Type[] { typeof(int), typeof(int) });
                    if (ctor != null) {
                        return new InstanceDescriptor(ctor, new object[] { pt.X, pt.Y });
                    }
                }
                 * */
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /*
        /// <include file='doc\PointConverter.uex' path='docs/doc[@for="PointConverter.CreateInstance"]/*' />
        /// <devdoc>
        ///      Creates an instance of this type given a set of property values
        ///      for the object.  This is useful for objects that are immutable, but still
        ///      want to provide changable properties.
        /// </devdoc>
        public override object CreateInstance (ITypeDescriptorContext context, IDictionary propertyValues) {
            if (propertyValues == null) {
                throw new ArgumentNullException("propertyValues");
            }

            object x = propertyValues["X"];
            object y = propertyValues["Y"];

            if (x == null || y == null ||
                !(x is int) || !(y is int)) {
                throw new ArgumentException();
            }
            return
                new Point((int)x, (int)y);

        }

        /// <include file='doc\PointConverter.uex' path='docs/doc[@for="PointConverter.GetCreateInstanceSupported"]/*' />
        /// <devdoc>
        ///      Determines if changing a value on this object should require a call to
        ///      CreateInstance to create a new value.
        /// </devdoc>
        public override bool GetCreateInstanceSupported (ITypeDescriptorContext context) {
            return true;
        }

        /// <include file='doc\PointConverter.uex' path='docs/doc[@for="PointConverter.GetPropertiesSupported"]/*' />
        /// <devdoc>
        ///      Determines if this object supports properties.  By default, this
        ///      is false.
        /// </devdoc>
        public override bool GetPropertiesSupported (ITypeDescriptorContext context) {
            return true;
        }
         * */
	}
}
