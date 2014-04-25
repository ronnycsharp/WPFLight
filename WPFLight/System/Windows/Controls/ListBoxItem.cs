namespace System.Windows.Controls {
    public class ListBoxItem : RadioButton {
        public ListBoxItem ( ) { }

        public override void Initialize () {
            base.Initialize();
            this.GroupName = "LB" + this.Parent.GetHashCode().ToString();
        }
    }
}
