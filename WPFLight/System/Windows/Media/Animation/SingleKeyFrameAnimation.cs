using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace System.Windows.Media.Animation
{
	public class SingleKeyFrameAnimation : SingleAnimationBase {
		public SingleKeyFrameAnimation ( ) {
			this.KeyFrames = new List<SingleKeyFrame> ( );
		}

	    #region Eigenschaften

	    public List<SingleKeyFrame> KeyFrames { get; private set; }

	    #endregion

	    public void Add ( SingleKeyFrame item ) {
			this.KeyFrames.Add ( item );
	        _lastDuration = TimeSpan.FromSeconds ( -1 );
		}
		
		public void Add ( TimeSpan time, float value ) {
			this.KeyFrames.Add ( new SingleKeyFrame ( time, value ) );
			_lastDuration = TimeSpan.FromSeconds(-1);
		}
		
		public void Clear ( ) {
			this.KeyFrames.Clear ( );
	        _lastDuration = TimeSpan.FromSeconds ( -1 );
		}
		
		public void Remove ( SingleKeyFrame item ) {
			this.KeyFrames.Remove ( item );
	        _lastDuration = TimeSpan.FromSeconds ( -1 );
		}
		
		float GetPrevValue ( ) {
			SingleKeyFrame? frame = null;
			foreach ( SingleKeyFrame f in this.KeyFrames ) {
				if ( f.Time <= CurrentTime && ( frame==null || ( frame != null && f.Time > frame.Value.Time ) ) ) {
					frame = f;
				}
			}
			if ( frame != null )
				return frame.Value.Value;
			
			return float.NaN;
		}
		
		float GetNextValue ( ) {
			SingleKeyFrame? frame = null;
			foreach ( SingleKeyFrame f in this.KeyFrames ) {
				if ( f.Time > CurrentTime &&  ( frame==null || ( frame != null && f.Time < frame.Value.Time ) ) ) {
					frame = f;
				}
			}
			if ( frame != null )
				return frame.Value.Value;

			return float.NaN;
		}
		
		TimeSpan GetPrevTime ( ) {
			SingleKeyFrame? frame = null;
			foreach ( SingleKeyFrame f in this.KeyFrames ) {
				if ( f.Time <= CurrentTime &&  ( frame==null || ( frame != null && f.Time > frame.Value.Time ) ) ) {
					frame = f;
				}
			}
			if ( frame != null )
				return frame.Value.Time;
			
			return TimeSpan.FromSeconds(-1);
		}
		
		TimeSpan GetNextTime ( ) {
			SingleKeyFrame? frame = null;
			foreach ( SingleKeyFrame f in this.KeyFrames ) {
				if ( f.Time > CurrentTime && ( frame==null || ( frame != null && f.Time < frame.Value.Time ) ) ) {
					frame = f;
				}
			}
			if ( frame != null )
				return frame.Value.Time;
			
			return TimeSpan.FromSeconds(-1);
		}
		
		public override float GetCurrentValue ( ) {	
			var prevTimeMillis = TimeSpan.Zero;
			var nextTimeMillis = TimeSpan.Zero;

			if ((lastPrevTime==TimeSpan.FromMinutes(-1)) 
			   	 	|| this.CurrentTime >= lastNextTime )
				lastPrevTime = GetPrevTime ();

			if ((lastNextTime==TimeSpan.FromMinutes(-1)) 
			    	|| this.CurrentTime >= lastNextTime )
				lastNextTime = GetNextTime ();

			prevTimeMillis = lastPrevTime;		
			nextTimeMillis = lastNextTime;

			if (CurrentTime - prevTimeMillis <= TimeSpan.Zero)
				return GetPrevValue ();
			else if ((CurrentTime - prevTimeMillis) >= nextTimeMillis)
				return GetNextValue ();
			else {
				float v = ( float ) ( 1.0 / ( ( double ) ( nextTimeMillis - prevTimeMillis ).Ticks / ( double ) ( CurrentTime - prevTimeMillis ).Ticks ) );
				if (lastV != v) {
					lastV = v;
					lastComputedValue = ComputeValue ( 
						GetPrevValue(), GetNextValue(), v );
				}
				return lastComputedValue;
			}
		}

		private TimeSpan 	lastNextTime;
		private TimeSpan 	lastPrevTime;
		private float 		lastComputedValue;
		private float 		lastV = float.MinValue;

		public override void Begin (TimeSpan beginTime)
		{
			lastNextTime = TimeSpan.FromSeconds (-1);
			lastPrevTime = TimeSpan.FromSeconds (-1);
			base.Begin (beginTime);
		}

		public TimeSpan GetDuration() {
			if ( _lastDuration > TimeSpan.FromSeconds(-1) )
				return _lastDuration;
			
			var max = TimeSpan.Zero;
			foreach ( SingleKeyFrame f in this.KeyFrames ) {
				if ( f.Time > max )
					max = f.Time;
			}
			_lastDuration = max;
			return max;
		}

		private TimeSpan _lastDuration;
	}
}