using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.ComponentModel {
    public sealed class TypeDescriptor {
        public static TypeConverter GetConverter (Type type) {
            if (type == null)
                throw new ArgumentNullException("type");

            var attributes = type.GetCustomAttributes(
                typeof(TypeConverterAttribute), true);

            if (attributes.Length > 0) {
                return ( TypeConverter ) Activator.CreateInstance ( 
                        Type.GetType (
                        ((TypeConverterAttribute)attributes[0]).ConverterTypeName, true)
                    );
            }

            if (DefaultConverters.ContainsKey(type)) {
                return ( TypeConverter ) 
                    Activator.CreateInstance(DefaultConverters[type]);
            }
            return null;
        }

        static Dictionary<Type, Type> DefaultConverters {
            get {
                if ( defaultConverters == null ) {
                    defaultConverters = new Dictionary<Type,Type> ( );
                    /* 
                    defaultConverters.Add(typeof(bool), typeof(BooleanConverter));
                    defaultConverters.Add(typeof(byte), typeof(ByteConverter));
                    defaultConverters.Add(typeof(sbyte), typeof(SByteConverter));
                    defaultConverters.Add(typeof(string), typeof(StringConverter));
                    defaultConverters.Add(typeof(char), typeof(CharConverter));
                    defaultConverters.Add(typeof(short), typeof(Int16Converter));
                    defaultConverters.Add(typeof(int), typeof(Int32Converter));
                    defaultConverters.Add(typeof(long), typeof(Int64Converter));
                    defaultConverters.Add(typeof(ushort), typeof(UInt16Converter));
                    defaultConverters.Add(typeof(uint), typeof(UInt32Converter));
                    defaultConverters.Add(typeof(ulong), typeof(UInt64Converter));
                    defaultConverters.Add(typeof(float), typeof(SingleConverter));
                    defaultConverters.Add(typeof(double), typeof(DoubleConverter));
                    defaultConverters.Add(typeof(decimal), typeof(DecimalConverter));
                    defaultConverters.Add(typeof(void), typeof(TypeConverter));
                    defaultConverters.Add(typeof(Array), typeof(ArrayConverter));
                    defaultConverters.Add(typeof(CultureInfo), typeof(CultureInfoConverter));
                    defaultConverters.Add(typeof(DateTime), typeof(DateTimeConverter));
                    defaultConverters.Add(typeof(Guid), typeof(GuidConverter));
                    defaultConverters.Add(typeof(TimeSpan), typeof(TimeSpanConverter));
                    defaultConverters.Add(typeof(ICollection), typeof(CollectionConverter));
                    defaultConverters.Add(typeof(Enum), typeof(EnumConverter));
                     * */
                }
                return defaultConverters;
            }
        }

        static Dictionary<Type, Type> defaultConverters;
    }
}