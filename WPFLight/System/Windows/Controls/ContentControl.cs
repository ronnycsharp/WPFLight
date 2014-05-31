using System;
using System.Windows;
using System.Windows.Markup;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WPFLight.Resources;

namespace System.Windows.Controls {
	[ContentProperty("Content")]
	public abstract class ContentControl : Control {
		public ContentControl () : base ( ) {
			this.HorizontalContentAlignment = HorizontalAlignment.Center;
			this.VerticalContentAlignment = VerticalAlignment.Center;
		}

		#region Eigenschaften

		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register ( 
				"Content", 
				typeof ( Object ), 
				typeof ( ContentControl ), 
				new PropertyMetadata ( 
					(s,e)=> {
                        if (e.NewValue is UIElement)
                            ((UIElement)e.NewValue).Parent = (UIElement)s;

					    var ctrl = e.NewValue as Control;
					    if ( ctrl != null ) {
						    ((ContentControl)s).contentControl = ctrl;
					    } else
						    ((ContentControl)s).contentControl = null;

                        ((ContentControl)s).OnContentChanged(e.OldValue, e.NewValue);
                    } ) );

		public object Content {
			get { return GetValue (ContentProperty); }
			set { this.SetValue (ContentProperty, value); }
		}

		public bool HasContent {
			get {
				return this.Content != null;
			}
		}

		public static readonly DependencyProperty HorizontalContentAlignmentProperty =
			DependencyProperty.Register ( 
				"HorizontalContentAlignment", 
				typeof ( HorizontalAlignment ), 
				typeof ( ContentControl ),
				new PropertyMetadata ( 
					HorizontalAlignment.Center ) );

		public HorizontalAlignment HorizontalContentAlignment {
			get { return (HorizontalAlignment)this.GetValue (HorizontalContentAlignmentProperty); }
			set { this.SetValue (HorizontalContentAlignmentProperty, value); }
		}

		public static readonly DependencyProperty VerticalContentAlignmentProperty =
			DependencyProperty.Register ( 
				"VerticalContentAlignment", 
				typeof ( VerticalAlignment ), 
				typeof ( ContentControl ),
				new PropertyMetadata ( 
					VerticalAlignment.Center ) );

		public VerticalAlignment VerticalContentAlignment {
			get { return (VerticalAlignment) this.GetValue (VerticalContentAlignmentProperty); }
			set { this.SetValue (VerticalContentAlignmentProperty, value); }
		}

		#endregion

        protected virtual void OnContentChanged (object oldContent, object newContent) {

        }

		public override void Invalidate () {
			textSize = null;
			base.Invalidate ();

            if (this.Content is UIElement)
                ((UIElement)this.Content).Invalidate();


		}

		public override void Initialize () {
			if (contentControl != null
					&& !contentControl.IsInitialized )
				contentControl.Initialize ();

			base.Initialize ();
		}

		public override void Update (GameTime gameTime) {
			if (contentControl != null) {
				if (contentControl.IsInitialized)
					contentControl.Update (gameTime);
				else
					contentControl.Initialize ();
			}
			base.Update (gameTime);
		}

		internal override float GetAbsoluteLeft (UIElement child) {
			if (child == this.Content) {
				var absLeft = GetAbsoluteLeft ();
				switch (this.HorizontalContentAlignment) {
					case HorizontalAlignment.Left:
						{
							return absLeft + this.Padding.Left - this.Padding.Right;
						}
					case HorizontalAlignment.Center:
						{
							return absLeft + this.ActualWidth / 2f - child.ActualWidth / 2f;
						}
					case HorizontalAlignment.Right:
						{
							return absLeft + ActualWidth - child.ActualWidth - this.Padding.Right + this.Padding.Left;
						}
				}
			}
			return base.GetAbsoluteLeft (child);
		}

		internal override float GetAbsoluteTop (UIElement child) {
			if (child == this.Content) {
				var absTop = GetAbsoluteTop ();
				switch (this.VerticalContentAlignment) {
					case VerticalAlignment.Top:
						{
							return absTop + this.Padding.Top - this.Padding.Bottom;
						}
					case VerticalAlignment.Center:
						{
							return absTop + this.ActualHeight / 2f - child.ActualHeight / 2f;
						}
					case VerticalAlignment.Bottom:
						{
							return absTop + ActualHeight - child.ActualHeight + this.Padding.Top - this.Padding.Bottom;
						}
				}
			}
			return base.GetAbsoluteTop (child);
		}

		public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
			base.Draw (gameTime, batch, alpha, transform);
			if (this.Content != null && this.IsVisible ) {
				if (contentControl != null 
						&& contentControl.IsInitialized )
					contentControl.Draw (
						gameTime, batch, this.Opacity * alpha, transform);
				else if (this.Content is String) {
					// Beschriftung zeichnen
					var text = this.Content as String;
					if (text.Length > 0) {
						var left = GetAbsoluteLeft ();
						var top = GetAbsoluteTop ();
						var height = this.ActualHeight;
						var width = this.ActualWidth;

						if (textSize == null)
							textSize = this.GetFont().MeasureString (text) * this.FontSize; //* this.GetConvertedFontScale ( );
							
						var newLeft = 0f;
						var newTop = 0f;

						var fontHeight = textSize.Value.Y;

						switch ( this.HorizontalContentAlignment ) {
							case HorizontalAlignment.Center: {
									newLeft = left + ((this.ActualWidth * .5f) - (textSize.Value.X * .5f)) + this.Padding.Left - this.Padding.Right;
									break;
							}
							case HorizontalAlignment.Left: {
									newLeft = left + this.Padding.Left - this.Padding.Right;
									break;
								}
							case HorizontalAlignment.Right:
								{
									newLeft = left + this.ActualWidth - textSize.Value.X + this.Padding.Left - this.Padding.Right;
									break;
								}
						}

						switch ( this.VerticalContentAlignment ) {
							case VerticalAlignment.Center: {
									newTop = top + ((this.ActualHeight * .5f) - (fontHeight * .5f)) + this.Padding.Top - this.Padding.Bottom;
									break;
								}
							case VerticalAlignment.Top: {
									newTop = top + this.Padding.Top - this.Padding.Bottom;
									break;
								}
							case VerticalAlignment.Bottom:
								{
									newTop = top + this.ActualHeight - fontHeight + this.Padding.Top - this.Padding.Bottom;
									break;
								}
						}
								
						batch.Begin (
							SpriteSortMode.Deferred,
							BlendState.AlphaBlend,//this.Alpha < 1 ? BlendState.AlphaBlend : BlendState.Opaque,
							null,
							DepthStencilState.None,
							SCISSOR_ENABLED,
							null,
							transform);

						// Text
						batch.DrawString (
							GetFont(),
							text,
							new Vector2 (newLeft, newTop),
							this.Forecolor	* alpha * this.Opacity,
							0.0f,
							Vector2.Zero,
							GetConvertedFontScale ( ),
							SpriteEffects.None,
							1.0f);

						batch.End ();
					}
				}
			}
		}
			
		static readonly RasterizerState SCISSOR_ENABLED = 
			new RasterizerState { 
			CullMode = CullMode.None, 
			ScissorTestEnable = true };

		private Vector2? textSize;
		private Control  contentControl;
	}
}
