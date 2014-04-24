using System.ComponentModel;
//using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Windows {
	public class ThicknessConverter : TypeConverter {
		public override bool CanConvertFrom (ITypeDescriptorContext typeDescriptorContext, Type sourceType) {
			// We can only handle strings, integral and floating types
			TypeCode tc = Type.GetTypeCode (sourceType);
			switch (tc) {
				case TypeCode.String:
				case TypeCode.Decimal:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return true;

				default:
					return false;
			}
		}

		public override bool CanConvertTo (ITypeDescriptorContext typeDescriptorContext, Type destinationType) {
			return destinationType == typeof(String)/*
			|| destinationType == typeof(InstanceDescriptor)*/;
		}

		public override object ConvertFrom (ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source) {
			if (source != null) {
				if (source is string) {
					return FromString ((string)source, cultureInfo);
				} else if (source is float) {
					return new Thickness ((float)source);
				} else {
					return new Thickness (Convert.ToSingle (source, cultureInfo));
				}
			}
			throw new Exception ("Cannot convert type");
		}

		public override object ConvertTo (ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType) {
			if (null == value) {
				throw new ArgumentNullException ("value");
			}

			if (null == destinationType) {
				throw new ArgumentNullException ("destinationType");
			}

			if (!(value is Thickness)) {
				throw new ArgumentException ();
			}

			Thickness th = (Thickness)value;
			if (destinationType == typeof(string)) {
				return ToString (th, cultureInfo);
			}
            /*
			if (destinationType == typeof(InstanceDescriptor)) {
				ConstructorInfo ci = typeof(Thickness).GetConstructor (new Type[] {
					typeof(double),
					typeof(double),
					typeof(double),
					typeof(double)
				});
				return new InstanceDescriptor (
					ci, new object[] { th.Left, th.Top, th.Right, th.Bottom });
			}
             * */
			throw new Exception ("Cannot convert type");
		}

		static internal string ToString(Thickness th, CultureInfo cultureInfo)
		{
			char listSeparator = ',';

			// Initial capacity [64] is an estimate based on a sum of:
			// 48 = 4x double (twelve digits is generous for the range of values likely)
			//  8 = 4x Unit Type string (approx two characters)
			//  4 = 4x separator characters
			StringBuilder sb = new StringBuilder(64);

			sb.Append(th.Left.ToString ( cultureInfo ));
			sb.Append(listSeparator);
			sb.Append(th.Top.ToString ( cultureInfo ));
			sb.Append(listSeparator);
			sb.Append(th.Right.ToString ( cultureInfo ));
			sb.Append(listSeparator);
			sb.Append (th.Bottom.ToString (cultureInfo));

			return sb.ToString();
		}

		static internal Thickness FromString (string s, CultureInfo cultureInfo) {
			if (String.IsNullOrEmpty (s))
				throw new ArgumentException ();

			if (s.Contains (",")) {
				var values = s.Split (',');
				return new Thickness (
					float.Parse (values [0]),
					float.Parse (values [1]),
					float.Parse (values [2]),
					float.Parse (values [3]));
			} else {
				return new Thickness (
					float.Parse (s));
			}
		}
	}
}
