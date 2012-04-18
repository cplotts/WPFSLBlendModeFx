//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- SoftLightEffect
//
//--------------------------------------------------------------------------------------

#define BlendSoftLightf(base, blend)	((blend < 0.5) ? (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) : (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend)))

sampler2D input : register(s0);
sampler2D blend : register(s1);

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
	resultColor.r = BlendSoftLightf(inputColor.r, blendColor.r);
	resultColor.g = BlendSoftLightf(inputColor.g, blendColor.g);
	resultColor.b = BlendSoftLightf(inputColor.b, blendColor.b);


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
