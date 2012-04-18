//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- DarkenEffect
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
	if (inputColor.r > blendColor.r)
		resultColor.r = blendColor.r;
	else
		resultColor.r = inputColor.r;

	if (inputColor.g > blendColor.g)
		resultColor.g = blendColor.g;
	else
		resultColor.g = inputColor.g;

	if (inputColor.b > blendColor.b)
		resultColor.b = blendColor.b;
	else
		resultColor.b = inputColor.b;


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
