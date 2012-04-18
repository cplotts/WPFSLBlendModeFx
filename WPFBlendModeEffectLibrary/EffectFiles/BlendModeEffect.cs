using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class BlendModeEffect : ShaderEffect
	{
		public BlendModeEffect()
		{
			UpdateShaderValue(AInputProperty);
			UpdateShaderValue(BInputProperty);
		}

		[System.ComponentModel.BrowsableAttribute(false)]
		public Brush AInput
		{
			get { return (Brush)GetValue(AInputProperty); }
			set { SetValue(AInputProperty, value); }
		}
		public static readonly DependencyProperty AInputProperty =
			ShaderEffect.RegisterPixelShaderSamplerProperty
			(
				"AInput",
				typeof(BlendModeEffect),
				0
			);

		public Brush BInput
		{
			get { return (Brush)GetValue(BInputProperty); }
			set { SetValue(BInputProperty, value); }
		}
		public static readonly DependencyProperty BInputProperty =
			ShaderEffect.RegisterPixelShaderSamplerProperty
			(
				"BInput",
				typeof(BlendModeEffect),
				1
			);
	}
}
