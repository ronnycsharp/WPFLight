using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Markup;

namespace System.Windows.Controls {
	[ContentProperty("Items")]
    public class ItemsControl : Control {
        public ItemsControl () {
            this.Items = new ItemCollection();
			((ItemCollection)this.Items).CollectionChanged += OnItemsCollectionChanged;
        }

        #region Eigenschaften

        public static DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                "Items",
                typeof(IList),
                typeof(ItemsControl),
                new PropertyMetadata(
                    new PropertyChangedCallback(
                        (s,e)=>{
                        
                        })));

        public IList Items {
            get { return (IList)GetValue(ItemsProperty); }
			private set { SetValue(ItemsProperty, value); }
        }

		public static DependencyProperty ItemContainerStyleProperty =
			DependencyProperty.Register(
				"ItemContainerStyle",
				typeof(Style),
				typeof(ItemsControl));

		public Style ItemContainerStyle {
			get { return (Style)GetValue(ItemContainerStyleProperty); }
			set { SetValue(ItemContainerStyleProperty, value); }
		}

        #endregion

		protected virtual void OnItemsCollectionChanged ( 
			object sender, NotifyCollectionChangedEventArgs e ) { }
    }
}