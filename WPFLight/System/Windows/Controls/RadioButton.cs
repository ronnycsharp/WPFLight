using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Controls {
	public class RadioButton : CheckButton {
        public RadioButton () { }

		#region Properties
        
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register(
                "GroupName", typeof(string), typeof(RadioButton));
        
        public string GroupName {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

		#endregion

		protected override void OnCheckedChanged (bool chk) {
			if (chk) {
                // uncheck all RadioButtons with same group-name 
				var root = VisualTreeHelper.GetRoot (this);
				var controls = VisualTreeHelper.GetAllChildren<RadioButton> (root);
				foreach (var c in controls) {
                    if (c != this
                            && c.IsChecked
                            && c.GroupName != null
                            && c.GroupName == this.GroupName
                            && c.IsChecked ) {
                        c.IsChecked = false;
                    }
				}
			}
            base.OnCheckedChanged(chk);
		}

        protected override void OnClick () {
            base.OnClick();
            if (this.Parent is Panel) {
                foreach (var c in ((Panel)this.Parent).Children.OfType<RadioButton>()) {
                    if (c != this && c.IsChecked && c.GroupName == this.GroupName) {
                        c.IsChecked = false;
                    }
                }
            }
        }
	}
}
