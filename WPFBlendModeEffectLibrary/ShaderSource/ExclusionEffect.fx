//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ExclusionEffect
//
//--------------------------------------------------------------------------------------

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
	// R = ½ - 2×(Base-½)×(Blend-½) 
	resultColor.rgb = 0.5 - 2 * (inputColor.rgb - 0.5) * (blendColor.rgb - 0.5);


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
