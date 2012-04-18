using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class MultiplyEffect : BlendModeEffect
	{
		static MultiplyEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/MultiplyEffect.ps");
		}

		public MultiplyEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
