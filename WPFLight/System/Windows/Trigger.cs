using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace System.Windows {
    [ContentProperty("Setters")]
	public class Trigger {
		public Trigger () {
			
		}

		public Trigger (DependencyProperty property, object value) {
			if (property == null)
				throw new ArgumentNullException ();
            
			this.Property = property;
			this.Value = value;
		}

        #region Eigenschaften

        public DependencyProperty Property { get; set; }

		public object Value { get; set; }

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

        #endregion

        private SetterCollection setters;
	}
}
