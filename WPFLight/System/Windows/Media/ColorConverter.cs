using System.ComponentModel;
using System.Globalization;
//using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace System.Windows.Media {
	public sealed class ColorConverter : TypeConverter {
		public override bool CanConvertFrom (ITypeDescriptorContext td, Type t) {
			if (t == typeof(string)) {
				return true;
			} else {
				return false;
			}
		}

		public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType) {
			return base.CanConvertTo (context, destinationType);
		}

		public static new object ConvertFromString (string value) {
			if (null == value) {
				return null;
			}

			return ColorHelper.Parse (value, null);
		}

		public override object ConvertFrom (ITypeDescriptorContext td, System.Globalization.CultureInfo ci, object value) {
			if (value == null)
				throw new ArgumentNullException ();
				
			var s = value as string;
			if (s == null)
				throw new ArgumentException ();
				
			return ColorHelper.Parse (s, td);
		}

		public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			/*
            if (destinationType != null && value is Color) {
				if (destinationType == typeof(InstanceDescriptor)) {
					MethodInfo mi = typeof(Color).GetMethod ("FromArgb", new Type[] {
						typeof(byte),
						typeof(byte),
						typeof(byte),
						typeof(byte)
					});
					Color c = (Color)value;
					return new InstanceDescriptor (
						mi, new object[]{ c.A, c.R, c.G, c.B });
				} else if (destinationType == typeof(string)) {
					Color c = (Color)value;
					return c.ToString ();
				}
			}
             * */
			return base.ConvertTo (context, culture, value, destinationType);
		}

	}
}
