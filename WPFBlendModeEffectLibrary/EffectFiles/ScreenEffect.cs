using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class ScreenEffect : BlendModeEffect
	{
		static ScreenEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/ScreenEffect.ps");
		}

		public ScreenEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
