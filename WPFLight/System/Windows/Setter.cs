using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows {
    public class Setter {
		public Setter () {
			
		}

        public Setter ( DependencyProperty property, object value ) {
            if (property == null)
                throw new ArgumentNullException();
            
            this.Property = property;
            this.Value = value;
        }

        #region Eigenschaften

		public string               TargetName  { get; internal set; }
		public DependencyProperty   Property  	{ get; internal set; }
        public object               Value       { get; internal set; }

        #endregion
    }
}
