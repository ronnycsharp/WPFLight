using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Helpers;

namespace System.Windows.Controls {
	public abstract class DialogBox : Window {
		public DialogBox ( ) {
			this.Width = 200;
			this.Height = 200;

			gridRoot = new Grid ();
			gridRoot.RowDefinitions.Add (new RowDefinition (new GridLength (45, GridUnitType.Pixel)));
			gridRoot.RowDefinitions.Add (RowDefinition.Star);
			gridRoot.RowDefinitions.Add (new RowDefinition (new GridLength (65, GridUnitType.Pixel)));
            gridRoot.Background = Brushes.Yellow; //new SolidColorBrush(new System.Windows.Media.Color(.2f, .2f, .2f, .8f));
            gridRoot.BorderBrush = new SolidColorBrush(new System.Windows.Media.Color(1, 1, 1, .8f));
            gridRoot.BorderThickness = new Thickness(2);
            gridRoot.Parent = this;
		}

        protected override void OnContentChanged (object oldContent, object newContent) {
            base.OnContentChanged(oldContent, newContent);
            var oldElement = oldContent as UIElement;
            if (oldElement != null)
                gridRoot.Children.Remove(oldElement);

            var newElement = newContent as UIElement;
            if (newElement != null) {
                gridRoot.Children.Add(newElement);
                Grid.SetRow(gridRoot, newElement, 1);
            }
        }

        public override void Invalidate () {
            base.Invalidate();
            gridRoot.Invalidate();
        }

		public override void Initialize () {

			lblTitle = new Label ();
			lblTitle.Text = this.Title;
			lblTitle.FontScale = .4f;
			lblTitle.Margin = new Thickness (8, 4, 0, 0);

			gridRoot.Children.Add (lblTitle);

			cmdCancel = new Button ();
			cmdCancel.Content = "CANCEL";
            cmdCancel.IsEnabled = true;
			cmdCancel.Margin = new Thickness (15, 5, 15, 10);
			cmdCancel.Width = 120;
			cmdCancel.Foreground = Brushes.White;
			cmdCancel.HorizontalAlignment = HorizontalAlignment.Right;
			cmdCancel.Click += delegate {
				this.DialogResult = false;
				this.Close ();
			};

			gridRoot.Children.Add (cmdCancel);
			Grid.SetRow (gridRoot, cmdCancel, 2);

			cmdOkay = new Button ();
			cmdOkay.Content = "OK";
            cmdOkay.IsEnabled = true;
			cmdOkay.Margin = new Thickness (15, 5, 145, 10);
			cmdOkay.Width = 80;
			cmdOkay.Foreground = Brushes.White;
			cmdOkay.HorizontalAlignment = HorizontalAlignment.Right;
			cmdOkay.Click += delegate {
				this.DialogResult = true;
				this.Close ();
			};

			gridRoot.Children.Add (cmdOkay);
			Grid.SetRow (gridRoot, cmdOkay, 2);

			if (this.WindowStartUpLocation == WindowStartUpLocation.CenterScreen) {
				var left = ((float)ScreenHelper.ORIGINAL_WIDTH / 2f) - this.ActualWidth / 2f;
				var top = ((float)ScreenHelper.ORIGINAL_HEIGHT / 2f) - this.ActualHeight / 2f;

				this.Left = left;
				this.Top = top;
			}

            gridRoot.Initialize();
			base.Initialize ();
		}

        public override void OnTouchDown (Microsoft.Xna.Framework.Input.Touch.TouchLocation state) {
            base.OnTouchDown(state);
            gridRoot.OnTouchDown(state);
        }

        public override void OnTouchUp (Microsoft.Xna.Framework.Input.Touch.TouchLocation state) {
            base.OnTouchUp(state);
            gridRoot.OnTouchUp(state);
        }

        public override void Update (GameTime gameTime) {
            base.Update(gameTime);
            gridRoot.Update(gameTime);
        }

        public override void Draw (GameTime gameTime, SpriteBatch batch, float alpha, Matrix transform) {
            gridRoot.Draw(gameTime, batch, alpha, transform);
            //base.Draw(gameTime, batch, alpha, transform);
        }

		#region Eigenschaften

		public string Title { get; set; }

		#endregion

		private Button cmdCancel;
		private Button cmdOkay;
		private Label lblTitle;
		private Grid gridRoot;
	}
}
