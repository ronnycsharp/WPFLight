using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace System.Windows.Media {
    public static class VisualTreeHelper {
        public static List<T> GetChildren<T> (UIElement root) where T : UIElement {
            var children = new List<T>();
            if (root is Panel) {
                foreach (var c in ((Panel)root).Children) {
                    if (c is T)
                        children.Add((T)c);

                    children.AddRange(GetChildren<T>(c));
                }
            } else if (root is ItemsControl) {
                foreach (var c in ((ItemsControl)root).Items.OfType<UIElement> ( )) {
                    if (c is T)
                        children.Add((T)c);

                    children.AddRange(GetChildren<T>(c));
                }
            } else if (root is ContentControl) {
                var element = ((ContentControl)root).Content as UIElement;
                if (element != null) {
                    if (element is T)
                        children.Add((T)element);

                    children.AddRange(
                        GetChildren<T>(element));
                }
            }
            return children;
        }

		public static bool IsVisible ( UIElement c ) {
			return c.IsVisible 
				&& (c.Parent == null || IsVisible (c.Parent));
		}

        public static UIElement GetRoot (UIElement c) {
            if (c.Parent == null)
                return c;
            else
                return GetRoot(c.Parent);
        }
    }
}