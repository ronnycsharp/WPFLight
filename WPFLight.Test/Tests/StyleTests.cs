using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace WPFLight.Test {
	[TestFixture]
	public class StyleTests {
		[SetUp]
		public void Setup ( ) {
			root = new Grid ();
			root.Width = 500;
			root.Height = 400;
			button = new Button ();
			button2 = new Button ();

			root.Children.Add (button);
			root.Children.Add (button2);
		}

		[Test]
		public void TestSetter ( ) {
			var style = new Style ();
			style.TargetType = typeof(Button);
			style ["Width"] = 150;
			style ["Height"] = 120;
			style ["Background"] = new SolidColorBrush ( Color.Red );

			Assert.AreEqual (button.ActualWidth, root.ActualWidth);
			Assert.AreEqual (button.ActualHeight, root.ActualHeight);

			button.Style = style;
			button2.Style = style;

			Assert.AreEqual (button.ActualWidth, style ["Width"]);
			Assert.AreEqual (button.ActualHeight, style ["Height"]);
			Assert.AreSame (button.Background, style ["Background"]);

			button.Width = 100;
			Assert.AreEqual (button.ActualWidth, button.Width);

			button.Style = style;
			Assert.AreEqual (button.ActualWidth, style ["Width"]);

			Assert.AreSame (button2.Background, button.Background);
		}

		private Grid 	root;
		private Button 	button;
		private Button 	button2;
	}
}

