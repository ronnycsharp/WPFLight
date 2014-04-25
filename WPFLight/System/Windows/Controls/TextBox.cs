using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;

namespace System.Windows.Controls {
	public class TextBox : Panel {
		public TextBox () {
			lblText = new Label () { ScissorTest = true };
			rcCursor = new System.Windows.Shapes.Rectangle ();
			rcBackground = new System.Windows.Shapes.Rectangle ();

			rcCursor.Visible = false;

			this.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.VerticalAlignment = VerticalAlignment.Stretch;
			//this.Foreground = Color.White;
			this.BorderThickness = new Thickness (1);
			//this.BorderColor = new Color (1f, 1f, 1f, .1f);
			//this.Width = 520;
			//this.Height = 40;

			this.Children.Add (lblText);
			this.Children.Add (rcCursor);
			this.Children.Add (rcBackground);

			this.FontScale = .35f;
			this.Focusable = true;
			this.Text = String.Empty;

			this.rcBackground.Alpha = .3f;
			this.rcCursor.Visible = false;
		}

		#region Eigenschaften

		//public float FontScale { get; set; }
		public string Text {
			get { return text; }
			set {
				if (text != value && value != null) {
					text = value;
					OnTextChanged (text);
				}
			} 
		}

		public ushort CursorIndex {
			get { return cursor; } 
			set {
				if (cursor != value) {
					if ((int)this.GetFont().MeasureString (text.Substring (0, value)).X * FontScale > this.ActualWidth) {
						lblText.Left = -1 - (int)(this.GetFont().MeasureString (text.Substring (0, value)).X * FontScale - this.ActualWidth);
					} else
						lblText.Left = 1;

					rcCursor.Left = 1 + Math.Min (this.ActualWidth - 2, (int)this.GetFont().MeasureString (text.Substring (0, value)).X * FontScale);
					rcCursor.Visible = true;
					cursor = value;
				} 
			}
		}

		#endregion

		public override void Initialize () {
			rcBackground.HorizontalAlignment = HorizontalAlignment.Stretch;
			rcBackground.VerticalAlignment = VerticalAlignment.Stretch;
			rcBackground.Fill = new SolidColorBrush (this.GraphicsDevice, new System.Windows.Media.Color (1, 1, 1, .3f));
			rcBackground.Visible = true;

			rcCursor.Width = 2;
			rcCursor.HorizontalAlignment = HorizontalAlignment.Left;
			rcCursor.VerticalAlignment = VerticalAlignment.Stretch;
			rcCursor.Margin = new Thickness (0, 5, 0, 5);
			rcCursor.Fill = Brushes.White;
			rcCursor.Alpha = 1f;
			rcCursor.Visible = false;

			rcCursor.Left = 1;

			lblText.Left = 0;
			lblText.Top = (float)(this.ActualHeight / 2.0 - (this.GetFont().MeasureString ("A").Y * this.FontScale) / 2.0);
			lblText.Width = this.ActualWidth;
			lblText.FontScale = this.FontScale;
			lblText.HorizontalAlignment = HorizontalAlignment.Left;
			lblText.VerticalAlignment = VerticalAlignment.Top;
			lblText.Foreground = this.Foreground;



			base.Initialize ();
		}

		public override void Update (GameTime gameTime) {
			if (this.Visible && this.IsEnabled && this.IsFocused) {
				if ((DateTime.UtcNow - lastCursorBlink) > TimeSpan.FromMilliseconds (300)) {
					rcCursor.Visible = !rcCursor.Visible;
					lastCursorBlink = DateTime.UtcNow;
				}
				base.Update (gameTime);
			}
		}

		public override void OnTouchMove (Microsoft.Xna.Framework.Input.Touch.TouchLocation state) {
			base.OnTouchMove (state);
			if (IsFocused && state.State == Microsoft.Xna.Framework.Input.Touch.TouchLocationState.Moved) {
				SetCursorIndexByPosition (state.Position);
			}
		}

		public override void OnTouchDown (Microsoft.Xna.Framework.Input.Touch.TouchLocation state) {
			this.Focus ();
			base.OnTouchDown (state);

			SetCursorIndexByPosition (state.Position);
		}

		public void SetCursorIndexByPosition (Vector2 position) {
			var left = 0f + GetAbsoluteLeft ();
			for (ushort i = 0; i < this.Text.Length; i++) {
				left += this.GetFont().MeasureString (this.Text [i].ToString ()).X * this.FontScale;

				if (position.X < left) {
					this.CursorIndex = i;
					return;
				}
			}

			if (position.X > left)
				this.CursorIndex = (ushort)this.Text.Length;
		}

		public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
			if (this.Visible)
				base.Draw (gameTime, batch, alpha, transform);
		}

		protected virtual void OnTextChanged (string text) {
			if (text == String.Empty)
				this.CursorIndex = 0;

			lblText.Text = text;
		}

		protected override void OnGotFocus () {
			this.rcBackground.Alpha = .9f;
			this.rcCursor.Visible = true;
			lastCursorBlink = DateTime.UtcNow + TimeSpan.FromMilliseconds (50);
			this.CursorIndex = (ushort)this.Text.Length;

			base.OnGotFocus ();
		}

		protected override void OnLostFocus () {
			this.rcBackground.Alpha = .3f;
			this.rcCursor.Visible = false;

			base.OnLostFocus ();
		}

		public void Insert (string text) {
			this.Text = this.Text.Insert (cursor, text);
			this.CursorIndex += (ushort)text.Length;
		}

		protected override void OnForegroundChanged (Brush foreground) {
			lblText.Foreground = foreground;
		}

		private DateTime lastCursorBlink;
		private System.Windows.Shapes.Rectangle rcBackground;
		private System.Windows.Shapes.Rectangle rcCursor;
		private Label lblText;
		private string text;
		private ushort cursor;
	}
}
