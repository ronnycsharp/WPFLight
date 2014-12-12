using System;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;

namespace System.Windows.Controls {
	public class TrackBar : Panel {
        public TrackBar () {
            this.Height = 60;
            this.MinValue = 0;
            this.MaxValue = 100;
            this.Value = 0;
        }

		#region Ereignisse

		public event EventHandler ValueChanged;

		#endregion

		#region Eigenschaften

		public float MinValue { 
			get { return minValue; } 
			set { 
				if (value != minValue) {
					minValue = value;

					if (minValue > maxValue)
						maxValue = minValue;

					Refresh ();
				} 
			} 
		}

		public float MaxValue { 
			get { return maxValue; } 
			set { 
				if (value != maxValue) {
					maxValue = value;
					Refresh ();
				} 
			} 
		}

		public float Value { 
			get { return myValue; }
			set {
                if (value != myValue) {
                    var v = MathHelper.Clamp(value, MinValue, MaxValue);
                    if (v != myValue) {
                        myValue = v;
                        this.RaiseValueChanged();
                    }
                }
			}
		}

		#endregion

		public override void Initialize () {
			rcBackground = new System.Windows.Shapes.Rectangle ();
			rcBackground.Fill = Brushes.Gray;
			rcBackground.HorizontalAlignment = HorizontalAlignment.Stretch;
			rcBackground.Height = 5;
			rcBackground.Margin = new Thickness (17,2,17,2);
			rcBackground.Top = (int)((this.ActualHeight / 2f) - (rcBackground.Height / 2f));
			rcBackground.StrokeThickness = 1;
			rcBackground.RadiusX = 0;
			rcBackground.RadiusY = 0;
			this.Children.Add (rcBackground);

			cmdTrack = new Button ( );
            cmdTrack.FontFamily = this.FontFamily;
			cmdTrack.Content = 
				new System.Windows.Shapes.Rectangle { 
					Fill = Brushes.White, 
					Width = 30, 
					Height = 30,
			};

			cmdTrack.Background = Brushes.Transparent;
			cmdTrack.BorderThickness = new Thickness (0);
			cmdTrack.Width = 60;
			cmdTrack.Height = 60;
			cmdTrack.VerticalAlignment = VerticalAlignment.Center;

			cmdTrack.Left = 0;
			cmdTrack.Margin = new Thickness (0, 0, 0, 1);
			cmdTrack.CornerRadiusX = 0;
			cmdTrack.CornerRadiusY = 0;

			this.Children.Add (cmdTrack);

			base.Initialize ();
			this.Refresh ();
		}

		public override void Update (GameTime gameTime) {
			base.Update (gameTime);
			cmdTrack.Left = (int)(this.Value * factor);
			cmdTrack.Visible = this.IsEnabled;
		}

		public override void OnTouchDown (Microsoft.Xna.Framework.Input.Touch.TouchLocation state) {
			base.OnTouchDown (state);
			if (cmdTrack.IsTouchDown) {
				startPosX = state.Position.X;
				startValue = this.Value;
				moving = true;
			}
		}

		public override void OnTouchUp (Microsoft.Xna.Framework.Input.Touch.TouchLocation state) {
			base.OnTouchUp (state);
			moving = false;
		}

		public override void OnTouchMove (Microsoft.Xna.Framework.Input.Touch.TouchLocation state) {
			base.OnTouchMove (state);
			if (moving) {
				var x = state.Position.X - startPosX;
				this.Value = (x / factor) + startValue;
			}
		}

		void Refresh ( ) {
			if (this.IsInitialized) {
				factor = (this.ActualWidth - this.cmdTrack.ActualWidth)
					/ MathHelper.Distance (MinValue, MaxValue);
			}
		}

		void RaiseValueChanged () {
			if (this.ValueChanged != null)
				this.ValueChanged (this, EventArgs.Empty);
		}

		private System.Windows.Shapes.Rectangle rcBackground;
		private Button cmdTrack;
		private float factor;
		private bool moving;
		private float startPosX;
		private float startValue;
		private float myValue;

		private float minValue;
		private float maxValue;
	}
}
