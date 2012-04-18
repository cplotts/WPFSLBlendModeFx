using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class OverlayEffect : BlendModeEffect
	{
		static OverlayEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/OverlayEffect.ps");
		}

		public OverlayEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
