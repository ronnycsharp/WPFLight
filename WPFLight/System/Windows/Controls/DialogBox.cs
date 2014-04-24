using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFLight.Helpers;
using System.Windows.Media;

namespace System.Windows.Controls {
	public abstract class DialogBox : Window {
		public DialogBox (SpriteFont font) : base (font) {
			this.Width = 200;
			this.Height = 200;

			gridRoot = new Grid ();
			gridRoot.RowDefinitions.Add (new RowDefinition (new GridLength (45, GridUnitType.Pixel)));
			gridRoot.RowDefinitions.Add (RowDefinition.Star);
			gridRoot.RowDefinitions.Add (new RowDefinition (new GridLength (65, GridUnitType.Pixel)));

			gridContent = new Grid ();

			this.Children.Add (gridRoot);
			Grid.SetRow (gridRoot, gridContent, 1);
		}

		public override void Initialize () {
			rcTitle = new System.Windows.Shapes.Rectangle ();
			rcTitle.RadiusX = 0;
			rcTitle.RadiusY = 0;
			rcTitle.Fill = new SolidColorBrush (System.Windows.Media.Color.FromArgb ( 240,50,50, 50 ));
			rcTitle.Stroke = new SolidColorBrush (System.Windows.Media.Colors.White * .5f);
			rcTitle.StrokeThickness = 1;

			Grid.SetRowSpan (gridRoot, rcTitle, 3);

			gridRoot.Children.Add (rcTitle);

			lblTitle = new Label (this.Font);
			lblTitle.Text = this.Title;
			lblTitle.FontScale = .4f;
			lblTitle.Margin = new Thickness (8, 4, 0, 0);

			gridRoot.Children.Add (lblTitle);

			cmdCancel = new Button (this.Font);
			cmdCancel.Content = "CANCEL";
			//cmdCancel.FontScale = .35f;
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

			cmdOkay = new Button (this.Font);
			cmdOkay.Content = "OK";
			//cmdOkay.FontScale = .35f;
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

			gridRoot.Children.Add (gridContent);
			base.Initialize ();
		}

		#region Eigenschaften

		public string Title { get; set; }

		protected Grid Content { get { return gridContent; } }

		#endregion

		private System.Windows.Shapes.Rectangle rcTitle;
		private Button cmdCancel;
		private Button cmdOkay;
		private Label lblTitle;
		private Grid gridRoot;
		private Grid gridContent;
	}
}
