namespace System.Windows {
    public delegate void SizeChangedEventHandler ( object sender, SizeChangedEventArgs e );
	public class SizeChangedEventArgs : EventArgs {
        internal SizeChangedEventArgs (Size newSize, Size previousSize) {
            this.NewSize = newSize;
            this.PreviousSize = previousSize;
        }

        #region properties

        public Size NewSize { get; private set; }
        public Size PreviousSize { get; private set; }

        public bool HeightChanged {
            get {
                return NewSize.Height != PreviousSize.Height;
            }
        }

        public bool WidthChanged {
            get {
                return NewSize.Width != PreviousSize.Width;
            }
        }

        #endregion
	}
}
