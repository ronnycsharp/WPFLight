namespace System.Windows.Media.Animation {
    public class Clock {
        protected internal Clock ( Timeline timeline ) {
            Timeline = timeline;
        }

        #region Ereignisse

        public event EventHandler CurrentStateInvalidated;
        public event EventHandler CurrentTimeInvalidated;

        #endregion

        #region Eigenschaften

        public ClockState CurrentState { get; private set; }

        public double? CurrentProgress { get; set; }

        public Timeline Timeline { get; private set; }

		public TimeSpan? CurrentTime { get; private set; }

        protected TimeSpan CurrentGlobalTime {
            get {
                return currentGlobalTime;
            }
        }

        #endregion

        /// <summary>
        /// Wird nur einmal global innerhalb der GameLoop aufgerufen
        /// </summary>
        /// <param name="time"></param>
        internal static void SetCurrentGlobalTime (TimeSpan time) {
            currentGlobalTime = time;
        }

        static TimeSpan currentGlobalTime;
    }

    // Summary:
    //     Describes the potential states of a timeline's System.Windows.Media.Animation.Clock
    //     object.
    public enum ClockState {
        // Summary:
        //     The current System.Windows.Media.Animation.Clock time changes in direct relation
        //     to that of its parent. If the timeline is an animation, it is actively affecting
        //     targeted properties, so their value may change from tick (a sampling point
        //     in time) to tick. If the timeline has children, they may be System.Windows.Media.Animation.ClockState.Active,
        //     System.Windows.Media.Animation.ClockState.Filling, or System.Windows.Media.Animation.ClockState.Stopped.
        Active = 0,
        //
        // Summary:
        //     The System.Windows.Media.Animation.Clock timing continues, but does not change
        //     in relation to that of its parent. If the timeline is an animation, it is
        //     actively affecting targeted properties, but its values don't change from
        //     tick to tick. If the timeline has children, they may be System.Windows.Media.Animation.ClockState.Active,
        //     System.Windows.Media.Animation.ClockState.Filling, or System.Windows.Media.Animation.ClockState.Stopped.
        Filling = 1,
        //
        // Summary:
        //     The System.Windows.Media.Animation.Clock timing is halted, making the clock's
        //     current time and progress values undefined. If this timeline is an animation,
        //     it no longer affects targeted properties. If this timeline has children,
        //     they are also System.Windows.Media.Animation.ClockState.Stopped.
        Stopped = 2,
    }
}