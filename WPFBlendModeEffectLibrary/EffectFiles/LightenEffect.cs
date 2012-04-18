using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class LightenEffect : BlendModeEffect
	{
		static LightenEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/LightenEffect.ps");
		}

		public LightenEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
