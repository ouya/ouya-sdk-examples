sampler SourceSampler: register(s0);
sampler LightSampler: register(s1);

float Alpha;

float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR0
{
	float4 source = tex2D(SourceSampler, Tex);
	float4 light = tex2D(LightSampler, Tex);
	
	source.r -= light.r * light.a * Alpha;
	source.g -= light.g * light.a * Alpha;
	source.b -= light.b * light.a * Alpha;

	return source;
}

technique hit
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}