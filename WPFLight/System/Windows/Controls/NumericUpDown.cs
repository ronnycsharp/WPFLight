using System;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Resources;
using System.Windows.Media;

namespace System.Windows.Controls {
	public class NumericUpDown : Panel {
        public NumericUpDown () : base ( ) {
			this.Value = 0;
            this.MinValue = 0;
            this.MaxValue = 100;
			this.Background = new SolidColorBrush (this.GraphicsDevice, System.Windows.Media.Colors.White * .28f);
        }

		#region Ereignisse

		public event EventHandler ValueChanged;

		#endregion

        #region Eigenschaften

		public static readonly DependencyProperty MinValueProperty = 
			DependencyProperty.Register ( 
				"MinValue", typeof ( int ), typeof ( NumericUpDown ) );

		public int MinValue { 
			get { return ( int ) GetValue (MinValueProperty); } 
			set { SetValue (MinValueProperty, value); } 
		}

		public static readonly DependencyProperty MaxValueProperty = 
			DependencyProperty.Register ( 
				"MaxValue", typeof ( int ), typeof ( NumericUpDown ) );

		public int MaxValue { 
			get { return ( int ) GetValue (MaxValueProperty); } 
			set { SetValue (MaxValueProperty, value); } 
		}
			
		public static readonly DependencyProperty ValueProperty = 
			DependencyProperty.Register ( 
				"Value", 
				typeof ( int ), 
				typeof ( NumericUpDown ), 
				new PropertyMetadata ( 
					0, OnValueChanged ) );

		public int Value { 
			get { return ( int ) GetValue (ValueProperty); } 
			set { SetValue (ValueProperty, value); } 
		}

        #endregion

		static void OnValueChanged ( DependencyObject sender, DependencyPropertyChangedEventArgs e ) {
			var ctrl = sender as NumericUpDown;
			ctrl.UpdateValue ();
			if (ctrl.ValueChanged != null)
				ctrl.ValueChanged (sender, EventArgs.Empty);
		}

        public override void Initialize () {
            gridRoot = new Grid();
            gridRoot.RowDefinitions.Add(RowDefinition.Star);
            gridRoot.ColumnDefinitions.Add(ColumnDefinition.Star);
			gridRoot.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(64, GridUnitType.Pixel)));
			gridRoot.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(64, GridUnitType.Pixel)));
            
            this.Children.Add(this.gridRoot);

            cmdDown = new Button();
			cmdDown.Content = new Image (Textures.ArrowDown){ Margin = new Thickness ( 10,15,10,15 ), Alpha = .6f };
			//cmdDown.FontScale = .3f;
			//cmdDown.Foreground = System.Windows.Media.Color.White;
			//cmdDown.Background = new SolidColorBrush (this.GraphicsDevice, Color.Black * .45f);
			cmdDown.BorderThickness = new Thickness (1);
			//cmdDown.BorderColor = Color.White * .45f;
			cmdDown.Margin = new Thickness (3);
			cmdDown.Click += delegate {
				this.Value =  ( int ) MathHelper.Clamp ( 
					this.Value - 1, this.MinValue, this.MaxValue );
			};

            gridRoot.Children.Add(cmdDown);

            Grid.SetColumn(gridRoot, cmdDown, 1);

            cmdUp = new Button();
			cmdUp.Content = new Image (Textures.ArrowUp){ Margin = new Thickness ( 10,15,10,15 ), Alpha = .6f };
			//cmdUp.Background = new SolidColorBrush (this.GraphicsDevice, Color.Black * .45f);
			cmdUp.BorderThickness = new Thickness (1);
			//cmdUp.BorderColor = Color.White * .45f;
			cmdUp.Margin = new Thickness (3);
			cmdUp.Click += delegate {
				this.Value =  ( int ) MathHelper.Clamp ( 
					this.Value + 1, this.MinValue, this.MaxValue );
			};

            gridRoot.Children.Add(cmdUp);

            Grid.SetColumn(gridRoot, cmdUp, 2);

			lblValue = new Label();
			lblValue.VerticalAlignment = VerticalAlignment.Center;
			lblValue.FontScale = .46f;
			lblValue.Margin = new Thickness (15,3,3,3);

			gridRoot.Children.Add (lblValue);

            base.Initialize();
			UpdateValue ();
        }

		void UpdateValue ( ) {
			if ( this.IsInitialized )
				lblValue.Text = this.Value.ToString ();
		}

		private Label lblValue;
        private Button cmdUp;
        private Button cmdDown;
        private Grid gridRoot;
    }
}