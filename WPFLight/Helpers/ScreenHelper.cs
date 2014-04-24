using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;

namespace WPFLight.Helpers {
	public static class ScreenHelper {
		#if WINDOWS_PHONE || ANDROID
        public static int ORIGINAL_WIDTH = 800;
        public static int ORIGINAL_HEIGHT = 480;
	 	#endif
		#if WIN8
        public static int ORIGINAL_WIDTH = 1366;
        public static int ORIGINAL_HEIGHT = 768;
		#endif
		#if IOS
		public static int ORIGINAL_WIDTH = 1024;
		public static int ORIGINAL_HEIGHT = 768;
		#endif
		public static int SCREEN_WIDTH = GraphicsDeviceManager.DefaultBackBufferWidth;
		public static int SCREEN_HEIGHT = GraphicsDeviceManager.DefaultBackBufferHeight;
		static float SCALE_TRANSFORM_X;
		static float SCALE_TRANSFORM_Y;
		static Matrix SCALE_TRANSFORM;

		public static void SetScreenSize (int width, int height) {
			ORIGINAL_WIDTH = width;
			ORIGINAL_HEIGHT = height;
		}

		public static void SetScreenResolution (int width, int height) {
			SCREEN_WIDTH = width;
			SCREEN_HEIGHT = height;
			SCALE_TRANSFORM_X = ((float)SCREEN_WIDTH) / ORIGINAL_WIDTH;
			SCALE_TRANSFORM_Y = ((float)SCREEN_HEIGHT) / ORIGINAL_HEIGHT;
			SCALE_TRANSFORM = Matrix.CreateScale (
				((float)SCREEN_WIDTH / (float)ORIGINAL_WIDTH),
				((float)SCREEN_HEIGHT / (float)ORIGINAL_HEIGHT),
				1);
		}

		public static TouchLocation Unproject (TouchLocation touchLocation) {
			return new TouchLocation (
				touchLocation.Id,
				touchLocation.State,
				Unproject (touchLocation.Position));
		}

		public static Vector2 Unproject (Vector2 position) {
			var result = position;
			if (ORIGINAL_WIDTH != SCREEN_WIDTH) {
				if (SCREEN_WIDTH > ORIGINAL_WIDTH)
					result.X = position.X / ((float)SCREEN_WIDTH / ORIGINAL_WIDTH);
				else
					result.X = position.X * ((float)ORIGINAL_WIDTH / SCREEN_WIDTH);
			}
			if (ORIGINAL_HEIGHT != SCREEN_HEIGHT) {
				if (SCREEN_HEIGHT > ORIGINAL_HEIGHT)
					result.Y = position.Y / ((float)SCREEN_HEIGHT / ORIGINAL_HEIGHT);
				else
					result.Y = position.Y * ((float)ORIGINAL_HEIGHT / SCREEN_HEIGHT);
			}
			return result;
		}

		public static Vector2 Project (Vector2 position) {
			var result = position;
			if (ORIGINAL_WIDTH != SCREEN_WIDTH) {
				if (SCREEN_WIDTH > ORIGINAL_WIDTH)
					result.X = position.X * ((float)SCREEN_WIDTH / ORIGINAL_WIDTH);
				else
					result.X = position.X / ((float)ORIGINAL_WIDTH / SCREEN_WIDTH);
			}
			if (ORIGINAL_HEIGHT != SCREEN_HEIGHT) {
				if (SCREEN_HEIGHT > ORIGINAL_HEIGHT)
					result.Y = position.Y * ((float)SCREEN_HEIGHT / ORIGINAL_HEIGHT);
				else
					result.Y = position.Y / ((float)ORIGINAL_HEIGHT / SCREEN_HEIGHT);
			}
			return result;
		}

		public static Microsoft.Xna.Framework.Rectangle Project (Rectangle rc) {
			var position = Project (new Vector2 (rc.Left, rc.Top));
			var size = Project (new Vector2 (rc.Width, rc.Height));
			return new Rectangle (
				(int)position.X,
				(int)position.Y,
				(int)size.X,
				(int)size.Y);
		}


		public static Rectangle CheckScissorRect ( Rect bounds ) {
			return CheckScissorRect (
				new Rectangle ((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height));
		}

		/// <summary>
		/// Hilfsmethode, zum transformieren eines Scissor-Bereichs
		/// - Methode wird für Platformunabhängkeit benötigt
		/// </summary>
		/// <returns>The scissor.</returns>
		/// <param name="bounds">Bounds.</param>
		public static Microsoft.Xna.Framework.Rectangle CheckScissorRect (Rectangle bounds) {
			if (bounds.X < 0)
				bounds.X = 0;

			if (bounds.Y < 0)
				bounds.Y = 0;

			if (bounds.Width < 0)
				bounds.Width = 0;

			if (bounds.Height < 0)
				bounds.Height = 0;




			// LANDSCAPE-MODE
#if WINDOWS_PHONE

            if (bounds.Right > ORIGINAL_WIDTH)
                bounds.Width = ORIGINAL_WIDTH - bounds.X;

            if (bounds.Bottom > ORIGINAL_HEIGHT)
                bounds.Height = ORIGINAL_HEIGHT - bounds.Y;
#else
            // TEST

            if (bounds.Right > ORIGINAL_HEIGHT)
                bounds.Width = ORIGINAL_HEIGHT - bounds.X;

			if (bounds.Bottom > ORIGINAL_WIDTH)
				bounds.Height = ORIGINAL_WIDTH - bounds.Y;
#endif


			// PORTRAIT MODE
            /*
			if (bounds.Right > ORIGINAL_WIDTH)
				bounds.Width = ORIGINAL_WIDTH - bounds.X;

			if (bounds.Bottom > ORIGINAL_HEIGHT)
				bounds.Height = ORIGINAL_HEIGHT - bounds.Y;
             * */

			var scaleX = (float)ScreenHelper.SCREEN_WIDTH / (float)ScreenHelper.ORIGINAL_WIDTH;
			var scaleY = (float)ScreenHelper.SCREEN_HEIGHT / (float)ScreenHelper.ORIGINAL_HEIGHT;

			return
				new Microsoft.Xna.Framework.Rectangle (
				(int)(bounds.Left * scaleX), 
				(int)(bounds.Top * scaleY), 
				(int)(bounds.Width * scaleX), 
				(int)(bounds.Height * scaleY));
		}

		public static Matrix GetScreenTransform () {
			return SCALE_TRANSFORM;
		}

		public static GraphicsDevice Device { get; set; }
        public static Game Game { get; set; }

	}
}

