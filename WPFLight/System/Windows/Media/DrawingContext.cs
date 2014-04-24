using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Helpers;
namespace System.Windows.Media {
    public class DrawingContext {
        public DrawingContext () {
			//batch = new SpriteBatch(ScreenHelper.Device);
        }

        public void DrawText (SpriteFont spriteFont, string text, Vector2 position, Color color) {
            throw new NotImplementedException();
        }

        public void DrawText (SpriteFont spriteFont, string text, Point position, Color color) {
            throw new NotImplementedException();
        }

        public void DrawImage () {
            throw new NotImplementedException();
        }

		//private SpriteBatch batch;
    }
}