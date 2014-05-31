using System.Collections.Generic;
using System.Windows.Controls.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace System.Windows.Controls {
	public class ListBox : Selector {
		public ListBox ( ) {
			contentPanel = new StackPanel { 
                Orientation = Orientation.Vertical, 
                Padding =  new Thickness ( ) 
            };

			scrollViewer = new ScrollViewer();
			scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
			scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.Content = contentPanel;
            scrollViewer.Parent = this;
            scrollViewer.ScissorTest = true;
        }

		#region Properties

        #endregion

        public override void Initialize () {
            base.Initialize();
            scrollViewer.Initialize();
            this.Invalidate();
        }

        public override void Invalidate ( )  {
            contentPanel.Children.Clear();
			var list = new List<Object> ();
			foreach (var item in Items) {
				if (!list.Contains (item)) {
					list.Add (item);
					if (!(item is ListBoxItem)) {
						var lbItem =
							new ListBoxItem {
                                FontFamily = this.FontFamily,
								Padding = new Thickness ( 10,0,0,0 ),
								Parent = contentPanel,
								HorizontalContentAlignment = HorizontalAlignment.Left,	// TODO REMOVE
								Content = item,
							};

						lbItem.CheckedChanged += (s, e) => {
							if (((ListBoxItem)s).IsChecked) {
								this.SelectedItem = ((ListBoxItem)s).Content;
							}
						};

						if (this.ItemContainerStyle != null) {
							lbItem.Style = this.ItemContainerStyle;
						}
							
						lbItem.Initialize ();
						contentPanel.Children.Add (lbItem);
					} else {
						contentPanel.Children.Add (
							item as ListBoxItem);
					}
				}
            }
			if ( scrollViewer.IsInitialized ) 
				scrollViewer.Invalidate ();

            base.Invalidate();
        }

		public override void OnTouchMove (TouchLocation state) {
			base.OnTouchMove (state);
			scrollViewer.OnTouchMove (state);
		}

		public override void OnTouchDown (TouchLocation state) {
			base.OnTouchDown (state);
			scrollViewer.OnTouchDown (state);
		}

		public override void OnTouchUp (TouchLocation state) {
			base.OnTouchUp (state);
			scrollViewer.OnTouchUp (state);
		}

        public override void Update (GameTime gameTime) {
            base.Update(gameTime);
            scrollViewer.Update(gameTime);
        }

        public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
			base.Draw(gameTime, batch, alpha, transform);
            scrollViewer.Draw(gameTime, batch, alpha, transform);
        }

        private ScrollViewer scrollViewer;
        private StackPanel   contentPanel;

    }
}
