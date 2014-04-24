using System;
using System.Windows.Controls;
using NUnit.Framework;

namespace WPFLight.Test {
	[TestFixture]
	public class LayoutTests {
		[SetUp]
		public void Setup ( ) {
			gridParent = new Grid ();
			gridParent.Width = 500;
			gridParent.Height = 400;

			button = new Button ();
			gridParent.Children.Add (button);

			gridRoot = new Grid ();
			gridRoot.Width = 1000;
			gridRoot.Height = 1000;

			gridRoot.Children.Add (gridParent);
		}

		[Test]
		public void TestActualSize ( ) {
			button.Width = 100;
			button.Height = 150;
			Assert.AreEqual (button.Width, button.ActualWidth);
			Assert.AreEqual (button.Height, button.ActualHeight);
		}

		[Test]
		public void TestDerivedSize ( ) {
			Assert.AreEqual (gridParent.ActualWidth, button.ActualWidth);
			Assert.AreEqual (gridParent.ActualHeight, button.ActualHeight);
		}

		[Test]
		public void TestParentSize ( ) {
			gridParent.HorizontalAlignment = HorizontalAlignment.Left;
			gridParent.VerticalAlignment = VerticalAlignment.Top;

			Assert.AreEqual (gridParent.ActualWidth, button.ActualWidth);
			Assert.AreEqual (gridParent.ActualHeight, button.ActualHeight);
		}

		[Test]
		public void TestParentSizeCenter_Unsized ( ) {
			gridParent.HorizontalAlignment = HorizontalAlignment.Center;
			gridParent.VerticalAlignment = VerticalAlignment.Center;

			Assert.AreEqual (gridParent.ActualWidth, button.ActualWidth);
			Assert.AreEqual (gridParent.ActualHeight, button.ActualHeight);
		}

		//[Test]
		public void TestParentSizeCenter_Sized ( ) {
			gridParent.HorizontalAlignment = HorizontalAlignment.Center;
			gridParent.VerticalAlignment = VerticalAlignment.Center;

			gridParent.Width = null;
			gridParent.Height = null;

			button.Width = 100;
			button.Height = 120;

			Assert.AreEqual (gridParent.ActualWidth, button.ActualWidth);
			Assert.AreEqual (gridParent.ActualHeight, button.ActualHeight);
		}

		private Grid 	gridRoot;
		private Grid 	gridParent;
		private Button 	button;
	}
}

