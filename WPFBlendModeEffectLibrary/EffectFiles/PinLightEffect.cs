using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class PinLightEffect : BlendModeEffect
	{
		static PinLightEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/PinLightEffect.ps");
		}

		public PinLightEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
