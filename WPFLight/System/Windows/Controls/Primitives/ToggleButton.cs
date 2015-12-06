namespace System.Windows.Controls.Primitives {
	public class ToggleButton : Button {
		public ToggleButton ()  { }

		#region Ereignisse

		public event EventHandler CheckedChanged;

		#endregion

		#region Eigenschaften

		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register (
				"IsChecked", 
				typeof(bool), 
				typeof(ToggleButton), 
				new PropertyMetadata (
					new PropertyChangedCallback (
						( sender, e) => {
							((ToggleButton)sender).OnCheckedChanged ((bool)e.NewValue);
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
