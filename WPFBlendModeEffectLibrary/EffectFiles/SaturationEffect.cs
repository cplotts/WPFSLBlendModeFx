using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class SaturationEffect : BlendModeEffect
	{
		static SaturationEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/SaturationEffect.ps");
		}

		public SaturationEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
