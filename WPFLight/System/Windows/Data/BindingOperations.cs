using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Windows.Data {
	public static class BindingOperations {
		static BindingOperations ( ) {
			bindings = new Dictionary<DependencyObject, Dictionary<DependencyProperty, Binding>> ();
		}

		public static void SetBinding ( 
			DependencyObject target, 
			DependencyProperty property,
			Binding binding ) {

			if (target == null || property == null || binding == null)
				throw new ArgumentNullException ();
				

		}

		public static Binding GetBinding ( 
			DependencyObject target,
			DependencyProperty property ) {



			return null;

		}

		private static Dictionary<DependencyObject,Dictionary<DependencyProperty, Binding>> bindings;
	}
}
