using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace System.Windows.Media
{
    public class LinearGradientBrush : GradientBrush
    {
		public LinearGradientBrush () : base ( ) { }

        public override bool Equals (object obj) {
            var brush = obj as LinearGradientBrush;
            if (brush != null
                    && brush.StartPoint == this.StartPoint
                    && brush.EndPoint == this.EndPoint
                    && brush.Opacity == this.Opacity
                    && brush.GradientStops.Count == this.GradientStops.Count ) {

                var stops = this.GradientStops
                    .OrderBy(g => g.Offset)
                    .ToArray();

                var brushStops = brush.GradientStops
                    .OrderBy ( g => g.Offset )
                    .ToArray ( );

                for ( var i = 0; i < brushStops.Length; i++ ) {
                    if (!brushStops[i].Equals(stops[i]))
                        return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode () {
            var hash = "LGB_"
                + this.StartPoint.GetHashCode() + ";"
                + this.EndPoint.GetHashCode() + ";"
                + this.Opacity.GetHashCode() + ";";
            
            foreach (var gs in this.GradientStops.OrderBy(g => g.Offset)) {
                hash += gs.GetHashCode() + ";";
            }
            return hash.GetHashCode();
        }

        protected override Texture2D CreateTexture()
        {
            var width = 100;
            var height = 100;

            var tex = new Texture2D(this.GraphicsDevice, width, height);
            var data = new int[width * height];
            
            var deltaX = EndPoint.X - StartPoint.X;
            var deltaY = EndPoint.Y - StartPoint.Y;
            var denom = 1.0f / ((deltaX * deltaX) + (deltaY * deltaY));

            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    var t = (deltaX * (x - StartPoint.X) + deltaY * (y - StartPoint.Y)) * denom;
                    data[y * width + x] =
                        (int)GetGradientColor(t / 100f).PackedValue;
                }
            }

            tex.SetData(data);
            return tex;
        }

		internal override Color GetPixel ( int x, int y, int width, int height ) {
			var deltaX = EndPoint.X - StartPoint.X;
			var deltaY = EndPoint.Y - StartPoint.Y;
			var denom = 1.0f / ((deltaX * deltaX) + (deltaY * deltaY));
			var t = (deltaX * (x - StartPoint.X) + deltaY * (y - StartPoint.Y)) * denom;

			return this.GetGradientColor (t / 100f);
		}
    }
}
