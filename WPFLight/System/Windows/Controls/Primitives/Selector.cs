namespace System.Windows.Controls.Primitives {
    public class Selector : ItemsControl {
        public event EventHandler SelectionChanged;
	
		#region Eigenschaften

        public static DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(
                "SelectedIndex",
                typeof(int),
                typeof(Selector),
                new PropertyMetadata(
                    -1, new PropertyChangedCallback(
                        (s, e) => {
                            ((Selector)s).OnSelectedIndexChanged( e );
                        })));

		public int SelectedIndex {
			get{ return ( int ) GetValue(SelectedIndexProperty); }
			set{ SetValue(SelectedIndexProperty, value); }
		}
		
		public static DependencyProperty SelectedItemProperty =
			DependencyProperty.Register(
				"SelectedItem", 
                typeof(Object), 
                typeof(Selector),
                new PropertyMetadata(
                    new PropertyChangedCallback(
                        (s, e) => {
                            ((Selector)s).OnSelectedItemChanged ( e );
                        })));

		public object SelectedItem {
			get{ return ( object ) GetValue(SelectedItemProperty); }
			set{ SetValue(SelectedItemProperty, value); }
		}
		
		#endregion

        void OnSelectedItemChanged ( DependencyPropertyChangedEventArgs e) {
            this.SelectedIndex = this.GetIndex(this.SelectedItem);

			/*
            if (e.OldValue != null)
                ((TabItem)e.OldValue).IsSelected = false;

            if (e.NewValue != null)
                ((TabItem)e.NewValue).IsSelected = true;
                */
        }

        void OnSelectedIndexChanged ( DependencyPropertyChangedEventArgs e ) {
            this.SelectedItem = this.GetItem(this.SelectedIndex);
            this.OnSelectionChanged();
        }

        protected virtual void OnSelectionChanged ( ) {
            if (this.SelectionChanged != null)
                this.SelectionChanged(this, EventArgs.Empty);
        }

        int GetIndex (object item) {
            if (item != null) {
                var index = 0;
                foreach (var child in Items ) {
                    if (item == child)
                        return index;

                    index++;
                }
            }
            return -1;
        }

        object GetItem (int index) {
            if (index != -1 ) {
                return this.Items[index];
            }
            return null;
        }
    }
}