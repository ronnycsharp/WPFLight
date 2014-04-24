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
using System.Collections.ObjectModel;

namespace System.Windows
{
    public sealed class ListBoxCollection : Collection<string>
    {
        protected override void InsertItem ( int index, string item )
        {
            base.InsertItem ( index, item );
            OnCollectionChanged ( );
        }

        protected override void ClearItems ( )
        {
            var changed = false;
            if ( this.Count > 0 )
                changed = true;

            base.ClearItems ( );
            if ( changed )
                OnCollectionChanged ( );
        }

        protected override void RemoveItem ( int index )
        {
            base.RemoveItem ( index );
            OnCollectionChanged ( );
        }

        protected override void SetItem ( int index, string item )
        {
            base.SetItem ( index, item );
            OnCollectionChanged ( );
        }

        public event EventHandler<EventArgs> CollectionChanged;

        void OnCollectionChanged ( )
        {
            if ( this.CollectionChanged != null )
                this.CollectionChanged ( this, EventArgs.Empty );
        }
    }
}
