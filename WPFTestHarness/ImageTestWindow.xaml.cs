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

namespace WPFTestHarness
{
	/// <summary>
	/// Interaction logic for ImageTestWindow.xaml
	/// </summary>
	public partial class ImageTestWindow : Window
	{
		public ImageTestWindow()
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


			List<object> images = new List<object>();
			var anImage =
				new
				{
					DisplayName = "image1",
					Image =
						new Image()
						{
							Source = new BitmapImage(new Uri("pack://application:,,/Resources/image1_325x244.jpg"))
						}
				};
			images.Add(anImage);
			anImage =
				new
				{
					DisplayName = "image2",
					Image =
						new Image()
						{
							Source = new BitmapImage(new Uri("pack://application:,,/Resources/image2_325x244.jpg"))
						}
				};
			images.Add(anImage);
			anImage =
				new
				{
					DisplayName = "summer",
					Image =
						new Image()
						{
							Source = new BitmapImage(new Uri("pack://application:,,/Resources/summer_325x244.jpg"))
						}
				};
			images.Add(anImage);
			this.aImageComboBox.ItemsSource = images;


			images = new List<object>();
			anImage =
				new
				{
					DisplayName = "texture1",
					Image =
						new Image()
						{
							Source = new BitmapImage(new Uri("pack://application:,,/Resources/texture1_325x244.jpg"))
						}
				};
			images.Add(anImage);
			anImage =
				new
				{
					DisplayName = "texture2",
					Image =
						new Image()
						{
							Source = new BitmapImage(new Uri("pack://application:,,/Resources/texture2_325x244.jpg"))
						}
				};
			images.Add(anImage);
			anImage =
				new
				{
					DisplayName = "fall",
					Image =
						new Image()
						{
							Source = new BitmapImage(new Uri("pack://application:,,/Resources/fall_325x244.jpg"))
						}
				};
			images.Add(anImage);
			this.bImageComboBox.ItemsSource = images;
		}

		private void effectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BlendModeEffect effect = e.AddedItems[0] as BlendModeEffect;
			if (effect != null)
			{
				effect.BInput = (ImageBrush)this.Resources["imageBrushTexture"];
				this.rImage.Effect = effect;
			}
		}
	}
}
