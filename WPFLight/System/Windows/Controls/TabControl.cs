using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace System.Windows.Controls {
    public class TabControl : Selector {
        public TabControl () {
            gridItems = new Grid();
            gridItems.Parent = this;
            gridItems.RowDefinitions.Add(RowDefinition.Auto);
            gridItems.RowDefinitions.Add(RowDefinition.Star);
            gridItems.ColumnDefinitions.Add(ColumnDefinition.Star);

            lbItems = new ListBox();
            lbItems.SelectionChanged += 
                (s, e) => { this.SelectedIndex = lbItems.SelectedIndex; };

            Grid.SetRow(gridItems, lbItems, 0);
        }

        public override void Initialize () {
            foreach (var item in this.Items.OfType<TabItem> ( ) ) {
                gridItems.Children.Add(item);
                Grid.SetRow(gridItems, item, 1);
                lbItems.Items.Add(item.Header);
            }
            base.Initialize();
        }

        protected override void OnSelectionChanged () {
            base.OnSelectionChanged();
            foreach (var item in this.Items.OfType<TabItem> ( ) )
                item.Visible = false;

            if (this.SelectedItem != null)
                ((TabItem)this.SelectedItem).Visible = true;

            lbItems.SelectedIndex = this.SelectedIndex;
        }

        protected internal override void OnRender (DrawingContext dc) {
            base.OnRender(dc);
            gridItems.OnRender(dc);
        }

        private ListBox lbItems;
        private Grid    gridItems;
    }
}