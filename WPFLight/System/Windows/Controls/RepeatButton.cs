using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace System.Windows.Controls {
	public class RepeatButton : Button {
		public RepeatButton () {
			
		}

		public RepeatButton ( SpriteFont font ) {
			this.Font = font;
		}

		#region Eigenschaften

		public static readonly DependencyProperty IntervalProperty =
			DependencyProperty.Register ( 
				"Interval", typeof ( TimeSpan ), typeof ( RepeatButton ), 
				new PropertyMetadata ( 
					TimeSpan.FromMilliseconds ( 80 ) ) );

		public TimeSpan Interval {
			get { return (TimeSpan)GetValue (IntervalProperty); }
			set { SetValue (IntervalProperty,value); }
		}

		public static readonly DependencyProperty DelayProperty =
			DependencyProperty.Register ( 
				"Delay", typeof ( TimeSpan ), typeof ( RepeatButton ), 
				new PropertyMetadata ( 
					TimeSpan.FromMilliseconds ( 130 ) ) );

		public TimeSpan Delay {
			get { return (TimeSpan)GetValue (DelayProperty); }
			set { SetValue (DelayProperty,value); }
		}

		#endregion

		public override void OnTouchDown (TouchLocation state) {
			mouseDown = true;
			lastClick = DateTime.UtcNow + this.Delay;
			base.OnTouchDown (state);
		}

		public override void OnTouchMove (TouchLocation state) {
			base.OnTouchMove (state);
			if (mouseDown) {
				if (DateTime.UtcNow - lastClick >= this.Interval) {
					OnClick ();
					lastClick = DateTime.UtcNow;
				}
			}
		}
		/*
		public override void OnTouchUp (TouchLocation state) {
			if (mouseDown) {
				OnClick ();
				mouseDown = false;
			}
			base.OnTouchUp (state);
		}
		*/

		private bool 		mouseDown;
		private DateTime 	lastClick;
	}
}