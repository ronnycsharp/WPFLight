using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Helpers;

namespace System.Windows.Media {
	public class SolidColorBrush : Brush {
		public SolidColorBrush () { }

		public SolidColorBrush (GraphicsDevice device) : base (device) {}

		public SolidColorBrush (GraphicsDevice device, Color color) 
			: base (device) {
			this.Color = color;
		}

		public SolidColorBrush (Color color) {
			this.Color = color;
		}

		public SolidColorBrush (byte r, byte g, byte b) {
			this.Color = new Color (r, g, b);
		}

		public SolidColorBrush (byte r, byte g, byte b, byte a) {
			this.Color = new Color (r, g, b, a);
		}

		public SolidColorBrush (float r, float g, float b) {
			this.Color = new Color (r, g, b);
		}

		public SolidColorBrush (float r, float g, float b, float a) {
			this.Color = new Color (r, g, b, a);
		}
			
		#region Eigenschaften

		public System.Windows.Media.Color Color { get; set; }

		internal Texture2D Texture { get { return solid; } }

		#endregion

		public override void Draw (SpriteBatch batch, Rectangle bounds, Matrix transform, float alpha) {
			if (solid == null)
				solid = CreateSolidTexture ();

			batch.Begin (
				SpriteSortMode.Texture,
				BlendState.AlphaBlend,
				SamplerState.LinearClamp,
				DepthStencilState.None,
				RasterizerState.CullNone,
				null,
				transform);

			//this.Color = Colors.Red * .1f;

			var color = ColorHelper.ToXnaColor (this.Color);

			//color = new Microsoft.Xna.Framework.Color ( 1f,0,0 ) * .5f;

			batch.Draw (solid, bounds, (color * ((float)color.A / 255f) * alpha));
			batch.End ();
		}

		Texture2D CreateSolidTexture () {
			var width = 10;
			var height = 10;
			//var widthF = (float)width;

			var tex = new Texture2D (ScreenHelper.Device, width, height);
			var pixels = new int[width * height];

			for (var x = 0; x < width; x++) {
				for (var y = 0; y < height; y++)
					pixels [y * width + x] = (int)Colors.White.PackedValue;
			}
			tex.SetData (pixels);
			return tex;
		}

		internal override Color GetPixel (int x, int y, int width, int height) {
			return this.Color;
		}

        public override int GetHashCode () {
            return this.Color.GetHashCode();
        }

        public override bool Equals (object obj) {
            return obj is SolidColorBrush 
                && ((SolidColorBrush)obj).Color == this.Color;
        }

		static Texture2D solid;
	}
}
