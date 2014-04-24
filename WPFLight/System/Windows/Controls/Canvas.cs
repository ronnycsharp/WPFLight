using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace System.Windows.Controls {
	public class Canvas : Panel {
		public Canvas () : base ( ) {
			
		}

		internal override float GetAbsoluteLeft (UIElement child) {
			return 
				this.GetAbsoluteLeft ( ) 
					+ this.GetChildLeft (child);
		}

		internal override float GetAbsoluteTop (UIElement child) {
			return
				this.GetAbsoluteTop ( )
					+ this.GetChildTop (child);
		}

		public static int GetLeft ( UIElement ctrl ) {
			if ( ctrl == null)
				throw new ArgumentNullException ();

            if ( ctrl.Parent is Canvas )
			    return ((Canvas)ctrl.Parent).GetChildLeft (ctrl);

            return 0;
		}

        public static int GetTop (UIElement ctrl) {
            if (ctrl == null)
                throw new ArgumentNullException();

            if (ctrl.Parent is Canvas)
                return ((Canvas)ctrl.Parent).GetChildTop(ctrl);

            return 0;
        }
			
		public static void SetLeft ( UIElement ctrl, int value ) {
            if ( ctrl.Parent is Canvas )
			    ((Canvas)ctrl.Parent).SetChildLeft (ctrl, value);
		}

        public static void SetTop (UIElement ctrl, int value) {
            if (ctrl.Parent is Canvas)
                ((Canvas)ctrl.Parent).SetChildTop(ctrl, value);
        }

		int GetChildLeft (UIElement item) {
			if (dicLeft != null && dicLeft.ContainsKey (item))
				return dicLeft [item];

			return 0;
		}

		int GetChildTop (UIElement item) {
			if (dicTop != null && dicTop.ContainsKey (item))
				return dicTop [item];

			return 0;
		}

		void SetChildLeft (UIElement item, int value) {
			if (dicLeft == null)
				dicLeft = new Dictionary<UIElement, int> ();

			dicLeft [item] = value;
		}

		void SetChildTop (UIElement item, int value) {
			if (dicTop == null)
				dicTop = new Dictionary<UIElement, int> ();

			dicTop [item] = value;
		}

		private Dictionary<UIElement, int> dicTop;
		private Dictionary<UIElement, int> dicLeft;
	}
}
