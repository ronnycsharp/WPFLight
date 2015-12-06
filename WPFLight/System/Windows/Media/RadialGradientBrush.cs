using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace System.Windows.Media {
	public class RadialGradientBrush : GradientBrush {
		public RadialGradientBrush ( ) : base ( ) {
			this.Radius = 1;
			this.Center = new Vector2 (.5f, .5f);
			this.GradientOrigin = new Vector2 (0.5f, 1f);			
		}

		protected override Texture2D CreateTexture () {
			var width = 100;
			var height = 100;

			var tex = new Texture2D (this.GraphicsDevice, width, height);
			var data = new int[width * height];

			int x, y;
			float grad, fx_, fy_, dx, dy, denom, radius2, dx2, dy2;
            
			fx_ = this.GradientOrigin.X - this.Center.X;
			fy_ = this.GradientOrigin.Y - this.Center.Y;
			radius2 = this.Radius * this.Radius;
			denom = 1.0f / (radius2 - ((fx_ * fx_) + (fy_ * fy_)));
            
			for (y = 0; y < height; ++y) {
				for (x = 0; x < width; ++x) {
					dx = x - this.GradientOrigin.X;
					dy = y - this.GradientOrigin.Y;
					dx2 = dx * dx;
					dy2 = dy * dy;
					grad = (radius2 * (dx2 + dy2)) - ((dx * fy_ - dy * fx_) * (dx * fy_ - dy * fx_));
					grad = ((dx * fx_ + dy * fy_) + (float)Math.Sqrt (grad));
					grad *= denom;
                    
					data [y * width + x] =
                        (int)GetGradientColor (grad / 100f).PackedValue;
				}
			}
			tex.SetData (data);
			return tex;
		}

		internal override Color GetPixel (int x, int y, int width, int height) {
			float grad, fx_, fy_, dx, dy, denom, radius2, dx2, dy2;

			fx_ = this.GradientOrigin.X - this.Center.X;
			fy_ = this.GradientOrigin.Y - this.Center.Y;
			radius2 = this.Radius * this.Radius;
			denom = 1.0f / (radius2 - ((fx_ * fx_) + (fy_ * fy_)));

			dx = x - this.GradientOrigin.X;
			dy = y - this.GradientOrigin.Y;
			dx2 = dx * dx;
			dy2 = dy * dy;
			grad = (radius2 * (dx2 + dy2)) - ((dx * fy_ - dy * fx_) * (dx * fy_ - dy * fx_));
			grad = ((dx * fx_ + dy * fy_) + (float)Math.Sqrt (grad));
			grad *= denom;

			return this.GetGradientColor (grad / 100f);
		}

        internal override int[] GetTextureData (int width, int height) {
            throw new NotImplementedException();
        }

		#region Eigenschaften

		/// <summary>
		/// gets or sets the radius of the radial-gradient
		/// </summary>
		public float Radius { get; set; }

		/// <summary>
		/// gets or sets the center-point
		/// </summary>
		public Vector2 Center { get; set; }

		/// <summary>
		/// gets or sets the focus-point
		/// </summary>
		public Vector2 GradientOrigin { get; set; }

		#endregion
	}
}
