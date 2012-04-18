//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- HardLightEffect
//
//--------------------------------------------------------------------------------------

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
	// if (Blend > ½) R = 1 - (1-Base) × (1-2×(Blend-½))
	// if (Blend <= ½) R = Base × (2×Blend)
	if (blendColor.r > 0.5)
		resultColor.r = 1 - (1 - inputColor.r) * (1 - 2 * (blendColor.r - 0.5));
	else
		resultColor.r = inputColor.r * (2 * blendColor.r);

	if (blendColor.g > 0.5)
		resultColor.g = 1 - (1 - inputColor.g) * (1 - 2 * (blendColor.g - 0.5));
	else
		resultColor.g = inputColor.g * (2 * blendColor.g);

	if (blendColor.b > 0.5)
		resultColor.b = 1 - (1 - inputColor.b) * (1 - 2 * (blendColor.b - 0.5));
	else
		resultColor.b = inputColor.b * (2 * blendColor.b);


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
