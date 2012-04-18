using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class ColorBurnEffect : BlendModeEffect
	{
		static ColorBurnEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/ColorBurnEffect.ps");
		}

		public ColorBurnEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
