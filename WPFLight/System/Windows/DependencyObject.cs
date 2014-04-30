using System.Collections.Generic;
using System.Linq;
namespace System.Windows {
	public abstract class DependencyObject {
        public object GetValue (DependencyProperty property) {
			
			
	
            if (properties != null
                    && properties.ContainsKey(property))
                return properties[property];
				
			

			if (property.DefaultMetadata != null
			    	&& property.DefaultMetadata.DefaultValue != null)
				return property.DefaultMetadata.DefaultValue;

			return GetDefault (property.PropertyType);
        }

		static object GetDefault(Type type) {
			return type.IsValueType 
				? Activator.CreateInstance(type) : null;
		}

		protected void SetValue ( DependencyProperty property, object value, bool setAsAssigned ) {
			if (properties == null)
				properties = new Dictionary<DependencyProperty, object>();

			// Zieltyp prüfen
			if ( value != null && !property.PropertyType.IsAssignableFrom (value.GetType ()) )
				throw new InvalidCastException ();

			var oldValue = properties.ContainsKey (property) ? properties [property] : null;
			if (( oldValue == null && value != null ) 
				|| ( oldValue != null && value == null ) 
				|| ( oldValue != null && value != null && !oldValue.Equals ( value ) ) ) {

				OnPropertyChanging (property, oldValue, value);
				properties [property] = value;
				OnPropertyChanged (property, oldValue, value);

				if (setAsAssigned) {
					// Eigenschaft als gesetzt markieren, 
					// AssignedProperties können nicht mehr vom Style überschrieben werden
					SetPropertyAsAssigned (property);
				}

				// TODO DefaultValue Metadata...

				if (property.DefaultMetadata != null
					&& property.DefaultMetadata.PropertyChangedCallback != null)
					property.DefaultMetadata.PropertyChangedCallback (
						this, new DependencyPropertyChangedEventArgs (
							property, oldValue, value));
			}
		}

        public void SetValue (DependencyProperty property, object value) {
			var frameworkElement = this as FrameworkElement;
			var setAsAssigned = frameworkElement != null 
				&& !frameworkElement.IsInitialized;
				
			SetValue (property, value, setAsAssigned);
        }

		protected virtual void OnPropertyChanging ( 
			DependencyProperty dp, object oldValue, object newValue ) { }

		protected virtual void OnPropertyChanged ( 
			DependencyProperty dp, object oldValue, object newValue ) { }

		/// <summary>
		/// Gibt true zurück, wenn die zugehörige DependencyProperty vor dem initialisieren zugewiesen wurde,
		/// - Wenn zugewiesen, kann die DependencyProperty nicht mehr über einen Style verändert werden.
		/// </summary>
		internal bool IsAssignedProperty ( DependencyProperty dp ) {
			return propertySet != null && propertySet.Contains (dp);
		}

		/// <summary>
		/// Gibt den aktuellen Eigenschaftswert zurück
		/// </summary>
		/// <returns>The current value.</returns>
		/// <param name="propertyName">Property name.</param>
		protected object GetCurrentValue ( string propertyName ) {
			var propertyInfo = this.GetType().GetProperty(propertyName);
			if (propertyInfo != null) {
				return propertyInfo.GetValue (this,null);
			}
			return null;
		}

		protected DependencyProperty GetPropertyOrNull ( string name ) {
			return properties != null 
				? properties
					.Where (p => p.Key.Name == name)
					.Select ( p => p.Key )
					.FirstOrDefault ( ) 
				: null;
		}

		/// <summary>
		/// Eigenschaft als gesetzt markieren
		/// </summary>
		/// <param name="property">Property.</param>
		protected void SetPropertyAsAssigned ( DependencyProperty property ) {
            if (propertySet == null) {
#if WINDOWS_PHONE
                propertySet = new List<DependencyProperty>();
#else
                propertySet = new HashSet<DependencyProperty>();
#endif
            }

			if (!propertySet.Contains (property))
				propertySet.Add (property);
		}
			
		private Dictionary<DependencyProperty, object> 	properties;
		private ICollection<DependencyProperty> propertySet;
    }
}