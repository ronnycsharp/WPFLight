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
			this.Background = Brushes.Green;

            this.ItemsPanel = new StackPanel() { Orientation = Orientation.Vertical };
			this.ItemsPanel.VerticalAlignment = VerticalAlignment.Stretch;
			this.ItemsPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.ItemsPanel.Padding = new Thickness(2);

			window = new Window(this);

			this.Padding = new Thickness ();
			this.Margin = new Thickness ();

			cmdItem = new Button ();

			this.ItemsPanel.Parent = window;
            window.Content = this.ItemsPanel;
        }

        #region Ereignisse


        #endregion

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

		public Panel ItemsPanel { get; set; }

        #endregion

        public override void Initialize () {
			cmdItem.Padding = new Thickness (9, 0, 0, 0);
			cmdItem.HorizontalContentAlignment = HorizontalAlignment.Left;
			cmdItem.Parent = this;
			cmdItem.Style = ( Style ) this.FindResource ("ButtonNumberStyle");
            cmdItem.FontSize = .35f;
			cmdItem.Content = this.SelectedItem;
			cmdItem.Initialize ();

            window.FontFamily = this.FontFamily;
			window.IsToolTip = false;
			window.Left = (int)this.GetAbsoluteLeft();
			window.Top = (int)this.GetAbsoluteTop() + this.ActualHeight;
			window.Width = this.ActualWidth;
			window.Height = 208;//this.ItemsPanel.ActualHeight;
            window.Background = new SolidColorBrush(Colors.White * .7f);
            window.BorderBrush = new SolidColorBrush(Colors.Gray * .9f);
            window.BorderThickness = new Thickness(1);
            window.LostFocus += delegate {

				ignoreTouchDown = window.DialogResult == null;
				/*
				if ( !selectionChanged && window.DialogResult != null )
					ignoreTouchDown = true;
				*/

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
			//base.Draw(gameTime, batch, a, transform);

			cmdItem.Draw (gameTime, batch, Alpha * a, transform);

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
					(int)Math.Floor (left + this.ActualWidth - 24), 
					(int)Math.Floor (top + this.ActualHeight / 2f), 24, 24),
				null, 
				new Microsoft.Xna.Framework.Color ( .32f, .32f, .32f ),
				MathHelper.ToRadians (0),
				new Vector2 (
					WPFLight.Resources.Textures.ArrowDown.Bounds.Width / 2f,
					WPFLight.Resources.Textures.ArrowDown.Height / 2f), 
				SpriteEffects.None, 
				0);

			batch.End ();
        }
			
		public override void OnTouchDown (TouchLocation state) {
			if (!ignoreTouchDown) {
				selectionChanged = false;
				base.OnTouchDown (state);
				this.IsDropDownOpen = !IsDropDownOpen;
			}
			ignoreTouchDown = false;
		}
			
		protected override void OnItemsCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
			base.OnItemsCollectionChanged (sender, e);
			switch ( e.Action ) {
				case NotifyCollectionChangedAction.Add: {
						foreach ( var item in e.NewItems ) {
							var cmd = 
								new Button ( ) {
									FontFamily = this.FontFamily,
									Content = item,
									CornerRadiusX = 5,
									CornerRadiusY = 5,
									VerticalAlignment = VerticalAlignment.Top,
									HorizontalContentAlignment = HorizontalAlignment.Left,
									Background = new SolidColorBrush ( Colors.White * .6f ),
									Foreground = Brushes.Black,
									Height = 58,
                                    FontSize = .36f,
									Alpha =  1,
									Margin = new Thickness ( 5,3,5,0 ),
									Padding = new Thickness ( 10,0,0,0 ),
							};
							cmd.Click += (object s, EventArgs ea) => {
								selectionChanged = true;
								this.SelectedItem = cmd.Content;
								window.DialogResult = true;
								window.Close ();
							};
							this.ItemsPanel.Children.Add ( cmd );
						}
						break;
					}
			}
			window.Height = this.Items.Count * 84;
		}

		protected override void OnSelectionChanged () {
			base.OnSelectionChanged ();
			this.cmdItem.Content = this.SelectedItem;
		}


		private bool selectionChanged;
		private bool ignoreTouchDown;
		private Window window;
		private Button cmdItem;
    }
}