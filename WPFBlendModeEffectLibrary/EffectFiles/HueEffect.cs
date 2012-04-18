using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class HueEffect : BlendModeEffect
	{
		static HueEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/HueEffect.ps");
		}

		public HueEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
