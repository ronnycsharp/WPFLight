using System;
using System.Net;
using System.Windows.Media;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace System.Windows.Controls {
	public class Button : ContentControl {
		public Button () { }

		#region Ereignisse

		public event EventHandler<EventArgs> Click;

		#endregion

		#region Eigenschaften

        public static readonly DependencyProperty CornerRadiusXProperty =
            DependencyProperty.Register(
                "CornerRadiusX", 
                typeof(int), 
                typeof(Button));

        public int CornerRadiusX {
            get { return (int)GetValue(CornerRadiusXProperty); }
			set { SetValue(CornerRadiusXProperty, value); 
			}
        }

        public static readonly DependencyProperty CornerRadiusYProperty =
            DependencyProperty.Register(
                "CornerRadiusY",
                typeof(int),
                typeof(Button));

        public int CornerRadiusY {
            get { return (int)GetValue(CornerRadiusYProperty); }
            set { SetValue(CornerRadiusYProperty, value); }
        }

		#endregion

		public override void Invalidate () {
			base.Invalidate ();
			if (this.IsInitialized) {
				if (rcBackground == null ) {
					rcBackground = new System.Windows.Shapes.Rectangle( );
					rcBackground.Parent = this;
					rcBackground.Initialize ();
				}
				rcBackground.Stroke = this.BorderBrush;
				rcBackground.Fill = this.Background;
				rcBackground.StrokeThickness = this.BorderThickness.Left;
				rcBackground.RadiusX = this.CornerRadiusX;
				rcBackground.RadiusY = this.CornerRadiusY;
				rcBackground.Invalidate ();
			}
		}

        public override void Initialize () {
            base.Initialize();
        }

		protected override void OnBackgroundChanged (Brush color) {
			base.OnBackgroundChanged (color);
		}

        protected override void DrawBackground (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
			if ( rcBackground != null )
				rcBackground.Draw(gameTime, batch, alpha, transform);
        }
						
		protected virtual void OnClick () {
			this.RaiseClick();
		}

		internal protected void RaiseClick ( ) {
			if (this.Click != null)
				this.Click (this, EventArgs.Empty);
		}
			
		public override void OnTouchDown (TouchLocation state) {
			mouseDown = true;
			base.OnTouchDown (state);
		}

		public override void OnTouchUp (TouchLocation state) {
			if (mouseDown) {
				OnClick ();
				mouseDown = false;
			}
			base.OnTouchUp (state);
		}

		private bool mouseDown;
        private Shapes.Rectangle rcBackground;
	}
}
