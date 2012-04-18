using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BlendModeEffectLibrary
{
	public class ReflectEffect : BlendModeEffect
	{
		static ReflectEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("ShaderSource/ReflectEffect.ps");
		}

		public ReflectEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
