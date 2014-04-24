using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WPFLight.Helpers;
using System.Windows.Media;

namespace System.Windows.Controls {
    public class Label : Control {
        public Label ( SpriteFont spriteFont ) : base ( spriteFont ) {
			this.TextAlignment = TextAlignment.Left;
			this.Foreground = Brushes.White;
            this.Alpha = 1.0f;
			this.FontScale = .35f;
            this.TextShadow = false;
        }

        #region Eigenschaften

		public TextAlignment TextAlignment { get; set; }
        public string Text { get; set; }
        //public float FontScale { get; set; }
        public bool TextShadow { get; set; }

        #endregion

        public override void Initialize ( ) {
			if ( DefaultTexture == null )
				DefaultTexture = this.Game.Content.Load<Texture2D> ( "bg_button" );

            base.Initialize ( );
        }

		internal override float MeasureWidth (float availableWidth)
		{
			if (measureWidth != null)
				return measureWidth.Value;

			var result = 0f;
			if (!String.IsNullOrEmpty (this.Text) && this.Font != null && this.FontScale > 0)
				result = this.Font.MeasureString (this.Text).X * this.FontScale;


			measureWidth = result;
			return result;
		}

		internal override float MeasureHeight (float availableHeight)
		{
			if (measureHeight != null)
				return measureHeight.Value;

			var result = 0f;
			if (!String.IsNullOrEmpty (this.Text) && this.Font != null && this.FontScale > 0)
				result = this.Font.MeasureString (this.Text).Y * this.FontScale;


			measureHeight = result;
			return result;
		}

		public override void Invalidate ()
		{
			base.Invalidate ();
			measureWidth = null;
			measureHeight = null;
		}

        public override void Draw ( GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform )
        {
            if ( this.IsVisible ( ) )
            {
				base.Draw ( gameTime, batch, alpha, transform );

                if ( !String.IsNullOrEmpty ( Text ) )
                {
                    var x 		= this.Alpha * alpha * ( IsEnabled  ? 1f : .5f );
                    var left 	= GetAbsoluteLeft ( );
                    var top 	= GetAbsoluteTop ( );

					if (this.TextAlignment == TextAlignment.Right)
						left = left + this.ActualWidth - this.MeasureWidth (this.ActualWidth);

                    if ( this.Parent != null && this.ScissorTest ) {
						var newLeft = this.Parent.GetAbsoluteLeft ();
						var newTop = this.Parent.GetAbsoluteTop ();
						var newWidth = this.Parent.ActualWidth;
						var newHeight = this.Parent.ActualHeight;

                        if ( newLeft < 0 )
                            newLeft = 0;

                        if ( newTop < 0 )
                            newTop = 0;

                        if ( newWidth < 0 )
                            newWidth = 0;

                        if ( newHeight < 0 )
                            newHeight = 0;

                        var scaleX = (float)ScreenHelper.SCREEN_WIDTH / (float)ScreenHelper.ORIGINAL_WIDTH;
                        var scaleY = ( float ) ScreenHelper.SCREEN_HEIGHT / ( float ) ScreenHelper.ORIGINAL_HEIGHT;

						var newBounds = 
							new Microsoft.Xna.Framework.Rectangle (
								(int)(newLeft*scaleX), 
                                (int)(newTop*scaleY), 
                                ( int ) ( newWidth*scaleX), 
                                ( int ) ( newHeight*scaleY) );

#if WINDOWS_PHONE
						if ( newBounds.Right > ScreenHelper.ORIGINAL_WIDTH )
							newBounds.Width = ScreenHelper.ORIGINAL_WIDTH - newBounds.Left;

						if ( newBounds.Bottom > ScreenHelper.ORIGINAL_HEIGHT )
							newBounds.Height = ScreenHelper.ORIGINAL_HEIGHT - newBounds.Top;
#endif

                        if ( newBounds.Left < ScreenHelper.ORIGINAL_WIDTH
                                && newBounds.Top < ScreenHelper.ORIGINAL_HEIGHT ) {
                            GraphicsDevice.ScissorRectangle = newBounds;
                        }
                    }

                    batch.Begin ( 
                        SpriteSortMode.Deferred, 
                        BlendState.AlphaBlend, 
                        null, 
                        DepthStencilState.None,
                        ( this.Parent != null && this.ScissorTest )
                            ? scissorEnabled : scissorDisabled,
                        null, 
                        transform );

					/*
					// DEBUGGING
					batch.Draw (
						DefaultTexture,
						new Microsoft.Xna.Framework.Rectangle (
						( int ) ( left  ),
						( int ) ( top ),
						( int ) ( this.ActualWidth - this.BorderThickness.Left - this.BorderThickness.Right ),
						( int ) ( this.ActualHeight - this.BorderThickness.Top - this.BorderThickness.Bottom ) ),
						Color.Red );
						*/

                    if ( this.TextShadow ) {
                        batch.DrawString (
                            this.Font,
                            this.Text,
                            new Vector2 ( left + 1f, top + 1f ),
                            Microsoft.Xna.Framework.Color.Black * .6f * x,
                            0.0f,
                            Vector2.Zero,
                            this.FontScale,
                            SpriteEffects.None,
                            1.0f );
                    }

                    batch.DrawString (
                        this.Font,
                        this.Text,
                        new Vector2 ( left, top ),
						this.Forecolor * x,
                        0.0f,
                        Vector2.Zero,
                        this.FontScale,
                        SpriteEffects.None,
                        1.0f );

                    batch.End ( );
                }
            }
        }

		static Texture2D DefaultTexture;

		private float? measureWidth;
		private float? measureHeight;

		static RasterizerState scissorEnabled = 
			new RasterizerState { CullMode = CullMode.None, ScissorTestEnable = true };

		static RasterizerState scissorDisabled = 
			new RasterizerState { CullMode = CullMode.None, ScissorTestEnable = false };

    }

	public enum TextAlignment {
		Left,
		Center,
		Right,
	}
}
