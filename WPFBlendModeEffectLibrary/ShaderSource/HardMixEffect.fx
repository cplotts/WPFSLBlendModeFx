//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- HardMixEffect
//
//--------------------------------------------------------------------------------------

#define BlendColorDodgef(base, blend)	((blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0))
#define BlendColorBurnf(base, blend)	((blend == 0.0) ? blend : max((1.0 - ((1.0 - base) / blend)), 0.0))
#define BlendVividLightf(base, blend)	((blend < 0.5) ? BlendColorBurnf(base, (2.0 * blend)) : BlendColorDodgef(base, (2.0 * (blend - 0.5))))
#define BlendHardMixf(base, blend)		((BlendVividLightf(base, blend) < 0.5) ? 0.0 : 1.0)

sampler2D input : register(s0);
sampler2D blend : register(s1);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 inputColor;
	inputColor = tex2D(input, uv);

	float4 blendColor;
	blendColor = tex2D(blend, uv);


	float4 resultColor;
	resultColor.a = inputColor.a;

	// un-premultiply the blendColor alpha out from blendColor
	blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);


	// apply the blend mode math
	resultColor.r = BlendHardMixf(inputColor.r, blendColor.r);
	resultColor.g = BlendHardMixf(inputColor.g, blendColor.g);
	resultColor.b = BlendHardMixf(inputColor.b, blendColor.b);


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
