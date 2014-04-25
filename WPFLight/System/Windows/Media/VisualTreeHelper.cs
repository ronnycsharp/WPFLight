using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace System.Windows.Media {
    public static class VisualTreeHelper {
        public static List<T> GetAllChildren<T> (UIElement parent) where T : UIElement {
            var children = new List<T>();
            if (parent is Panel) {
                foreach (var c in ((Panel)parent).Children) {
                    if (c is T)
                        children.Add((T)c);

                    children.AddRange(GetAllChildren<T>(c));
                }
            }
            if (parent is ItemsControl) {
                foreach (var c in ((ItemsControl)parent).Items.OfType<UIElement> ( )) {
                    if (c is T)
                        children.Add((T)c);

                    children.AddRange(GetAllChildren<T>(c));
                }
            }
            return children;
        }

        public static UIElement GetRoot (UIElement c) {
            if (c.Parent == null)
                return c;
            else
                return GetRoot(c.Parent);
        }
    }
}