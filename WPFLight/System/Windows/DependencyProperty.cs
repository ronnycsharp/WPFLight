using System.Collections.Generic;
namespace System.Windows {
    public sealed class DependencyProperty {
        static DependencyProperty () {
            registeredProperties = new List<DependencyProperty>();
			attachedProperties = new List<DependencyProperty>();
        }

        DependencyProperty ( 
			string propertyName, Type propertyType, Type ownerType ) {

            this.Name = propertyName;
            this.PropertyType = propertyType;
            this.OwnerType = ownerType;
        }

		DependencyProperty ( 
			string propertyName, 
			Type propertyType, 
			Type ownerType, 
			PropertyMetadata metadata ) {

			this.Name = propertyName;
			this.PropertyType = propertyType;
			this.OwnerType = ownerType;
			this.DefaultMetadata = metadata;
		}


        #region Eigenschaften

		public string   		Name            { get; private set; }
		public Type     		PropertyType    { get; private set; }
		public Type     		OwnerType       { get; private set; }
		public PropertyMetadata DefaultMetadata { get; private set; }

        #endregion

        public static DependencyProperty Register ( string name, Type propertyType, Type ownerType ) {
			return Register (name, propertyType, ownerType, null);
        }

		public static DependencyProperty Register ( string name, Type propertyType, Type ownerType, PropertyMetadata metadata ) {
			var prop = new DependencyProperty(name, propertyType, ownerType,metadata);
			registeredProperties.Add(prop);
			return prop;
		}

		public static DependencyProperty RegisterAttached ( string name, Type propertyType, Type ownerType ) {
			return RegisterAttached (name, propertyType, ownerType, null);
        }
		
        public static DependencyProperty RegisterAttached (string name, Type propertyType, Type ownerType, PropertyMetadata metadata) {
            var prop = new DependencyProperty(name, propertyType, ownerType, metadata);
            attachedProperties.Add(prop);
            return prop;
        }

        internal static bool IsRegisteredProperty (string propertyName, Type ownerType) {
            foreach (var prop in registeredProperties)
                if (prop.OwnerType == ownerType && prop.Name == propertyName)
                    return true;

            return false;
        }

		public static readonly object UnsetValue = new object();

        private static List<DependencyProperty> registeredProperties;
        private static List<DependencyProperty> attachedProperties;
    }
}