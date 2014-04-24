using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace System.Windows.Controls
{
    public interface IDrawable2D : IDrawable
    {
        void Draw ( GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform );
    }
}
