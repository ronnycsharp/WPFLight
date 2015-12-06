using System.Globalization;
using System.ComponentModel;

namespace System.Windows.Media {
	public class FontFamilyConverter : TypeConverter {
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType) {
			if (sourceType == typeof(string)) {
				return true;
			} 
			return base.CanConvertFrom (
				context, sourceType);
		}

		public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType) {
			if (destinationType == typeof(FontFamily))
				return true;
			else if (destinationType == typeof(String))
				return true;

			return base.CanConvertTo (
				context, destinationType);
		}

		public override object ConvertFrom (
			ITypeDescriptorContext context, CultureInfo culture, object value) {
			string strValue = value as string;
			if (!String.IsNullOrEmpty (strValue))
				return new FontFamily (strValue);

			return base.ConvertFrom (
				context, culture, value);
		}

		public override object ConvertTo (
			ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			if (value is FontFamily
			    	&& value != null
			    	&& destinationType == typeof(string))
				return ((FontFamily)value).Source;
		
			return base.ConvertTo (
				context, culture, value, destinationType);
		}
	}
}