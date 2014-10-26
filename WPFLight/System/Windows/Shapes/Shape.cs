using System.Windows.Media;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Helpers;
using System.Threading.Tasks;

namespace System.Windows.Shapes {
	public abstract class Shape : FrameworkElement {
		#region Eigenschaften

		/// <summary>
		/// Fill property
		/// </summary>
		public static readonly DependencyProperty FillProperty =
			DependencyProperty.Register (
				"Fill",
				typeof(Brush),
				typeof(Shape));

		/// <summary>
		/// Fill property
		/// </summary>
		public Brush Fill {
			get { return (Brush)GetValue (FillProperty); }
			set { SetValue (FillProperty, value); }
		}

		/// <summary>
		/// Stroke property
		/// </summary>
		public static readonly DependencyProperty StrokeProperty =
			DependencyProperty.Register (
				"Stroke",
				typeof(Brush),
				typeof(Shape));

		/// <summary>
		/// Stroke property
		/// </summary>
		public Brush Stroke {
			get { return (Brush)GetValue (StrokeProperty); }
			set { SetValue (StrokeProperty, value); }
		}

		/// <summary>
		/// StrokeThickness property
		/// </summary>
		public static readonly DependencyProperty StrokeThicknessProperty =
			DependencyProperty.Register (
				"StrokeThickness",
				typeof(double),
				typeof(Shape) );

		/// <summary>
		/// StrokeThickness property
		/// </summary>
		public double StrokeThickness {
			get { return (double)GetValue (StrokeThicknessProperty); }
			set { SetValue (StrokeThicknessProperty, value); }
		}

		#endregion

		protected internal static Point GetCubicBezier (double t, Point p0, Point p1, Point p2, Point p3) {
			double cx = 3 * (p1.X - p0.X);
			double cy = 3 * (p1.Y - p0.Y);

			double bx = 3 * (p2.X - p1.X) - cx;
			double by = 3 * (p2.Y - p1.Y) - cy;

			double ax = p3.X - p0.X - cx - bx;
			double ay = p3.Y - p0.Y - cy - by;

			double Cube = t * t * t;
			double Square = t * t;

			double resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
			double resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

			return new Point ((float)resX, (float)resY);
		}

		public override void Invalidate ( ) {
            if ( this.ActualWidth > 0 && this.ActualHeight > 0 )
                texture = CreateTexture();

            /*
			if (this.ActualWidth > 0 && this.ActualHeight > 0) {
				CreateTextureAsync().ContinueWith(
					(t) => {
						//lock ( this ) {
							if ( t.IsCompleted && !t.IsFaulted && t.Result != null ) {
							//if ( texture != null && !texture.IsDisposed )
							//texture.Dispose ( );

									texture = t.Result;
							}
						//}
					} );
			}
             * */

			base.Invalidate ();
		}

        Task<Texture2D> CreateTextureAsync () {
            return Task.Factory.StartNew<Texture2D>(
                () => { return CreateTexture(); });
        }

		public override void Initialize () {
			base.Initialize ();
		}

		public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
			//base.Draw (gameTime, batch, alpha, transform);
		
			if (texture != null
				&& !texture.IsDisposed
			    && alpha > 0
				&& (this.Stroke != null || this.Fill != null)
				&&	this.IsVisible
				&& this.ActualWidth > 0
				&& this.ActualHeight > 0) {

				batch.Begin (
					SpriteSortMode.Deferred,
					BlendState.AlphaBlend,
					SamplerState.LinearClamp,
					DepthStencilState.None,
					SCISSOR_ENABLED,
					null,
					transform);

				var left = this.GetAbsoluteLeft ();
				var top = this.GetAbsoluteTop ();

				batch.Draw (
					texture,
					new Microsoft.Xna.Framework.Rectangle (
						(int)(left),
						(int)(top),
						(int)Math.Ceiling (this.ActualWidth),
						(int)Math.Ceiling (this.ActualHeight)),
					Microsoft.Xna.Framework.Color.White * alpha
				);

				batch.End ();
			}
		}

		protected abstract Texture2D CreateTexture ();

		private Texture2D 	texture;
		private bool 		invalidTexture;

		static readonly RasterizerState SCISSOR_ENABLED = 
			new RasterizerState { 
			CullMode = CullMode.None, 
			ScissorTestEnable = true };
	}
}
