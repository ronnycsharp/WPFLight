using System;

namespace System.ComponentModel
{
	public class PropertyChangedEventArgs<T> : EventArgs
	{
		public PropertyChangedEventArgs (T newValue, T oldValue) : base ( )
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public T OldValue { get; private set;}
		public T NewValue { get; private set;}
	}
}

