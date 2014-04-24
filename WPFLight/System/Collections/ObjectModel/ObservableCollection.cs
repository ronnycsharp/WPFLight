using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.Generic;

namespace System.Collections.ObjectModel {
    public class ObservableCollection<T>
        : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged {
		public ObservableCollection ()
            : base() { }
        
		public ObservableCollection ( IList<T> list ) 
            : base ( list ) { }

        #region Events

        public event NotifyCollectionChangedEventHandler 	CollectionChanged;
		public event PropertyChangedEventHandler	        PropertyChanged;

        #endregion

        protected override void InsertItem (int index, T item) {
            base.InsertItem(index, item);

            OnCollectionChanged(
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Add, item, index));
					
            OnPropertyChanged(
				new PropertyChangedEventArgs("Count"));
            
			OnPropertyChanged(
				new PropertyChangedEventArgs("Item[]"));
        }

        protected virtual void MoveItem (int oldIndex, int newIndex) {
            var item = Items[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);

            OnCollectionChanged(
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
            
			OnPropertyChanged(
				new PropertyChangedEventArgs("Item[]"));
        }

        protected override void SetItem (int index, T item) {
			var oldItem = Items [index];
			base.SetItem (index, item);
			
			OnCollectionChanged (
				new NotifyCollectionChangedEventArgs (
					NotifyCollectionChangedAction.Replace, item, oldItem, index));
				
			OnPropertyChanged (
				new PropertyChangedEventArgs ("Item[]"));
        }

        protected override void ClearItems () {
            base.ClearItems();
			OnCollectionChanged (
				new NotifyCollectionChangedEventArgs (
					NotifyCollectionChangedAction.Reset));
			
			OnPropertyChanged (
				new PropertyChangedEventArgs ("Count"));
				
			OnPropertyChanged (
				new PropertyChangedEventArgs ("Item[]"));
        }

        protected override void RemoveItem (int index) {
			var item = Items [index];
			base.RemoveItem (index);

			this.OnCollectionChanged (
				new NotifyCollectionChangedEventArgs (
					NotifyCollectionChangedAction.Remove, item, index));
					
			this.OnPropertyChanged (
				new PropertyChangedEventArgs ("Count"));
			
			this.OnPropertyChanged (
				new PropertyChangedEventArgs ("Item[]"));
        }

        protected virtual void OnCollectionChanged (NotifyCollectionChangedEventArgs e) {
			if ( this.CollectionChanged != null )
				this.CollectionChanged ( this, e );
        }

        protected virtual void OnPropertyChanged (PropertyChangedEventArgs e) {
            if (this.PropertyChanged != null) 
                this.PropertyChanged ( this, e );
        }
    }
}