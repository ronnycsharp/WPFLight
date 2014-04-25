using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WPFLight.Helpers;

namespace System.Windows.Controls {
	public abstract class Control : FrameworkElement {
		public Control () {
			this.HitTestEnabled = true;
			this.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.VerticalAlignment = VerticalAlignment.Stretch;
			this.Forecolor = Microsoft.Xna.Framework.Color.White;

            rcBackground = new Shapes.Rectangle();
		}

		#region Events

		public event EventHandler TouchDown;
		public event EventHandler TouchUp;

		#endregion

		#region Properties

		public static DependencyProperty FontFamilyProperty =
			DependencyProperty.Register (
				"FontFamily", typeof(FontFamily), typeof(Control));

		public FontFamily FontFamily {
			get {
                // if property is null, the property is inherited from its parent-control
                var fontFamily = (FontFamily)GetValue (FontFamilyProperty);
                if (fontFamily == null && this.Parent is Control)
                    fontFamily = ((Control)this.Parent).FontFamily;

                if (fontFamily == null)
                    fontFamily = DEFAULT_FONTFAMILY;

                return fontFamily;
            }
			set { SetValue (FontFamilyProperty, value); }
		}

		public static DependencyProperty FontSizeProperty =
			DependencyProperty.Register (
				"FontSize", typeof(float), typeof(Control));

		public float FontSize {
			get{ return (float)GetValue (FontSizeProperty); }
			set{ SetValue (FontSizeProperty, value); }
		}

		public bool HitTestEnabled { get; set; }

		public static readonly DependencyProperty BorderBrushProperty =
			DependencyProperty.Register (
				"BorderBrush",
				typeof(Brush),
				typeof(Control));

		public Brush BorderBrush {
			get { return (Brush)GetValue (BorderBrushProperty); }
			set { SetValue (BorderBrushProperty, value); }
		}

		public static readonly DependencyProperty BackgroundProperty =
			DependencyProperty.Register (
				"Background",
				typeof(Brush),
				typeof(Control));

		public Brush Background {
			get { return (Brush)GetValue (BackgroundProperty); }
			set { SetValue (BackgroundProperty, value); }
		}

		public static readonly DependencyProperty BorderThicknessProperty =
			DependencyProperty.Register (
				"BorderThickness",
				typeof(Thickness),
				typeof(Control));

		public Thickness BorderThickness {
			get { return (Thickness)GetValue (BorderThicknessProperty); }
			set { SetValue (BorderThicknessProperty, value); }
		}

		public bool ScissorTest { get; set; }

		public static readonly DependencyProperty IsTouchDownProperty =
			DependencyProperty.Register (
				"IsTouchDown",
				typeof(bool),
				typeof(Control));

		public bool IsTouchDown {
			get { return (bool)GetValue (IsTouchDownProperty); }
			internal set { SetValue (IsTouchDownProperty, value); }
		}

		public static readonly DependencyProperty ForegroundProperty =
			DependencyProperty.Register (
				"Foreground",
				typeof(Brush),
				typeof(Control),
				new PropertyMetadata (
					Brushes.White, 
					new PropertyChangedCallback ( 
						( s, e ) => {
							var solid = e.NewValue as SolidColorBrush;
							if ( solid == null )
								throw new NotSupportedException ( "Foreground can only be assigned with a SolidColorBrush" );

							((Control)s).Forecolor = new Microsoft.Xna.Framework.Color ( 
								solid.Color.R, solid.Color.G, solid.Color.B,  solid.Color.A );
						} )));

		public Brush Foreground {
			get { return (Brush)GetValue (ForegroundProperty); }
			set { SetValue (ForegroundProperty, value); }
		}

		internal Microsoft.Xna.Framework.Color Forecolor {
			get;
			set;
		}

		// TODO REMOVE, use only FontSize
		public float FontScale {
			get {
				return this.FontSize;
			}
			set {
				this.FontSize = value;
			}
		}

		#endregion

        internal SpriteFont GetFont () {
            return FontContainer.Resolve(this.FontFamily);
        }

		protected void RaiseTouchDown () {
			if (TouchDown != null)
				TouchDown (this, EventArgs.Empty);
		}

		protected void RaiseTouchUp () {
			if (TouchUp != null)
				TouchUp (this, EventArgs.Empty);
		}

        public override void Invalidate () {
            base.Invalidate();

            rcBackground.Fill = this.Background;
            rcBackground.Stroke = this.BorderBrush;
            rcBackground.StrokeThickness = this.BorderThickness.Left;
            rcBackground.Invalidate();
        }

		protected virtual void DrawBackground (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
            rcBackground.Draw(gameTime, batch, alpha*this.Alpha, transform);

            /*
            if (this.Background != null)
				this.Background.Draw (
					batch, 
					new Rectangle (
						(int)this.GetAbsoluteLeft (), 
						(int)this.GetAbsoluteTop (), 
						(int)this.ActualWidth, 
						(int)this.ActualHeight), 
					transform, 
					alpha);
             * */
		}

		public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {


			//base.Draw (gameTime, batch, alpha, transform);

			if (alpha > 0 && this.Alpha > 0 && this.IsVisible ()) {
				if (this.Parent != null && ( this.Parent is Control ) && ((Control) this.Parent).ScissorTest) {
					var parentHeight = this.Parent.ActualHeight;
					var parentWidth = this.Parent.ActualWidth;

					var parentLeft = this.Parent.GetAbsoluteLeft ();
					var parentTop = this.Parent.GetAbsoluteTop ();

					var parentBounds = new Microsoft.Xna.Framework.Rectangle (
						                                  (int)parentLeft, (int)parentTop, (int)parentWidth, (int)parentHeight);

					var bounds = new Microsoft.Xna.Framework.Rectangle (
						                            (int)GetAbsoluteLeft (),
						                            (int)GetAbsoluteTop (),
						                            (int)this.ActualWidth,
						                            (int)this.ActualHeight);

					var newLeft = bounds.Left;
					if (bounds.Left < parentBounds.Left)
						newLeft = parentBounds.Left;

					var newTop = bounds.Top;
					if (bounds.Top < parentBounds.Top)
						newTop = parentBounds.Top;

					var newWidth = bounds.Width;
					if ((bounds.Left + bounds.Width) > (parentBounds.Left + parentBounds.Width))
						newWidth = (int)this.Parent.ActualWidth;

					var newHeight = bounds.Height;
					if ((bounds.Top + bounds.Height) > (parentBounds.Top + parentBounds.Height)) {
						newHeight = (int)this.Parent.ActualHeight;
					}

					if (newLeft < 0)
						newLeft = 0;

					if (newTop < 0)
						newTop = 0;

					if (newWidth < 0)
						newWidth = 0;

					if (newHeight < 0)
						newHeight = 0;

					var scaleX = (float)ScreenHelper.SCREEN_WIDTH / (float)ScreenHelper.ORIGINAL_WIDTH;
					var scaleY = (float)ScreenHelper.SCREEN_HEIGHT / (float)ScreenHelper.ORIGINAL_HEIGHT;

					var newBounds =
						new Microsoft.Xna.Framework.Rectangle (
							(int)(newLeft * scaleX), (int)(newTop * scaleY), (int)(newWidth * scaleX), (int)(newHeight * scaleY));

#if WINDOWS_PHONE
                    if (newBounds.Right > ScreenHelper.ORIGINAL_WIDTH)
                        newBounds.Width = ScreenHelper.ORIGINAL_WIDTH - newBounds.Left;

                    if (newBounds.Bottom > ScreenHelper.ORIGINAL_HEIGHT)
                        newBounds.Height = ScreenHelper.ORIGINAL_HEIGHT - newBounds.Top;

#endif

					if (newBounds.Left < ScreenHelper.ORIGINAL_WIDTH
					                   && newBounds.Top < ScreenHelper.ORIGINAL_HEIGHT)
						GraphicsDevice.ScissorRectangle = newBounds;

				} else {
					if (this is Window) {
						GraphicsDevice.ScissorRectangle =
                            ScreenHelper.CheckScissorRect ( this.Bounds );
					}
				}

				this.DrawBackground (gameTime, batch, this.Alpha * alpha, transform);
			}
		}

		public virtual bool HitTest (Vector2 v) {
			if (this.HitTestEnabled) {
				var left = this.GetAbsoluteLeft ();
				var top = this.GetAbsoluteTop ();

				if (v.X >= left
				    && v.Y >= top
				    && v.X <= left + this.ActualWidth
				    && v.Y <= top + this.ActualHeight) {
					return true;
				}
			}
			return false;
		}

		public virtual void OnTouchDown (TouchLocation state) {
			this.IsTouchDown = true;
			this.RaiseTouchDown ();
		}

		public virtual void OnTouchUp (TouchLocation state) {
			this.IsTouchDown = false;
			this.RaiseTouchUp ();
		}

		public virtual void OnTouchMove (TouchLocation state) {

		}

		internal static float ConvertFontSizeToScale (SpriteFont font, float fontSize) {

			if (fontScales == null)
				fontScales = new Dictionary<SpriteFont,float> ();
				
			return fontSize;

			// TODO

			var fontScale = fontSize;
			if (fontScales.ContainsKey (font))
				return fontScales [font];
			else {
				fontScales [font] = fontScale;
				return fontScale;
			}

		}

		internal float GetConvertedFontScale () {
			return ConvertFontSizeToScale (
				this.GetFont ( ), this.FontSize);
		}

		protected virtual void OnForegroundChanged (Brush color) { }

		protected virtual void OnBackgroundChanged (Brush color) { }
			
		static Dictionary<SpriteFont, float> fontScales;
        static readonly FontFamily DEFAULT_FONTFAMILY = new FontFamily("Large");

        private Shapes.Rectangle rcBackground;
	}

	public enum VerticalAlignment {
		Top,
		Bottom,
		Stretch,
		Center,
	}

	public enum HorizontalAlignment {
		Left,
		Right,
		Stretch,
		Center,
	}
}
