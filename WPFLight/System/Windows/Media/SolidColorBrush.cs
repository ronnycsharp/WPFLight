using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Helpers;
using WPFLight.Extensions;

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
			
		#region Properties

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

			var color = ColorHelper.ToXnaColor (this.Color);
			batch.Draw (solid, bounds, (color * ((float)color.A / 255f) * alpha));
			batch.End ();
		}

		Texture2D CreateSolidTexture () {
			var width = 10;
			var height = 10;
			var tex = new Texture2D (ScreenHelper.Device, width, height);
            var pixels = GetTextureData(width, height);
			tex.SetData (pixels);
			return tex;
		}

		internal override Color GetPixel (int x, int y, int width, int height) {
			return this.Color;
		}

        internal override int[] GetTextureData (int width, int height) {
            var pixels = new int[width * height];
            pixels.Fill((int)this.Color.PackedValue);
            //pixels[y * width + x]
            return pixels;
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
