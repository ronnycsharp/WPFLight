using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Windows.Data {
	public static class BindingOperations {
		static BindingOperations ( ) {

		}

		public static void SetBinding ( 
			DependencyObject target, 
			DependencyProperty property,
			Binding binding ) {

			throw new NotImplementedException ( );
		}

		public static Binding GetBinding ( 
			DependencyObject target,
			DependencyProperty property ) {
			throw new NotImplementedException ();
		}
	}
}
