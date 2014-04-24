using Microsoft.Xna.Framework;
using System;

namespace System.Windows.Media.Animation
{
	public class SingleAnimation : SingleAnimationBase {

		/*
		public SingleAnimation ( Game game, float value ) : base ( game ) {
			this.To = value;
			this.From = value;
		}
		*/
	    public SingleAnimation ( float from, float to, TimeSpan duration, bool autoReverse ) {
	    	this.Duration = duration;
			this.From = from;
			this.To = to;
			this.AutoReverse = autoReverse;
	    }

	    #region Eigenschaften

	    public float From { get; set; }
	    public float To { get; set; }

	    #endregion

	    public override float GetCurrentValue() {
	        var currentTime = this.CurrentTime;
	        if ( this.Duration == TimeSpan.Zero || currentTime > this.Duration )
	            return this.To;
	        else if ( currentTime > TimeSpan.Zero && currentTime <= this.Duration )
	            return ComputeValue (
	                this.From, this.To, 1f / ( ( float ) this.Duration.Ticks / ( float ) currentTime.Ticks ) );
	        else
	            return this.From;
		}

		public void SetFromToCurrent ( ) {
			this.From = GetCurrentValue();
		}
	    
        /*
	    protected override void Reverse ( ) {
	    	float tmp = this.From;
	    	this.From = this.To;
	    	this.To = tmp;
	    }
         * */
	}
}
