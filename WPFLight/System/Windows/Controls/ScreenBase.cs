using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WPFLight.Helpers;

namespace System.Windows.Controls {
	public abstract class ScreenBase : Panel {
        public ScreenBase ( ) : base (  ) {
            this.DrawOrder = 1;
            this.Visible = false;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.Width = ScreenHelper.ORIGINAL_WIDTH;
            this.Height = ScreenHelper.ORIGINAL_HEIGHT;
			this.ScissorTest = false;
			this.HitTestEnabled = true;

            fadeAnimation = new SingleAnimation ( 
                0, 1, TimeSpan.FromMilliseconds ( 300 ), false );

            fadeAnimation.Completed += delegate {
                if ( hiding )
                {
                    this.Visible = false;
                    this.IsEnabled = false;
                    if ( this.Closed != null )
                        this.Closed ( this, EventArgs.Empty );
                }
                else
                {
                    this.IsEnabled = true;
                    if ( this.Open != null )
                        this.Open ( this, EventArgs.Empty );
                }
            };
        }

        #region Eigenschaften

        public bool IsPressed { get; protected set; }
		public new float ActualWidth { get { return ScreenHelper.ORIGINAL_WIDTH; } }
		public new float ActualHeight { get { return ScreenHelper.ORIGINAL_HEIGHT; } }

        #endregion

        #region Ereignisse

        public event EventHandler Open;
        public event EventHandler Closed;

        #endregion

        public virtual void Show ( ) {
            hiding = false;
            fadeAnimation.Begin ( );
            this.Visible = true;
            this.IsEnabled = true;
        }

        public virtual void Hide ( ) {
            hiding = true;
            fadeAnimation.Begin ( );
        }

        public override void Initialize () {
			//fadeAnimation.Initialize();
            base.Initialize();
        }

        public override void Update ( GameTime gameTime ) {
            var pressed = false;
            foreach ( var c in this.Children.OfType<IUpdateable> ( ) )
            {
                c.Update ( gameTime );
                if ( !pressed )
                {
                    var cmd = c as Button;
                    if ( cmd != null )
                    {
                        if ( cmd.IsTouchDown )
                            pressed = true;
                    }
                }
            }
            this.IsPressed = pressed;
            base.Update ( gameTime );

            fadeAnimation.Update(gameTime);
        }

        public override void Draw ( GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform ) {
            if ( this.Visible && alpha > 0 && this.Alpha > 0 ) {
                var currentValue = 0f;
                if ( hiding )
                    currentValue = fadeAnimation.GetCurrentValue ( );
                else
                    currentValue = 1- fadeAnimation.GetCurrentValue ( );

                base.Draw ( gameTime, batch, alpha * ( 1 - currentValue ), transform * Matrix.CreateTranslation ( this.ActualWidth * (hiding?-currentValue:currentValue), 0, 0 ) );
            }
        }

		public override float GetAbsoluteLeft ()
		{
			return 0;
		}

		public override float GetAbsoluteTop ()
		{
			return 0;
		}

        private bool hiding;
        private SingleAnimation fadeAnimation;
    }
}
