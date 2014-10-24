using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WPFLight.Resources;
using System.Windows.Data;

namespace System.Windows.Controls {
	public class ComboBox : Selector {
        public ComboBox ( ) {
			this.Background = Brushes.Red;

			this.Padding = new Thickness ();
			this.Margin = new Thickness ();

			window = new Window(this);
			cmdItem = new Button ();
			lbItems = new ListBox ();

			var itemsBinding = new Binding ();
			itemsBinding.Mode = BindingMode.OneWay;
			itemsBinding.Source = this;
			itemsBinding.Path = new PropertyPath ("Items");

			lbItems.Margin = new Thickness (2);
			lbItems.BorderThickness = new Thickness (0);
			lbItems.ItemContainerStyle = (Style)this.FindResource ("ComboBoxItemStyle");
			lbItems.SetBinding (ListBox.ItemsProperty, itemsBinding);
			lbItems.SelectionChanged += (s,e) => {
				window.DialogResult = this.SelectedItem != null;
				window.Close ();
				this.IsDropDownOpen = false;
			};
				
			window.Content = lbItems;

			var selectedItemBinding = new Binding ("SelectedItem");
			selectedItemBinding.Mode = BindingMode.TwoWay;
			selectedItemBinding.Source = lbItems;

			cmdItem.SetBinding (
				Button.ContentProperty, 
				selectedItemBinding);

			this.SetBinding (
				ListBox.SelectedItemProperty, 
				selectedItemBinding);
        }
			
        #region Properties

		public static DependencyProperty IsDropDownOpenProperty = 
			DependencyProperty.Register ( 
				"IsDropDownOpen", 
				typeof ( bool ), 
				typeof ( ComboBox ),
				new PropertyMetadata (
					new PropertyChangedCallback (
						( s, e ) => {
							if ( ( bool ) e.NewValue ) 
								((ComboBox)s).OpenDropDownList ( );
							else
								((ComboBox)s).CloseDropDownList ( );
						} ) ) );

		public bool IsDropDownOpen {
			get {
				return ( bool ) GetValue (IsDropDownOpenProperty);
			}
			set {
				SetValue (IsDropDownOpenProperty, value);
			}
		}

        #endregion

        public override void Initialize () {
			cmdItem.Padding = new Thickness (9, 0, 0, 0);
			cmdItem.HorizontalContentAlignment = HorizontalAlignment.Left;
			cmdItem.Parent = this;
			cmdItem.Style = ( Style ) this.FindResource ("ButtonNumberStyle");
			cmdItem.BorderBrush = Brushes.Transparent;
			cmdItem.BorderThickness = new Thickness ();
			cmdItem.FontSize = .32f;
			cmdItem.Content = this.SelectedItem;
			cmdItem.Initialize ();

            window.FontFamily = this.FontFamily;
			window.IsToolTip = false;
			window.Left = (int)this.GetAbsoluteLeft();
			window.Top = (int)this.GetAbsoluteTop() + this.ActualHeight;
			window.Width = this.ActualWidth;
			window.Height = 225;	// TODO ComputeItemsHeight()
			window.Background = new SolidColorBrush (Colors.White * .7f);
			window.BorderBrush = Brushes.Transparent; //new SolidColorBrush (Colors.CornflowerBlue * .4f);
			window.BorderThickness = new Thickness(1);
            window.LostFocus += delegate {
				if ( !ignoreLostFocus )
					this.IsDropDownOpen = false;
			};

			window.Initialize ();
            base.Initialize();
        }

		void OpenDropDownList ( ) {
			if (this.IsInitialized) {
				window.Show (false);
			}
		}

		void CloseDropDownList ( ) {
			if (this.IsInitialized) {
				window.Close ();
			}
		}

        protected override void OnVisibleChanged (bool visible) {
            base.OnVisibleChanged(visible);
            if (!visible)
                window.Close();
        }

        protected override void OnEnabledChanged (bool enabled) {
            base.OnEnabledChanged(enabled);
            if (!enabled)
                window.Close();
        }

		public override void Update (GameTime gameTime) {
			base.Update (gameTime);
			cmdItem.Update (gameTime);
		}

        public override void Draw (GameTime gameTime, SpriteBatch batch, float a, Matrix transform) {
			cmdItem.Draw (gameTime, batch, Opacity * a, transform);
			batch.Begin (
				SpriteSortMode.Deferred, 
				BlendState.AlphaBlend, 
				SamplerState.AnisotropicClamp, 
				DepthStencilState.None, 
				RasterizerState.CullNone, 
				null, 
				transform);

			var left = GetAbsoluteLeft ();
			var top = GetAbsoluteTop ();

			batch.Draw (
				Textures.ArrowDown, 
				new Rectangle (
					(int)Math.Floor (left + this.ActualWidth - 30), 
					(int)Math.Floor (top + this.ActualHeight / 2f), 20, 20),
				null, 
				new Microsoft.Xna.Framework.Color ( .8f, .8f, .8f ) * .5f,
				MathHelper.ToRadians (0),
				new Vector2 (
					WPFLight.Resources.Textures.ArrowDown.Bounds.Width / 2f,
					WPFLight.Resources.Textures.ArrowDown.Height / 2f), 
				SpriteEffects.None, 
				0);

			batch.End ();
        }
			
		public override void OnTouchDown (TouchLocation state) {
			base.OnTouchDown (state);
			ignoreLostFocus = true;
			this.IsDropDownOpen = !this.IsDropDownOpen;
		}

		public override void OnTouchUp (TouchLocation state) {
			base.OnTouchUp (state);
			ignoreLostFocus = false;

			if (this.IsDropDownOpen)
				window.Focus ();
		}
			
		protected override void OnSelectionChanged () {
			base.OnSelectionChanged ();
		}

		public override void Invalidate () {
			base.Invalidate ();
			window.Left = (int)this.GetAbsoluteLeft();
			window.Top = (int)this.GetAbsoluteTop() + this.ActualHeight - 5;
		}

		private bool ignoreLostFocus;
		private Window window;
		private ListBox lbItems;
		private Button cmdItem;
    }
}