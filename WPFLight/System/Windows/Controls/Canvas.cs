using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace System.Windows.Controls {
	public class Canvas : Panel {
		#region Properties

		public static readonly DependencyProperty LeftProperty =
			DependencyProperty.RegisterAttached (
				"Left",
				typeof ( float ),
				typeof ( Canvas ) );

		public static readonly DependencyProperty TopProperty =
			DependencyProperty.RegisterAttached (
				"Top",
				typeof ( float ),
				typeof ( Canvas ) );

		#endregion

		internal override float GetAbsoluteLeft (UIElement child) {
			return 
				this.GetAbsoluteLeft ()
					+ Canvas.GetLeft (child);
		}

		internal override float GetAbsoluteTop (UIElement child) {
			return
				this.GetAbsoluteTop ()
					+ Canvas.GetTop (child);
		}

		public static float GetTop ( UIElement ctrl ) {
			return (float)ctrl.GetValue (TopProperty);
		}

		public static float GetLeft ( UIElement ctrl ) {
			return (float)ctrl.GetValue (LeftProperty);
		}
			
		public static void SetLeft ( UIElement ctrl, float value ) {
			ctrl.SetValue (LeftProperty, value);
		}

		public static void SetTop (UIElement ctrl, float value) {
			ctrl.SetValue (TopProperty, value);
        }
	}
}
