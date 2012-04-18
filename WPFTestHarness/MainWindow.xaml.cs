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
using System.Reflection;

namespace WPFTestHarness
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)e.Source;
			if (button.Content.ToString() != "Close")
			{
				Type type = this.GetType();
				Assembly assembly = type.Assembly;
				object o = assembly.CreateInstance(type.Namespace + "." + button.Content);
				if (o is Window)
				{
					Window window = (Window)o;
					window.ShowDialog();
				}
				else if (o is Page)
				{
					Page p = (Page)o;
					Window w = new Window();
					w.Content = p;
					w.ShowDialog();
				}
			}
			else
			{
				this.Close();
			}
		}
	}
}
