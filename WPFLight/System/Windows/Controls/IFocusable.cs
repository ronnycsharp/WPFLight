using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace System.Windows
{
    public interface IFocusable
    {
        void Focus ( );
        bool Focused { get; }
        bool Focusable { get; }

        event EventHandler<EventArgs> LostFocus;
        event EventHandler<EventArgs> GotFocus;

        void OnGotFocus ( );
        void OnLostFocus ( );

    }
}
