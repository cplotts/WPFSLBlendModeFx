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
using BlendModeEffectLibrary;
using Microsoft.DwayneNeed.Media.Imaging;
using System.Windows.Threading;
using System.Threading;

namespace WPFTestHarness
{
	/// <summary>
	/// Interaction logic for GradientContoursTestWindow.xaml
	/// </summary>
	public partial class GradientContoursTestWindow : Window
	{
		public GradientContoursTestWindow()
		{
			InitializeComponent();

			this.effectsListBox.Items.Add(new NormalEffect());

			this.effectsListBox.Items.Add(new DarkenEffect());
			this.effectsListBox.Items.Add(new MultiplyEffect());
			this.effectsListBox.Items.Add(new ColorBurnEffect());
			this.effectsListBox.Items.Add(new LinearBurnEffect());

			this.effectsListBox.Items.Add(new LightenEffect());
			this.effectsListBox.Items.Add(new ScreenEffect());
			this.effectsListBox.Items.Add(new ColorDodgeEffect());
			this.effectsListBox.Items.Add(new LinearDodgeEffect());

			this.effectsListBox.Items.Add(new OverlayEffect());
			this.effectsListBox.Items.Add(new SoftLightEffect());
			this.effectsListBox.Items.Add(new HardLightEffect());
			this.effectsListBox.Items.Add(new VividLightEffect());
			this.effectsListBox.Items.Add(new LinearLightEffect());
			this.effectsListBox.Items.Add(new PinLightEffect());

			this.effectsListBox.Items.Add(new DifferenceEffect());
			this.effectsListBox.Items.Add(new ExclusionEffect());

			this.effectsListBox.Items.Add(new GlowEffect());
			this.effectsListBox.Items.Add(new ReflectEffect());

			this.effectsListBox.Items.Add(new HardMixEffect());
			this.effectsListBox.Items.Add(new NegationEffect());
			this.effectsListBox.Items.Add(new PhoenixEffect());

			this.effectsListBox.Items.Add(new AverageEffect());

			this.effectsListBox.Items.Add(new HueEffect());
			this.effectsListBox.Items.Add(new SaturationEffect());
			this.effectsListBox.Items.Add(new ColorEffect());
			this.effectsListBox.Items.Add(new LuminosityEffect());

			this.effectsListBox.SelectedIndex = 0;

			Brush brushA = (Brush)this.Resources["whiteToBlackTopToBottomGradientBrushOpacityBound"];
			Binding bindingA = new Binding();
			bindingA.Source = brushA;
			bindingA.Path = new PropertyPath("Opacity");
			this.aOpacitySlider.SetBinding(Slider.ValueProperty, bindingA);

			Brush brushB = (Brush)this.Resources["blackToWhiteLeftToRightGradientBrushOpacityBound"];
			Binding bindingB = new Binding();
			bindingB.Source = brushB;
			bindingB.Path = new PropertyPath("Opacity");
			this.bOpacitySlider.SetBinding(Slider.ValueProperty, bindingB);

			this.Loaded += (sender, e) => ShowGrayscaleBitmap();
		}


		private void effectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BlendModeEffect effect = e.AddedItems[0] as BlendModeEffect;
			if (effect != null)
			{
				effect.BInput = (ImageBrush)this.Resources["imageBrushTexture"];
				this.resultBorder.Effect = effect;

				ShowGrayscaleBitmap();
			}
		}

		private void ShowGrayscaleBitmap()
		{
			BitmapSource captureScreen = CaptureScreen(this.resultBorder, 96, 96);
			if (captureScreen == null)
				return;

			Grayscale4Bitmap grayscaleBitmap = new Grayscale4Bitmap();
			grayscaleBitmap.Source = captureScreen;
			this.resultImage.Source = grayscaleBitmap;
		}

		private static BitmapSource CaptureScreen(Visual target, double dpiX, double dpiY)
		{
			if (target == null)
				return null;

			Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
			if (bounds.IsEmpty)
				return null;

			RenderTargetBitmap rtb =
				new RenderTargetBitmap
				(
					(int)(bounds.Width * dpiX / 96.0),
					(int)(bounds.Height * dpiY / 96.0),
					dpiX,
					dpiY,
					PixelFormats.Pbgra32
				);

			DrawingVisual dv = new DrawingVisual();
			using (DrawingContext ctx = dv.RenderOpen())
			{
				VisualBrush vb = new VisualBrush(target);
				ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
			}
			rtb.Render(dv);

			return rtb;
		}

		private void opacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)(() => ShowGrayscaleBitmap()));
		}

		private void swap_Click(object sender, RoutedEventArgs e)
		{
			if (!swapped)
			{
				aLabel.Text = "B";
				aBorder.Fill = (Brush)this.Resources["blackToWhiteLeftToRightGradientBrush"];
				bLabel.Text = "A";
				bBorder.Fill = (Brush)this.Resources["whiteToBlackTopToBottomGradientBrush"];

				Brush brushA = (Brush)this.Resources["blackToWhiteLeftToRightGradientBrushOpacityBound"];
				Binding bindingA = new Binding();
				bindingA.Source = brushA;
				bindingA.Path = new PropertyPath("Opacity");
				this.aOpacitySlider.SetBinding(Slider.ValueProperty, bindingA);
				resultBorder.Fill = brushA;

				Brush brushB = (Brush)this.Resources["whiteToBlackTopToBottomGradientBrushOpacityBound"];
				Binding bindingB = new Binding();
				bindingB.Source = brushB;
				bindingB.Path = new PropertyPath("Opacity");
				this.bOpacitySlider.SetBinding(Slider.ValueProperty, bindingB);
				((GeometryDrawing)this.Resources["geometryDrawing"]).Brush = brushB;
			}
			else
			{
				aLabel.Text = "A";
				aBorder.Fill = resultBorder.Fill = (Brush)this.Resources["whiteToBlackTopToBottomGradientBrush"];
				bLabel.Text = "B";
				bBorder.Fill = (Brush)this.Resources["blackToWhiteLeftToRightGradientBrush"];

				Brush brushA = (Brush)this.Resources["whiteToBlackTopToBottomGradientBrushOpacityBound"];
				Binding bindingA = new Binding();
				bindingA.Source = brushA;
				bindingA.Path = new PropertyPath("Opacity");
				this.aOpacitySlider.SetBinding(Slider.ValueProperty, bindingA);
				resultBorder.Fill = brushA;

				Brush brushB = (Brush)this.Resources["blackToWhiteLeftToRightGradientBrushOpacityBound"];
				Binding bindingB = new Binding();
				bindingB.Source = brushB;
				bindingB.Path = new PropertyPath("Opacity");
				this.bOpacitySlider.SetBinding(Slider.ValueProperty, bindingB);
				((GeometryDrawing)this.Resources["geometryDrawing"]).Brush = brushB;
			}

			Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)(() => ShowGrayscaleBitmap()));

			swapped = !swapped;
		}
		private bool swapped = false;
	}
}
