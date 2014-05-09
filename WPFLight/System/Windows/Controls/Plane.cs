using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace System.Windows.Controls
{
	public sealed class Plane : UIElement, IDrawable3D
    {
        public Plane ( Game game, Vector3 x1, Vector3 x2, Vector3 y1, Vector3 y2, Color color ) : base ( )
        {
            this.Color = color;
            this.Opacity = 1.0f;
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = y1;
            this.Y2 = y2;
        }

        #region Eigenschaften

        public Vector3 X1 { get; private set; }
        public Vector3 X2 { get; private set; }
        public Vector3 Y1 { get; private set; }
        public Vector3 Y2 { get; private set; }

        public Color Color { get; private set; }

        #endregion

        public override void Initialize ( )
        {
            vertexBuffer =
                new VertexBuffer (
                    this.Game.GraphicsDevice,
                    typeof ( VertexPositionColor ),
                    6,
                    BufferUsage.None );

            vertexBuffer.SetData ( 
                new VertexPositionColor[]{
                    new VertexPositionColor ( X1, this.Color ),
                    new VertexPositionColor ( X2, this.Color ),
                    new VertexPositionColor ( Y1, this.Color ),
                    new VertexPositionColor ( Y1, this.Color ),
                    new VertexPositionColor ( X2, this.Color ),
                    new VertexPositionColor ( Y2, this.Color ),
                } );

            basicEffect = new BasicEffect ( this.Game.GraphicsDevice );
            basicEffect.EnableDefaultLighting ( );

            base.Initialize ( );
        }

        protected override void OnDeviceReset ( ) {
            base.OnDeviceReset ( );
            if ( this.IsInitialized ) {
                if ( vertexBuffer != null )
                    vertexBuffer.Dispose ( );

                if ( basicEffect != null )
                    basicEffect.Dispose ( );

                Initialize ( );
            }
        }

        public void Draw ( GameTime gameTime, Matrix world, Matrix view, Matrix projection, float alpha )
        {
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            basicEffect.PreferPerPixelLighting = false;
            basicEffect.Alpha = this.Opacity * ( this.Color.A / 256f ) * alpha;

            this.Draw ( basicEffect );
        }

        void Draw ( Effect effect ) {
            this.Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            this.Game.GraphicsDevice.SetVertexBuffer ( vertexBuffer );
            foreach ( EffectPass effectPass in effect.CurrentTechnique.Passes ) {
                effectPass.Apply ( );
                this.Game.GraphicsDevice.DrawPrimitives (
                    PrimitiveType.TriangleList, 0, 2 );
            }
        }

        public override void Dispose ( )
        {
            basicEffect.Dispose ( );
            vertexBuffer.Dispose ( );

            base.Dispose ( );
        }

        private BasicEffect     basicEffect;
        private VertexBuffer    vertexBuffer;
    }
}
