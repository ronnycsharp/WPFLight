using System;

namespace System.Windows.Controls
{
    public interface IDrawable
    {
        float Alpha { get; }
        bool Visible { get; }
        int DrawOrder { get; }

        event EventHandler<EventArgs> DrawOrderChanged;
        event EventHandler<EventArgs> VisibleChanged;
    }
}
