namespace System.Windows {
	public delegate void DependencyPropertyChangedEventHandler ( object sender, DependencyPropertyChangedEventArgs e );
	public class DependencyPropertyChangedEventArgs {
		public DependencyPropertyChangedEventArgs(
			DependencyProperty property, object oldValue, object newValue ) {
			this.Property = property;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		#region Eigenschaften

		public DependencyProperty 	Property { get; private set; }
		public object 				OldValue { get; private set; }
		public object 				NewValue { get; private set; }

		#endregion
	}
}