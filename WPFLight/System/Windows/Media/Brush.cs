using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WPFLight.Helpers;
using System.ComponentModel;

namespace System.Windows.Media {
	[TypeConverter (typeof(BrushConverter))]
	public abstract class Brush {
		public Brush () {
			this.Opacity = 1;
			this.GraphicsDevice = ScreenHelper.Device;
		}

		public Brush (GraphicsDevice graphicsDevice) {
			/*
            if (graphicsDevice == null)
                throw new ArgumentNullException();

            this.GraphicsDevice = graphicsDevice;
            */

			this.GraphicsDevice = ScreenHelper.Device;
		}

		#region Eigenschaften

		public GraphicsDevice GraphicsDevice { get; private set; }

		public float Opacity { get; set; }

		#endregion

		internal abstract Color GetPixel (int x, int y, int width, int height);

		public abstract void Draw (SpriteBatch batch, Rectangle bounds, Matrix transform, float alpha);

		internal static Brush Parse (string value, ITypeDescriptorContext context) {
			if (value == null)
				throw new ArgumentNullException ();

			if (value.StartsWith ("#")) {
				return new SolidColorBrush (
					ColorHelper.ConvertFromHex (value));
			} else {
				return Brushes.GetBrush (
					ColorHelper.GetNamedColor (value));
			}
		}
	}
}
