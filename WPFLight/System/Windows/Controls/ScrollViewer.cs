using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;

namespace System.Windows.Controls {
	public class ScrollViewer : ContentControl {
		public ScrollViewer () {}

		#region Eigenschaften

		public static DependencyProperty VerticalOffsetProperty =
			DependencyProperty.Register (
				"VerticalOffset",
				typeof(int),
				typeof(ScrollViewer),
				new PropertyMetadata (
					new PropertyChangedCallback (
						(s, e) => {
							((ScrollViewer)s).RefreshVScroll ();
						})));

		public int VerticalOffset {
			get { return (int)GetValue (VerticalOffsetProperty); }
			set { SetValue (VerticalOffsetProperty, value); }
		}

		public static DependencyProperty HorizontalOffsetProperty =
			DependencyProperty.Register (
				"HorizontalOffset",
				typeof(int),
				typeof(ScrollViewer),
				new PropertyMetadata (
					new PropertyChangedCallback (
						(s, e) => {
							((ScrollViewer)s).RefreshHScroll ();
						})));

		public int HorizontalOffset {
			get { return (int)GetValue (HorizontalOffsetProperty); }
			set { SetValue (HorizontalOffsetProperty, value); }
		}

		public static DependencyProperty VerticalScrollBarVisibilityProperty =
			DependencyProperty.Register (
				"VerticalScrollBarVisibility",
				typeof(ScrollBarVisibility),
				typeof(ScrollViewer), 
				new PropertyMetadata (
					ScrollBarVisibility.Auto));

		public ScrollBarVisibility VerticalScrollBarVisibility {
			get { return (ScrollBarVisibility)GetValue (VerticalScrollBarVisibilityProperty); }
			set { SetValue (VerticalScrollBarVisibilityProperty, value); }
		}

		public static DependencyProperty HorizontalScrollBarVisibilityProperty =
			DependencyProperty.Register (
				"HorizontalScrollBarVisibility",
				typeof(ScrollBarVisibility),
				typeof(ScrollViewer), 
				new PropertyMetadata (
					ScrollBarVisibility.Auto));

		public ScrollBarVisibility HorizontalScrollBarVisibility {
			get { return (ScrollBarVisibility)GetValue (HorizontalScrollBarVisibilityProperty); }
			set { SetValue (HorizontalScrollBarVisibilityProperty, value); }
		}

		public static DependencyProperty CanContentScrollProperty =
			DependencyProperty.Register (
				"CanContentScroll",
				typeof(bool),
				typeof(ScrollViewer), 
				new PropertyMetadata (true));

		public bool CanContentScroll {
			get { return (bool)GetValue (CanContentScrollProperty); }
			set { SetValue (CanContentScrollProperty, value); }
		}

		#endregion

		public void ResetScrollOffset () {
			this.VerticalOffset = 0;
			this.HorizontalOffset = 0;
		}

		public override void Initialize () {
			rcVScroll = new System.Windows.Shapes.Rectangle ();
			rcVScroll.VerticalAlignment = VerticalAlignment.Top;
			rcVScroll.HorizontalAlignment = HorizontalAlignment.Right;
			rcVScroll.Margin = new Thickness ();
			rcVScroll.Width = 3;
			rcVScroll.Visible = true;
			rcVScroll.Opacity = .9f;
			rcVScroll.Fill = Brushes.White;
			rcVScroll.Parent = this;
			rcVScroll.Initialize ();

			rcHScroll = new System.Windows.Shapes.Rectangle ();
			rcHScroll.VerticalAlignment = VerticalAlignment.Bottom;
			rcHScroll.HorizontalAlignment = HorizontalAlignment.Stretch;
			rcHScroll.Height = 3;
			rcHScroll.Opacity = .9f;
			rcHScroll.Fill = Brushes.White;
			rcHScroll.Parent = this;
			rcHScroll.Initialize ();

			rcVScroll.Top = 100;
			rcVScroll.Height = 100;

			base.Initialize ();
		}
			
		public override void Invalidate () {
			if (this.ActualHeight > 0 && this.ActualHeight > 0) {
				var element = this.Content as FrameworkElement;
				if (element != null) {
                    element.Measure(
                        new Size(this.ActualWidth, this.ActualHeight));

                    measure = element.DesiredSize;

					rcVScroll.Invalidate ();
					rcHScroll.Invalidate ();

					RefreshHScroll ();
					RefreshVScroll ();
				}
			}

			base.Invalidate ();
		}

		public override void Update (GameTime gameTime) {
			base.Update (gameTime);

			rcVScroll.Update (gameTime);
			rcHScroll.Update (gameTime);

			if (this.ActualHeight < measure.Height)
				this.rcVScroll.Top = (int)(-(this.VerticalOffset * (this.ActualHeight) / measure.Height));

			if (this.ActualWidth < measure.Width)
				this.rcHScroll.Left = (int)(-(this.HorizontalOffset * (this.ActualWidth) / measure.Width));
		}

		void RefreshVScroll () {
			if (measure.Height > this.ActualHeight) {
				rcVScroll.Height = (int)(System.Math.Pow ((this.ActualHeight), 2) / measure.Height);
				rcVScroll.Visible = true;
			} else
				rcVScroll.Visible = false;
		}

		void RefreshHScroll () {
			if (measure.Width > this.ActualWidth) {
				rcHScroll.Width = (int)(System.Math.Pow ((this.ActualWidth), 2) / measure.Width);
				rcHScroll.Visible = true;
			} else
				rcHScroll.Visible = false;
		}

		public override void OnTouchDown (TouchLocation state) {
			var offsetState = new TouchLocation (
				                  state.Id, state.State, state.Position - new Vector2 (this.HorizontalOffset, this.VerticalOffset));

			var ctrl = this.Content as Control;
			if (ctrl != null)
				ctrl.OnTouchDown (offsetState);

			if (CanContentScroll) {
				if (!scrolling) {
					startPosX = (int)(state.Position.X - this.HorizontalOffset);
					startPosY = (int)(state.Position.Y - this.VerticalOffset);
				}
			}

			base.OnTouchDown (state);
		}

		public override void OnTouchUp (TouchLocation state) {
			var offsetState = new TouchLocation (
				                  state.Id, state.State, state.Position - new Vector2 (this.HorizontalOffset, this.VerticalOffset));

			var ctrl = this.Content as Control;
			if (ctrl != null)
				ctrl.OnTouchUp (offsetState);
				
			if (CanContentScroll) {
				scrolling = false;
				if (!scrolling)
					base.OnTouchUp (state);
			}
		}

		public override void OnTouchMove (TouchLocation state) {
			var offsetState = new TouchLocation (
				                  state.Id, state.State, state.Position - new Vector2 (this.HorizontalOffset, this.VerticalOffset));

			var ctrl = this.Content as Control;
			if (ctrl != null)
				ctrl.OnTouchMove (offsetState);


			if (CanContentScroll) {
				if (this.ActualHeight < measure.Height) {
					scrolling = true;
					var offset = System.Math.Min (0, state.Position.Y - startPosY);

					if (-offset > this.measure.Height - this.ActualHeight)
						offset = (float)(-(this.measure.Height - this.ActualHeight));

					this.VerticalOffset = (int)offset;
				}

				if (this.ActualWidth < measure.Width) {
					scrolling = true;
					var offset = System.Math.Min (0, state.Position.X - startPosX);

					if (-offset > this.measure.Width - this.ActualWidth)
						offset = (float)(-(this.measure.Width - this.ActualWidth));

					this.HorizontalOffset = (int)offset;
				}
			}
			base.OnTouchMove (state);
		}

		public override void Draw (
			GameTime gameTime, 
			SpriteBatch batch, 
			float alpha, 
			Matrix transform) {

			if (this.IsVisible && alpha > 0) {
				if (this.HasContent && this.Content is FrameworkElement) {

					if (this.HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden)
						rcHScroll.Draw (gameTime, batch, this.Opacity * alpha, transform);

					if (this.VerticalScrollBarVisibility != ScrollBarVisibility.Hidden)
						rcVScroll.Draw (gameTime, batch, this.Opacity * alpha, transform);

					var element = this.Content as FrameworkElement;

					var absLeft = GetAbsoluteLeft ();
					var absTop = GetAbsoluteTop ();

					var height = this.ActualHeight - 6;

                    element.Measure(new Size(this.ActualWidth, this.ActualHeight));
                    measure = element.DesiredSize;
						
					GraphicsDevice.ScissorRectangle = WPFLight.Helpers.ScreenHelper.CheckScissorRect (this.Bounds);
					GraphicsDevice.RasterizerState = SCISSOR_ENABLED;

					element.Draw (
						gameTime,
						batch,
						alpha,
						transform * Matrix.CreateTranslation (
							HorizontalOffset, 
							VerticalOffset, 
							0));
				}
			}
		}

		static readonly RasterizerState SCISSOR_ENABLED = 
			new RasterizerState { 
				CullMode = CullMode.None, 
				ScissorTestEnable = true
			};
				
		private int startPosX;
		private int startPosY;
		private bool scrolling;
		private System.Windows.Shapes.Rectangle rcVScroll;
		private System.Windows.Shapes.Rectangle rcHScroll;
		private Size measure;
	}

	public enum ScrollBarVisibility {
		Auto,
		Disabled,
		Hidden,
		Visible,
	}
}
