//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- OverlayEffect
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
	// if (Base > ½) R = 1 - (1-2×(Base-½)) × (1-Blend)
	// if (Base <= ½) R = (2×Base) × Blend

	if (inputColor.r > 0.5)
		resultColor.r = 1 - (1 - 2 * (inputColor.r - 0.5)) * (1 - blendColor.r);
	else
		resultColor.r = (2 * inputColor.r) * blendColor.r;

	if (inputColor.g > 0.5)
		resultColor.g = 1 - (1 - 2 * (inputColor.g - 0.5)) * (1 - blendColor.g);
	else
		resultColor.g = (2 * inputColor.g) * blendColor.g;

	if (inputColor.b > 0.5)
		resultColor.b = 1 - (1 - 2 * (inputColor.b - 0.5)) * (1 - blendColor.b);
	else
		resultColor.b = (2 * inputColor.b) * blendColor.b;


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
