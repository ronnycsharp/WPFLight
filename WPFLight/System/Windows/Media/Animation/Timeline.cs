namespace System.Windows.Media.Animation {
    public abstract class Timeline : DependencyObject {
        public Timeline () {
            currentClock = AllocateClock();
        }

        public event EventHandler Completed;

        #region Eigenschaften

        public TimeSpan BeginTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool AutoReverse { get; set; }
        public int RepeatBehavior { get; set; }

        #endregion

        protected virtual Clock AllocateClock () {
            return new Clock(this);
        }

        public void BeginAnimation ( ) {

        }

        private Clock currentClock;
    }
}