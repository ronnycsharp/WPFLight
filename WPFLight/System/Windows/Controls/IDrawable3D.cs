using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace System.Windows.Controls
{
    public interface IDrawable3D : IDrawable
    {
        void Draw ( GameTime time, Matrix world, Matrix view, Matrix projection, float alpha );
    }
}
