// ------------------------------------------------------------------------------------------------ //
// ----- Copyright 2011 Christopher Harris --------------------- http://krypton.codeplex.com/ ----- //
// ----------------------------------------------------------------- mailto:xixonia@gmail.com ----- //
// ------------------------------------------------------------------------------------------------ //

// ------------------------------------------------------------------------------------------------ //
// ----- Parameters ------------------------------------------------------------------------------- //

float4x4 Matrix;

texture Texture0;
texture Texture1;
float2	LightPosition;
float	LightIntensityFactor = 1;

float	ShadowStrech = 1000000;

float2 TexelBias;

float4 AmbientColor;

float Bluriness = 0.5f;
float BlurFactorU = 0;
float BlurFactorV = 0;

// ------------------------------------------------------------------------------------------------ //
// ----- Samplers --------------------------------------------------------------------------------- //

sampler2D tex0 = sampler_state
{
	Texture = <Texture0>;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D tex1 = sampler_state
{
	Texture = <Texture1>;
	AddressU = Clamp;
	AddressV = Clamp;
};

// ------------------------------------------------------------------------------------------------ //
// ----- Structures ------------------------------------------------------------------------------- //

struct ShadowHullVertex
{
	float4 Position : POSITION0;
	float2 Normal	: NORMAL0;
	float4 Color	: COLOR0;
};

struct VertexPositionColor
{
	float4 Position : POSITION0;
	float4 Color	: COLOR0;
};

struct VertexPositionNormalTexture
{
	float4 Position	: POSITION0;
	float4 Normal	: NORMAL0;
	float2 TexCoord	: TEXCOORD0;
};

struct VertexPositionColorTexture
{
	float4 Position	: POSITION0;
	float4 Color	: COLOR0;
	float2 TexCoord	: TEXCOORD0;
};

struct VertexPositionTexture
{
	float4 Position	: POSITION0;
	float2 TexCoord : TEXCOORD0;
};

// ------------------------------------------------------------------------------------------------ //
// ----- Techniques ------------------------------------------------------------------------------- //

// ------------------------------------------------------------------------------------------------
// ----- Technique: TextureToTarget ---------------------------------------------------------------

VertexPositionTexture VS_ScreenCopy(VertexPositionTexture input)
{
	VertexPositionTexture output;
	
	output.Position = input.Position;
	output.TexCoord = input.TexCoord;
	
	return input;
};

float4 PS_ScreenCopy(VertexPositionTexture input) : COLOR0
{
	return tex2D(tex0, input.TexCoord + TexelBias);
};

technique TextureToTarget_Add
{
	pass Pass1
	{
		StencilEnable = False;

		BlendOp = Add;
		SrcBlend = One;
		DestBlend = One;

		AlphaBlendEnable = True;
		
		CullMode = CCW;

		VertexShader = compile vs_2_0 VS_ScreenCopy();
		PixelShader = compile ps_2_0 PS_ScreenCopy();
	}
};

technique TextureToTarget_Multiply
{
	pass Pass1
	{
		StencilEnable = False;

		BlendOp = Add;
		SrcBlend = Zero;
		DestBlend = SrcColor;

		AlphaBlendEnable = True;
		
		CullMode = CCW;

		VertexShader = compile vs_2_0 VS_ScreenCopy();
		PixelShader = compile ps_2_0 PS_ScreenCopy();
	}
};

float4 PS_ClearTarget() : COLOR0
{
	return float4(0, 0, 0, 1);
}

// ------------------------------------------------------------------------------------------------
// ----- Technique: SimpleTexture -----------------------------------------------------------------

VertexPositionColorTexture VS_SimpleTexture(VertexPositionColorTexture input)
{
	VertexPositionColorTexture output;

	output.Position = mul(input.Position, Matrix);
	output.Color = input.Color;
	output.TexCoord = input.TexCoord;

	return output;
};

float4 PS_SimpleTexture(VertexPositionColorTexture input) : COLOR0
{
	return tex2D(tex0, input.TexCoord) * input.Color;
};

float4 PS_LightTexture(VertexPositionColorTexture input) : COLOR0
{
	return pow(tex2D(tex0, input.TexCoord) * input.Color, LightIntensityFactor);
};

technique SimpleTexture
{
	pass Pass1
	{
		StencilEnable = False;

		VertexShader = compile vs_2_0 VS_SimpleTexture();
		PixelShader = compile ps_2_0 PS_SimpleTexture();
	}
};

technique LightTexture
{
	pass Pass1
	{
		StencilEnable = False;

		BlendOp = Add;
		DestBlend = Zero;
		SrcBlend = SrcAlpha;
		
		AlphaBlendEnable = True;

		VertexShader = compile vs_2_0 VS_SimpleTexture();
		PixelShader = compile ps_2_0 PS_LightTexture();
	}
};

// ------------------------------------------------------------------------------------------------
// ----- Technique: PointLight_Shadow -------------------------------------------------------------

VertexPositionColor VS_PointLight_Shadow(ShadowHullVertex input)
{
    float2 direction = normalize(LightPosition.xy - input.Position.xy);
    
    if(dot(input.Normal.xy, direction) < 0)
    {
		// Stretch backfacing vertices
		input.Position.xy -= direction * ShadowStrech;
    }
	
	VertexPositionColor output;

	output.Position = mul(input.Position, Matrix);
	output.Color = input.Color;

	return output;
};

float4 PS_PointLight_Shadow(float4 input : COLOR0) : COLOR0
{
	return input;
};

VertexPositionColor VS_Shadow_HullIllumination(ShadowHullVertex input)
{
	VertexPositionColor output;

	output.Position = mul(input.Position, Matrix);
	output.Color = input.Color;

	return output;
};

float4 PS_Shadow_HullIllumination() : COLOR0
{
	return float4(1, 1, 1, 1);
};

technique PointLight_Shadow
{
	pass Shadow
	{
		StencilEnable = False;

		AlphaBlendEnable = True;

		BlendOp = RevSubtract;
		SrcBlend = One;
		DestBlend = One;

		VertexShader = compile vs_2_0 VS_PointLight_Shadow();
		PixelShader = compile ps_2_0 PS_PointLight_Shadow();
	}
};

// ------------------------------------------------------------------------------------------------
// ----- Technique: PointLight_ShadowWithIllumination ---------------------------------------------

technique PointLight_ShadowWithIllumination
{
	pass Shadow_Illumination
	{
		// This outlines where our hulls are currently, so we don't draw shadows there
		StencilEnable = True;
		StencilFunc = Never;
		StencilFail = Incr;

		AlphaBlendEnable = False;

		VertexShader = compile vs_2_0 VS_Shadow_HullIllumination();
		PixelShader = compile ps_2_0 PS_Shadow_HullIllumination();
	}

	pass Shadow
	{
		// Only draw where the Stencil hasn't touched
		StencilEnable = True;
		StencilFunc = Equal;
		StencilRef = 0;
		StencilFail = Incr;

		AlphaBlendEnable = True;

		BlendOp = RevSubtract;
		SrcBlend = One;
		DestBlend = One;

		VertexShader = compile vs_2_0 VS_PointLight_Shadow();
		PixelShader = compile ps_2_0 PS_PointLight_Shadow();
	}
	
};

// ------------------------------------------------------------------------------------------------
// ----- Technique: PointLight_ShadowWithOcclusion ------------------------------------------------

technique PointLight_ShadowWithOcclusion
{
	pass Shadow_HullStencil
	{
		// This outlines where our hulls are currently, so we don't draw shadows there, unless the hull is occluded
		StencilEnable = True;
		StencilFunc = Never;
		StencilFail = Incr;

		VertexShader = compile vs_2_0 VS_Shadow_HullIllumination();
		PixelShader = compile ps_2_0 PS_Shadow_HullIllumination();
	}

	pass Shadow
	{
		// This allows us to draw shadows on hulls behind other hulls
		StencilEnable = True;
		StencilFunc = NotEqual;
		StencilRef = 1;
		StencilPass = Keep;
		StencilFail = Incr;

		AlphaBlendEnable = True;

		BlendOp = RevSubtract;
		SrcBlend = One;
		DestBlend = One;

		VertexShader = compile vs_2_0 VS_PointLight_Shadow();
		PixelShader = compile ps_2_0 PS_PointLight_Shadow();
	}
};

// ------------------------------------------------------------------------------------------------
// ----- Technique: DebugDraw ---------------------------------------------------------------------

technique DebugDraw
{
	pass Solid
	{
		StencilEnable = False;
		AlphaBlendEnable = False;

		VertexShader = compile vs_2_0 VS_Shadow_HullIllumination();
		PixelShader = compile ps_2_0 PS_Shadow_HullIllumination();
	}
};

// ------------------------------------------------------------------------------------------------
// ----- Technique: Blur --------------------------------------------------------------------------

float4 PS_BlurH(in float2 texCoord : TEXCOORD0) : COLOR0
{
	float blurFactor = Bluriness * BlurFactorU / 4.0f;

	return
		tex2D(tex0, float2(texCoord.x - blurFactor * 4,	texCoord.y)	+ TexelBias) * 0.05f +
		tex2D(tex0, float2(texCoord.x - blurFactor * 3,	texCoord.y)	+ TexelBias) * 0.09f +
		tex2D(tex0, float2(texCoord.x - blurFactor * 2,	texCoord.y)	+ TexelBias) * 0.12f +
		tex2D(tex0, float2(texCoord.x - blurFactor,		texCoord.y)	+ TexelBias) * 0.15f +
		tex2D(tex0, float2(texCoord.x,					texCoord.y)	+ TexelBias) * 0.18f +
		tex2D(tex0, float2(texCoord.x + blurFactor,		texCoord.y)	+ TexelBias) * 0.15f +
		tex2D(tex0, float2(texCoord.x + blurFactor * 2,	texCoord.y)	+ TexelBias) * 0.12f +
		tex2D(tex0, float2(texCoord.x + blurFactor * 3,	texCoord.y)	+ TexelBias) * 0.09f +
		tex2D(tex0, float2(texCoord.x + blurFactor * 4,	texCoord.y)	+ TexelBias) * 0.05f;
}

float4 PS_BlurV(in float2 texCoord : TEXCOORD0) : COLOR0
{
	float blurFactor = Bluriness * BlurFactorV / 4.0f;

	return
		tex2D(tex0, float2(texCoord.x, texCoord.y - blurFactor * 4)	+ TexelBias) * 0.05f +
		tex2D(tex0, float2(texCoord.x, texCoord.y - blurFactor * 3)	+ TexelBias) * 0.09f +
		tex2D(tex0, float2(texCoord.x, texCoord.y - blurFactor * 2)	+ TexelBias) * 0.12f +
		tex2D(tex0, float2(texCoord.x, texCoord.y - blurFactor)		+ TexelBias) * 0.15f +
		tex2D(tex0, float2(texCoord.x, texCoord.y)					+ TexelBias) * 0.18f +
		tex2D(tex0, float2(texCoord.x, texCoord.y + blurFactor)		+ TexelBias) * 0.15f +
		tex2D(tex0, float2(texCoord.x, texCoord.y + blurFactor * 2)	+ TexelBias) * 0.12f +
		tex2D(tex0, float2(texCoord.x, texCoord.y + blurFactor * 3)	+ TexelBias) * 0.09f +
		tex2D(tex0, float2(texCoord.x, texCoord.y + blurFactor * 4)	+ TexelBias) * 0.05f;
}

technique Blur
{
    pass HorizontalBlur
    {
		ScissorTestEnable = False;
		StencilEnable = False;
		AlphaBlendEnable = False;

		CullMode = CCW;

		VertexShader = compile vs_2_0 VS_ScreenCopy();
        PixelShader = compile ps_2_0 PS_BlurH();
    }
    
    pass VerticalBlur
    {
		ScissorTestEnable = False;
		StencilEnable = False;
		AlphaBlendEnable = False;

		CullMode = CCW;

		VertexShader = compile vs_2_0 VS_ScreenCopy();
        PixelShader = compile ps_2_0 PS_BlurV();
    }
}


technique PointLight_Shadow_Fast
{
	pass ShadowStencil
	{
		StencilEnable = True;
		StencilFunc = Always;
		StencilPass = Incr;

		ScissorTestEnable = True;

		AlphaBlendEnable = True;
		BlendOp = Add;
		SrcBlend = DestAlpha;
		DestBlend = Zero;

		ColorWriteEnable = Alpha;

		VertexShader = compile vs_2_0 VS_PointLight_Shadow();
		PixelShader = compile ps_2_0 PS_PointLight_Shadow();
	}

	pass Light
	{
		StencilEnable = True;
		StencilFunc = Equal;
		StencilRef = 1;
		StencilPass = Keep;

		ScissorTestEnable = True;

		AlphaBlendEnable = True;
		BlendOp = Add;
		SrcBlend = One;
		DestBlend = One;
		
		ColorWriteEnable = Red | Green | Blue;

		VertexShader = compile vs_2_0 VS_SimpleTexture();
		PixelShader = compile ps_2_0 PS_LightTexture();
	}

	pass LightAlpha
	{
		StencilEnable = True;
		StencilFunc = NotEqual;
		StencilRef = 1;
		StencilPass = Keep;

		ScissorTestEnable = True;

		AlphaBlendEnable = True;
		BlendOp = Add;
		SrcBlend = DestAlpha;
		DestBlend = One;

		VertexShader = compile vs_2_0 VS_SimpleTexture();
		PixelShader = compile ps_2_0 PS_LightTexture();
	}

	pass ClearAlpha
	{
		StencilEnable = False;

		AlphaBlendEnable = False;

		CullMode = CCW;
		
		ColorWriteEnable = Alpha;

		VertexShader = compile vs_2_0 VS_ScreenCopy();
		PixelShader = compile ps_2_0 PS_ClearTarget();
	}

};