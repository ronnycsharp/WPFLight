using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using WPFLight.Extensions;

namespace System.Windows.Data {
	public class Binding {
		public Binding () {
            targetProperties = new List<Tuple<FrameworkElement, DependencyProperty>>();
			this.Mode = BindingMode.TwoWay;
		}

		public Binding (string path) : this ( ) {
			this.Path = new PropertyPath (path);
		}

		#region Ereignisse

		public event EventHandler SourceUpdated;
		public event EventHandler TargetUpdated;

		#endregion

		#region Eigenschaften

		/// <summary>
		/// Gets or sets the data context which is set when the source-property is null and the bound 
		/// datacontext is changed
		/// </summary>
		/// <value>The data context.</value>
		internal object DataContext { get; set; }

		[DefaultValue ("")]
		public string ElementName { get; set; }

		public BindingMode Mode { get; set; }

		public PropertyPath Path { get; set; }

		public object Source { 
			get { return source; }
			set {
				if (targetProperties.Count > 0)
					throw new InvalidOperationException ("Source cannot be changed after binding");

				if (value != source) {

					// binds to DependencyObjects or objets which implements INotifyPropertyChanged

					sourceProperty = null;
					if ( source != null ) {
						var notifyPropertyChanged = source as INotifyPropertyChanged;
						if (notifyPropertyChanged != null) {
							notifyPropertyChanged.PropertyChanged -= OnSourcePropertyChanged;
						} else {
							var dependencyObject = source as DependencyObject;
							if (dependencyObject != null) {
								dependencyObject.PropertyChanged -= OnSourceDependencyPropertyChanged;
							}
						}
					}
					source = value;
					if ( source != null ) {
						var notifyPropertyChanged = source as INotifyPropertyChanged;
						if (notifyPropertyChanged != null) {
							notifyPropertyChanged.PropertyChanged += OnSourcePropertyChanged;
						} else {
							var dependencyObject = source as DependencyObject;
							if (dependencyObject != null) {
								dependencyObject.PropertyChanged += OnSourceDependencyPropertyChanged;
							}
						}
					}

					if (value != null)
						this.DataContext = null;
				}
			}
		}

		public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

		#endregion

		void OnSourceDependencyPropertyChanged ( object sender, DependencyPropertyChangedEventArgs e ) {
			if (e.Property.Name == this.Path.Path)
				OnSourceUpdated ();
		}

		void OnSourcePropertyChanged ( object sender, PropertyChangedEventArgs e ) {
			if (e.PropertyName == this.Path.Path)
				OnSourceUpdated ();
		}

		internal void OnSourceUpdated ( ) {
			if (SourceUpdated != null)
				SourceUpdated (this, EventArgs.Empty);
		}

		internal void OnTargetUpdated ( ) {
			if (TargetUpdated != null)
				TargetUpdated (this, EventArgs.Empty);
		}

		internal void AddTargetProperty ( FrameworkElement element, DependencyProperty dp ) {
            if (element == null || dp == null)
                throw new ArgumentNullException();

            targetProperties.Add(
                new Tuple<FrameworkElement, DependencyProperty>(element, dp));
		}

        internal IEnumerable<DependencyProperty> GetTargetProperties (FrameworkElement element) {
            foreach (var prop in targetProperties) {
                if ( prop.Item1 == element )
                    yield return prop.Item2;
            }
        }

        internal bool HasTargetProperty (DependencyProperty dp) {
            return targetProperties.Count(t => t.Item2 == dp) > 0;
        }

        /// <summary>
        /// is called after a target-property is changed
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dp"></param>
        internal void OnTargetPropertyUpdated (FrameworkElement element, DependencyProperty dp) {
			var prop = element.GetType ().GetProperty (dp.Name);
			var value = prop.GetValue (element, null);

			this.SetSourceValue (value);
            OnTargetUpdated();
        }

		internal void SetSourceValue ( object value ) {
			if (sourceProperty == null)
				sourceProperty = (this.Source 
					?? this.DataContext).GetType ().GetProperty (this.Path.Path);

			sourceProperty
				.SetValue (this.Source 
					?? this.DataContext, value, null);
		}

        internal object GetSourceValue () {
			if (sourceProperty == null)
				sourceProperty = 
					(this.Source ?? this.DataContext).GetType ().GetProperty (this.Path.Path);

			return sourceProperty
					.GetValue (this.Source ?? this.DataContext, null);
        }

		/// <summary>
		/// Determines whether this instance is bound
		/// </summary>
		/// <returns><c>true</c> if this instance is bound; otherwise, <c>false</c>.</returns>
		internal bool IsBound ( ) {
			return ((this.Source != null || this.DataContext != null) 
				&& targetProperties.Count > 0);
		}

        private PropertyInfo                                        sourceProperty;
		private object 					                            source;
		private List<Tuple<FrameworkElement,DependencyProperty>> 	targetProperties;
	}
	// Zusammenfassung:
	//     Beschreibt die zeitliche Steuerung von Aktualisierungen der Bindungsquelle.
	public enum UpdateSourceTrigger {
		// Zusammenfassung:
		//     Der System.Windows.Data.UpdateSourceTrigger-Standardwert der Bindungsziel-Eigenschaft.
		//     Der Standardwert für die meisten Abhängigkeitseigenschaften lautet System.Windows.Data.UpdateSourceTrigger.PropertyChanged,
		//     während die System.Windows.Controls.TextBox.Text-Eigenschaft den Standardwert
		//     System.Windows.Data.UpdateSourceTrigger.LostFocus aufweist.
		Default = 0,
		//
		// Zusammenfassung:
		//     Aktualisiert die Bindungsquelle, sobald die Bindungsziel-Eigenschaft geändert
		//     wird.
		PropertyChanged = 1,
		//
		// Zusammenfassung:
		//     Aktualisiert die Bindungsquelle immer dann, wenn das Bindungsziel-Element
		//     den Fokus verliert.
		LostFocus = 2,
		//
		// Zusammenfassung:
		//     Aktualisiert die Bindungsquelle nur bei einem Aufruf der System.Windows.Data.BindingExpression.UpdateSource()-Methode.
		Explicit = 3,
	}
	// Zusammenfassung:
	//     Beschreibt die Richtung des Datenflusses in einer Bindung.
	public enum BindingMode {
		// Zusammenfassung:
		//     Bewirkt, dass bei Änderungen an der Quelleigenschaft bzw. der Zieleigenschaft
		//     die jeweils andere automatisch aktualisiert wird. Dieser Typ von Bindung
		//     ist für bearbeitbare Formulare und sonstige vollständig interaktive Benutzeroberflächenszenarien
		//     geeignet.
		TwoWay = 0,
		//
		// Zusammenfassung:
		//     Aktualisiert die Bindungsziel-Eigenschaft (Zieleigenschaft), wenn die Bindungsquelle
		//     (Quelle) geändert wird. Dieser Typ von Bindung empfiehlt sich, wenn das gebundene
		//     Steuerelement implizit als schreibgeschützt festgelegt wurde. Sie können
		//     beispielsweise eine Bindung an eine Quelle wie einen Börsenticker erstellen.
		//     Möglicherweise ist für die Zieleigenschaft auch keine Steuerungsoberfläche
		//     zum Vornehmen von Änderungen verfügbar, beispielsweise eine datengebundene
		//     Hintergrundfarbe einer Tabelle. Wenn die Änderungen der Zieleigenschaft nicht
		//     überwacht werden müssen, vermeiden Sie mit dem System.Windows.Data.BindingMode.OneWay-Bindungsmodus
		//     den zusätzlichen Aufwand durch den System.Windows.Data.BindingMode.TwoWay-Bindungsmodus.
		OneWay = 1,
		//
		// Zusammenfassung:
		//     Aktualisiert das Bindungsziel, wenn die Anwendung gestartet oder der Datenkontext
		//     geändert wird. Dieser Typ von Bindung empfiehlt sich, wenn Sie Daten verwenden,
		//     bei denen eine Momentaufnahme des aktuellen Zustands verwendet werden kann
		//     oder die Daten tatsächlich statisch sind. Dieser Bindungstyp ist auch hilfreich,
		//     wenn die Zieleigenschaft mit einem bestimmten Wert der Quelleigenschaft initialisiert
		//     werden soll und der Datenkontext vorab nicht bekannt ist. Dies ist eine wesentlich
		//     einfachere Form der System.Windows.Data.BindingMode.OneWay-Bindung, die eine
		//     bessere Leistung in Situationen bietet, in denen der Quellwert unverändert
		//     bleibt.
		OneTime = 2,
		//
		// Zusammenfassung:
		//     Aktualisiert die Quelleigenschaft, wenn die Zieleigenschaft geändert wird.
		OneWayToSource = 3,
		//
		// Zusammenfassung:
		//     Verwendet den System.Windows.Data.Binding.Mode-Standardwert des Bindungsziels.
		//     Der Standardwert ändert sich für jede Abhängigkeitseigenschaft. Im Allgemeinen
		//     verfügen Steuerelementeigenschaften, die vom Benutzer bearbeitet werden können
		//     (z. B. Textfelder und Kontrollkästchen) standardmäßig über bidirektionale
		//     Bindungen, während die meisten anderen Eigenschaften standardmäßig unidirektionale
		//     Bindungen aufweisen. Eine programmgesteuerte Möglichkeit zu bestimmen, ob
		//     eine Abhängigkeitseigenschaft über eine unidirektionale oder bidirektionale
		//     Bindung verfügt, besteht darin, die Eigenschaftenmetadaten der Eigenschaft
		//     mithilfe von System.Windows.DependencyProperty.GetMetadata(System.Type) abzurufen
		//     und anschließend den booleschen Wert der System.Windows.FrameworkPropertyMetadata.BindsTwoWayByDefault-Eigenschaft
		//     zu überprüfen.
		Default = 4,
	}
}
