using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Controls {
	// TODO Ableitung von CheckButton

	public class RadioButton : Button {
        public RadioButton () : base ( ) {
            this.ScissorTest = false;
        }

		public RadioButton (SpriteFont font) 
            : base ( font ) {
			this.ScissorTest = false;
		}

		public event EventHandler CheckedChanged;

		#region Eigenschaften

		public string GroupName { get; set; }

		/// <summary>
		/// Bestimmt ob der RadioButton wieder deaktiviert werden kann, (wie CheckButton)
		/// </summary>
		/// <value><c>true</c> if uncheckable; otherwise, <c>false</c>.</value>
		public bool Uncheckable { get; set; }

		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register (
				"IsChecked", 
				typeof(bool), 
				typeof(RadioButton), 
				new PropertyMetadata (
					new PropertyChangedCallback (
						( sender, e) => {
							((RadioButton)sender).OnCheckedChanged ((bool)e.NewValue);
						})));

		public bool IsChecked {
			get { return (bool)GetValue (IsCheckedProperty); }
			set { SetValue (IsCheckedProperty, value); }
		}

		#endregion

		protected virtual void OnCheckedChanged (bool chk) {
			if (this.IsChecked) {
				var root = GetRoot (this);
				var controls = GetAllChildren<RadioButton> (root);

				foreach (var c in controls) {
					if (c != this && c.IsChecked && c.GroupName != null && c.GroupName == this.GroupName && c.IsChecked)
						c.IsChecked = false;
				}

				/*
				foreach ( var c in this.Parent.Children.OfType<RadioButton> ( ) ) {
					if ( c != this && c.IsChecked && c.GroupName == this.GroupName ) {
						c.IsChecked = false;
						break;
					}
				}*/
			}

			if (this.CheckedChanged != null)
				this.CheckedChanged (this, EventArgs.Empty);
		}

		static UIElement GetRoot (UIElement c) {
			if (c.Parent == null)
				return c;
			else
				return GetRoot (c.Parent);
		}

		static List<T> GetAllChildren<T> (UIElement parent) where T : UIElement {
			var children = new List<T> ();
			if (parent is Panel) {
				foreach (var c in ((Panel)parent).Children) {
					if (c is T)
						children.Add ((T)c);

					children.AddRange (GetAllChildren<T> (c));
				}
			}
			return children;
		}

		protected override void OnClick () {
			base.OnClick ();
			if (this.IsVisible () && this.IsEnabled ) {
				if (this.Parent is Panel) {
					foreach (var c in ((Panel)this.Parent).Children.OfType<RadioButton> ( )) {
						if (c != this && c.IsChecked && c.GroupName == this.GroupName) {
							c.IsChecked = false;
							break;
						}
					}
				}

				if (this.IsChecked) {
					if (this.Uncheckable)
						this.IsChecked = false;
				} else
					this.IsChecked = true;
			}
		}
	}
}
