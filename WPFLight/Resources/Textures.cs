using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WPFLight.Resources {
	public static class Textures {
		public static void LoadContent (ContentManager manager) {
			Background = manager.Load<Texture2D> ("bg_button");
			ArrowUp = manager.Load<Texture2D> ("ArrowUp");
			ArrowDown = manager.Load<Texture2D> ("ArrowDown");
			ArrowLeft = manager.Load<Texture2D> ("ArrowLeft");
			ArrowRight = manager.Load<Texture2D> ("ArrowRight");
		}

		public static void Dispose () {
			Background.Dispose ();
			ArrowUp.Dispose ();
			ArrowDown.Dispose ();
			ArrowLeft.Dispose ();
			ArrowRight.Dispose ();
		}

		#region Properties 

		public static Texture2D Background 	{ get; private set; }

		public static Texture2D ArrowUp		{ get; private set; }

		public static Texture2D ArrowDown	{ get; private set; }

		public static Texture2D ArrowLeft	{ get; private set; }

		public static Texture2D ArrowRight	{ get; private set; }

		#endregion
	}
}

