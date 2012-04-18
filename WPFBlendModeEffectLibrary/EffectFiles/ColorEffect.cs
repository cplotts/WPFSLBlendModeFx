using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class ColorEffect : BlendModeEffect
	{
		static ColorEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/ColorEffect.ps");
		}

		public ColorEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
