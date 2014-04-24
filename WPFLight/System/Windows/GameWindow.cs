using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace System.Windows {
    public abstract class GameWindow : Microsoft.Xna.Framework.Game {
        public GameWindow () {
            drawingContext = new DrawingContext();
        }

        #region Eigenschaften

        public List<ScreenBase> Screens { get; private set; }

        #endregion

        protected override void Update (Microsoft.Xna.Framework.GameTime gameTime) {
            Clock.SetCurrentGlobalTime(gameTime.TotalGameTime);
            base.Update(gameTime);
        }

        protected override void Draw (Microsoft.Xna.Framework.GameTime gameTime) {
            base.Draw(gameTime);
            this.OnRender(drawingContext);
        }

        protected virtual void OnRender (DrawingContext dc) {
            // Screens rendern
            foreach (var screen in this.Screens) {
                screen.OnRender(dc);
            }
        }

        private DrawingContext drawingContext;
    }
}