using System.Windows.Media;
using WPFLight.Helpers;

namespace System.Windows.Controls {
	public class MessageBox : Window {
		private MessageBox ( ) {
			this.Width = 350;
			this.Height = 200;

            this.Background = new SolidColorBrush (System.Windows.Media.Color.FromArgb (230, 150, 150, 150 ));
			this.BorderBrush = new SolidColorBrush (new Color(.8f,.8f,.8f )*.4f);
			this.BorderThickness = new Thickness (2);

			gridRoot = new Grid ();
			gridRoot.RowDefinitions.Add (new RowDefinition (new GridLength (45, GridUnitType.Pixel)));
			gridRoot.RowDefinitions.Add (RowDefinition.Star);
			gridRoot.RowDefinitions.Add (new RowDefinition (new GridLength (75, GridUnitType.Pixel)));
            gridRoot.Parent = this;

			base.Content = gridRoot;
		}

		#region Properties

		public new object Content {
			get { 
				return dialogContent;
			}
			set { 
				if (dialogContent != value) {
					var oldElement = dialogContent as UIElement;
					if (oldElement != null)
						gridRoot.Children.Remove(oldElement);

					var newElement = value as UIElement;
					if (newElement != null) {
						gridRoot.Children.Add(newElement);
						Grid.SetRow(newElement, 1);
					}

					dialogContent = value;
				}
			}
		}

		#endregion


        public override void Invalidate () {
            base.Invalidate();
			lblText.Text = this.Text;
            gridRoot.Invalidate();
        }

		public override void Initialize () {
			lblTitle = new Label ();
			lblTitle.Text = this.Title;
			lblTitle.FontScale = .17f;
			lblTitle.Margin = new Thickness (8, 4, 0, 0);

			gridRoot.Children.Add (lblTitle);

			lblText = new Label ();
			lblText.Text = this.Text;
			lblText.Foreground = Brushes.White;
			lblText.FontScale = .20f;
			lblText.Margin = new Thickness (30, 0, 0, 0);
			lblText.HorizontalAlignment = HorizontalAlignment.Left;
			lblText.VerticalAlignment = VerticalAlignment.Top;

			this.Content = lblText;

			cmdOkay = new Button ();
			cmdOkay.Content = "OK";
            cmdOkay.IsEnabled = true;
			cmdOkay.Margin = new Thickness (0, 0, 0, 18);
			cmdOkay.Width = 140;
            cmdOkay.FontSize = .18f;
			cmdOkay.Foreground = Brushes.White;
			cmdOkay.HorizontalAlignment = HorizontalAlignment.Center;
			cmdOkay.VerticalAlignment = VerticalAlignment.Stretch;
			cmdOkay.Click += delegate {
				this.DialogResult = true;
				this.Close ();
			};

			gridRoot.Children.Add (cmdOkay);
			Grid.SetRow (cmdOkay, 2);

			if (this.WindowStartUpLocation == WindowStartUpLocation.CenterScreen) {
				var left = ((float)ScreenHelper.ORIGINAL_WIDTH / 2f) - this.ActualWidth / 2f;
				var top = ((float)ScreenHelper.ORIGINAL_HEIGHT / 2f) - this.ActualHeight / 2f;

				this.Left = left;
				this.Top = top;
			}

            gridRoot.Initialize();
			base.Initialize ();
		}
			
		#region Properties

		public string Title	{ get; set; }
		public string Text 	{ get; set; }

		#endregion

		public static void Show ( string text ) {
			if (text == null)
				throw new ArgumentNullException ();

			var msg = new MessageBox { 
				Text = text,
			};
			msg.Show (true);
		}

		private Button cmdOkay;
		private Label lblTitle;
		private Label lblText;
		private Grid gridRoot;

		private object dialogContent;
	}
}