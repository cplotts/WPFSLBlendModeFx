using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class LinearBurnEffect : BlendModeEffect
	{
		static LinearBurnEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/LinearBurnEffect.ps");
		}

		public LinearBurnEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
