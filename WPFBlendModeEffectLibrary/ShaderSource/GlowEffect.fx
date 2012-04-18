//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- GlowEffect
//
//--------------------------------------------------------------------------------------

#define BlendReflectf(base, blend)	((blend == 1.0) ? blend : min(base * base / (1.0 - blend), 1.0))

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
	resultColor.r = BlendReflectf(blendColor.r, inputColor.r);
	resultColor.g = BlendReflectf(blendColor.g, inputColor.g);
	resultColor.b = BlendReflectf(blendColor.b, inputColor.b);


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
