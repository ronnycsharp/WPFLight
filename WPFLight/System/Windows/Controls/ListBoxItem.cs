using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace System.Windows.Controls
{
    public class ListBoxItem : RadioButton
    {
        public ListBoxItem ( SpriteFont font ) : base ( font ) {
			this.ScissorTest = false;
        }

        public override void Initialize () {
            base.Initialize();
            this.GroupName = "LB" + this.Parent.GetHashCode().ToString();
        }
    }
}
