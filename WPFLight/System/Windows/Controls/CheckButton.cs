using System;
using System.Linq;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace System.Windows.Controls {
	public class CheckButton : Button {
		public CheckButton () : base () {
		}

		public CheckButton (SpriteFont font) : base (font) {

		}

		#region Ereignisse

		public event EventHandler CheckedChanged;

		#endregion

		#region Eigenschaften

		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register (
				"IsChecked", 
				typeof(bool), 
				typeof(CheckButton), 
				new PropertyMetadata (
					new PropertyChangedCallback (
						( sender, e) => {
							((CheckButton)sender).OnCheckedChanged ((bool)e.NewValue);
						})));

		public bool IsChecked {
			get { return (bool)GetValue (IsCheckedProperty); }
			set { SetValue (IsCheckedProperty, value); }
		}

		#endregion

		protected virtual void OnCheckedChanged (bool chk) {
			if (this.CheckedChanged != null)
				this.CheckedChanged (this, EventArgs.Empty);
		}

		protected override void OnClick () {
			base.OnClick ();
			this.IsChecked = !this.IsChecked;
		}
	}
}
