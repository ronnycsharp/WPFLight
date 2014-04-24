using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Resources;

namespace System.Windows.Controls {
    public class Image : Control {
        public Image ( Texture2D texture ) : base ( ) {
            if ( texture == null )
                throw new ArgumentNullException ( "texture" );

            texImage = texture;
			this.BorderThickness = new Thickness ();
        }

		#region Eigenschaften

		public float Rotation { get; set; }

		#endregion

        public override void Draw ( GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform ) {
            base.Draw ( gameTime, batch, alpha, transform );

            batch.Begin (
				SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null,
                DepthStencilState.None,
				rasterizerState,
                null,
                transform );
				
			var bounds = this.Bounds;

			batch.Draw (
				texImage, 
				new Rectangle ( 
					(int)this.GetAbsoluteLeft ( ), 
					(int)this.GetAbsoluteTop ( ), 
					(int)this.ActualWidth, 
					(int)this.ActualHeight ),
				(Rectangle?)null,
				Color.White * alpha * this.Alpha,
				this.Rotation,
				Vector2.Zero,
				SpriteEffects.None, 
				0);

            batch.End ( );
        }

        private Texture2D texImage;

		static RasterizerState rasterizerState = 
			new RasterizerState { CullMode = CullMode.None, FillMode = FillMode.Solid, MultiSampleAntiAlias = true };
    }
}
