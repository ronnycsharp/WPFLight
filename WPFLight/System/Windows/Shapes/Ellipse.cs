using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using WPFLight.Helpers;
using WPFLight.Resources;

// TODO Unfinished

namespace System.Windows.Shapes {
	public class Ellipse : Shape {
		public Ellipse () : base () {

		}

		#region Eigenschaften

		public float RadiusX { get; set; }

		public float RadiusY { get; set; }

		#endregion

		public override void Invalidate () {
			base.Invalidate ();

            if (texture != null && !texture.IsDisposed)
                texture.Dispose();

			texture = null;
		}

        public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
            base.Draw(gameTime, batch, alpha, transform);

            //return; // TODO
			if (this.IsVisible ( ) && this.Alpha > 0 && alpha > 0 && this.Background != null
                    && (this.RadiusX != 0 || this.RadiusY != 0)
                    && this.ActualWidth > 0
                    && this.ActualHeight > 0) {

				if (texture == null)
					texture = this.CreateRoundedRectangle ();

                batch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    null,
                    DepthStencilState.None,
                    RasterizerState.CullNone,
                    null,
                    transform);

                var left = this.GetAbsoluteLeft();
                var top = this.GetAbsoluteTop();

                if (texture != null) {
                    // Hintergrund
                    batch.Draw(
                        texture,
                        new Microsoft.Xna.Framework.Rectangle(
                            (int)(left),
                            (int)(top),
                            (int)Math.Ceiling((this.ActualWidth)),
                            (int)Math.Ceiling((this.ActualHeight))),
						Color.White * alpha
                    );
                }
                batch.End();
            }
        }

		static Point GetCubicBezier (double t, Point p0, Point p1, Point p2, Point p3) {
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

			return new Point (resX, resY);
		}

		static Point [] GetPoints ( Rect rect, float radiusX, float radiusY, Point center ) {
            var points = new Point[13];

            radiusX = Math.Abs(radiusX);
            radiusY = Math.Abs(radiusY);

            double c_arcAsBezier = 0.5522847498307933984;

            // Set the X coordinates
            double mid = radiusX * c_arcAsBezier;

            points[0].X = points[1].X = points[11].X = points[12].X = center.X + radiusX;
            points[2].X = points[10].X = center.X + mid;
            points[3].X = points[9].X = center.X;
            points[4].X = points[8].X = center.X - mid;
            points[5].X = points[6].X = points[7].X = center.X - radiusX;

            // Set the Y coordinates
            mid = radiusY * c_arcAsBezier;

            points[2].Y = points[3].Y = points[4].Y = center.Y + radiusY;
            points[1].Y = points[5].Y = center.Y + mid;
            points[0].Y = points[6].Y = points[12].Y = center.Y;
            points[7].Y = points[11].Y = center.Y - mid;
            points[8].Y = points[9].Y = points[10].Y = center.Y - radiusY;

			return points;
		}

		static Rect [] GetRects ( Point [] points ) {
			var rcList = new List<Rect> ();
			var start = default(Point);
			var end = default(Point);

			var lastX1 = -1.0;
			var lastX2 = -1.0;

			for (var m = 0.0M; m <= 1.0M; m += .01M) {
				var t = (float)m;
				var i = 0;
				var p1 = GetCubicBezier (
					t, points [0 + i], points [1 + i], points [2 + i], points [3 + i]);

				i = 4;
				var p2 = GetCubicBezier (
					1 - t, points [0 + i], points [1 + i], points [2 + i], points [3 + i]);

				i = 8;
				var p3 = GetCubicBezier (
					t, points [0 + i], points [1 + i], points [2 + i], points [3 + i]);

				i = 12;
				var p4 = GetCubicBezier (
					1 - t, points [0 + i], points [1 + i], points [2 + i], points [3 + i]);

				var r1 = new Rect ((int)p1.X, (int)p1.Y, 1, (int)(p2.Y - p1.Y));
				var r2 = new Rect ((int)p3.X, (int)p3.Y, 1, (int)(p4.Y - p3.Y));

				if (Math.Floor (p1.X) != lastX1) {
					rcList.Add (r1);
					lastX1 = Math.Floor (p1.X);
				}

				if (Math.Floor (p3.X) != lastX2) {
					rcList.Add (r2);
					lastX2 = Math.Floor (p3.X);
				}

				if (t == 1.0)
					start = p1;
				else if (t == 0.0)
					end = p4;
			}

			rcList.Add (new Rect (
				(int)start.X, 
                (int)start.Y, 
                (int)(end.X - start.X), 
                (int)(end.Y - start.Y)));

			return rcList.ToArray ( );
		}

		static void FillPixels ( 
			Brush brush, Rect [] rects, int width, int height, ref int [] pixels ) {
			foreach (var rc in rects) {
				for (var x = (int)rc.Left; x < rc.Right; x++) {

					var pixelTopColor = new Color { PackedValue = (uint)pixels [((int)rc.Top) * width + x] };
					var pixelBottomColor = new Color { PackedValue = (uint)pixels [((int)rc.Bottom-1) * width + x] };

					var topColor = brush.GetPixel (x, (int)rc.Top, width, height);
					var bottomColor = brush.GetPixel (x, (int)rc.Bottom, width, height);

					// Antialiazing, Top

					if (rc.Top > 0) {
						pixels [((int)rc.Top - 1) * width + x] = (int)Color.Lerp (pixelTopColor, topColor, .65f).PackedValue;
					}
					if (rc.Top > 1) {
						pixels [((int)rc.Top - 2) * width + x] = (int)Color.Lerp (pixelTopColor, topColor, .38f).PackedValue;
					}

					for (var y = (int)rc.Top; y < rc.Bottom; y++) {
						pixels [y * width + x] = (int)brush.GetPixel (x, y, width, height).PackedValue;
					}

					// Antialiazing, Bottom

					if (rc.Bottom < height)
						pixels [((int)rc.Bottom) * width + x] = (int)Color.Lerp (pixelBottomColor, bottomColor, .65f).PackedValue;
						
					if (rc.Bottom < height - 1)
						pixels [(int)(rc.Bottom + 1) * width + x] = (int)Color.Lerp (pixelBottomColor, bottomColor, .38f).PackedValue;
				}
			}
		}

		protected Texture2D CreateRoundedRectangle ( ) {
			var width = (int)Math.Ceiling (this.ActualWidth*2.0);
			var height = (int)Math.Ceiling (this.ActualHeight*2.0);

			var rcBorder = new Rect (
				0, 0, width, height);

			var rcBackground = new Rect (
				(int)Math.Ceiling (this.BorderThickness.Left*2.0),
				(int)Math.Ceiling (this.BorderThickness.Top*2.0),
				(int)Math.Ceiling (this.ActualWidth*2.0 - this.BorderThickness.Left*2.0 - this.BorderThickness.Right*2.0),
				(int)Math.Ceiling (this.ActualHeight*2.0 - this.BorderThickness.Top*2.0 - this.BorderThickness.Bottom*2.0));
				
			var borderRects = GetRects (GetPoints (rcBorder, this.RadiusX, this.RadiusY));
			var bgRects = GetRects (GetPoints (rcBackground, this.RadiusX, this.RadiusY));

			var tex = new Texture2D (ScreenHelper.Device, width, height);
			var pixels = new int[width * height];

			for (var i = 0; i < pixels.Length; i++)
				pixels [i] = (int)Color.Transparent.PackedValue;


			if ( this.BorderBrush != null && !this.BorderThickness.IsEmpty )
				FillPixels (
					BorderBrush, borderRects, width, height, ref pixels);

			if ( this.Background != null )
				FillPixels (
					Background, bgRects, width, height, ref pixels);

			tex.SetData (pixels);
			return tex;
		}
			
		private Texture2D texture;
	}
}
