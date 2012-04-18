using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class NegationEffect : BlendModeEffect
	{
		static NegationEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/NegationEffect.ps");
		}

		public NegationEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
