using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorPicker
{
	#region ColorPicker
	public class ColorPicker : Control
	{
		#region Static Constructor
		static ColorPicker()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
		}
		#endregion

		#region Public Constructor
		public ColorPicker()
		{
			SetupColorBindings();
		}
		#endregion

		#region Public Dependency Properties
		#region Color
		/// <summary>
		/// Gets or sets the Color property.
		/// </summary>
		public Color Color
		{
			get { return (Color)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}
		/// <summary>
		/// Color Dependency Property
		/// </summary>
		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register
			(
				"Color",
				typeof(Color),
				typeof(ColorPicker),
				new FrameworkPropertyMetadata
				(
					Colors.Black,
					new PropertyChangedCallback(OnColorChanged)
				)
			);
		private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker colorPicker = (ColorPicker)d;

			Color oldValue = (Color)e.OldValue;
			Color newValue = (Color)e.NewValue;

			colorPicker.InternalColor = newValue;

			colorPicker.OnColorChanged(oldValue, newValue);
		}

		/// <summary>
		/// Occurs after Color has changed
		/// </summary>
		public event RoutedPropertyChangedEventHandler<Color> ColorChanged
		{
			add { AddHandler(ColorChangedEvent, value); }
			remove { RemoveHandler(ColorChangedEvent, value); }
		}
		/// <summary>
		/// ColorChanged Routed Event
		/// </summary>
		public static readonly RoutedEvent ColorChangedEvent =
			EventManager.RegisterRoutedEvent
			(
				"ColorChanged",
				RoutingStrategy.Bubble,
				typeof(RoutedPropertyChangedEventHandler<Color>),
				typeof(ColorPicker)
			);
		protected virtual void OnColorChanged(Color oldValue, Color newValue)
		{
			RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue);
			args.RoutedEvent = ColorPicker.ColorChangedEvent;
			RaiseEvent(args);
		}
		#endregion

		#region Alpha
		/// <summary>
		/// Gets or sets the Alpha property.
		/// </summary>
		public byte Alpha
		{
			get { return (byte)GetValue(AlphaProperty); }
			set { SetValue(AlphaProperty, value); }
		}
		/// <summary>
		/// Alpha Dependency Property
		/// </summary>
		public static readonly DependencyProperty AlphaProperty =
			DependencyProperty.Register
			(
				"Alpha",
				typeof(byte),
				typeof(ColorPicker)
			);
		#endregion

		#region Red
		/// <summary>
		/// Gets or sets the Red property.
		/// </summary>
		public byte Red
		{
			get { return (byte)GetValue(RedProperty); }
			set { SetValue(RedProperty, value); }
		}
		/// <summary>
		/// Red Dependency Property
		/// </summary>
		public static readonly DependencyProperty RedProperty =
			DependencyProperty.Register
			(
				"Red",
				typeof(byte),
				typeof(ColorPicker)
			);
		#endregion

		#region Green
		/// <summary>
		/// Gets or sets the Green property.
		/// </summary>
		public byte Green
		{
			get { return (byte)GetValue(GreenProperty); }
			set { SetValue(GreenProperty, value); }
		}
		/// <summary>
		/// Green Dependency Property
		/// </summary>
		public static readonly DependencyProperty GreenProperty =
			DependencyProperty.Register
			(
				"Green",
				typeof(byte),
				typeof(ColorPicker)
			);
		#endregion

		#region Blue
		/// <summary>
		/// Gets or sets the Blue property.
		/// </summary>
		public byte Blue
		{
			get { return (byte)GetValue(BlueProperty); }
			set { SetValue(BlueProperty, value); }
		}
		/// <summary>
		/// Blue Dependency Property
		/// </summary>
		public static readonly DependencyProperty BlueProperty =
			DependencyProperty.Register
			(
				"Blue",
				typeof(byte),
				typeof(ColorPicker)
			);
		#endregion
		#endregion

		#region InternalColor
		/// <summary>
		/// Gets or sets the InternalColor property.
		/// </summary>
		private Color InternalColor
		{
			get { return (Color)GetValue(InternalColorProperty); }
			set { SetValue(InternalColorProperty, value); }
		}
		/// <summary>
		/// InternalColor Dependency Property
		/// </summary>
		private static readonly DependencyProperty InternalColorProperty =
			DependencyProperty.Register
			(
				"InternalColor",
				typeof(Color),
				typeof(ColorPicker),
				new FrameworkPropertyMetadata((Color)Colors.Black,
				new PropertyChangedCallback(OnInternalColorChanged))
			);
		/// <summary>
		/// Synchronizes any change in the InternalColor property with the publicly 
		/// </summary>
		private static void OnInternalColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker colorPicker = (ColorPicker)d;
			Color newValue = (Color)e.NewValue;

			colorPicker.Color = newValue;
		}
		#endregion

		#region Private Methods
		private void SetupColorBindings()
		{
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Converter = new ByteColorMultiConverter();
			multiBinding.Mode = BindingMode.TwoWay;

			Binding alphaBinding = new Binding("Alpha");
			alphaBinding.Source = this;
			alphaBinding.Mode = BindingMode.TwoWay;
			multiBinding.Bindings.Add(alphaBinding);

			Binding redBinding = new Binding("Red");
			redBinding.Source = this;
			redBinding.Mode = BindingMode.TwoWay;
			multiBinding.Bindings.Add(redBinding);

			Binding greenBinding = new Binding("Green");
			greenBinding.Source = this;
			greenBinding.Mode = BindingMode.TwoWay;
			multiBinding.Bindings.Add(greenBinding);

			Binding blueBinding = new Binding("Blue");
			blueBinding.Source = this;
			blueBinding.Mode = BindingMode.TwoWay;
			multiBinding.Bindings.Add(blueBinding);

			this.SetBinding(InternalColorProperty, multiBinding);
		}
		#endregion
	}
	#endregion

	#region ByteColorMultiConverter
	public class ByteColorMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (values.Length != 4)
				throw new ArgumentException("The ByteColorMultiConverter needs four byte values in order to convert them to a Color.");

			byte alpha = (byte)values[0];
			byte red = (byte)values[1];
			byte green = (byte)values[2];
			byte blue = (byte)values[3];

			return Color.FromArgb(alpha, red, green, blue);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			Color color = (Color)value;
			return new object[] { color.A, color.R, color.G, color.B };
		}
	}
	#endregion

	#region ByteDoubleConverter
	[ValueConversion(typeof(byte), typeof(double))]
	public class ByteDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (double)(byte)value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (byte)(double)value;
		}
	}
	#endregion

	#region ColorBrushConverter
	[ValueConversion(typeof(Color), typeof(SolidColorBrush))]
	public class ColorBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Color color = (Color)value;
			return new SolidColorBrush(color);
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
	}
	#endregion
}
