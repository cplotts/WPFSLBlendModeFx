using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class HardMixEffect : BlendModeEffect
	{
		static HardMixEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/HardMixEffect.ps");
		}

		public HardMixEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
