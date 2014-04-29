using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using WPFLight.Helpers;

namespace System.Windows {
	public class FrameworkElement : UIElement, IDrawable2D {
		public FrameworkElement () {
			bindings = new List<Binding> ();
		}

		#region Events

		public event DependencyPropertyChangedEventHandler DataContextChanged;

		#endregion

		#region Properties

		public Style Style {
			get { return style; }
			set {
				if (value != null) {
					var oldStyle = style;
					style = value;
					this.OnStyleChanged (oldStyle, value);
					ApplyStyle (style);
					Invalidate ();
				}
			}
		}

		public static readonly DependencyProperty DataContextProperty =
			DependencyProperty.Register (
				"DataContext",
				typeof(object),
				typeof(FrameworkElement),
				new PropertyMetadata (
					(s, e) => {
						((FrameworkElement)s).OnDataContextChanged (s, e);
					}));

		public object DataContext {
			get {

				// gets the datacontext derived from its parent

				var dc = GetValue (DataContextProperty);
				if (dc == null) {
					var parentElement = this.Parent as FrameworkElement;
					if (parentElement != null)
						dc = parentElement.DataContext;
				}
				return dc;
			}
			set { SetValue (DataContextProperty, value); }
		}

		#endregion

		public override void Initialize () {
			base.Initialize ();
			this.ApplyStyle ();
		}

		protected virtual void OnStyleChanged (Style oldStyle, Style newStyle) {

		}

		protected virtual void OnDataContextChanged (object sender, DependencyPropertyChangedEventArgs e) {
			if (this.DataContextChanged != null)
				this.DataContextChanged (this, e);

			if (e.OldValue != null) {
				var pc = e.OldValue as INotifyPropertyChanged;
				if (pc != null)
					pc.PropertyChanged -= OnContextPropertyChanged;
			}

			if (e.NewValue != null) {
				var pc = e.NewValue as INotifyPropertyChanged;
				if (pc != null)
					pc.PropertyChanged += OnContextPropertyChanged;

				foreach (var binding in bindings) {
					if (binding.Source == null) {
						binding.DataContext = this.DataContext;
						binding.OnSourceUpdated ();
					}
				}
			}
		}

		void OnContextPropertyChanged ( object sender, PropertyChangedEventArgs e ) {
			foreach (var binding in bindings) {
				if (binding.Path.Path == e.PropertyName) {
					binding.OnSourceUpdated ();
				}
			}
		}
			
		protected override void OnPropertyChanged (
			DependencyProperty dp, object oldValue, object newValue) {

			this.CheckTrigger (dp);
			this.CheckBinding (dp);
		}

		/// <summary>
		/// check whether a trigger has been fired
		/// </summary>
		/// <param name="propertyName"></param>
		void CheckTrigger (DependencyProperty dp) {
			var propTriggers = new List<Trigger> ();
			if (this.Style != null) {
				foreach (var trig in this.Style.Triggers) {
					if (trig.Property == dp)
						propTriggers.Add (trig);
				}
			}
			if (this.Style == null || !this.Style.OverridesDefaultStyle) {
				// gets the default style based on the current type
				var defaultStyle = Style.GetStyleResource (this.GetType ());
				if (defaultStyle != null) {
					// iterate triggers
					foreach (var trig in defaultStyle.Triggers) {
						var count = propTriggers
                            .Where (p => p.Property.Name == trig.Property.Name)
                            .Count ();

						// check whether the trigger-property does not overwrite the default style
						if (count == 0) {
							if (trig.Property == dp)
								propTriggers.Add (trig);
						}
					}
				}
			}
			if (propTriggers.Count > 0) {
				// get current property value
				var propValue = this.GetCurrentValue (dp.Name);
				foreach (var trig in propTriggers) {
					// check whether the trigger is active
					var active = (trig.Value == null && propValue == null)
					             || (trig.Value != null && trig.Value.Equals (propValue));

					if (active) {
						// apply trigger
						this.ApplyTrigger (trig);

						// set trigger as active
						this.SetTriggerState (trig, true);
					} else {
						// check whether a trigger has already been activated
						if (this.IsActiveTrigger (trig)) {
							// set trigger as disabled
							this.SetTriggerState (trig, false);

							// reset changed trigger-values
							this.ResetTrigger (trig);

							// reset stored values
							this.ResetStoredValues (trig);
						}
					}
				}
			}
		}

		/// <summary>
		/// set trigger-state
		/// </summary>
		/// <param name="trigger">Trigger.</param>
		/// <param name="state">If set to <c>true</c> state.</param>
		void SetTriggerState (Trigger trigger, bool state) {
			if (triggerStates == null)
				triggerStates = new Dictionary<Trigger,Boolean> ();

			triggerStates [trigger] = state;
		}

		/// <summary>
		/// gets the trigger-state
		/// </summary>
		/// <returns><c>true</c> if this instance is active trigger the specified trigger; otherwise, <c>false</c>.</returns>
		/// <param name="trigger">Trigger.</param>
		bool IsActiveTrigger (Trigger trigger) {
			return triggerStates != null
			&& triggerStates.ContainsKey (trigger)
			&& triggerStates [trigger];
		}

		internal void ApplyStyle (Style style) {
			foreach (var setter in style.Setters) {
				this.ApplyValue (setter.Property, setter.Value);
			}
		}

		/// <summary>
		/// Apply the default type style and the style-Property
		/// </summary>
		protected void ApplyStyle () {
			// gets the default type style, like this <Style x:key="{x:Type Button}"... />
			var defaultStyle = Style.GetStyleResource (this.GetType ());

			// check whether a default type style exists and if exists
			// the overridesdefaultstyle-property is false

			// apply default style, if style isn't set or OverridesDefaultStyle-property is false
			if (defaultStyle != null && (this.Style == null || !this.Style.OverridesDefaultStyle))
				this.ApplyStyle (defaultStyle);

			if (this.Style != null && defaultStyle != this.Style)
				// apply style-property
				this.ApplyStyle (this.Style);

			this.Invalidate ();
		}

		/// <summary>
		/// Apply trigger - apply setters of the trigger
		/// </summary>
		/// <param name="trigger">Trigger.</param>
		void ApplyTrigger (Trigger trigger) {
			if (trigger == null)
				throw new ArgumentNullException ();

			var updated = false;

			// Setter des Triggers durchlaufen
			foreach (var setter in trigger.Setters) {
				// Gibt den aktuellen Wert zurück
				var oldValue = this.GetValue (setter.Property);

				// Prüfen ob der Wert übernommen wurde 
				// - Vorherige Zuweisungen ignorieren, da Trigger (letzter Parameter, true)
				if (this.ApplyValue (setter.Property, setter.Value, true)) {
					updated = true;

					// Vorherigen zur sp?teren Wiederherstellung speichern
					this.StoreValue (trigger, setter.Property, oldValue);
				}
			}

			if (updated)
				// FrameworkElement neuzeichnen
				this.Invalidate ();
		}

		/// <summary>
		/// save the property value
		/// </summary>
		/// <param name="trigger">Trigger.</param>
		/// <param name="propName">Property name.</param>
		/// <param name="value">Value.</param>
		void StoreValue (Trigger trigger, DependencyProperty dp, object value) {
			if (storedValues == null)
				storedValues = new Dictionary<Trigger, Dictionary<DependencyProperty, object>> ();

			var dic = default ( Dictionary<DependencyProperty, Object> );
			if (storedValues.ContainsKey (trigger))
				dic = storedValues [trigger];
			else {
				dic = new Dictionary<DependencyProperty, object> ();
				storedValues [trigger] = dic;
			}

			// Wert zuweisen
			dic [dp] = value;
		}

		/// <summary>
		/// Stellt einen gespeicherten Eigenschaftswert nach einer Trigger-Deaktivierung wieder her
		/// </summary>
		/// <returns><c>true</c>, if value was restored, <c>false</c> otherwise.</returns>
		/// <param name="trigger">Trigger.</param>
		/// <param name="propName">Property name.</param>
		bool RestoreValue (Trigger trigger, DependencyProperty dp) {
			if (storedValues != null
			    && storedValues.ContainsKey (trigger)
			    && storedValues [trigger].ContainsKey (dp)) {
				return ApplyValue (dp, storedValues [trigger] [dp]);
			}
			return false;
		}

		/// <summary>
		/// L?scht die gespeicherten Eigenschaftswerte f¨¹r den angegebenen Trigger
		/// </summary>
		/// <param name="trigger">Trigger.</param>
		void ResetStoredValues (Trigger trigger) {
			if (storedValues != null
			    && storedValues.ContainsKey (trigger)) {
				storedValues [trigger].Clear ();
			}
		}

		/// <summary>
		/// resets a trigger
		/// - resets all properties which have been modified by the trigger to their stored values
		/// </summary>
		/// <param name="trigger">Trigger.</param>
		void ResetTrigger (Trigger trigger) {
			if (trigger == null)
				throw new ArgumentNullException ();

			var updated = false;
			foreach (var setter in trigger.Setters) {
				// Wert wiederherstellen
				if (this.RestoreValue (trigger, setter.Property))
					updated = true;
			}
			if (updated)
				this.Invalidate ();
		}

		protected bool ApplyValue (DependencyProperty dp, object value) {
			return ApplyValue (dp, value, false);
		}

		/// <summary>
		/// Gibt den Zieltyp einer Property zur¨¹ck, egal ob DependencyProperty oder Klassenproperty
		/// </summary>
		/// <returns>The property type.</returns>
		/// <param name="propertyName">Property name.</param>
		protected Type GetPropertyType (string propertyName) {
			return this.GetType ().GetProperty (propertyName).PropertyType;
		}

		/// <summary>
		/// ?bernimmt einen Eigenschaftswert, welcher vom Style definiert wurde
		/// </summary>
		/// <param name="propertyName">Eigenschaftsname</param>
		/// <param name="value">Neuer Wert</param>
		/// <param name="ignoreAssignment">Wenn true, dann wird ein bereits zugewiesene Eigenschaft ¨¹berschrieben</param>
		bool ApplyValue (DependencyProperty dp, object value, bool ignoreAssignment) {
			var result = false;

			// Pr¨¹fem ob die DependencyProperty noch nicht zugewiesen wurde oder 
			// ob die Zuweisung ignoriert werden soll (zb. durch trigger-Ausl?sung)
			if (ignoreAssignment || !this.IsAssignedProperty (dp)) {
				this.SetValue (dp, ConvertValue (value, dp.PropertyType), false);
				result = true;
			}
			
			return result;
		}

		static object ConvertValue (object value, Type targetType) {
			// check whether the target is nullable
			if (targetType.IsGenericType
			    && targetType.GetGenericTypeDefinition () == typeof(Nullable<>)) {

				var underlyingType = Nullable.GetUnderlyingType (targetType);
				var convertedValue = Convert.ChangeType (value, underlyingType, null);

				return convertedValue;
			}
			return value;
		}

		public virtual void Draw (
			Microsoft.Xna.Framework.GameTime gameTime, 
			Microsoft.Xna.Framework.Graphics.SpriteBatch batch, 
			float alpha, 
			Microsoft.Xna.Framework.Matrix transform) {

		}

		/// <summary>
		/// check whether the target has changed properties which are binded
		/// </summary>
		/// <param name="dp"></param>
		protected void CheckBinding (DependencyProperty dp) {
			if (dp == null)
				throw new ArgumentNullException ();

			if (bindings != null) {
				foreach (var binding in bindings) {
					if (binding.HasTargetProperty (dp)&&binding.IsBound()) {
						// TODO Add BindingMode-Standard, ... FrameworkMetadata ...
						if (binding.Mode == BindingMode.TwoWay
						    || binding.Mode == BindingMode.OneWayToSource) {
							binding.OnTargetPropertyUpdated (this, dp);
						}
					}
				}
			}
		}

		public void SetBinding (DependencyProperty dp, Binding binding) {
			if (dp == null || binding == null)
				throw new ArgumentNullException ();

			if (!bindings.Contains (binding))
				bindings.Add (binding);

			binding.AddTargetProperty (this, dp);
			binding.SourceUpdated +=
                (s, e) => {
				var bind = (Binding)s;
				if (bind.Mode != BindingMode.OneWayToSource) {
					if ( bind.IsBound ( ) ) {
						var value = bind.GetSourceValue ();
						foreach (var prop in bind.GetTargetProperties(this)) {
							this.SetValue (prop, value);
						}
					}
				}
			};

			if (binding.Source == null)
				binding.DataContext = this.DataContext;

			binding.OnSourceUpdated ();
		}

		public void SetBinding (DependencyProperty dp, string path) {
			SetBinding (dp, new Binding (path));
		}

		public object FindResource (object resourceKey) {
			return ResourceHelper.GetResource (resourceKey);
		}

		private List<Binding> bindings;
		private Dictionary<Trigger, Dictionary<DependencyProperty, Object>> storedValues;
		private Dictionary<Trigger, Boolean> triggerStates;
		private Style style;
	}
}