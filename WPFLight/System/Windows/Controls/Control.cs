using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WPFLight.Resources;
using WPFLight.Helpers;
using System.Windows.Media;

namespace System.Windows.Controls {
	public abstract class Control : FrameworkElement {
		public Control () {
			this.ScissorTest = false;
			this.HitTestEnabled = true;
			this.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.VerticalAlignment = VerticalAlignment.Stretch;

			this.Forecolor = Microsoft.Xna.Framework.Color.White;
		}

		public Control (SpriteFont font)
            : this () {
			this.Font = font;
		}

		#region Ereignisse

		public event EventHandler TouchDown;
		public event EventHandler TouchUp;

		#endregion

		#region Eigenschaften

		public static DependencyProperty FontProperty =
			DependencyProperty.Register (
				"Font", typeof(SpriteFont), typeof(Control));

		public SpriteFont Font {
			get{ return (SpriteFont)GetValue (FontProperty); }
			set{ SetValue (FontProperty, value); }
		}

		public static DependencyProperty FontNameProperty =
			DependencyProperty.Register (
				"FontName", typeof(String), typeof(Control));

		public string FontName {
			get{ return (string)GetValue (FontNameProperty); }
			set{ SetValue (FontNameProperty, value); }
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

		// TODO
		public float FontScale {
			get {
				return this.FontSize;
			}
			set {
				this.FontSize = value;
			}
		}

		#endregion

		static RasterizerState scissorEnabled =
			new RasterizerState {
				CullMode = CullMode.None,
				ScissorTestEnable = true
			};

		protected void RaiseTouchDown () {
			if (TouchDown != null)
				TouchDown (this, EventArgs.Empty);
		}

		protected void RaiseTouchUp () {
			if (TouchUp != null)
				TouchUp (this, EventArgs.Empty);
		}

		protected virtual void DrawBackground (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {

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


			/*
			batch.Begin (
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				null,
				DepthStencilState.None,
				this.ScissorTest ? scissorEnabled : RasterizerState.CullNone,
				null,
				transform);

			var left = this.GetAbsoluteLeft ();
			var top = this.GetAbsoluteTop ();
			*/

			/*
            // Hintergrund
            batch.Draw(
                Textures.Background,
                new Microsoft.Xna.Framework.Rectangle(
                (int)(left + this.BorderThickness.Left),
                (int)(top + this.BorderThickness.Top),
                (int)(this.ActualWidth - this.BorderThickness.Left - this.BorderThickness.Right),
                (int)(this.ActualHeight - this.BorderThickness.Top - this.BorderThickness.Bottom)),
                this.Background
                * ((float)this.Background.A / 256f)
				* this.Alpha * alpha);*/
			/*
			if (BorderThickness.Top > 0) {
				// Rahmen TOP
				batch.Draw (
					Textures.Background,
					new Microsoft.Xna.Framework.Rectangle (
						(int)(left + BorderThickness.Left),
						(int)top,
						(int)(this.ActualWidth - BorderThickness.Left - BorderThickness.Right),
						(int)this.BorderThickness.Top),
					this.BorderColor
					* ((float)this.BorderColor.A / 256f)
					* alpha);
			}

			if (BorderThickness.Bottom > 0) {
				// Rahmen BOTTOM
				batch.Draw (
					Textures.Background,
					new Microsoft.Xna.Framework.Rectangle (
						(int)(left + BorderThickness.Left),
						(int)(top + this.ActualHeight - BorderThickness.Bottom),
						(int)(this.ActualWidth - BorderThickness.Left - BorderThickness.Right),
						(int)this.BorderThickness.Bottom),
					this.BorderColor
					* ((float)this.BorderColor.A / 256f)
					* alpha);
			}

			if (BorderThickness.Left > 0) {
				// Rahmen LEFT
				batch.Draw (
					Textures.Background,
					new Microsoft.Xna.Framework.Rectangle (
						(int)(left),
						(int)(top),
						(int)(BorderThickness.Left),
						(int)(this.ActualHeight)),
					this.BorderColor
					* ((float)this.BorderColor.A / 256f)
					* alpha);
			}

			if (BorderThickness.Right > 0) {
				// Rahmen RIGHT
				batch.Draw (
					Textures.Background,
					new Microsoft.Xna.Framework.Rectangle (
						(int)(left + this.ActualWidth - BorderThickness.Right),
						(int)(top),
						(int)(BorderThickness.Right),
						(int)(this.ActualHeight)),
					this.BorderColor
					* ((float)this.BorderColor.A / 256f)
					* alpha);
			}

			batch.End ();
			*/
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
                            ScreenHelper.CheckScissorRect (
							new Microsoft.Xna.Framework.Rectangle (
								(int)this.Margin.Left,
								(int)this.Margin.Top,
								(int)(this.Width ?? 0),
								(int)(this.Height ?? 0)));
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
				this.Font, this.FontSize);
		}

		protected virtual void OnForegroundChanged (Brush color) { }

		protected virtual void OnBackgroundChanged (Brush color) { }

			
		static Dictionary<SpriteFont, float> fontScales;
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
