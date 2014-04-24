using System.Collections.Generic;
namespace System.Windows {
	public class PropertyMetadata {
		public PropertyMetadata ( ) { }

		public PropertyMetadata ( object defaultValue ) {
			this.DefaultValue = defaultValue;
		}

		public PropertyMetadata ( object defaultValue, PropertyChangedCallback callback ) {
			this.DefaultValue = defaultValue;
			this.PropertyChangedCallback = callback;
		}

		public PropertyMetadata ( PropertyChangedCallback callback ) {
			this.PropertyChangedCallback = callback;
		}

        #region Eigenschaften

		public PropertyChangedCallback 	PropertyChangedCallback { get; set; }
		public object 					DefaultValue 			{ get; set; }

        #endregion
    }
}