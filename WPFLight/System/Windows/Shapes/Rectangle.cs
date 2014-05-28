using System.Collections.Generic;
using System.Windows.Media;
using WPFLight.Helpers;
using WPFLight.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace System.Windows.Shapes {
	public class Rectangle : Shape {
        static Rectangle () {
            dicTextures = new Dictionary<Rectangle, Texture2D>();
        }

        public Rectangle () { }

		#region Eigenschaften

		public static readonly DependencyProperty RadiusXProperty =
			DependencyProperty.Register (
				"RadiusX",
				typeof(float),
				typeof(Rectangle));
				
		public float RadiusX {
			get { return (float)GetValue (RadiusXProperty); }
			set { SetValue (RadiusXProperty, value); }
		}

		public static readonly DependencyProperty RadiusYProperty =
			DependencyProperty.Register (
				"RadiusY",
				typeof(float),
				typeof(Rectangle));

		public float RadiusY {
			get { return (float)GetValue (RadiusYProperty); }
			set { SetValue (RadiusYProperty, value); }
		}

		#endregion

		static Point [] GetPoints ( Rect rect, float radiusX, float radiusY ) {
			radiusX = Math.Min (rect.Width * (1.0f / 2.0f), Math.Abs (radiusX));
			radiusY = Math.Min (rect.Height * (1.0f / 2.0f), Math.Abs (radiusY));

			var points = new Point[17];

			double c_arcAsBezier = 0.5522847498307933984;

			float bezierX = (float)((1.0 - c_arcAsBezier) * radiusX);
			float bezierY = (float)((1.0 - c_arcAsBezier) * radiusY);

			points [1].X = points [0].X = points [15].X = points [14].X = rect.X;
			points [2].X = points [13].X = rect.X + bezierX;
			points [3].X = points [12].X = rect.X + radiusX;
			points [4].X = points [11].X = rect.Right - radiusX;
			points [5].X = points [10].X = rect.Right - bezierX;
			points [6].X = points [7].X = points [8].X = points [9].X = rect.Right;

			points [2].Y = points [3].Y = points [4].Y = points [5].Y = rect.Y;
			points [1].Y = points [6].Y = rect.Y + bezierY;
			points [0].Y = points [7].Y = rect.Y + radiusY;
			points [15].Y = points [8].Y = rect.Bottom - radiusY;
			points [14].Y = points [9].Y = rect.Bottom - bezierY;
			points [13].Y = points [12].Y = points [11].Y = points [10].Y = rect.Bottom;

			points [16] = points [0];
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

				i = 12;
				var p2 = GetCubicBezier (
					1 - t, points [0 + i], points [1 + i], points [2 + i], points [3 + i]);

				i = 4;
				var p3 = GetCubicBezier (
					t, points [0 + i], points [1 + i], points [2 + i], points [3 + i]);

				i = 8;
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
			if (width < 1 || height < 1)
				throw new ArgumentNullException ();

            var brushPixels = brush.GetTextureData(width, height);
            var getBrushPixel = new Func<int, int, Color>(
                (x, y) => {
                    return new Color { PackedValue = (uint)brushPixels[Math.Min ( y, height - 1 ) * width + x] };
                });

			foreach (var rc in rects) {
				for (var x = (int)rc.Left; x < rc.Right && rc.Right <= width; x++) {

					var pixelTopColor = new Color { PackedValue = (uint)pixels [((int)rc.Top) * width + x] };
					var pixelBottomColor = new Color { PackedValue = (uint)pixels [((int)rc.Bottom-1) * width + x] };

                    var topColor = getBrushPixel(x, (int)rc.Top);//brush.GetPixel (x, (int)rc.Top, width, height);
                    var bottomColor = getBrushPixel(x, (int)rc.Bottom); //brush.GetPixel (x, (int)rc.Bottom, width, height);

					// Antialiazing, Top

					/*
					if (rc.Top > 0 ) {
						pixels [((int)rc.Top - 1) * width + x] = (int)Color.Lerp (pixelTopColor, topColor, .65f).PackedValue;
					}
					*/

					if (rc.Top > 1) {
						pixels [((int)rc.Top - 2) * width + x] = (int)Color.Lerp (pixelTopColor, topColor, .38f).PackedValue;
					}

					for (var y = (int)rc.Top; y < rc.Bottom; y++) {
						pixels [y * width + x] = ( int ) ( getBrushPixel ( x, y ) ).PackedValue;
					}

					// Antialiazing, Bottom

					if (rc.Bottom < height)
						pixels [((int)rc.Bottom) * width + x] = (int)Color.Lerp (pixelBottomColor, bottomColor, .65f).PackedValue;
						
					/*
					if (rc.Bottom < height - 1)
						pixels [(int)(rc.Bottom + 1) * width + x] = (int)Color.Lerp (pixelBottomColor, bottomColor, .38f).PackedValue;
						*/
				}
			}
		}

        public override bool Equals (object obj) {
            var rc = obj as Rectangle;
            if (rc != null) {
                return this.StrokeThickness.Equals ( rc.StrokeThickness )
                        && this.RadiusX == rc.RadiusX
                        && this.RadiusY == rc.RadiusY
                        && ((rc.Stroke == null && this.Stroke == null)
                            || (this.Stroke != null && this.Stroke.Equals(rc.Stroke)))
                        && ((rc.Fill == null && this.Fill == null)
                            || (this.Fill != null && this.Fill.Equals(rc.Fill)));
            }
            return false;
        }

        public override int GetHashCode () {
            var hash = ("RC_"
                + this.StrokeThickness.GetHashCode().ToString() + ";"
                + this.RadiusX.ToString("0.0") + ";"
                + this.RadiusY.ToString("0.0") + ";"
                + (this.Fill != null ? this.Fill.GetHashCode().ToString() : String.Empty) + ";"
                + (this.Stroke != null ? this.Stroke.GetHashCode().ToString() : String.Empty) + ";" );

            return hash.GetHashCode();
        }

		protected override Microsoft.Xna.Framework.Graphics.Texture2D CreateTexture ( ) {
            // check whether the rectangle draws itself and not a created texture
            var renderShape = (this.RadiusX == 0 && this.RadiusY == 0
                    && (this.Fill == null || this.Fill is SolidColorBrush));

            if (renderShape)
                return null;

            if (dicTextures.ContainsKey(this))
                return dicTextures[this];

			// Image-Scaling 2x - looks better

			var width = (int)Math.Ceiling (this.ActualWidth*2.0);
            var height = (int)Math.Ceiling (this.ActualHeight*2.0);

			var rcBorder = new Rect (
				0, 0, width, height);

			var rcBackground = new Rect (
				(int)Math.Ceiling (this.StrokeThickness*2.0),
				(int)Math.Ceiling (this.StrokeThickness*2.0),
				(int)(Math.Ceiling ((double)width) - this.StrokeThickness * 2.0 - this.StrokeThickness * 2.0),
				(int)(Math.Ceiling ((double)height) - this.StrokeThickness * 2.0 - this.StrokeThickness * 2.0));
				
			var borderRects = GetRects (GetPoints (rcBorder, this.RadiusX, this.RadiusY));
			var bgRects = GetRects (GetPoints (rcBackground, this.RadiusX, this.RadiusY));

			var tex = new Microsoft.Xna.Framework.Graphics.Texture2D (ScreenHelper.Device, width, height);
			var pixels = new int[width * height];

            pixels.Fill((int)Colors.Transparent.PackedValue);

			if ( this.Stroke != null && this.StrokeThickness > 0 )
				FillPixels (
					this.Stroke, borderRects, width, height, ref pixels);

			if ( this.Fill != null )
				FillPixels (
					this.Fill, bgRects, width, height, ref pixels);

			tex.SetData (pixels);
			dicTextures [this] = tex;
			return tex;
		}

        public override void Draw (Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch batch, float alpha, Microsoft.Xna.Framework.Matrix transform) {
            if (this.IsVisible && alpha > 0 && this.Opacity > 0 ) {
                // check whether the rectangle draws itself and not a created texture
                var renderShape = (this.RadiusX == 0 && this.RadiusY == 0
                        && (this.Fill == null || this.Fill is SolidColorBrush)
                        && (this.Stroke == null || this.Stroke is SolidColorBrush));

                if (renderShape) {
                    var backgroundColor = this.Fill != null
                        ? ColorHelper.ToXnaColor(((SolidColorBrush)this.Fill).Color)
                        : Microsoft.Xna.Framework.Color.Transparent;

                    var strokeColor = this.Stroke != null
                        ? ColorHelper.ToXnaColor(((SolidColorBrush)this.Stroke).Color)
                        : Microsoft.Xna.Framework.Color.Transparent;

                    batch.Begin(
                        SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        DepthStencilState.None,
						SCISSOR_ENABLED,
                        null,
                        transform);

                    var left = this.GetAbsoluteLeft();
                    var top = this.GetAbsoluteTop();

                    // Hintergrund
                    batch.Draw(
                        WPFLight.Resources.Textures.Background,
                        new Microsoft.Xna.Framework.Rectangle(
                        (int)(left + this.StrokeThickness),
                        (int)(top + this.StrokeThickness),
                        (int)(this.ActualWidth - this.StrokeThickness - this.StrokeThickness),
                        (int)(this.ActualHeight - this.StrokeThickness - this.StrokeThickness)),
                        backgroundColor
                            
                            * this.Opacity * alpha);

                    if (StrokeThickness > 0) {
                        // Rahmen TOP
                        batch.Draw(
                            WPFLight.Resources.Textures.Background,
                            new Microsoft.Xna.Framework.Rectangle(
                                (int)(left + StrokeThickness),
                                (int)top,
                                (int)(this.ActualWidth - StrokeThickness - StrokeThickness),
                                (int)StrokeThickness),
                            strokeColor
                                //* ((float)strokeColor.A / 256f)
                                * alpha);

                        // Rahmen BOTTOM
                        batch.Draw(
                            WPFLight.Resources.Textures.Background,
                            new Microsoft.Xna.Framework.Rectangle(
                                (int)(left + StrokeThickness),
                                (int)(top + this.ActualHeight - StrokeThickness),
                                (int)(this.ActualWidth - StrokeThickness - StrokeThickness),
                                (int)this.StrokeThickness),
                            strokeColor
                                //* ((float)strokeColor.A / 256f)
                                * alpha);

                        // Rahmen LEFT
                        batch.Draw(
                            WPFLight.Resources.Textures.Background,
                            new Microsoft.Xna.Framework.Rectangle(
                                (int)(left),
                                (int)(top),
                                (int)(StrokeThickness),
                                (int)(this.ActualHeight)),
                            strokeColor
                                //* ((float)strokeColor.A / 256f)
                                * alpha);

                        // Rahmen RIGHT
                        batch.Draw(
                            WPFLight.Resources.Textures.Background,
                            new Microsoft.Xna.Framework.Rectangle(
                                (int)(left + this.ActualWidth - StrokeThickness),
                                (int)(top),
                                (int)(StrokeThickness),
                                (int)(this.ActualHeight)),
                            strokeColor
                                //* ((float)strokeColor.A / 256f)
                                * alpha);
                    }
                    batch.End();
                } else {
                    base.Draw(gameTime, batch, alpha, transform);
                }
            }
        }

        public override void Invalidate () {
            base.Invalidate();
        }

		static readonly RasterizerState SCISSOR_ENABLED = 
			new RasterizerState { 
			CullMode = CullMode.None, 
			ScissorTestEnable = true 
		};

        private static Dictionary<Rectangle, Texture2D> dicTextures;
	}
}
