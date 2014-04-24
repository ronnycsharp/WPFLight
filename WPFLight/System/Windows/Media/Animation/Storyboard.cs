using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Xna.Framework;
using System.Windows.Controls;

namespace System.Windows.Media.Animation {
    public class Storyboard : IAnimationBase {
        static Storyboard ( ) {
            storyboards = new List<Storyboard> ( );
        }

        #region Eigenschaften

        public bool IsActive { get; private set; }

        public TimeSpan BeginTime { get; set; }

        public List<SingleAnimationBase> Children {
            get {
                if ( children == null )
                    children = new List<SingleAnimationBase> ( );

                return children;
            }
        }

        private static Dictionary<SingleAnimationBase, Control> Targets {
            get {
                if ( targets == null )
                    targets = new Dictionary<SingleAnimationBase, Control> ( );

                return targets;
            }
        }

        private static Dictionary<SingleAnimationBase, String> TargetProperties {
            get {
                if ( targetProperties == null )
                    targetProperties = new Dictionary<SingleAnimationBase, String> ( );

                return targetProperties;
            }
        }

        #endregion

        public void Begin ( ) {
            Begin ( TimeSpan.Zero );
        }

        public void Begin ( TimeSpan beginTime ) {
            this.BeginTime = beginTime;
            this.IsActive = true;
            foreach ( var child in this.Children ) {
                child.Completed += delegate { CheckState ( ); };
                child.Begin ( this.BeginTime );
            }
            if ( !storyboards.Contains ( this ) )
                storyboards.Add ( this );
        }

        public void Pause ( ) {
            this.IsActive = false;
            foreach ( var child in this.Children )
                child.Pause ( );

            storyboards.Remove ( this );
        }

        public void Resume ( ) {
            this.IsActive = true;
            foreach ( var child in this.Children )
                child.Resume ( );

            if ( !storyboards.Contains ( this ) )
                storyboards.Add ( this );
        }

        public void Stop ( ) {
            this.IsActive = false;
            foreach ( var child in this.Children )
                child.Stop ( );

            storyboards.Remove ( this );
        }

        public void Update ( GameTime gameTime ) {
            foreach ( var child in this.Children ) {
                child.Update ( gameTime );
            }
        }

        public static Control GetTarget ( SingleAnimationBase animation ) {
            return Storyboard.Targets.ContainsKey ( animation )
                ? Storyboard.Targets[animation] : null;
        }

        public static string GetTargetProperty ( SingleAnimationBase animation ) {
            return Storyboard.TargetProperties.ContainsKey ( animation )
                ? Storyboard.TargetProperties[animation] : null;
        }

        public static void SetTarget ( SingleAnimationBase animation, Control target ) {
            Storyboard.Targets[animation] = target;
        }

        public static void SetTargetProperty ( SingleAnimationBase animation, string propertyName ) {
            Storyboard.TargetProperties[animation] = propertyName;
        }

        void CheckState ( ) {
            var active = false;
            foreach ( var child in this.Children ) {
                if ( child.State == AnimationState.Started )
                    active = true;
            }
            this.IsActive = active;
            if ( !active )
                storyboards.Remove ( this );
        }

        public static void UpdateActive ( GameTime gameTime ) {
            foreach ( var s in storyboards.ToArray ( ) ) {
                if ( s.IsActive ) {
                    s.Update ( gameTime );
                }
            }
        }

        private List<SingleAnimationBase> children;

        private static IList<Storyboard>                        storyboards;
        private static Dictionary<SingleAnimationBase, Control>  targets;
        private static Dictionary<SingleAnimationBase, String>   targetProperties;
    }
}
