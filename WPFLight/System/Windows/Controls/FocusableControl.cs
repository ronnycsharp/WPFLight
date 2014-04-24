using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace System.Windows.Controls
{
	// TODO Replace Panel
	public abstract class FocusableControl : Panel, IFocusable
    {
        public FocusableControl (  ) : base ( )
        {

        }

        #region Ereignisse



        #endregion

        #region Eigenschaften

        #endregion

        public bool Focused
        {
            get
            {
                if ( this.Focusable )
                    return (this.FocusableParent??this.Parent).IsFocused ( this );
                
                return false;
            }
        }

        public void Focus ( )
        {
            if ( this.Focusable )
                (this.FocusableParent??this.Parent).Focus ( this );
        }

        public virtual void OnLostFocus ( )
        {
            if ( this.LostFocus != null )
                this.LostFocus ( this, EventArgs.Empty );
		
			if (this.FocusChanged != null)
				this.FocusChanged (this, EventArgs.Empty);
        }

        public virtual void OnGotFocus ( )
        {
            if ( this.GotFocus != null )
                this.GotFocus ( this, EventArgs.Empty );

			if (this.FocusChanged != null)
				this.FocusChanged (this, EventArgs.Empty);
        }
    }
}
