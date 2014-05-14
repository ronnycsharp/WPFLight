
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls {
	public class MenuButton : RadioButton {
        public MenuButton () {
            items = new List<Button>();

			this.HorizontalContentAlignment = HorizontalAlignment.Left;
			this.GroupName = "MenuButton";
			this.ItemsPanel = new StackPanel();
        }

        #region Ereignisse

		public event EventHandler ItemClick;

        #endregion

        #region Eigenschaften

		public Panel ItemsPanel { get; set; }

        #endregion

        public override void Initialize () {
			window = new Window(this);
			window.IsToolTip = true;
			window.FontFamily = this.FontFamily;
			window.Left = (int)this.GetAbsoluteLeft() + this.ActualWidth + 2;
			window.Top = (int)this.GetAbsoluteTop() + this.ActualHeight / 2f - this.ItemsPanel.ActualHeight / 2f;
			window.Width = this.ItemsPanel.ActualWidth;
			window.Height = this.ItemsPanel.ActualHeight;

			this.ItemsPanel.Parent = window;
            window.Content = this.ItemsPanel;

            foreach (var item in items) {
				this.ItemsPanel.Children.Add(item);
            }
            base.Initialize();
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

        public override void Draw (GameTime gameTime, SpriteBatch batch, float a, Matrix transform) {
            base.Draw(gameTime, batch, a, transform);

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
				WPFLight.Resources.Textures.ArrowDown, 
				new Rectangle (
					(int)Math.Floor (left + this.ActualWidth - 15 + 6), 
					(int)Math.Floor (top + this.ActualHeight / 2f), 16, 16),
				null, 
				Microsoft.Xna.Framework.Color.White * .7f,
				MathHelper.ToRadians (-90),
				new Vector2 (
					WPFLight.Resources.Textures.ArrowDown.Bounds.Width / 2f,
					WPFLight.Resources.Textures.ArrowDown.Height / 2f), 
				SpriteEffects.None, 
				0);

			batch.End ();
        }

        public void AddItem (string text) {
			var cmd = new Button ();
			cmd.Parent = this.ItemsPanel;
			cmd.Content = text;
			cmd.Visible = true;
			cmd.Height = 60;
			cmd.Width = 80;
			cmd.Style = Style.GetStyleResource ("ButtonNumberStyle");
			cmd.Margin = new Thickness(2);
			cmd.HorizontalAlignment = HorizontalAlignment.Left;
			cmd.BorderBrush = new SolidColorBrush ( Colors.Black * .17f );
			cmd.BorderThickness = new Thickness (1f);
			cmd.Tag = text;
			cmd.Click += (sender, e) => {
				if (!(sender is ToggleButton)) {
					window.Close();
					this.IsChecked = false;
				}
				if ( this.ItemClick != null )
					this.ItemClick ( sender, e );
			};

			if (this.IsInitialized)
				this.ItemsPanel.Children.Add(cmd);
			else
				items.Add(cmd);
		}
			
        public void AddItem (Button cmd) {
			cmd.Parent = this.ItemsPanel;
            cmd.Visible = true;
			cmd.Height = 60;
			cmd.Width = 80;
			cmd.Margin = new Thickness(2);
            cmd.HorizontalAlignment = HorizontalAlignment.Left;
            cmd.Click += (sender, e) => {
				if (!(sender is ToggleButton)) {
                    window.Close();
                    this.IsChecked = false;
                }
				if ( this.ItemClick != null )
					this.ItemClick ( sender, e );
            };

            if (this.IsInitialized)
				this.ItemsPanel.Children.Add(cmd);
            else
                items.Add(cmd);
        }

		public void AddItem ( Image img ) {
			AddItem (img, null);
		}

		public void AddItem (Image img, object tag) {
			var cmd = new Button ();
			cmd.Content = img;
			cmd.Parent = this.ItemsPanel;
			//cmd.Background = new SolidColorBrush ( .2f, .2f, .2f );
			cmd.Visible = true;
			cmd.Height = 60;
			cmd.Width = 80;
			cmd.Margin = new Thickness(2);
			cmd.FontSize = this.FontSize;
			cmd.HorizontalAlignment = HorizontalAlignment.Left;
			cmd.Tag = tag;
			cmd.Click += (sender, e) => {
				if (!(sender is ToggleButton)) {
					window.Close();
					this.IsChecked = false;
				}
				if ( this.ItemClick != null )
					this.ItemClick ( sender, e );
			};

			if (this.IsInitialized)
				this.ItemsPanel.Children.Add(cmd);
			else
				items.Add(cmd);
		}
			
		public void AddItem (Texture2D image) {
			AddItem (image, new Thickness (7));
		}

		public void AddItem ( Texture2D image, Thickness margin ) {
			AddItem (image, margin, null);
		}

		public void AddItem ( Texture2D image, Thickness margin, object tag ) {
			var cmd = new Button ();
			cmd.Content = new Image ( image) { Margin = margin };
			cmd.Parent = this.ItemsPanel;
			//cmd.Background = new SolidColorBrush ( .2f, .2f, .2f );
			cmd.Visible = true;
			cmd.Height = 60;
			cmd.Width = 80;
			cmd.Margin = new Thickness(2);
			cmd.FontSize = this.FontSize;
			cmd.HorizontalAlignment = HorizontalAlignment.Left;
			cmd.Tag = tag;
			cmd.Click += (sender, e) => {
				if (!(sender is ToggleButton)) {
					window.Close();
					this.IsChecked = false;
				}
				if ( this.ItemClick != null )
					this.ItemClick ( sender, e );
			};

			if (this.IsInitialized)
				this.ItemsPanel.Children.Add(cmd);
			else
				items.Add(cmd);
		}

		public override void OnTouchDown (TouchLocation state) {
			base.OnTouchDown (state);

		}

        protected override void OnCheckedChanged (bool chk) {
			base.OnCheckedChanged (chk);
			if (chk) {
				window.Invalidate ();
				window.Show (false);
			} else {
				/*
				if (window.IsActive) {
					unchecking = true;
					window.Close ();
					unchecking = false;
				}
				*/
			}
        }
			
		private bool 			unchecking;
		private Window 			window;
		private List<Button> 	items;
    }
}
