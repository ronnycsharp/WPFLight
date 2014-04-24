using System;

namespace System.Windows.Media.Animation
{
	public struct SingleKeyFrame {
		public SingleKeyFrame ( TimeSpan time, float value ) {
			Time = time;
			Value = value;
		}
		
		public float Value;
		public TimeSpan Time;
	}
}