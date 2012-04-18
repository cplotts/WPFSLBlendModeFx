using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFTestHarness
{
	/// <summary>
	/// Interaction logic for HelloWorldVideoTestWindow.xaml
	/// </summary>
	public partial class HelloWorldVideoTestWindow : Window
	{
		public HelloWorldVideoTestWindow()
		{
			InitializeComponent();
		}

		private void mediaElement_Loaded(object sender, RoutedEventArgs e)
		{
			this.mediaElement.Play();
		}

		private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			this.mediaElement.Position = TimeSpan.FromSeconds(0);
		}
	}
}
