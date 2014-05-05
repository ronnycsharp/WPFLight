using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace System.Windows.Controls {
	[ContentProperty("Children")]
	public abstract class Panel : Control {
		public Panel ( ) {
            this.Children = new UIElementCollection(this);
		}

		#region Eigenschaften

		public UIElementCollection Children { get; protected set; }

		#endregion

		public override void Initialize () {
			if (this.Children.Count > 0) {
				foreach (var c in this.Children) {
					if (c != null)
						c.Initialize();
				}
			}
			base.Initialize ();
		}

		public override void Update (GameTime gameTime) {
			if (this.IsEnabled) {
				foreach (var c in this.Children.OfType<IUpdateable>())
					c.Update(gameTime);
			}
			base.Update (gameTime);
		}

		public override void Draw (GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch batch, float alpha, Matrix transform) {
			base.Draw (gameTime, batch, alpha, transform);
			if (this.IsVisible () && this.Alpha > 0) {
				var drawable = this.Children.OfType<IDrawable> ()
									.OrderBy (d => d.DrawOrder)
									.ToArray ();

				if (drawable.Length > 0) {
					foreach (var d in drawable.OfType<IDrawable2D>())
						d.Draw (gameTime, batch, alpha * this.Alpha, transform);
				}
			}
		}

		public override void Invalidate () {
			base.Invalidate ();
			if (this.IsInitialized) {
				foreach (var child in this.Children.OfType<Control>())
					child.Invalidate ();
			}
		}

		public override void OnTouchDown (TouchLocation state) {
			foreach (var c in this.Children.OfType<Control>()) {
				if (c.IsEnabled && c.Alpha > 0 && c.Visible && c.HitTest(state.Position)) {
					c.OnTouchDown(state);
				}
			}
			base.OnTouchDown (state);
		}

		public override void OnTouchUp (TouchLocation state) {
			foreach (var c in this.Children.OfType<Control>())
				if (c.IsEnabled && c.Alpha > 0 && c.Visible && (c.HitTest(state.Position) || c.IsTouchDown))
					c.OnTouchUp(state);

			base.OnTouchUp (state);
		}

		public override void OnTouchMove (TouchLocation state) {
			foreach (var c in this.Children.OfType<Control>())
				if (c.IsEnabled && c.Alpha > 0 && c.Visible && (c.HitTest(state.Position) || c.IsTouchDown))
					c.OnTouchMove(state);

			base.OnTouchMove (state);
		}

		public void ResetTouchStates () {
			var children = GetAllControls(this);
			foreach (var c in children)
				c.IsTouchDown = false;
		}

		static IEnumerable<Control> GetAllControls (Control control) {
			var children = new List<Control>();
			children.Add(control);
			if (control is Panel) {
				foreach (var c in ((Panel)control).Children.OfType<Control>())
					children.AddRange (GetAllControls (c));
			}
			return children;
		}
	}
}
