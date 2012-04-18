using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class LinearDodgeEffect : BlendModeEffect
	{
		static LinearDodgeEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/LinearDodgeEffect.ps");
		}

		public LinearDodgeEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
