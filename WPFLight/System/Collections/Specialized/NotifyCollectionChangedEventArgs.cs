namespace System.Collections.Specialized {
    public class NotifyCollectionChangedEventArgs : EventArgs {
        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action) {
            this.Action = action;
            if (action != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("This constructor can only be used with the Reset action.", "action");
        }
        
        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, object changedItem, int index) {
            IList changedItems = new object[] { changedItem };
            this.Action = action;

            if (action == NotifyCollectionChangedAction.Add) {
                this.NewItems = new[] { changedItem };
                this.NewStartingIndex = index;
            } else if (action == NotifyCollectionChangedAction.Remove) {
                this.OldItems = new[] { changedItem };
                this.OldStartingIndex = index;
            } else if (action == NotifyCollectionChangedAction.Reset) {
                if (changedItem != null)
                    throw new ArgumentException("This constructor can only be used with the Reset action if changedItem is null", "changedItem");

                if (index != -1)
                    throw new ArgumentException("This constructor can only be used with the Reset action if index is -1", "index");
            } else {
                throw new ArgumentException("This constructor can only be used with the Reset, Add, or Remove actions.", "action");
            }
        }

        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, object newItem, object oldItem, int index) {
            this.Action = action;

            if (action != NotifyCollectionChangedAction.Replace)
                throw new ArgumentException("This constructor can only be used with the Replace action.", "action");

            this.NewItems = new[] { newItem };
            this.NewStartingIndex = index;

            this.OldItems = new[] { oldItem };
            this.OldStartingIndex = index;
        }

        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
            : this(action, new object[] { changedItem }, index, oldIndex) {

        }

        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) {
            this.Action = action;

            if (action != NotifyCollectionChangedAction.Move)
                throw new ArgumentException("This constructor can only be used with the Move action.", "action");

            if (index < -1)
                throw new ArgumentException("The value of index must be -1 or greater.", "index");

            this.NewItems = changedItems;
            this.NewStartingIndex = index;

            this.OldItems = changedItems;
            this.OldStartingIndex = oldIndex;
        }

        public NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction action, IList changedItems) {
            switch (action) {
                case NotifyCollectionChangedAction.Add: {
                        NewItems = changedItems;
                        break;
                    }
                case NotifyCollectionChangedAction.Remove: {
                        OldItems = changedItems;
                        break;
                    }
                case NotifyCollectionChangedAction.Move: {
                        throw new NotImplementedException();
                    }
                case NotifyCollectionChangedAction.Replace: {
                        throw new NotImplementedException();
                    }
                case NotifyCollectionChangedAction.Reset: {
                        OldItems = changedItems;
                        break;
                    }
            }

            this.Action = action;
        }

        #region Properties

        public NotifyCollectionChangedAction    Action              { get; private set; }
        public IList                            NewItems            { get; private set; }
        public IList                            OldItems            { get; private set; }
        public int                              OldStartingIndex    { get; private set; }
        public int                              NewStartingIndex    { get; private set; }

        #endregion
    }

    public enum NotifyCollectionChangedAction {
        Add,
        Move,
        Remove,
        Replace,
        Reset,
    }
}