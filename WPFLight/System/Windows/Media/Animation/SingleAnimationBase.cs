using System;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace System.Windows.Media.Animation {
    public class ValueChangedEventArgs : EventArgs {
        public ValueChangedEventArgs ( float value )
            : base ( ) {
            this.Value = value;
        }

        public float Value { get; private set; }
    }

    public delegate void ValueChangedEventHandler ( object sender, ValueChangedEventArgs e );
	public abstract class SingleAnimationBase : IAnimationBase {
        public SingleAnimationBase ( ) {
            State = AnimationState.Stopped;
        }

        public event EventHandler<EventArgs> Completed;

        public TimeSpan BeginTime { get; set; }
        public TimeSpan Duration { get; set; }
        public AnimationState State { get; protected set; }
        public bool AutoReverse { get; set; }
        public int RepeatBehavior { get; set; }

        protected DateTime StartTime { get; private set; }
        protected TimeSpan CurrentTime { get; private set; }

        protected float ComputeValue ( float from, float to, float v ) {
            float delta = 0f;
            if ( from < to )
                delta = to - from;
            else
                delta = -( from - to );

            return from
                + ( delta * v );
        }

        public abstract float GetCurrentValue ( );

        public void Begin ( ) {
            StartTime = DateTime.UtcNow;
            if ( reverse )
                CurrentTime = this.Duration;
            else
                CurrentTime = TimeSpan.Zero;

            State = AnimationState.Started;
        }

        public virtual void Begin ( TimeSpan beginTime ) {
            this.BeginTime = beginTime;
            this.Begin ( );
        }

        public virtual void Stop ( ) {
            repeatIndex = 0;
            State = AnimationState.Stopped;
            reverse = false;
        }

        public virtual void Pause ( ) {
            State = AnimationState.Paused;
        }

        public void Resume ( ) {
            State = AnimationState.Started;
        }

        public void Update ( GameTime gameTime ) {
            var ctm = DateTime.UtcNow;
            if ( State == AnimationState.Started ) {
                var time = ctm - this.StartTime - this.BeginTime;
                if ( reverse )
                    time = this.Duration - time;

                if ( time.Ticks > 0 ) {
                    if ( time > this.Duration ) {
                        this.CurrentTime = this.Duration;
                        this.UpdateTargetProperty ( );

                        if ( this.AutoReverse ) {
                            reverse = true;
                            this.Begin ( );
                        } else {
                            if ( repeatIndex < RepeatBehavior - 1 ) {
                                repeatIndex++;
                                this.Begin ( );
                            } else {
                                repeatIndex = 0;
                                State = AnimationState.Stopped;
                                this.RaiseCompletedEvent ( );
                            }

                        }
                    } else
                        this.CurrentTime = time;
                } else {
                    this.CurrentTime = TimeSpan.Zero;
                    if ( reverse ) {
                        if ( repeatIndex < RepeatBehavior - 1 ) {
                            repeatIndex++;
                            reverse = false;
                            this.Begin ( );
                        } else {
                            repeatIndex = 0;
                            State = AnimationState.Stopped;
                            reverse = false;
                            this.RaiseCompletedEvent ( );
                        }
                    }
                }
            }
            if ( lastTime != CurrentTime ) {
                lastTime = CurrentTime;
                //System.Diagnostics.Debug.WriteLine ( this.GetCurrentValue ( ) );
                this.UpdateTargetProperty ( );
            }
			//base.Update ( gameTime );
        }

        void UpdateTargetProperty ( ) {
            if ( targetPropertyInfo == null && !propertyInfoChecked ) {
                target = Storyboard.GetTarget ( this );
                targetProperty = Storyboard.GetTargetProperty ( this );

                if ( target != null && targetProperty != null ) {
                    targetPropertyInfo = target
                        .GetType ( )
                        .GetProperty (
                            targetProperty );
                }
                propertyInfoChecked = true;
            }
            if ( targetPropertyInfo != null ) {
                targetPropertyInfo.SetValue (
                    target, GetCurrentValue ( ), null );
            }
        }

        protected void RaiseCompletedEvent ( ) {
            if ( this.Completed != null )
                this.Completed ( this, EventArgs.Empty );
        }

        private int             repeatIndex;

        private bool            reverse;
        private bool            propertyInfoChecked;
        private TimeSpan        lastTime;
        private Control         target;
        private String          targetProperty;
        private PropertyInfo    targetPropertyInfo;
    }

    public enum AnimationState {
        Started,
        Paused,
        Stopped,
    }
}