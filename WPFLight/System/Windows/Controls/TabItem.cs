using System.Windows.Controls.Primitives;
namespace System.Windows.Controls {
    public class TabItem : HeaderedContentControl {
        #region Eigenschaften

        public static DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                "IsSelected",
                typeof(bool),
                typeof(TabItem),
                new PropertyMetadata(
                    new PropertyChangedCallback(
                        (s, e) => {
                            if ((bool)e.NewValue) {
                                ((TabItem)s).OnSelected();
                            } else {
                                ((TabItem)s).OnUnselected();
                            }
                        })));

        public bool IsSelected {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        #endregion

        protected virtual void OnSelected () {
            var tabControl = this.Parent as TabControl;
            if (tabControl != null)
                tabControl.SelectedItem = this;
        }

        protected virtual void OnUnselected () {

        }
    }
}