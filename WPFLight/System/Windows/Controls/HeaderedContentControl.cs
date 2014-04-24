namespace System.Windows.Controls {
    public abstract class HeaderedContentControl : ContentControl {
		#region Eigenschaften

        public static DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(Object),
                typeof(HeaderedContentControl));

        public object Header {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public bool HasHeader {
            get { return Header != null; }
        }

		#endregion
    }
}