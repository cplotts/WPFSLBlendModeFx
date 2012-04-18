using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class LuminosityEffect : BlendModeEffect
	{
		static LuminosityEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/LuminosityEffect.ps");
		}

		public LuminosityEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
