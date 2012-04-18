//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- NormalEffect
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

	inputColor.rgb = (1 - blendColor.a) * inputColor.rgb + blendColor.rgb;

	return inputColor;
}
