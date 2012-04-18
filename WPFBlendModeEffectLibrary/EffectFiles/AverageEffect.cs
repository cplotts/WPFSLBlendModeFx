using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class AverageEffect : BlendModeEffect
	{
		static AverageEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/AverageEffect.ps");
		}

		public AverageEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
