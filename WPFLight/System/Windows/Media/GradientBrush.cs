using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows.Markup;

namespace System.Windows.Media {
    [ContentProperty("GradientStops")]
	public abstract class GradientBrush : Brush {
		public GradientBrush () : base ( ) {
			this.GradientStops = new GradientStopCollection ();
			this.StartPoint = new Point (0, 0);
			this.EndPoint = new Point (1, 1);
		}
			

		#region Eigenschaften

		public Point StartPoint { get; set; }

		public Point EndPoint { get; set; }

		public GradientStopCollection GradientStops { get; internal set; }

		internal Texture2D Texture {
			get {
				if (texture == null)
					texture = CreateTexture ();

				return texture;
			}
		}

		#endregion

		public override void Draw (SpriteBatch batch, Rectangle bounds, Matrix transform, float alpha) {
			if (blendState == null)
				blendState = ContainsAlpha ()
                        ? BlendState.AlphaBlend : BlendState.Opaque;

			var state = blendState;
			if (state == BlendState.Opaque && alpha < 1)
				state = BlendState.AlphaBlend;

			batch.Begin (
				SpriteSortMode.Deferred,
				state,
				SamplerState.LinearClamp,
				DepthStencilState.None,
				RasterizerState.CullNone,
				null,
				transform);
				
			batch.Draw (
				this.Texture,
				bounds,
				null,
				Microsoft.Xna.Framework.Color.White * alpha,
				0,
				new Vector2 (),
				SpriteEffects.None,
				0);

			batch.End ();
		}

		/// <summary>
		/// Gibt true zurück, wenn mindestens ein Wert eine Transparenz enthält
		/// </summary>
		/// <returns></returns>
		protected bool ContainsAlpha () {
			foreach (var s in GradientStops) {
				if (s.Color.A < 255)
					return true;
			}
			return false;
		}

		protected abstract Texture2D CreateTexture ();

		Color GetPrevColor (float offset) {
			GradientStop frame = null;
			foreach (var f in this.GradientStops) {
				if (f.Offset <= offset && (frame == null || (frame != null && f.Offset > frame.Offset)))
					frame = f;
			}
			if (frame != null)
				return frame.Color;

			return Colors.Transparent;
		}

		Color GetNextColor (float offset) {
			GradientStop frame = null;
			foreach (var f in this.GradientStops) {
				if (f.Offset > offset && (frame == null || (frame != null && f.Offset < frame.Offset)))
					frame = f;
			}
			if (frame != null)
				return frame.Color;

			return Colors.Transparent;
		}

		float GetPrevOffset (float offset) {
			GradientStop frame = null;
			foreach (var f in this.GradientStops) {
				if (f.Offset <= offset && (frame == null || (frame != null && f.Offset > frame.Offset)))
					frame = f;
			}
			if (frame != null)
				return frame.Offset;

			return float.NaN;
		}

		float GetNextOffset (float offset) {
			GradientStop frame = null;
			foreach (var f in this.GradientStops) {
				if (f.Offset > offset && (frame == null || (frame != null && f.Offset < frame.Offset)))
					frame = f;
			}
			if (frame != null)
				return frame.Offset;

			return float.NaN;
		}

		protected Color GetGradientColor (float offset) {
            offset = MathHelper.Clamp(offset, 0, 1);
            /*
			if (offset > 1)
				offset = 1;
             * */

			var prevTimeMillis = GetPrevOffset (offset);
			var nextTimeMillis = GetNextOffset (offset);

			var prevValue = GetPrevColor (offset);
			var nextValue = GetNextColor (offset);

			if (offset - prevTimeMillis <= 0.0)
				return prevValue;
			else if ((offset - prevTimeMillis) >= nextTimeMillis)
				return nextValue;
			else {
				float v = (float)(1.0 / ((double)(nextTimeMillis - prevTimeMillis)
				                      / (double)(offset - prevTimeMillis)));

				return Color.Lerp (
					prevValue, nextValue, v);
			}
		}

		private BlendState blendState;
		private Texture2D texture;
	}
}
