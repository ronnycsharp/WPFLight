using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Media {
	public class BrushConverter : TypeConverter {
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType) {
			if (sourceType == typeof(String))
				return true;

			if (sourceType == typeof(Microsoft.Xna.Framework.Color))	// TODO Replace with System.Windows.Media.Color
				return true;

			return base.CanConvertFrom (context, sourceType);
		}

		public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value) {
			String source = value as string;
			if (source != null)
				return Brush.Parse(source, context);

			return base.ConvertFrom (context, culture, value);
		}

        public override object ConvertFrom ( object value ) {
            String source = value as string;
            if ( source != null )
                return Brush.Parse ( source, null );

            return base.ConvertFrom ( value );           
        }

		public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			return base.ConvertTo (context, culture, value, destinationType);
		}
	}
}
