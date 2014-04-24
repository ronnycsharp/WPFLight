using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace System.Windows.Controls {
	public class ListBox : Selector {
		public ListBox ( ) {
            this.ScissorTest = true;

			contentPanel = new StackPanel { Orientation = Orientation.Vertical, Padding =  new Thickness ( ) };

			scrollViewer = new ScrollViewer();
			scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
			scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.Content = contentPanel;
            scrollViewer.Parent = this;
            scrollViewer.ScissorTest = true;
        }

        public ListBox (SpriteFont font)
            : this() {
            this.Font = font;
        }

        #region Eigenschaften

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
							new ListBoxItem (this.Font) {
								Parent = contentPanel,
								Content = item,
								Height = 60,
							};

						lbItem.CheckedChanged += (s, e) => {
							if (((ListBoxItem)s).IsChecked) {
								this.SelectedItem = ((ListBoxItem)s).Content;
							}
						};

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
