using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;
using System.Windows.Data;

namespace System.Windows {
	public class Window : ContentControl {
		static Window () {
			ActiveWindows = new List<Window> ();
		}

		public Window () {
			this.WindowStartUpLocation = WindowStartUpLocation.CenterScreen;
            
            rcBackground = new System.Windows.Shapes.Rectangle();
			rcBackground.StrokeThickness = 1;
            rcBackground.Parent = this;
			rcBackground.RadiusX = 0;
			rcBackground.RadiusY = 0;

            rcBackground.SetBinding (
                System.Windows.Shapes.Rectangle.FillProperty, 
                new Binding("Background") { 
                    Source = this,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                });

            rcBackground.SetBinding (
                System.Windows.Shapes.Rectangle.StrokeProperty,
                new Binding("BorderBrush") {
                    Source = this,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                });
		}

		public Window (FrameworkElement owner) : this ( ) {
			this.Owner = owner;
		}

		#region Events

		public event EventHandler Activated;
		public event EventHandler Closed;

		#endregion

		#region Properties

		public bool?					DialogResult			{ get; set; }

		public bool 					IsOpened				{ get; private set; }

		public WindowStartUpLocation 	WindowStartUpLocation 	{ get; set; }

		public bool         			IsActive    			{ get; private set; }

		public bool         			IsModal     			{ get; private set; }

		public FrameworkElement			Owner					{ get; private set; }

		public new bool 				IsFocused { 
			get { 
				return focused;
			} 
			private set { 
				if (focused != value) {
					focused = value;
					if (value) {
						OnGotFocus ();
					} else {
						OnLostFocus ();
					}
				}
			}
		}

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register(
                "Left", typeof(float), typeof(Window));

        public new float Left {
            get { return (float)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register(
                "Top", typeof(float), typeof(Window));

        public new float Top {
            get { return (float)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

		internal static List<Window> 	ActiveWindows 			{ get; private set; }

		public static Window 			ActiveModal 			{ get; private set; }

		public static Window 			FocusedWindow 			{ get; private set; }

		/// <summary>
		/// Wenn True, dann wird das Fenster geschlossen, sobald es den Focus verliert.
		/// </summary>
		/// <value><c>true</c> if this instance is tool tip; otherwise, <c>false</c>.</value>
		public bool IsToolTip { get; set; }

		#endregion

        public override void Initialize () {
            rcBackground.Initialize();
            base.Initialize();
        }

        public override float GetAbsoluteLeft () {
			return this.Left + this.Margin.Left - this.Margin.Right;
        }

        public override float GetAbsoluteTop () {
			return this.Top + this.Margin.Top - this.Margin.Bottom;
        }

		internal override float GetAbsoluteLeft (UIElement child) {
			return base.GetAbsoluteLeft (child);
		}

		internal override float GetAbsoluteTop (UIElement child) {
			return base.GetAbsoluteTop (child);
		}

        public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
            rcBackground.Draw(gameTime, batch, alpha, transform);
            if (this.Content != null
                    && this.Content is FrameworkElement ) {
                ((FrameworkElement)this.Content).Draw(
                    gameTime, batch, alpha, transform);
            }
            //base.Draw(gameTime, batch, alpha, transform);
        }

		public static void Focus (Window window) {
			if (FocusedWindow != window) {
				if (FocusedWindow != null)
					FocusedWindow.IsFocused = false;

				FocusedWindow = window;
				if (window != null)
					window.IsFocused = true;
			}
		}

		public new void Focus () {
			Focus (this);
		}

		public static void DrawWindows (
			GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
			foreach (var window in ActiveWindows)
				window.Draw (gameTime, batch, alpha, transform);
		}

		public static void UpdateWindows (GameTime gameTime) {
			foreach (var window in ActiveWindows)
				window.Update (gameTime);
		}

		/// <summary>
		/// Gibt das Window-Objekt an der Postion zur�ck,
		/// - Priorit�t haben Modal-Windows
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static Window GetHitWindow (Vector2 v) {
			var modalWindow = ActiveWindows.FirstOrDefault (w => w.IsModal);
			if (modalWindow != null)
				return modalWindow;
			else {
				// Neue aktive Fenster werden bevorzugt, da sie sp�ter gezeichnet wurden
				// und somit im Fordergrund sind
				for (var n = ActiveWindows.Count - 1; n >= 0; n--) {
					var window = ActiveWindows [n];
					if (window.HitTest (v))
						return window;
				}
			}
			return null;
		}

		public virtual void Show (bool modal) {
			if (!IsInitialized)
				this.Initialize ();
				
			this.DialogResult = null;
			this.IsOpened = true;
			this.IsActive = true;
            this.Visibility = Visibility.Visible;
			this.IsModal = modal;

			if (modal)
				ActiveModal = this;
            
			this.Invalidate ();

			if (!ActiveWindows.Contains (this))
				ActiveWindows.Add (this);

			if (this.Activated != null)
				this.Activated (this, EventArgs.Empty);

			this.Focus ();
		}

		public virtual void Close () {
			this.IsActive = false;
            this.Visibility = Visibility.Hidden;
			this.IsOpened = false;

			if (this.IsModal)
				ActiveModal = null;

			ActiveWindows.Remove (this);

			if (this.Closed != null)
				this.Closed (this, EventArgs.Empty);
		}

		protected override void OnGotFocus () {

		}

		protected override void OnLostFocus () {
			base.OnLostFocus ();
			if (this.IsToolTip && this.IsActive) {
				this.Close ();
			}
		}

		private bool                focused;
        private Shapes.Rectangle    rcBackground;
	}

	public enum WindowStartUpLocation {
		CenterOwner,
		CenterScreen,
		Manual,
	}
}