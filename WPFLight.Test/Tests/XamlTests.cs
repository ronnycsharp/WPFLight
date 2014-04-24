using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NUnit.Framework;

namespace WPFLight.Test {
	[TestFixture]
	public class XamlTests {
		[SetUp]
		public void Setup ( ) {
			xml += "<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:c=\"clr-namespace:System.Windows.Controls\" TargetType=\"{x:Type c:Button}\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">"
					+ "<Style.Setters>"
						+ "<Setter Property=\"Background\" Value=\"Red\"/>"
					+ "</Style.Setters>"
					+ "<Style.Triggers>"
						+ "<Trigger Property=\"IsTouchDown\" Value=\"True\">"
							+ "<Setter Property=\"Background\" Value=\"Yellow\"/>"
						+ "</Trigger>"
					+ "</Style.Triggers>"
				+ "</Style>";
		}

		[Test]
		public void TestStyle ( ) {
			var style = Style.Parse (xml);
			Assert.AreSame (style["Background"], Brushes.Red);
			Assert.AreSame (style["IsTouchDown", true].Setters["Background"], Brushes.Yellow);
		}

		private string xml;
	}
}

