using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using WPFLight.Helpers;

namespace System.Windows {
    [ContentProperty("Setters")]
	public class Style {
		public Style () {}

        public Style (Type targetType) {
            this.TargetType = targetType;
        }

		public Style (Type targetType, Style basedOn) {
            this.TargetType = targetType;
            this.BasedOn = basedOn;

            if (basedOn != null) {
                foreach (var setter in basedOn.Setters)
                    this.Setters.Add(setter);

                foreach (var trigger in basedOn.Triggers)
                    this.Triggers.Add(trigger);
            }
		}

		#region Eigenschaften

		public Type TargetType { get; set; }

		public SetterCollection Setters {
			get {
                if (setters == null)
                    setters = new SetterCollection();

				return setters;
            }
            internal set {
                setters = value;
            }
		}

        public TriggerCollection Triggers {
            get {
                if (triggers == null)
                    triggers = new TriggerCollection();

                return triggers;
            }
            internal set {
                triggers = value;
            }
        }

        public Style BasedOn {
            get;
            internal set;
        }

		#endregion

		/// <summary>
		/// Wenn true, dann wird der Type, welcher über die Style-Key-Eigenschaft zugewiesen wurde
		/// mit diesem Style überschrieben
		/// </summary>
		/// <value><c>true</c> if overrides default style; otherwise, <c>false</c>.</value>
		public bool OverridesDefaultStyle {
			get {
                var setter = this.Setters
                    .Where ( s => s.Property.Name == "OverridesDefaultStyle" )
                    .FirstOrDefault ( );

                if (setter != null)
                    return (bool)setter.Value;

				return false;
			}
		}

		/// <summary>
		/// Gibt eine Style-Resource zurück, welcher zuvor über einen Namen oder einen Type zugewiesen wurde
		/// </summary>
		/// <returns>The style.</returns>
		/// <param name="key">Key.</param>
		public static Style GetStyleResource ( object resourceKey ) {
			return ( Style ) ResourceHelper.GetResource (resourceKey);
		}
			
		public static Style Parse ( string xaml ) {
			return ( Style ) XamlReader.Parse (xaml);
		}
	
		private SetterCollection    setters;
        private TriggerCollection   triggers;
	}
}
