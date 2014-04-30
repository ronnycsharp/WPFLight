using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace System.Windows.Controls {
	public abstract class UIElement 
        : DependencyObject, IGameComponent, IUpdateable, IDrawable, IDisposable {
		public UIElement () {
			this.Game = WPFLight.Helpers.ScreenHelper.Game;
			this.GraphicsDevice = WPFLight.Helpers.ScreenHelper.Device;

			this.Visible = true;
			this.IsEnabled = true;

			enabled = true;
			visible = true;
		}

		#region Ereignisse

		public event EventHandler<EventArgs> LostFocus;
		public event EventHandler<EventArgs> GotFocus;
		public event EventHandler<EventArgs> FocusChanged;
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;

		#endregion

		#region Properties

		// TODO Remove and Replace IUpdateable-Interface and methods
		[Obsolete]
		public bool Enabled {
            get { return enabled; }
            set {
                if (enabled != value) {
                    enabled = value;
                    OnEnabledChanged(enabled);
                }
            }
        }

		public static readonly DependencyProperty IsEnabledProperty =
			DependencyProperty.Register (
				"IsEnabled",
				typeof(bool),
				typeof(UIElement),
				new FrameworkPropertyMetadata ( true ) { Inherits = true } );

		public bool IsEnabled {
			get { return (bool)GetValue (IsEnabledProperty); }
			set { SetValue (IsEnabledProperty, value); }
		}

		public Rect Bounds {
			get {
				return new Rect (
					this.GetAbsoluteLeft (), 
					this.GetAbsoluteTop (), 
					this.ActualWidth, 
					this.ActualHeight);
			}
		}

		public UIElement FocusedControl {
			get { return focusedControl; }
			protected set {
				if (focusedControl != value) {
					if (focusedControl != null)
						focusedControl.OnLostFocus ();

					focusedControl = value;

					if (value != null)
						value.OnGotFocus ();
				}
			}
		}

		public bool IsFocused {
			get {
				if (this.Focusable)
					return (this.FocusableParent ?? this.Parent).IsChildFocused (this);

				return false;
			}
		}

		public bool Focusable { get; set; }

		/// <summary>
		/// Fokusierbares Elternelement, muss zugewiesen werden 
		/// falls sich mehrere Controls einen Focus über mehrere Parents teilen.
		/// </summary>
		/// <value>The focusable parent.</value>
		public UIElement FocusableParent { get; set; }

		public UIElement Parent { get; set; }

		public bool IsInitialized { get; protected set; }

		public GraphicsDevice GraphicsDevice { get; private set; }

		public Game Game { get; private set; }

		public int DrawOrder {
			get { return drawOrder; }
			set {
				if (drawOrder != value) {
					drawOrder = value;
					OnDrawOrderChanged (drawOrder);
				}
			}
		}

		public int UpdateOrder {
			get { return updateOrder; }
			set {
				if (updateOrder != value) {
					updateOrder = value;
					OnUpdateOrderChanged (updateOrder);
				}
			}
		}

		public object Tag { get; set; }

		public static readonly DependencyProperty AlphaProperty =
			DependencyProperty.Register (
				"Alpha",
				typeof(float),
				typeof(UIElement),
				new PropertyMetadata ( 1f ) );

		public float Alpha {
			get { return (float)GetValue (AlphaProperty); }
			set { SetValue (AlphaProperty, value); }
		}

		public bool Visible {
			get {
				return visible;
			}
			set {
				if (visible != value) {
					visible = value;
					OnVisibleChanged (value);
				}
			}
		}
			
		public static readonly DependencyProperty HeightProperty =
			DependencyProperty.Register (
				"Height",
				typeof(float?),
				typeof(UIElement),
				new PropertyMetadata (
					new PropertyChangedCallback (
						(s,e) => {
							((UIElement)s).InvalidateMeasure ( );
						} ) ) );

		public float? Height {
			get { return (float?)GetValue (HeightProperty); }
			set { SetValue (HeightProperty, value); }
		}

		public static readonly DependencyProperty WidthProperty =
			DependencyProperty.Register (
				"Width",
				typeof(float?),
				typeof(UIElement),
				new PropertyMetadata (
					new PropertyChangedCallback (
						(s,e) => {
							((UIElement)s).InvalidateMeasure ( );
						} ) ) );

		public float? Width {
			get { return (float?)GetValue (WidthProperty); }
			set { SetValue (WidthProperty, value); }
		}

		public float ActualWidth {
			get {
				if (actualWidth != null)
					return actualWidth.Value;

				var result = 0f;
				if (this.Width != null)
					result = this.Width.Value;
				else if (this.Parent != null)
					result = this.Parent.MeasureWidth (this) - this.Margin.Left - this.Margin.Right;

				actualWidth = result;
				return result;
			}
		}

		public float ActualHeight {
			get {
				if (actualHeight != null)
					return actualHeight.Value;

				var result = 0f;
				if (this.Height != null)
					result = this.Height.Value;
				else if (this.Parent != null)
					result = this.Parent.MeasureHeight (this) - this.Margin.Top - this.Margin.Bottom;

				actualHeight = result;
				return result;
			}
		}

		public static readonly DependencyProperty MarginProperty =
			DependencyProperty.Register (
				"Margin",
				typeof(Thickness),
				typeof(UIElement));

		public Thickness Margin {
			get { return (Thickness)GetValue (MarginProperty); }
			set { SetValue (MarginProperty, value); }
		}

		public static readonly DependencyProperty PaddingProperty =
			DependencyProperty.Register (
				"Padding",
				typeof(Thickness),
				typeof(UIElement));

		public Thickness Padding {
			get { return (Thickness)GetValue (PaddingProperty); }
			set { SetValue (PaddingProperty, value); }
		}

		public static readonly DependencyProperty VerticalAlignmentProperty =
			DependencyProperty.Register (
				"VerticalAlignment",
				typeof(VerticalAlignment),
				typeof(UIElement),
				new PropertyMetadata ( 
					VerticalAlignment.Stretch) );

		public VerticalAlignment VerticalAlignment {
			get { return (VerticalAlignment)GetValue (VerticalAlignmentProperty); }
			set { SetValue (VerticalAlignmentProperty, value); }
		}
			
		public static readonly DependencyProperty HorizontalAlignmentProperty =
			DependencyProperty.Register (
				"HorizontalAlignment",
				typeof(HorizontalAlignment),
				typeof(UIElement),
				new PropertyMetadata ( 
					HorizontalAlignment.Stretch) );

		public HorizontalAlignment HorizontalAlignment {
			get { return (HorizontalAlignment)GetValue (HorizontalAlignmentProperty); }
			set { SetValue (HorizontalAlignmentProperty, value); }
		}

        // Nur WorkArround- Bis alle Left/Top auf Margin umgestellt sind.
		[Obsolete]
        public float Left {
            set {
                this.Margin = new Thickness(value, this.Margin.Top, this.Margin.Right, this.Margin.Bottom);
                this.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }

        // Nur WorkArround- Bis alle Left/Top auf Margin umgestellt sind.
		[Obsolete]
        public float Top {
            set {
                this.Margin = new Thickness(this.Margin.Left, value, this.Margin.Right, this.Margin.Bottom);
                this.VerticalAlignment = VerticalAlignment.Top;
            }
        }

		#endregion

		public virtual void Invalidate () {
			this.InvalidateMeasure ();
		}

		protected virtual void OnMarginChanged (Thickness newValue, Thickness oldValue) {}

		internal virtual float MeasureWidth (UIElement child) {
			var result = 0f;
			if (child.HorizontalAlignment == HorizontalAlignment.Stretch)
				result = this.ActualWidth;
			else
				result = child.MeasureWidth (this.ActualWidth);

			return result;
		}

		internal virtual float MeasureHeight (UIElement child) {
			var result = 0f;
			if (child.VerticalAlignment == VerticalAlignment.Stretch)
				result = this.ActualHeight;
			else
				result = child.MeasureHeight (this.ActualHeight);

			return result;
		}

		internal Size Measure (Size availableSize) {
            return new Size(
				this.MeasureWidth((float)availableSize.Width),
				this.MeasureHeight((float)availableSize.Height));
        }

		internal virtual float MeasureWidth (float availableWidth) {
			return 0;
		}

		internal virtual float MeasureHeight (float availableHeight) {
			return 0;
		}

		internal virtual float GetAbsoluteLeft (UIElement child) {
			var result = 0f;
			if (child.HorizontalAlignment == HorizontalAlignment.Left
			             || child.HorizontalAlignment == HorizontalAlignment.Stretch) {
				result = child.Margin.Left;
				if (child.Parent != null)
					result += child.Parent.GetAbsoluteLeft ();
			} else if (child.HorizontalAlignment == HorizontalAlignment.Right) {
				result = child.Parent.ActualWidth
					- child.Margin.Right
					- child.ActualWidth
					+ child.Parent.GetAbsoluteLeft ();
			} else if (child.HorizontalAlignment == HorizontalAlignment.Center) {
				result = (child.Parent.ActualWidth / 2f) - (child.ActualWidth / 2f) + child.Margin.Left - child.Margin.Right;
				if (child.Parent != null)
					result += child.Parent.GetAbsoluteLeft ();
			}

			return result;
		}

		internal virtual float GetAbsoluteTop (UIElement child) {
			var result = 0f;

			if (child.VerticalAlignment == VerticalAlignment.Top
			             || child.VerticalAlignment == VerticalAlignment.Stretch) {
				result = child.Margin.Top;
				if (child.Parent != null)
					result += child.Parent.GetAbsoluteTop ();
			} else if (child.VerticalAlignment == VerticalAlignment.Bottom) {
				result = child.Parent.ActualHeight
					- child.Margin.Bottom
					- child.ActualHeight
					+ child.Parent.GetAbsoluteTop ();
			} else if (child.VerticalAlignment == VerticalAlignment.Center) {
				result = (child.Parent.ActualHeight / 2f) - (child.ActualHeight / 2f) + child.Margin.Top - child.Margin.Bottom;
				if (child.Parent != null)
					result += child.Parent.GetAbsoluteTop ();

			}
			return result;
		}

		public virtual float GetAbsoluteLeft () {
			if (this.Parent != null)
				return
                    this.Parent.GetAbsoluteLeft (this);
			else
				return
                    this.Margin.Left;
		}

		public virtual float GetAbsoluteTop () {
			if (this.Parent != null)
				return
                    this.Parent.GetAbsoluteTop (this);
			else
				return
                    this.Margin.Top;
		}

		protected bool IsVisible () {
			var result = this.Visible && this.Alpha > 0;
			if (result) {
				if (this.Parent != null && !this.Parent.IsVisible ())
					result = false;
			}
			return result;
		}

		/*
		protected bool IsEnabled () {
			var result = this.IsEnabled;
			if (result) {
				if (this.Parent != null && !this.Parent.IsEnabled ()) {
					result = false;
				}
			}
			return result;
		}
		*/

		protected virtual void OnDeviceReset () {

		}

		protected virtual void OnEnabledChanged (bool enabled) {
			if (this.EnabledChanged != null)
				this.EnabledChanged (this, EventArgs.Empty);
		}

		protected virtual void OnVisibleChanged (bool visible) {
			if (this.VisibleChanged != null)
				this.VisibleChanged (this, EventArgs.Empty);
		}

		protected virtual void OnDrawOrderChanged (int drawOrder) {
			if (this.DrawOrderChanged != null)
				this.DrawOrderChanged (this, EventArgs.Empty);
		}

		protected virtual void OnUpdateOrderChanged (int updateOrder) {
			if (this.UpdateOrderChanged != null)
				this.UpdateOrderChanged (this, EventArgs.Empty);
		}

		public virtual void Initialize () {
			this.Game.GraphicsDevice.DeviceReset
                += delegate {
				OnDeviceReset ();
			};
			this.IsInitialized = true;
		}

		public void Focus () {
			if (this.Focusable)
				(this.FocusableParent ?? this.Parent).Focus (this);
		}

		public void Focus (UIElement ctrl) {
			this.FocusedControl = ctrl;
		}

		public bool IsChildFocused (UIElement ctrl) {
			if (this.FocusedControl == null)
				return false;

			return this.FocusedControl == ctrl;
		}

		protected virtual void OnLostFocus () {
			if (this.LostFocus != null)
				this.LostFocus (this, EventArgs.Empty);

			if (this.FocusChanged != null)
				this.FocusChanged (this, EventArgs.Empty);
		}

		protected virtual void OnGotFocus () {
			if (this.GotFocus != null)
				this.GotFocus (this, EventArgs.Empty);

			if (this.FocusChanged != null)
				this.FocusChanged (this, EventArgs.Empty);
		}

		protected void InvalidateMeasure ( ) {
			actualWidth = null;
			actualHeight = null;
		}

        protected internal virtual void OnRender (DrawingContext dc) { }

		public virtual void Update (GameTime gameTime) { }

		public virtual void Dispose () { }

		private bool visible;
		private bool enabled;
		private int drawOrder;
		private int updateOrder;
		private float? actualWidth;
		private float? actualHeight;
		private UIElement focusedControl;
	}
}
