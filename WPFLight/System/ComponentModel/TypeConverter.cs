using System.Globalization;
namespace System.ComponentModel {
    public abstract class TypeConverter {
        public virtual bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType ) {
            return false;
        }

        public virtual bool CanConvertTo ( ITypeDescriptorContext context, Type destinationType ) {
            return false;
        }

        public virtual object ConvertFrom ( ITypeDescriptorContext context, CultureInfo culture, object value ) {
            return null;
        }

        public virtual object ConvertFrom (  object value ) {
            return null;
        }

        public virtual object ConvertTo ( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {
            return Convert.ChangeType ( value, destinationType, culture );
        }

        public virtual string ConvertToString ( object value ) {
            return null;
        }
    }
}