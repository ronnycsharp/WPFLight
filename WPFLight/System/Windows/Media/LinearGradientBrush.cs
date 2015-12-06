using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WPFLight.Helpers;
using WPFLight.Extensions;

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

        protected override Texture2D CreateTexture() {
            var width = 100;
            var height = 100;

            var tex = new Texture2D(this.GraphicsDevice, width, height);
            tex.SetData(this.GetTextureData(width, height));
            return tex;
        }

		internal override Color GetPixel ( int x, int y, int width, int height ) {
			var deltaX = EndPoint.X - StartPoint.X;
			var deltaY = EndPoint.Y - StartPoint.Y;
			var denom = 1.0f / ((deltaX * deltaX) + (deltaY * deltaY));
			var t = (deltaX * (x - StartPoint.X) + deltaY * (y - StartPoint.Y)) * denom;
			return this.GetGradientColor (t / 100f);
		}

        internal override int[] GetTextureData ( int width, int height ) {
            var pixels = new int[width * height];
            var copyHorizontal = false;
            var copyVertical = false;

            if (this.StartPoint.X == this.EndPoint.X) {
                // 0° / 180° - copy vertical lines
                copyVertical = true;
            }

            if (this.EndPoint.Y == this.StartPoint.Y) {
                // 90° / 270° - copy horizontal lines
                copyHorizontal = true;
            }

            if (copyVertical) {
                var lastValue = 0;
                var srcLine = default ( int[] );

                // copy each line
                for (var y = 0; y < height; y++) {
                    var value = (int) this.GetPixel(0, y, width, height).PackedValue;
                    if ( srcLine == null || ( lastValue!= value ) ) {
                        if ( srcLine == null )
                            srcLine = new int [ width ];

                        srcLine.Fill<int> ( value );
                        lastValue = value;
                    }
                    Array.Copy (
                        srcLine, 0, pixels, y * width, width); 
                }
            } else if ( copyHorizontal ) {
                // Array.Copy can't be used for Horizontal-Copy
                for (var x = 0; x < width; x++) {
                    var value = (int)this.GetPixel(x, 0, width, height).PackedValue;
                    for (var y = 0; y < height; y++) {
                        pixels[y * width + x] = value;
                    }
                }
            } else {
                // copy each pixel
                for ( var y = 0; y < height; y++ ) {
                    for ( var x = 0; x < width; x++) {
                        pixels[y * width + x] = 
                            ( int ) this.GetPixel(x, y, width, height).PackedValue;
                    }
                }
            }
            return pixels;
        }
    }
}
