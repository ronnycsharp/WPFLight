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

			enabled = true;
		}

		#region Events

		public event EventHandler<EventArgs> LostFocus;
		public event EventHandler<EventArgs> GotFocus;
		public event EventHandler<EventArgs> FocusChanged;
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;

        public event DependencyPropertyChangedEventHandler IsVisibleChanged;

		#endregion

		#region Properties

        public Size DesiredSize {
            get {
                if (this.Visibility == Visibility.Collapsed)
                    return new Size(0, 0);

                return desiredSize;
            }
        }

		public bool IsMeasureValid  { get; private set; }
		public bool IsArrangeValid  { get; private set; }

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

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register(
                "IsVisible",
                typeof(bool),
                typeof(UIElement),
                new PropertyMetadata(
                    new PropertyChangedCallback(
                        OnIsVisibleChanged)));

        public bool IsVisible {
            get { return (bool)GetValue(IsVisibleProperty); }
            private set { SetValue(IsVisibleProperty, value); }
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

        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register(
                "Parent",
                typeof(UIElement),
                typeof(UIElement),
                new PropertyMetadata(
                    new PropertyChangedCallback(
                        OnParentChanged)));

        public UIElement Parent {
            get { return (UIElement)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

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

        [Obsolete]
        public float Alpha {
            get { return Opacity; }
            set { this.Opacity = value; }
        }

		public object Tag { get; set; }

		public static readonly DependencyProperty OpacityProperty =
			DependencyProperty.Register (
				"Opacity",
				typeof(float),
				typeof(UIElement),
				new PropertyMetadata ( 1f ) );

		public float Opacity {
			get { return (float)GetValue (OpacityProperty); }
			set { SetValue (OpacityProperty, value); }
		}

        [Obsolete]
		public bool Visible {
			get {
                return this.IsVisible;
			}
			set {
                this.IsVisible = value;
                if (value)
                    this.Visibility = Visibility.Visible;
                else {
                    this.Visibility = Visibility.Hidden;
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

        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register(
                "Visibility",
                typeof(Visibility),
                typeof(UIElement),
                new FrameworkPropertyMetadata(
                    Visibility.Visible,
                    new PropertyChangedCallback ( OnVisibilityChanged ),
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Visibility Visibility {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
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

        static void OnVisibilityChanged (object sender, DependencyPropertyChangedEventArgs e) {
            ((UIElement)sender).UpdateIsVisible();
        }

        static void OnIsVisibleChanged (object sender, DependencyPropertyChangedEventArgs e) {
            var uie = sender as UIElement;
            if (uie.IsVisibleChanged != null)
                uie.IsVisibleChanged(uie, e);
        }

        static void OnParentChanged (object sender, DependencyPropertyChangedEventArgs e ) {
            var uie = sender as UIElement;
            if (uie != null && e.NewValue != null ) {
                var parent = e.NewValue as UIElement;
				uie.UpdateIsVisible ();
                parent.IsVisibleChanged 
                    += (s, ea) => {
                        uie.UpdateIsVisible();
                    };
            }
        }

		//[Obsolete("Use UpdateLayout")]
		public virtual void Invalidate () {
			this.InvalidateMeasure ();
			this.InvalidateArrange ();
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

        public void Measure (Size availableSize) {
            if (!this.IsMeasureValid) {
                desiredSize = new Size(
                    this.MeasureWidth((float)availableSize.Width),
                    this.MeasureHeight((float)availableSize.Height));

                this.IsMeasureValid = true;
            }
        }

        protected virtual Size MeasureCore (Size availableSize) {
            return new Size(0, 0);
        }

        protected virtual void ArrangeCore (Size availableSize) {
            // TODO Implement
        }

        public void Arrange (Rect finalRect) {
            this.IsArrangeValid = true;
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
            this.UpdateIsVisible();
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

		public void InvalidateMeasure ( ) {
			this.IsMeasureValid = false;

			actualWidth = null;
			actualHeight = null;
		}

		public void InvalidateArrange ( ) {
			this.IsArrangeValid = false;
		}

        public void UpdateLayout () {
            this.InvalidateMeasure();
            this.InvalidateArrange();
        }

        void UpdateIsVisible ( ) {
            this.IsVisible = 
                this.Visibility == Visibility.Visible 
                    && ( this.Parent == null || this.Parent.IsVisible );
        }
			
        protected internal virtual void OnRender (DrawingContext dc) { }

		public virtual void Update (GameTime gameTime) { }

		public virtual void Dispose () { }

		private bool        enabled;
		private int         drawOrder;
		private int         updateOrder;
		private float?      actualWidth;
		private float?      actualHeight;
		private UIElement   focusedControl;
        private Size        desiredSize;
	}

	/// <summary>
	/// Visibility - Enum which describes 3 possible visibility options.
	/// </summary>
	/// <seealso cref="UIElement" />
	public enum Visibility : byte
	{
		/// <summary>
		/// Normally visible.
		/// </summary>
		Visible = 0,

		/// <summary>
		/// Occupies space in the layout, but is not visible (completely transparent).
		/// </summary>
		Hidden,

		/// <summary>
		/// Not visible and does not occupy any space in layout, as if it doesn't exist.
		/// </summary>
		Collapsed
	}
}
