//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- SaturationEffect
// 
// Saturation blend mode creates the result color by combining the luminance and hue
// of the base color with the saturation of the blend color.
//
//--------------------------------------------------------------------------------------

sampler2D input : register(s0);
sampler2D blend : register(s1);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float3 RGBToHSL(float3 color)
{
	float3 hsl;
	
	float fmin = min(min(color.r, color.g), color.b);
	float fmax = max(max(color.r, color.g), color.b);
	float delta = fmax - fmin;
	float maxplusmin = fmax + fmin;

	// set luminance
	hsl.z = maxplusmin / 2.0;

	if (delta == 0.0)
	{
		// a gray color, set hue and satuation to 0
		hsl.x = 0.0;
		hsl.y = 0.0;
	}
	else
	{
		// not a gray color

		// set saturation
		if (hsl.z < 0.5)
			hsl.y = delta / (maxplusmin);
		else
			hsl.y = delta / (2.0 - maxplusmin);
		
		if (color.r == fmax)
			hsl.x = ((color.g - color.b) / delta);
		else if (color.g == fmax)
			hsl.x = 2.0 + ((color.b - color.r) / delta);
		else if (color.b == fmax)
			hsl.x = 4.0 + ((color.r - color.g) / delta);

		hsl.x = hsl.x / 6.0;

		if (hsl.x < 0.0)
			hsl.x += 1.0;
	}

	return hsl;
}

float HueToRGB(float temp1, float temp2, float temp3)
{
	if (temp3 < 0.0)
		temp3 += 1.0;
	else if (temp3 > 1.0)
		temp3 -= 1.0;

	float rgbComponent;

	if ((6.0 * temp3) < 1.0)
		rgbComponent = temp1 + (temp2 - temp1) * 6.0 * temp3;
	else if ((2.0 * temp3) < 1.0)
		rgbComponent = temp2;
	else if ((3.0 * temp3) < 2.0)
		rgbComponent = temp1 + (temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0;
	else
		rgbComponent = temp1;

	return rgbComponent;
}

float3 HSLToRGB(float3 hsl)
{
	float3 rgb;
	
	if (hsl.y == 0.0)
	{
		rgb = float3(hsl.z, hsl.z, hsl.z);
	}
	else
	{
		float temp2;
		if (hsl.z < 0.5)
			temp2 = hsl.z * (1.0 + hsl.y);
		else
			temp2 = (hsl.y + hsl.z) - (hsl.y * hsl.z);
			
		float temp1 = 2.0 * hsl.z - temp2;

		rgb.r = HueToRGB(temp1, temp2, hsl.x + (1.0/3.0));
		rgb.g = HueToRGB(temp1, temp2, hsl.x);
		rgb.b = HueToRGB(temp1, temp2, hsl.x - (1.0/3.0));
	}
	
	return rgb;
}

float3 BlendSaturation(float3 base, float3 blend)
{
	float3 baseHSL = RGBToHSL(base);
	return HSLToRGB(float3(baseHSL.x, RGBToHSL(blend).y, baseHSL.z));
}

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
	resultColor.rgb = BlendSaturation(inputColor.rgb, blendColor.rgb);


	// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
	resultColor.rgb = (1 - blendColor.a) * inputColor.rgb + resultColor.rgb * blendColor.a;

	return resultColor;
}
