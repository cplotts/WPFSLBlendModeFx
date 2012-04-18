using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class VividLightEffect : BlendModeEffect
	{
		static VividLightEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/VividLightEffect.ps");
		}

		public VividLightEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
