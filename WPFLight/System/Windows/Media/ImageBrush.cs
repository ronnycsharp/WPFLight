using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WPFLight.Helpers;

namespace System.Windows.Media {
	public class ImageBrush : Brush {
		public ImageBrush (GraphicsDevice graphicsDevice, Texture2D texture)
            : base (graphicsDevice) {
			if (texture == null)
				throw new ArgumentNullException ();

			this.Texture = texture;
		}

		public ImageBrush (Texture2D texture)
			: base () {
			if (texture == null)
				throw new ArgumentNullException ();

			this.Texture = texture;
		}


		#region Eigenschaften

		internal Texture2D Texture { get; private set; }

		#endregion

		public override void Draw (SpriteBatch batch, Rectangle bounds, Matrix transform, float alpha) {
			if ( alpha > 0 ) {
				GraphicsDevice.ScissorRectangle = ScreenHelper.CheckScissorRect (bounds);
				batch.Begin (
					SpriteSortMode.Deferred,
					alpha == 1 ? BlendState.AlphaBlend : BlendState.AlphaBlend,
					SamplerState.LinearClamp,
					DepthStencilState.None,
					scissorEnabled,
					null,
					transform);

				var cx = (int)Math.Ceiling (
					(float)bounds.Width / (float)Texture.Width);

				var cy = (int)Math.Ceiling (
					(float)bounds.Height / (float)Texture.Height);

				for (var y = 0; y < Math.Max (1, cy); y++) {
					for (var x = 0; x < Math.Max (1, cx); x++) {
						batch.Draw (
							Texture,
							new Microsoft.Xna.Framework.Rectangle (
								bounds.X + x * Texture.Width, 
								bounds.Y + y * Texture.Height, 
								Texture.Width, 
								Texture.Height),
							null,
							Microsoft.Xna.Framework.Color.White * alpha,
							0,
							new Vector2 (),
							SpriteEffects.None,
							0);
					}
				}
				batch.End ();
			}
		}

		internal override Color GetPixel (int x, int y, int width, int height) {
			throw new NotImplementedException ();
		}
			
		static RasterizerState scissorEnabled = 
			new RasterizerState { 
				CullMode = CullMode.None, 
				FillMode = FillMode.Solid, 
				ScissorTestEnable = true, 
				MultiSampleAntiAlias =  false };

	}
}
